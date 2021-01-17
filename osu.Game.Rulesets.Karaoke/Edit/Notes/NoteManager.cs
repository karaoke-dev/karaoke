// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using System.Collections.Generic;
using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Game.Rulesets.Karaoke.Objects;
using osu.Game.Rulesets.Karaoke.Utils;
using osu.Game.Screens.Edit;

namespace osu.Game.Rulesets.Karaoke.Edit.Notes
{
    public class NoteManager : Component
    {
        [Resolved]
        private EditorBeatmap beatmap { get; set; }

        [Resolved(CanBeNull = true)]
        private IEditorChangeHandler changeHandler { get; set; }

        public void ChangeDisplay(Note note, bool display)
        {
            changeHandler.BeginChange();

            changeDisplay(note, display);

            changeHandler.EndChange();
        }

        public void ChangeDisplay(List<Note> notes, bool display)
        {
            changeHandler.BeginChange();

            foreach (var note in notes)
            {
                changeDisplay(note, display);
            }

            changeHandler.EndChange();
        }

        private void changeDisplay(Note note, bool display)
        {
            note.Display = display;

            // Move to center if note is not display
            if (!note.Display)
                note.Tone = new Tone();
        }

        public void SplitNote(Note note, float percentage = 0.5f)
        {
            var (firstNote, secondNote) = NotesUtils.SplitNote(note, 0.5);
            beatmap?.Add(firstNote);
            beatmap?.Add(secondNote);
            beatmap?.Remove(note);
        }
    }
}
