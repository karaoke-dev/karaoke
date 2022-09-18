// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

#nullable disable

using System;
using System.Linq;
using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Graphics;
using osu.Framework.Localisation;
using osu.Game.Graphics.UserInterfaceV2;
using osu.Game.Rulesets.Karaoke.Edit.ChangeHandlers.Notes;
using osu.Game.Rulesets.Karaoke.Edit.Lyrics.Settings.Components;
using osu.Game.Rulesets.Karaoke.Edit.Lyrics.States.Modes;
using osu.Game.Rulesets.Karaoke.Edit.Utils;
using osu.Game.Rulesets.Karaoke.Objects;
using osu.Game.Screens.Edit;

namespace osu.Game.Rulesets.Karaoke.Edit.Lyrics.Settings.Notes
{
    public class NoteEditPropertySection : LyricPropertySection
    {
        protected override LocalisableString Title => "Properties";

        private readonly Bindable<Note[]> notes = new();
        private readonly Bindable<NoteEditPropertyMode> bindableNoteEditPropertyMode = new();

        [Resolved]
        private EditorBeatmap beatmap { get; set; }

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
                RemoveAll(x => x is LabelledObjectFieldTextBox<Note>, true);
                RemoveAll(x => x is LabelledSwitchButton, true);

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
        private void load(IEditNoteModeState editNoteModeState)
        {
            bindableNoteEditPropertyMode.BindTo(editNoteModeState.NoteEditPropertyMode);
        }

        protected override void OnLyricChanged(Lyric lyric)
        {
            notes.Value = EditorBeatmapUtils.GetNotesByLyric(beatmap, lyric).ToArray();
        }

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

        private class LabelledNoteTextTextBox : LabelledObjectFieldTextBox<Note>
        {
            [Resolved]
            private INotePropertyChangeHandler notePropertyChangeHandler { get; set; }

            public LabelledNoteTextTextBox(Note item)
                : base(item)
            {
            }

            protected override string GetFieldValue(Note note)
                => note.Text;

            protected override void ApplyValue(Note note, string value)
                => notePropertyChangeHandler.ChangeText(value);

            [BackgroundDependencyLoader]
            private void load(IEditNoteModeState editNoteModeState)
            {
                SelectedItems.BindTo(editNoteModeState.SelectedItems);
            }
        }

        private class LabelledNoteRubyTextTextBox : LabelledObjectFieldTextBox<Note>
        {
            [Resolved]
            private INotePropertyChangeHandler notePropertyChangeHandler { get; set; }

            public LabelledNoteRubyTextTextBox(Note item)
                : base(item)
            {
            }

            protected override string GetFieldValue(Note note)
                => note.RubyText;

            protected override void ApplyValue(Note note, string value)
                => notePropertyChangeHandler.ChangeRubyText(value);

            [BackgroundDependencyLoader]
            private void load(IEditNoteModeState editNoteModeState)
            {
                SelectedItems.BindTo(editNoteModeState.SelectedItems);
            }
        }

        private class LabelledNoteDisplaySwitchButton : LabelledObjectFieldSwitchButton<Note>
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
}
