// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using System.Linq;
using Microsoft.EntityFrameworkCore.Internal;
using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Graphics;
using osu.Game.Graphics.UserInterfaceV2;
using osu.Game.Rulesets.Karaoke.Edit.Components.Containers;
using osu.Game.Rulesets.Karaoke.Edit.Lyrics.Extends.Components;
using osu.Game.Rulesets.Karaoke.Edit.Lyrics.States;
using osu.Game.Rulesets.Karaoke.Objects;
using osu.Game.Screens.Edit;

namespace osu.Game.Rulesets.Karaoke.Edit.Lyrics.Extends.Notes
{
    public class NoteEditPropertySection : Section
    {
        protected override string Title => "Properties";

        private readonly Bindable<Note[]> notes = new();

        [BackgroundDependencyLoader]
        private void load(EditorBeatmap beatmap, LyricCaretState lyricCaretState, Bindable<NoteEditPropertyMode> bindableNoteEditPropertyMode)
        {
            bindableNoteEditPropertyMode.BindValueChanged(e =>
            {
                // todo: use better way not to trigger bindable in disposed object.
                // this only happened in here.
                if (IsDisposed)
                    return;

                reCreateEditComponents();
            });

            notes.BindValueChanged(e =>
            {
                reCreateEditComponents();
            });

            lyricCaretState.BindableCaretPosition.BindValueChanged(e =>
            {
                var lyric = e.NewValue?.Lyric;
                notes.Value = beatmap.HitObjects.OfType<Note>().Where(x => x.ParentLyric == lyric).ToArray();
            }, true);

            void reCreateEditComponents()
            {
                RemoveAll(x => x is LabelledObjectFieldTextBox<Note>);
                RemoveAll(x => x is LabelledSwitchButton);
                AddRange(notes.Value?.Select(x =>
                {
                    var index = notes.Value.IndexOf(x);
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

        private class LabelledNoteTextTextBox : LabelledObjectFieldTextBox<Note>
        {
            public LabelledNoteTextTextBox(Note item)
                : base(item)
            {
            }

            protected override string GetFieldValue(Note note)
                => note.Text;

            protected override void ApplyValue(Note note, string value)
                => note.Text = value;

            [BackgroundDependencyLoader]
            private void load(BlueprintSelectionState blueprintSelectionState)
            {
                blueprintSelectionState.SelectedNotes.BindTo(SelectedItems);
            }
        }

        private class LabelledNoteRubyTextTextBox : LabelledObjectFieldTextBox<Note>
        {
            public LabelledNoteRubyTextTextBox(Note item)
                : base(item)
            {
            }

            protected override string GetFieldValue(Note note)
                => note.RubyText;

            protected override void ApplyValue(Note note, string value)
                => note.RubyText = value;

            [BackgroundDependencyLoader]
            private void load(BlueprintSelectionState blueprintSelectionState)
            {
                blueprintSelectionState.SelectedNotes.BindTo(SelectedItems);
            }
        }

        private class LabelledNoteDisplaySwitchButton : LabelledObjectFieldSwitchButton<Note>
        {
            public LabelledNoteDisplaySwitchButton(Note item)
                : base(item)
            {
            }

            protected override bool GetFieldValue(Note note)
                => note.Display;

            protected override void ApplyValue(Note note, bool value)
                => note.Display = value;

            [BackgroundDependencyLoader]
            private void load(BlueprintSelectionState blueprintSelectionState)
            {
                blueprintSelectionState.SelectedNotes.BindTo(SelectedItems);
            }
        }
    }
}
