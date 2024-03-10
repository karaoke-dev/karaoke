// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Graphics;
using osu.Framework.Localisation;
using osu.Game.Rulesets.Karaoke.Edit.ChangeHandlers.Lyrics;
using osu.Game.Rulesets.Karaoke.Edit.Utils;
using osu.Game.Rulesets.Karaoke.Extensions;
using osu.Game.Rulesets.Karaoke.Objects;
using osu.Game.Rulesets.Karaoke.Screens.Edit.Beatmaps.Lyrics.States.Modes;

namespace osu.Game.Rulesets.Karaoke.Screens.Edit.Beatmaps.Lyrics.Settings.Romanisation;

public partial class RomanisationEditSection : LyricPropertiesSection<TimeTag>
{
    protected override LocalisableString Title => "Romanisation";

    protected override LyricPropertiesEditor CreateLyricPropertiesEditor() => new RomanisationTagsEditor();

    protected override LockLyricPropertyBy? IsWriteLyricPropertyLocked(Lyric lyric)
        => HitObjectWritableUtils.GetLyricPropertyLockedBy(lyric, nameof(Lyric.TimeTags));

    protected override LocalisableString GetWriteLyricPropertyLockedDescription(LockLyricPropertyBy lockLyricPropertyBy) =>
        lockLyricPropertyBy switch
        {
            LockLyricPropertyBy.ReferenceLyricConfig => "Romanisation is sync to another romanisation.",
            LockLyricPropertyBy.LockState => "Romanisation is locked.",
            _ => throw new ArgumentOutOfRangeException(nameof(lockLyricPropertyBy), lockLyricPropertyBy, null),
        };

    protected override LocalisableString GetWriteLyricPropertyLockedTooltip(LockLyricPropertyBy lockLyricPropertyBy) =>
        lockLyricPropertyBy switch
        {
            LockLyricPropertyBy.ReferenceLyricConfig => "Cannot edit the romanisation because it's sync to another lyric's text.",
            LockLyricPropertyBy.LockState => "The lyric is locked, so cannot edit the romanisation.",
            _ => throw new ArgumentOutOfRangeException(nameof(lockLyricPropertyBy), lockLyricPropertyBy, null),
        };

    private partial class RomanisationTagsEditor : LyricPropertiesEditor
    {
        private readonly Bindable<RomanisationEditPropertyMode> bindableRomanisationEditPropertyMode = new();

        public RomanisationTagsEditor()
        {
            bindableRomanisationEditPropertyMode.BindValueChanged(e =>
            {
                RedrewContent();
            });
        }

        [BackgroundDependencyLoader]
        private void load(IEditRomanisationModeState editRomanisationModeState)
        {
            bindableRomanisationEditPropertyMode.BindTo(editRomanisationModeState.BindableRomanisationEditPropertyMode);
        }

        protected override Drawable CreateDrawable(TimeTag item)
        {
            int index = Items.IndexOf(item);
            return bindableRomanisationEditPropertyMode.Value switch
            {
                RomanisationEditPropertyMode.Text => new LabelledRomanisedSyllableTextBox(item)
                {
                    Label = $"#{index + 1}",
                    TabbableContentContainer = this,
                },
                RomanisationEditPropertyMode.Initial => new LabelledFirstSyllableSwitchButton(item)
                {
                    Label = item.RomanisedSyllable ?? string.Empty,
                },
                _ => throw new ArgumentOutOfRangeException(nameof(bindableRomanisationEditPropertyMode.Value)),
            };
        }

        protected override EditorSectionButton? CreateCreateNewItemButton() => null;

        protected override IBindableList<TimeTag> GetItems(Lyric lyric)
            => lyric.TimeTagsBindable;
    }

    private partial class LabelledRomanisedSyllableTextBox : LabelledObjectFieldTextBox<TimeTag>
    {
        [Resolved]
        private ILyricTimeTagsChangeHandler lyricTimeTagsChangeHandler { get; set; } = null!;

        [Resolved]
        private IEditRomanisationModeState editRomanisationModeState { get; set; } = null!;

        public LabelledRomanisedSyllableTextBox(TimeTag item)
            : base(item)
        {
        }

        protected override void TriggerSelect(TimeTag item)
            => editRomanisationModeState.Select(item);

        protected override string GetFieldValue(TimeTag timeTag)
            => timeTag.RomanisedSyllable ?? string.Empty;

        protected override void ApplyValue(TimeTag timeTag, string value)
            => lyricTimeTagsChangeHandler.SetTimeTagRomanisedSyllable(timeTag, value);

        [BackgroundDependencyLoader]
        private void load()
        {
            SelectedItems.BindTo(editRomanisationModeState.SelectedItems);
        }
    }

    private partial class LabelledFirstSyllableSwitchButton : LabelledObjectFieldSwitchButton<TimeTag>
    {
        [Resolved]
        private ILyricTimeTagsChangeHandler lyricTimeTagsChangeHandler { get; set; } = null!;

        public LabelledFirstSyllableSwitchButton(TimeTag item)
            : base(item)
        {
        }

        protected override bool GetFieldValue(TimeTag timeTag)
            => timeTag.FirstSyllable;

        protected override void ApplyValue(TimeTag timeTag, bool value)
            => lyricTimeTagsChangeHandler.SetTimeTagFirstSyllable(timeTag, value);

        [BackgroundDependencyLoader]
        private void load(IEditRomanisationModeState editRomanisationModeState)
        {
            SelectedItems.BindTo(editRomanisationModeState.SelectedItems);
        }
    }
}
