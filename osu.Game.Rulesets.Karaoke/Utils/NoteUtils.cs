// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Game.Rulesets.Karaoke.Objects;
using System;

namespace osu.Game.Rulesets.Karaoke.Utils
{
    public static class NoteUtils
    {
        public static Note SliceNote(Note note, double startPercentage,  double durationPercentage)
        {
            if (startPercentage < 0 || startPercentage + durationPercentage > 1)
                throw new ArgumentOutOfRangeException($"{nameof(Note)} cannot assign split range of start from {startPercentage} and duration {durationPercentage}");

            var startTime = note.StartTime + note.Duration * startPercentage;
            var duration = note.Duration * durationPercentage;

            return copyByTime(note, startTime, duration);
        }
        public static Tuple<Note, Note> SplitNote(Note note, double percentage = 0.5)
        {
            if (percentage < 0 || percentage > 1)
                throw new ArgumentOutOfRangeException(nameof(Note));

            if (percentage == 0 || percentage == 1)
                throw new InvalidOperationException($"{nameof(percentage)} cannot be {0} or {1}.");

            var firstNoteStartTime = note.StartTime;
            var firstNoteDuration = note.Duration * percentage;

            var secondNoteStartTime = firstNoteStartTime + firstNoteDuration;
            var secondNoteDuration = note.Duration * (1 - percentage);

            var firstNote = copyByTime(note, firstNoteStartTime, firstNoteDuration);
            var secondNote = copyByTime(note, secondNoteStartTime, secondNoteDuration);

            return new Tuple<Note, Note>(firstNote, secondNote);
        }

        private static Note copyByTime(Note oritinNote, double startTime, double duration)
        {
            return new Note
            {
                StartTime = startTime,
                Duration = duration,
                StartIndex = oritinNote.StartIndex,
                EndIndex = oritinNote.EndIndex,
                Text = oritinNote.Text,
                Singers = oritinNote.Singers?.Clone() as int[],
                Display = oritinNote.Display,
                Tone = oritinNote.Tone,
                ParentLyric = oritinNote.ParentLyric
            };
        }

        public static Note CombineNote(Note firstLyric, Note secondLyric)
        {
            return null;
        }
    }
}
