// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

#nullable disable

using System;
using System.Diagnostics;
using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Localisation;
using osu.Game.Rulesets.Karaoke.Edit.ChangeHandlers.Lyrics;
using osu.Game.Rulesets.Karaoke.Edit.Lyrics.Extends.RubyRomaji.Components;
using osu.Game.Rulesets.Karaoke.Edit.Lyrics.States.Modes;
using osu.Game.Rulesets.Karaoke.Edit.Utils;
using osu.Game.Rulesets.Karaoke.Objects;

namespace osu.Game.Rulesets.Karaoke.Edit.Lyrics.Extends.RubyRomaji
{
    public class RomajiTagEditSection : TextTagEditSection<RomajiTag>
    {
        protected override LocalisableString Title => "Romaji";

        [Resolved]
        private ILyricRomajiTagsChangeHandler romajiTagsChangeHandler { get; set; }

        protected override IBindableList<RomajiTag> GetBindableTextTags(Lyric lyric)
            => lyric.RomajiTagsBindable;

        protected override LabelledTextTagTextBox<RomajiTag> CreateLabelledTextTagTextBox(Lyric lyric, RomajiTag textTag)
            => new LabelledRomajiTagTextBox(lyric, textTag);

        protected override void AddTextTag(RomajiTag textTag)
            => romajiTagsChangeHandler.Add(textTag);

        protected override LocalisableString CreateNewTextTagButtonText()
            => "Create new romaji";

        protected override LocalisableString CreateNewTextTagTitle()
            => "Romaji";

        protected override LocalisableString CreateNewTextTagDescription()
            => "Please enter the romaji.";

        protected override LockLyricPropertyBy? IsWriteLyricPropertyLocked(Lyric lyric)
            => HitObjectWritableUtils.GetLyricPropertyLockedBy(lyric, nameof(Lyric.RomajiTags));

        protected override LocalisableString GetWriteLyricPropertyLockedDescription(LockLyricPropertyBy lockLyricPropertyBy) =>
            lockLyricPropertyBy switch
            {
                LockLyricPropertyBy.ReferenceLyricConfig => "Romaji is sync to another romaji.",
                LockLyricPropertyBy.LockState => "Romaji is locked.",
                _ => throw new ArgumentOutOfRangeException(nameof(lockLyricPropertyBy), lockLyricPropertyBy, null)
            };

        protected override LocalisableString GetWriteLyricPropertyLockedTooltip(LockLyricPropertyBy lockLyricPropertyBy) =>
            lockLyricPropertyBy switch
            {
                LockLyricPropertyBy.ReferenceLyricConfig => "Cannot edit the romaji because it's sync to another lyric's text.",
                LockLyricPropertyBy.LockState => "The lyric is locked, so cannot edit the romaji.",
                _ => throw new ArgumentOutOfRangeException(nameof(lockLyricPropertyBy), lockLyricPropertyBy, null)
            };

        protected class LabelledRomajiTagTextBox : LabelledTextTagTextBox<RomajiTag>
        {
            [Resolved]
            private ILyricRomajiTagsChangeHandler romajiTagsChangeHandler { get; set; }

            public LabelledRomajiTagTextBox(Lyric lyric, RomajiTag textTag)
                : base(lyric, textTag)
            {
                Debug.Assert(lyric.RomajiTags.Contains(textTag));
            }

            protected override void ApplyValue(RomajiTag item, string value)
                => romajiTagsChangeHandler.SetText(item, value);

            protected override void SetIndex(RomajiTag item, int? startIndex, int? endIndex)
                => romajiTagsChangeHandler.SetIndex(item, startIndex, endIndex);

            protected override void RemoveTextTag(RomajiTag textTag)
                => romajiTagsChangeHandler.Remove(textTag);

            [BackgroundDependencyLoader]
            private void load(IEditRomajiModeState editRomajiModeState)
            {
                SelectedItems.BindTo(editRomajiModeState.SelectedItems);
            }
        }
    }
}
