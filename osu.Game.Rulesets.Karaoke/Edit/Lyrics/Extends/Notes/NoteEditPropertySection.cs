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

        private readonly Bindable<Note[]> bindableNotes = new();

        [BackgroundDependencyLoader]
        private void load(EditorBeatmap beatmap, LyricCaretState lyricCaretState, Bindable<NoteEditPropertyMode> bindableNoteEditPropertyMode)
        {
            bindableNoteEditPropertyMode.BindValueChanged(e =>
            {
                reCreateEditComponents();
            });

            bindableNotes.BindValueChanged(e =>
            {
                reCreateEditComponents();
            });

            lyricCaretState.BindableCaretPosition.BindValueChanged(e =>
            {
                var lyric = e.NewValue?.Lyric;
                var notes = beatmap.HitObjects.OfType<Note>().Where(x => x.ParentLyric == lyric).ToList();
                bindableNotes.Value = notes.ToArray();
            }, true);

            void reCreateEditComponents()
            {
                RemoveAll(x => x is LabelledObjectFieldTextBox<Note>);
                RemoveAll(x => x is LabelledSwitchButton);
                AddRange(bindableNotes.Value?.Select(x =>
                {
                    var index = bindableNotes.Value.IndexOf(x);
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
        }
    }
}
