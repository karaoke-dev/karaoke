// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using System.Linq;
using osu.Framework.Allocation;
using osu.Game.Rulesets.Karaoke.Configuration;
using osu.Game.Rulesets.Karaoke.Edit.Generator.Notes;
using osu.Game.Rulesets.Karaoke.Objects;
using osu.Game.Rulesets.Karaoke.Utils;
using osu.Game.Screens.Edit;

namespace osu.Game.Rulesets.Karaoke.Edit.ChangeHandlers.Notes
{
    public class NotesChangeHandler : HitObjectChangeHandler<Note>, INotesChangeHandler
    {
        [Resolved]
        private EditorBeatmap beatmap { get; set; }

        [Resolved]
        private KaraokeRulesetEditGeneratorConfigManager generatorConfigManager { get; set; }

        public void AutoGenerate()
        {
            var config = generatorConfigManager.Get<NoteGeneratorConfig>(KaraokeRulesetEditGeneratorSetting.NoteGeneratorConfig);
            var generator = new NoteGenerator(config);

            PerformOnSelection(lyric =>
            {
                // clear exist notes if from those
                var matchedNotes = HitObjects.Where(x => x.ParentLyric == lyric).ToArray();
                RemoveRange(matchedNotes);

                var notes = generator.CreateNotes(lyric);
                AddRange(notes);
            });
        }

        public void ChangeDisplay(bool display)
        {
            PerformOnSelection(note =>
            {
                note.Display = display;

                // Move to center if note is not display
                if (!note.Display)
                    note.Tone = new Tone();
            });
        }

        public void Split(float percentage = 0.5f)
        {
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
            PerformOnSelection(lyric =>
            {
                var notes = beatmap.SelectedHitObjects.OfType<Note>().Where(n => n.ParentLyric == lyric).ToList();

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

        protected void PerformOnSelection(Action<Lyric> action) => beatmap.PerformOnSelection(h =>
        {
            if (h is Lyric lyric)
                action.Invoke(lyric);
        });
    }
}
