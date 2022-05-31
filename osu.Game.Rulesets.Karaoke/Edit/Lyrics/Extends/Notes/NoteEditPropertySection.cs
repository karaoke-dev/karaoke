// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using System.Linq;
using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Graphics;
using osu.Game.Graphics.UserInterfaceV2;
using osu.Game.Rulesets.Karaoke.Edit.ChangeHandlers.Notes;
using osu.Game.Rulesets.Karaoke.Edit.Components.Containers;
using osu.Game.Rulesets.Karaoke.Edit.Lyrics.CaretPosition;
using osu.Game.Rulesets.Karaoke.Edit.Lyrics.Extends.Components;
using osu.Game.Rulesets.Karaoke.Edit.Lyrics.States;
using osu.Game.Rulesets.Karaoke.Edit.Lyrics.States.Modes;
using osu.Game.Rulesets.Karaoke.Objects;
using osu.Game.Screens.Edit;

namespace osu.Game.Rulesets.Karaoke.Edit.Lyrics.Extends.Notes
{
    public class NoteEditPropertySection : Section
    {
        protected override string Title => "Properties";

        private readonly Bindable<Note[]> notes = new();
        private readonly Bindable<NoteEditPropertyMode> bindableNoteEditPropertyMode = new();
        private readonly IBindable<ICaretPosition> bindableCaretPosition = new Bindable<ICaretPosition>();

        public NoteEditPropertySection()
        {
            bindableNoteEditPropertyMode.BindValueChanged(e =>
            {
                reCreateEditComponents();
            });

            notes.BindValueChanged(e =>
            {
                reCreateEditComponents();
            });

            void reCreateEditComponents()
            {
                RemoveAll(x => x is LabelledObjectFieldTextBox<Note>);
                RemoveAll(x => x is LabelledSwitchButton);

                if (notes.Value == null)
                    return;

                AddRange(notes.Value.Select(x =>
                {
                    int index = Array.IndexOf(notes.Value, x);
                    return bindableNoteEditPropertyMode.Value switch
                    {
                        NoteEditPropertyMode.Text => new LabelledNoteTextTextBox(x)
                        {
                            Label = $"#{index + 1}",
                            TabbableContentContainer = this
                        } as Drawable,
                        NoteEditPropertyMode.RubyText => new LabelledNoteRubyTextTextBox(x)
                        {
                            Label = x.Text,
                            TabbableContentContainer = this
                        },
                        NoteEditPropertyMode.Display => new LabelledNoteDisplaySwitchButton(x)
                        {
                            Label = x.Text,
                        },
                        _ => throw new ArgumentOutOfRangeException(nameof(bindableNoteEditPropertyMode.Value))
                    };
                }));
            }
        }

        [BackgroundDependencyLoader]
        private void load(EditorBeatmap beatmap, ILyricCaretState lyricCaretState, IEditNoteModeState editNoteModeState)
        {
            bindableNoteEditPropertyMode.BindTo(editNoteModeState.NoteEditPropertyMode);
            bindableCaretPosition.BindTo(lyricCaretState.BindableCaretPosition);

            bindableCaretPosition.BindValueChanged(e =>
            {
                var lyric = e.NewValue?.Lyric;
                notes.Value = beatmap.HitObjects.OfType<Note>().Where(x => x.ParentLyric == lyric).ToArray();
            }, true);
        }

        private class LabelledNoteTextTextBox : LabelledObjectFieldTextBox<Note>
        {
            [Resolved]
            private INotesChangeHandler notesChangeHandler { get; set; }

            public LabelledNoteTextTextBox(Note item)
                : base(item)
            {
            }

            protected override string GetFieldValue(Note note)
                => note.Text;

            protected override void ApplyValue(Note note, string value)
                => notesChangeHandler.ChangeText(value);

            [BackgroundDependencyLoader]
            private void load(IEditNoteModeState editNoteModeState)
            {
                SelectedItems.BindTo(editNoteModeState.SelectedItems);
            }
        }

        private class LabelledNoteRubyTextTextBox : LabelledObjectFieldTextBox<Note>
        {
            [Resolved]
            private INotesChangeHandler notesChangeHandler { get; set; }

            public LabelledNoteRubyTextTextBox(Note item)
                : base(item)
            {
            }

            protected override string GetFieldValue(Note note)
                => note.RubyText;

            protected override void ApplyValue(Note note, string value)
                => notesChangeHandler.ChangeRubyText(value);

            [BackgroundDependencyLoader]
            private void load(IEditNoteModeState editNoteModeState)
            {
                SelectedItems.BindTo(editNoteModeState.SelectedItems);
            }
        }

        private class LabelledNoteDisplaySwitchButton : LabelledObjectFieldSwitchButton<Note>
        {
            [Resolved]
            private INotesChangeHandler notesChangeHandler { get; set; }

            public LabelledNoteDisplaySwitchButton(Note item)
                : base(item)
            {
            }

            protected override bool GetFieldValue(Note note)
                => note.Display;

            protected override void ApplyValue(Note note, bool value)
                => notesChangeHandler.ChangeDisplayState(value);

            [BackgroundDependencyLoader]
            private void load(IEditNoteModeState editNoteModeState)
            {
                SelectedItems.BindTo(editNoteModeState.SelectedItems);
            }
        }
    }
}
