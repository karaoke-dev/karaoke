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
            // partial lock will only lock some property change like texting because they are easy to be modified.
            // fully lock will basically lock all lyric properties.
            return propertyName switch
            {
                nameof(Lyric.ID) => false, // although the id is not changeable, but it's not locked by config.
                nameof(Lyric.Text) => lockState > LockState.None,
                nameof(Lyric.TimeTags) => lockState > LockState.None,
                nameof(Lyric.RubyTags) => lockState > LockState.None,
                nameof(Lyric.RomajiTags) => lockState > LockState.None,
                nameof(Lyric.StartTime) => lockState > LockState.Partial,
                nameof(Lyric.Duration) => lockState > LockState.Partial,
                nameof(Lyric.Singers) => lockState > LockState.Partial,
                nameof(Lyric.Translates) => lockState > LockState.Partial,
                nameof(Lyric.Language) => lockState > LockState.Partial,
                nameof(Lyric.Order) => false, // order can always be changed.
                nameof(Lyric.Lock) => false, // order can always be changed.
                nameof(Lyric.ReferenceLyric) => lockState > LockState.Partial,
                nameof(Lyric.ReferenceLyricConfig) => lockState > LockState.Partial,
                // base class
                nameof(Lyric.Samples) => false,
                _ => throw new NotSupportedException()
            };
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

        public static bool IsCreateOrRemoveNoteLocked(Lyric lyric)
        {
            return IsCreateOrRemoveNoteLocked(lyric.ReferenceLyricConfig);
        }

        public static bool IsCreateOrRemoveNoteLocked(IReferenceLyricPropertyConfig? config)
        {
            // todo: implementation.
            return config switch
            {
                ReferenceLyricConfig => false,
                SyncLyricConfig => true,
                null => false,
                _ => throw new NotSupportedException()
            };
        }

        public static bool IsWriteNotePropertyLocked(Note note, params string[] propertyNames)
            => propertyNames.All(x => IsWriteNotePropertyLocked(note, x));

        public static bool IsWriteNotePropertyLocked(Note note, string propertyName)
        {
            var lyric = note.ReferenceLyric;
            return lyric != null && IsWriteNotePropertyLockedByReferenceLyric(lyric, propertyName);
        }

        public static bool IsWriteNotePropertyLockedByReferenceLyric(Lyric lyric, string propertyName)
        {
            // todo: implement.
            return false;
        }
    }
}
