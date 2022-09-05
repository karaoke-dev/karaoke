// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using System.Linq;
using osu.Framework.Localisation;
using osu.Game.Rulesets.Karaoke.Objects;
using osu.Game.Rulesets.Karaoke.Objects.Properties;
using osu.Game.Rulesets.Karaoke.Objects.Types;

namespace osu.Game.Rulesets.Karaoke.Edit.Utils
{
    public static class HitObjectWritableUtils
    {
        #region Lyric property

        public static bool IsWriteLyricPropertyLocked(Lyric lyric, params string[] propertyNames)
            => getLyricPropertyLockedBy(lyric, propertyNames) != null;

        public static bool IsWriteLyricPropertyLocked(Lyric lyric, string propertyName)
            => getLyricPropertyLockedBy(lyric, propertyName) != null;

        public static LocalisableString? GetLyricPropertyLockedReason(Lyric lyric, params string[] propertyNames)
            => getLyricPropertyLockedMessage(getLyricPropertyLockedBy(lyric, propertyNames));

        public static LocalisableString? GetLyricPropertyLockedReason(Lyric lyric, string propertyName)
            => getLyricPropertyLockedMessage(getLyricPropertyLockedBy(lyric, propertyName));

        private static LocalisableString? getLyricPropertyLockedMessage(LockLyricPropertyBy? reasons)
        {
            return reasons switch
            {
                LockLyricPropertyBy.ReferenceLyricConfig => "Cannot modify this property due to this lyric is property is sync from another lyric.",
                LockLyricPropertyBy.LockState => "This property is locked and not editable",
                null => default(LocalisableString?),
                _ => throw new ArgumentOutOfRangeException()
            };
        }

        private static LockLyricPropertyBy? getLyricPropertyLockedBy(Lyric lyric, params string[] propertyNames)
        {
            var reasons = propertyNames.Select(x => getLyricPropertyLockedBy(lyric, x))
                                       .Where(x => x != null)
                                       .OfType<LockLyricPropertyBy>()
                                       .ToArray();

            if (reasons.Contains(LockLyricPropertyBy.ReferenceLyricConfig))
                return LockLyricPropertyBy.ReferenceLyricConfig;

            if (reasons.Contains(LockLyricPropertyBy.LockState))
                return LockLyricPropertyBy.LockState;

            return null;
        }

        private static LockLyricPropertyBy? getLyricPropertyLockedBy(Lyric lyric, string propertyName)
        {
            bool lockedByConfig = isWriteLyricPropertyLockedByConfig(lyric.ReferenceLyricConfig, propertyName);
            if (lockedByConfig)
                return LockLyricPropertyBy.ReferenceLyricConfig;

            bool lockedByState = isWriteLyricPropertyLockedByState(lyric.Lock, propertyName);
            if (lockedByState)
                return LockLyricPropertyBy.LockState;

            return null;
        }

        private static bool isWriteLyricPropertyLockedByState(LockState lockState, string propertyName)
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

        private static bool isWriteLyricPropertyLockedByConfig(IReferenceLyricPropertyConfig? config, string propertyName)
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

        #endregion

        #region Create or remove notes.

        public static bool IsCreateOrRemoveNoteLocked(Lyric lyric)
            => GetCreateOrRemoveNoteLockedReason(lyric) != null;

        public static LocalisableString? GetCreateOrRemoveNoteLockedReason(Lyric lyric)
            => getCreateOrRemoveNoteLockedMessage(getCreateOrRemoveNoteLockedBy(lyric));

        private static LocalisableString? getCreateOrRemoveNoteLockedMessage(LockLyricPropertyBy? reasons)
        {
            return reasons switch
            {
                LockLyricPropertyBy.ReferenceLyricConfig => "Cannot modify this property due to this lyric is property is sync from another lyric.",
                LockLyricPropertyBy.LockState => "This property is locked and not editable",
                null => default(LocalisableString?),
                _ => throw new ArgumentOutOfRangeException()
            };
        }

        private static LockLyricPropertyBy? getCreateOrRemoveNoteLockedBy(Lyric lyric, params string[] propertyNames)
        {
            bool lockedByConfig = isCreateOrRemoveNoteLocked(lyric.ReferenceLyricConfig);
            if (lockedByConfig)
                return LockLyricPropertyBy.ReferenceLyricConfig;

            return null;
        }

        private static bool isCreateOrRemoveNoteLocked(IReferenceLyricPropertyConfig? config)
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

        #endregion

        #region Note property

        public static bool IsWriteNotePropertyLocked(Note note, params string[] propertyNames)
            => getNotePropertyLockedBy(note, propertyNames) != null;

        public static bool IsWriteNotePropertyLocked(Note note, string propertyName)
            => getNotePropertyLockedBy(note, propertyName) != null;

        public static LocalisableString? GetNotePropertyLockedReason(Note note, params string[] propertyNames)
            => getNotePropertyLockedMessage(getNotePropertyLockedBy(note, propertyNames));

        public static LocalisableString? GetNotePropertyLockedReason(Note note, string propertyName)
            => getNotePropertyLockedMessage(getNotePropertyLockedBy(note, propertyName));

        private static LocalisableString? getNotePropertyLockedMessage(LockNotePropertyBy? reasons)
        {
            return reasons switch
            {
                LockNotePropertyBy.ReferenceLyricConfig => "Cannot modify this property due to this note's lyric is property is sync from another lyric.",
                null => default(LocalisableString?),
                _ => throw new ArgumentOutOfRangeException()
            };
        }

        private static LockNotePropertyBy? getNotePropertyLockedBy(Note note, params string[] propertyNames)
        {
            var reasons = propertyNames.Select(x => getNotePropertyLockedBy(note, x))
                                       .Where(x => x != null)
                                       .OfType<LockNotePropertyBy>()
                                       .ToArray();

            if (reasons.Contains(LockNotePropertyBy.ReferenceLyricConfig))
                return LockNotePropertyBy.ReferenceLyricConfig;

            return null;
        }

        private static LockNotePropertyBy? getNotePropertyLockedBy(Note note, string propertyName)
        {
            var lyric = note.ReferenceLyric;

            bool lockByReferenceLyricConfig = lyric != null && isWriteNotePropertyLockedByReferenceLyric(lyric, propertyName);
            if (lockByReferenceLyricConfig)
                return LockNotePropertyBy.ReferenceLyricConfig;

            return null;
        }

        private static bool isWriteNotePropertyLockedByReferenceLyric(Lyric lyric, string propertyName)
        {
            // todo: implement.
            return false;
        }

        #endregion

        private enum LockLyricPropertyBy
        {
            ReferenceLyricConfig,

            LockState,
        }

        private enum LockNotePropertyBy
        {
            ReferenceLyricConfig,
        }
    }
}
