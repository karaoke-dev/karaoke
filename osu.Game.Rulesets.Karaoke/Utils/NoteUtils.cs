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

            return CopyByTime(note, startTime, duration);
        }

        public static Note CopyByTime(Note originNote, double startTime, double duration)
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
