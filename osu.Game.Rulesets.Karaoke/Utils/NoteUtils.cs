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

            double startTime = note.StartTime + note.Duration * startPercentage;
            double duration = note.Duration * durationPercentage;

            return CopyByTime(note, startTime, duration);
        }

        public static Note CopyByTime(Note originNote, double startTime, double duration) =>
            new()
            {
                StartTime = startTime,
                Duration = duration,
                StartIndex = originNote.StartIndex,
                EndIndex = originNote.EndIndex,
                Text = originNote.Text,
                Display = originNote.Display,
                Tone = originNote.Tone,
                ParentLyric = originNote.ParentLyric
            };

        /// <summary>
        /// Get the display text while gameplay or in editor.
        /// </summary>
        /// <param name="note">Note</param>
        /// <param name="useRubyTextIfHave">Should use ruby text first if have.</param>
        /// <returns>Text should be display.</returns>
        public static string DisplayText(Note note, bool useRubyTextIfHave = false)
        {
            if (!useRubyTextIfHave)
                return note.Text;

            return string.IsNullOrEmpty(note.RubyText) ? note.Text : note.RubyText;
        }
    }
}
