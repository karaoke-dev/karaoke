// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Game.Rulesets.Karaoke.Objects;

namespace osu.Game.Rulesets.Karaoke.Utils
{
    public static class NoteUtils
    {
        public static Note CopyByTime(Note originNote, double startTime, double duration)
        {
            double fixedStartTime = originNote.StartTime - originNote.StartTimeOffset;
            double fixedEndTime = originNote.EndTime - originNote.EndTimeOffset;
            double endTime = startTime + duration;

            var note = originNote.DeepClone();
            note.StartTimeOffset = startTime - fixedStartTime;
            note.EndTimeOffset = endTime - fixedEndTime;

            return note;
        }

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
