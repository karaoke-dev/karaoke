// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using osu.Game.Rulesets.Karaoke.Objects;

namespace osu.Game.Rulesets.Karaoke.Utils
{
    public static class NoteUtils
    {
        public static Note SliceNote(Note note, double startPercentage, double durationPercentage)
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

        public static Note CombineNote(Note firstLyric, Note secondLyric)
        {
            if (firstLyric.ParentLyric != secondLyric.ParentLyric)
                throw new InvalidOperationException($"{nameof(firstLyric.ParentLyric)} and {nameof(secondLyric.ParentLyric)} should be same.");

            if (firstLyric.StartIndex != secondLyric.StartIndex)
                throw new InvalidOperationException($"{nameof(firstLyric.StartIndex)} and {nameof(secondLyric.StartIndex)} should be same.");

            if (firstLyric.EndIndex != secondLyric.EndIndex)
                throw new InvalidOperationException($"{nameof(firstLyric.EndIndex)} and {nameof(secondLyric.EndIndex)} should be same.");

            var startTime = Math.Min(firstLyric.StartTime, secondLyric.StartTime);
            var endTime = Math.Max(firstLyric.EndTime, secondLyric.EndTime);

            return copyByTime(firstLyric, startTime, endTime - startTime);
        }

        private static Note copyByTime(Note originNote, double startTime, double duration)
        {
            return new Note
            {
                StartTime = startTime,
                Duration = duration,
                StartIndex = originNote.StartIndex,
                EndIndex = originNote.EndIndex,
                Text = originNote.Text,
                Singers = originNote.Singers?.Clone() as int[],
                Display = originNote.Display,
                Tone = originNote.Tone,
                ParentLyric = originNote.ParentLyric
            };
        }
    }
}
