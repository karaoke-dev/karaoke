// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using osu.Framework.Allocation;
using osu.Game.Rulesets.Karaoke.Objects;
using osu.Game.Rulesets.Karaoke.Utils;
using osu.Game.Screens.Edit;

namespace osu.Game.Rulesets.Karaoke.Edit.ChangeHandlers.Notes
{
    public class NotesChangeHandler : HitObjectsChangeHandler<Note>, INotesChangeHandler
    {
        [Resolved, AllowNull]
        private EditorBeatmap beatmap { get; set; }

        public void Split(float percentage = 0.5f)
        {
            CheckExactlySelectedOneHitObject();

            PerformOnSelection(note =>
            {
                var (firstNote, secondNote) = NotesUtils.SplitNote(note);
                Add(firstNote);
                Add(secondNote);
                Remove(note);
            });
        }

        public void Combine()
        {
            PerformOnSelection<Lyric>(lyric =>
            {
                var notes = beatmap.SelectedHitObjects.OfType<Note>().Where(n => n.ReferenceLyric == lyric).ToList();

                if (notes.Count < 2)
                    throw new InvalidOperationException($"Should have select at lest two {nameof(notes)}.");

                var combinedNote = NotesUtils.CombineNote(notes[0], notes[1]);

                for (int i = 2; i < notes.Count; i++)
                {
                    combinedNote = NotesUtils.CombineNote(notes[i - 1], notes[i]);
                }

                RemoveRange(notes);
                Add(combinedNote);
            });
        }

        public void Clear()
        {
            PerformOnSelection<Lyric>(lyric =>
            {
                var notes = beatmap.HitObjects.OfType<Note>().Where(n => n.ReferenceLyric == lyric).ToList();
                RemoveRange(notes);
            });
        }
    }
}
