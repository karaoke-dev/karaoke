// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using System.Diagnostics.CodeAnalysis;
using osu.Framework.Allocation;
using osu.Framework.Localisation;
using osu.Game.Rulesets.Karaoke.Edit.ChangeHandlers.Lyrics;
using osu.Game.Rulesets.Karaoke.Edit.Utils;
using osu.Game.Rulesets.Karaoke.Objects;

namespace osu.Game.Rulesets.Karaoke.Edit.Lyrics.Settings.Reference
{
    public class ReferenceLyricSection : LyricPropertySection
    {
        protected override LocalisableString Title => "Reference lyric";

        [Resolved, AllowNull]
        private ILyricReferenceChangeHandler lyricReferenceChangeHandler { get; set; }

        private readonly LabelledReferenceLyricSelector labelledReferenceLyricSelector;

        public ReferenceLyricSection()
        {
            Children = new[]
            {
                labelledReferenceLyricSelector = new LabelledReferenceLyricSelector
                {
                    Label = "Referenced lyric",
                    Description = "Select the similar lyric that want to reference or sync the property."
                }
            };

            labelledReferenceLyricSelector.Current.BindValueChanged(x =>
            {
                if (!IsRebinding)
                    lyricReferenceChangeHandler.UpdateReferenceLyric(x.NewValue);
            });
        }

        protected override void OnLyricChanged(Lyric? lyric)
        {
            if (lyric == null)
                return;

            labelledReferenceLyricSelector.Current = lyric.ReferenceLyricBindable;
            labelledReferenceLyricSelector.IgnoredLyric = lyric;
        }

        protected override LockLyricPropertyBy? IsWriteLyricPropertyLocked(Lyric lyric)
            => HitObjectWritableUtils.GetLyricPropertyLockedBy(lyric, nameof(Lyric.ReferenceLyric), nameof(lyric.ReferenceLyricConfig));

        protected override LocalisableString GetWriteLyricPropertyLockedDescription(LockLyricPropertyBy lockLyricPropertyBy) =>
            lockLyricPropertyBy switch
            {
                // technically the property is always editable.
                _ => throw new ArgumentOutOfRangeException(nameof(lockLyricPropertyBy), lockLyricPropertyBy, null)
            };

        protected override LocalisableString GetWriteLyricPropertyLockedTooltip(LockLyricPropertyBy lockLyricPropertyBy) =>
            lockLyricPropertyBy switch
            {
                // technically the property is always editable.
                _ => throw new ArgumentOutOfRangeException(nameof(lockLyricPropertyBy), lockLyricPropertyBy, null)
            };
    }
}
