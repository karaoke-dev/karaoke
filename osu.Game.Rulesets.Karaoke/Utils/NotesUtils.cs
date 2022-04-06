// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using osu.Game.Rulesets.Karaoke.Objects;

namespace osu.Game.Rulesets.Karaoke.Utils
{
    public static class NotesUtils
    {
        public static Tuple<Note, Note> SplitNote(Note note, double percentage = 0.5)
        {
            switch (percentage)
            {
                case < 0 or > 1:
                    throw new ArgumentOutOfRangeException(nameof(note));

                case 0 or 1:
                    throw new InvalidOperationException($"{nameof(percentage)} cannot be {0} or {1}.");
            }

            double firstNoteStartTime = note.StartTime;
            double firstNoteDuration = note.Duration * percentage;

            double secondNoteStartTime = firstNoteStartTime + firstNoteDuration;
            double secondNoteDuration = note.Duration * (1 - percentage);

            var firstNote = NoteUtils.CopyByTime(note, firstNoteStartTime, firstNoteDuration);
            var secondNote = NoteUtils.CopyByTime(note, secondNoteStartTime, secondNoteDuration);

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

            double startTime = Math.Min(firstLyric.StartTime, secondLyric.StartTime);
            double endTime = Math.Max(firstLyric.EndTime, secondLyric.EndTime);

            return NoteUtils.CopyByTime(firstLyric, startTime, endTime - startTime);
        }
    }
}
