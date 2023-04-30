// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

#nullable disable

using System;
using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Graphics;
using osu.Framework.Localisation;
using osu.Game.Rulesets.Karaoke.Edit.ChangeHandlers.Notes;
using osu.Game.Rulesets.Karaoke.Edit.Utils;
using osu.Game.Rulesets.Karaoke.Extensions;
using osu.Game.Rulesets.Karaoke.Objects;
using osu.Game.Rulesets.Karaoke.Screens.Edit.Beatmaps.Lyrics.States.Modes;
using osu.Game.Screens.Edit;

namespace osu.Game.Rulesets.Karaoke.Screens.Edit.Beatmaps.Lyrics.Settings.Notes;

public partial class NoteEditPropertySection : LyricPropertiesSection<Note>
{
    protected override LocalisableString Title => "Properties";

    protected override LyricPropertiesEditor CreateLyricPropertiesEditor() => new NotePropertiesEditor();

    protected override LockLyricPropertyBy? IsWriteLyricPropertyLocked(Lyric lyric)
        => HitObjectWritableUtils.GetCreateOrRemoveNoteLockedBy(lyric); //todo: should reference by another utils.

    protected override LocalisableString GetWriteLyricPropertyLockedDescription(LockLyricPropertyBy lockLyricPropertyBy) =>
        lockLyricPropertyBy switch
        {
            LockLyricPropertyBy.ReferenceLyricConfig => "Notes is sync to another notes.",
            LockLyricPropertyBy.LockState => "Notes is locked.",
            _ => throw new ArgumentOutOfRangeException(nameof(lockLyricPropertyBy), lockLyricPropertyBy, null)
        };

    protected override LocalisableString GetWriteLyricPropertyLockedTooltip(LockLyricPropertyBy lockLyricPropertyBy) =>
        lockLyricPropertyBy switch
        {
            LockLyricPropertyBy.ReferenceLyricConfig => "Cannot edit the notes because it's sync to another lyric's notes.",
            LockLyricPropertyBy.LockState => "The lyric is locked, so cannot edit the note.",
            _ => throw new ArgumentOutOfRangeException(nameof(lockLyricPropertyBy), lockLyricPropertyBy, null)
        };

    private partial class NotePropertiesEditor : LyricPropertiesEditor
    {
        private readonly Bindable<NoteEditPropertyMode> bindableNoteEditPropertyMode = new();

        [Resolved]
        private EditorBeatmap beatmap { get; set; }

        public NotePropertiesEditor()
        {
            bindableNoteEditPropertyMode.BindValueChanged(e =>
            {
                RedrewContent();
            });
        }

        [BackgroundDependencyLoader]
        private void load(IEditNoteModeState editNoteModeState)
        {
            bindableNoteEditPropertyMode.BindTo(editNoteModeState.NoteEditPropertyMode);
        }

        protected override Drawable CreateDrawable(Note item)
        {
            // todo: deal with create or remove the notes.
            int index = Items.IndexOf(item);
            return bindableNoteEditPropertyMode.Value switch
            {
                NoteEditPropertyMode.Text => new LabelledNoteTextTextBox(item)
                {
                    Label = $"#{index + 1}",
                    TabbableContentContainer = this
                },
                NoteEditPropertyMode.RubyText => new LabelledNoteRubyTextTextBox(item)
                {
                    Label = item.Text,
                    TabbableContentContainer = this
                },
                NoteEditPropertyMode.Display => new LabelledNoteDisplaySwitchButton(item)
                {
                    Label = item.Text,
                },
                _ => throw new ArgumentOutOfRangeException(nameof(bindableNoteEditPropertyMode.Value))
            };
        }

        protected override EditorSectionButton CreateCreateNewItemButton() => null;

        protected override IBindableList<Note> GetItems(Lyric lyric)
        {
            var notes = EditorBeatmapUtils.GetNotesByLyric(beatmap, lyric);
            return new BindableList<Note>(notes);
        }
    }

    private partial class LabelledNoteTextTextBox : LabelledObjectFieldTextBox<Note>
    {
        [Resolved]
        private INotePropertyChangeHandler notePropertyChangeHandler { get; set; }

        [Resolved]
        private IEditNoteModeState editNoteModeState { get; set; }

        public LabelledNoteTextTextBox(Note item)
            : base(item)
        {
        }

        protected override void TriggerSelect(Note item)
            => editNoteModeState.Select(item);

        protected override string GetFieldValue(Note note)
            => note.Text;

        protected override void ApplyValue(Note note, string value)
            => notePropertyChangeHandler.ChangeText(value);

        [BackgroundDependencyLoader]
        private void load()
        {
            SelectedItems.BindTo(editNoteModeState.SelectedItems);
        }
    }

    private partial class LabelledNoteRubyTextTextBox : LabelledObjectFieldTextBox<Note>
    {
        [Resolved]
        private INotePropertyChangeHandler notePropertyChangeHandler { get; set; }

        [Resolved]
        private IEditNoteModeState editNoteModeState { get; set; }

        public LabelledNoteRubyTextTextBox(Note item)
            : base(item)
        {
        }

        protected override void TriggerSelect(Note item)
            => editNoteModeState.Select(item);

        protected override string GetFieldValue(Note note)
            => note.RubyText;

        protected override void ApplyValue(Note note, string value)
            => notePropertyChangeHandler.ChangeRubyText(value);

        [BackgroundDependencyLoader]
        private void load()
        {
            SelectedItems.BindTo(editNoteModeState.SelectedItems);
        }
    }

    private partial class LabelledNoteDisplaySwitchButton : LabelledObjectFieldSwitchButton<Note>
    {
        [Resolved]
        private INotePropertyChangeHandler notePropertyChangeHandler { get; set; }

        public LabelledNoteDisplaySwitchButton(Note item)
            : base(item)
        {
        }

        protected override bool GetFieldValue(Note note)
            => note.Display;

        protected override void ApplyValue(Note note, bool value)
            => notePropertyChangeHandler.ChangeDisplayState(value);

        [BackgroundDependencyLoader]
        private void load(IEditNoteModeState editNoteModeState)
        {
            SelectedItems.BindTo(editNoteModeState.SelectedItems);
        }
    }
}
