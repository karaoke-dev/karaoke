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

namespace osu.Game.Rulesets.Karaoke.Screens.Edit.Beatmaps.Lyrics.Settings.Romaji;

public partial class RomajiEditSection : LyricPropertiesSection<TimeTag>
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

    private partial class RomajiTagsEditor : LyricPropertiesEditor
    {
        private readonly Bindable<RomajiEditPropertyMode> bindableRomajiEditPropertyMode = new();

        public RomajiTagsEditor()
        {
            bindableRomajiEditPropertyMode.BindValueChanged(e =>
            {
                RedrewContent();
            });
        }

        [BackgroundDependencyLoader]
        private void load(IEditRomajiModeState editRomajiModeState)
        {
            bindableRomajiEditPropertyMode.BindTo(editRomajiModeState.BindableRomajiEditPropertyMode);
        }

        protected override Drawable CreateDrawable(TimeTag item)
        {
            int index = Items.IndexOf(item);
            return bindableRomajiEditPropertyMode.Value switch
            {
                RomajiEditPropertyMode.Text => new LabelledRomajiTextTextBox(item)
                {
                    Label = $"#{index + 1}",
                    TabbableContentContainer = this,
                },
                RomajiEditPropertyMode.Initial => new LabelledInitialSwitchButton(item)
                {
                    Label = item.RomanizedSyllable ?? string.Empty,
                },
                _ => throw new ArgumentOutOfRangeException(nameof(bindableRomajiEditPropertyMode.Value)),
            };
        }

        protected override EditorSectionButton? CreateCreateNewItemButton() => null;

        protected override IBindableList<TimeTag> GetItems(Lyric lyric)
            => lyric.TimeTagsBindable;
    }

    private partial class LabelledRomajiTextTextBox : LabelledObjectFieldTextBox<TimeTag>
    {
        [Resolved]
        private ILyricTimeTagsChangeHandler lyricTimeTagsChangeHandler { get; set; } = null!;

        [Resolved]
        private IEditRomajiModeState editRomajiModeState { get; set; } = null!;

        public LabelledRomajiTextTextBox(TimeTag item)
            : base(item)
        {
        }

        protected override void TriggerSelect(TimeTag item)
            => editRomajiModeState.Select(item);

        protected override string GetFieldValue(TimeTag timeTag)
            => timeTag.RomanizedSyllable ?? string.Empty;

        protected override void ApplyValue(TimeTag timeTag, string value)
            => lyricTimeTagsChangeHandler.SetTimeTagRomajiText(timeTag, value);

        [BackgroundDependencyLoader]
        private void load()
        {
            SelectedItems.BindTo(editRomajiModeState.SelectedItems);
        }
    }

    private partial class LabelledInitialSwitchButton : LabelledObjectFieldSwitchButton<TimeTag>
    {
        [Resolved]
        private ILyricTimeTagsChangeHandler lyricTimeTagsChangeHandler { get; set; } = null!;

        public LabelledInitialSwitchButton(TimeTag item)
            : base(item)
        {
        }

        protected override bool GetFieldValue(TimeTag timeTag)
            => timeTag.FirstSyllable;

        protected override void ApplyValue(TimeTag timeTag, bool value)
            => lyricTimeTagsChangeHandler.SetTimeTagInitialRomaji(timeTag, value);

        [BackgroundDependencyLoader]
        private void load(IEditRomajiModeState editRomajiModeState)
        {
            SelectedItems.BindTo(editRomajiModeState.SelectedItems);
        }
    }
}
