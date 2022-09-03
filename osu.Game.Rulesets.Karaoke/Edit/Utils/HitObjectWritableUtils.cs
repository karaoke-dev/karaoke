// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using System.Linq;
using osu.Game.Rulesets.Karaoke.Objects;
using osu.Game.Rulesets.Karaoke.Objects.Properties;
using osu.Game.Rulesets.Karaoke.Objects.Types;

namespace osu.Game.Rulesets.Karaoke.Edit.Utils
{
    public static class HitObjectWritableUtils
    {
        public static bool IsWriteLyricPropertyLocked(Lyric lyric, params string[] propertyNames)
            => propertyNames.All(x => IsWriteLyricPropertyLocked(lyric, x));

        public static bool IsWriteLyricPropertyLocked(Lyric lyric, string propertyName)
        {
            bool checkByState = IsWriteLyricPropertyLockedByState(lyric.Lock, propertyName);
            bool changeByReferenceLyricConfig = IsWriteLyricPropertyLockedByConfig(lyric.ReferenceLyricConfig, propertyName);

            return checkByState || changeByReferenceLyricConfig;
        }

        public static bool IsWriteLyricPropertyLockedByState(LockState lockState, string propertyName)
        {
            // todo: implement.
            return false;
        }

        public static bool IsWriteLyricPropertyLockedByConfig(IReferenceLyricPropertyConfig? config, string propertyName)
        {
            return config switch
            {
                ReferenceLyricConfig => false,
                SyncLyricConfig syncLyricConfig => propertyName switch
                {
                    nameof(Lyric.ID) => false, // although the id is not changeable, but it's not locked by config.
                    nameof(Lyric.Text) => true,
                    nameof(Lyric.TimeTags) => syncLyricConfig.SyncTimeTagProperty,
                    nameof(Lyric.RubyTags) => true,
                    nameof(Lyric.RomajiTags) => true,
                    nameof(Lyric.StartTime) => false,
                    nameof(Lyric.Duration) => false,
                    nameof(Lyric.Singers) => syncLyricConfig.SyncSingerProperty,
                    nameof(Lyric.Translates) => true,
                    nameof(Lyric.Language) => true,
                    nameof(Lyric.Order) => true,
                    nameof(Lyric.Lock) => true,
                    nameof(Lyric.ReferenceLyric) => false,
                    nameof(Lyric.ReferenceLyricConfig) => false,
                    // base class
                    nameof(Lyric.Samples) => false,
                    _ => throw new NotSupportedException()
                },
                null => false,
                _ => throw new NotSupportedException()
            };
        }

        public static bool IsWriteNotePropertyLocked(Note note, string propertyName)
        {
            var lyric = note.ReferenceLyric;
            return lyric == null || IsWriteNotePropertyLockedByReferenceLyric(lyric, propertyName);
        }

        public static bool IsWriteNotePropertyLockedByReferenceLyric(Lyric lyric, string propertyName)
        {
            // todo: implement.
            return false;
        }
    }
}
