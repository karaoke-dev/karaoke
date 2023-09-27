// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using System.Diagnostics;
using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Localisation;
using osu.Game.Rulesets.Karaoke.Edit.ChangeHandlers.Lyrics;
using osu.Game.Rulesets.Karaoke.Edit.Utils;
using osu.Game.Rulesets.Karaoke.Objects;
using osu.Game.Rulesets.Karaoke.Screens.Edit.Beatmaps.Lyrics.Settings.RubyRomaji;
using osu.Game.Rulesets.Karaoke.Screens.Edit.Beatmaps.Lyrics.Settings.RubyRomaji.Components;
using osu.Game.Rulesets.Karaoke.Screens.Edit.Beatmaps.Lyrics.States.Modes;

namespace osu.Game.Rulesets.Karaoke.Screens.Edit.Beatmaps.Lyrics.Settings.Romaji;

public partial class RomajiEditSection : TextTagEditSection<RomajiTag>
{
    protected override LocalisableString Title => "Romaji";

    protected override LyricPropertiesEditor CreateLyricPropertiesEditor() => new RomajiTagsEditor();

    protected override LockLyricPropertyBy? IsWriteLyricPropertyLocked(Lyric lyric)
        => HitObjectWritableUtils.GetLyricPropertyLockedBy(lyric, nameof(Lyric.RomajiTags));

    protected override LocalisableString GetWriteLyricPropertyLockedDescription(LockLyricPropertyBy lockLyricPropertyBy) =>
        lockLyricPropertyBy switch
        {
            LockLyricPropertyBy.ReferenceLyricConfig => "Romaji is sync to another romaji.",
            LockLyricPropertyBy.LockState => "Romaji is locked.",
            _ => throw new ArgumentOutOfRangeException(nameof(lockLyricPropertyBy), lockLyricPropertyBy, null),
        };

    protected override LocalisableString GetWriteLyricPropertyLockedTooltip(LockLyricPropertyBy lockLyricPropertyBy) =>
        lockLyricPropertyBy switch
        {
            LockLyricPropertyBy.ReferenceLyricConfig => "Cannot edit the romaji because it's sync to another lyric's text.",
            LockLyricPropertyBy.LockState => "The lyric is locked, so cannot edit the romaji.",
            _ => throw new ArgumentOutOfRangeException(nameof(lockLyricPropertyBy), lockLyricPropertyBy, null),
        };

    private partial class RomajiTagsEditor : TextTagsEditor
    {
        protected override IBindableList<RomajiTag> GetItems(Lyric lyric)
            => lyric.RomajiTagsBindable;

        protected override LabelledTextTagTextBox<RomajiTag> CreateLabelledTextTagTextBox(Lyric lyric, RomajiTag textTag)
            => new LabelledRomajiTagTextBox(lyric, textTag);
    }

    protected partial class LabelledRomajiTagTextBox : LabelledTextTagTextBox<RomajiTag>
    {
        [Resolved]
        private ILyricRomajiTagsChangeHandler romajiTagsChangeHandler { get; set; } = null!;

        [Resolved]
        private IEditRomajiModeState editRomajiModeState { get; set; } = null!;

        public LabelledRomajiTagTextBox(Lyric lyric, RomajiTag textTag)
            : base(lyric, textTag)
        {
            Debug.Assert(lyric.RomajiTags.Contains(textTag));
        }

        protected override void TriggerSelect(RomajiTag item)
            => editRomajiModeState.Select(item);

        protected override void ApplyValue(RomajiTag item, string value)
            => romajiTagsChangeHandler.SetText(item, value);

        protected override void SetIndex(RomajiTag item, int? startIndex, int? endIndex)
            => romajiTagsChangeHandler.SetIndex(item, startIndex, endIndex);

        protected override void RemoveTextTag(RomajiTag textTag)
            => romajiTagsChangeHandler.Remove(textTag);

        [BackgroundDependencyLoader]
        private void load()
        {
            SelectedItems.BindTo(editRomajiModeState.SelectedItems);
        }
    }
}
