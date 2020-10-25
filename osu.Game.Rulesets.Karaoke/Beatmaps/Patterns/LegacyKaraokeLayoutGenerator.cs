// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Collections.Generic;
using System.Linq;
using osu.Framework.Extensions.IEnumerableExtensions;
using osu.Game.Rulesets.Karaoke.Objects;

namespace osu.Game.Rulesets.Karaoke.Beatmaps.Patterns
{
    public class LegacyKaraokeLayoutGenerator : IPatternGenerator<Lyric>
    {
        private const int number_of_line = 2;

        public void Generate(IEnumerable<Lyric> hitObjects)
        {
            var lyrics = hitObjects.ToList();

            // Re-arrange layout
            assignLayoutArrangement(lyrics);

            // Re-assign time
            assignLyricTime(lyrics);
        }

        /// <summary>
        /// Calculate arrangement and assign layout number
        /// </summary>
        /// <example>
        ///    Lyric  | Anchor | LayoutIndex |
        /// ----------------------------------------
        /// ****        (left)        1
        ///      *****  (right)       0
        /// -----------
        /// *******     (left)        3
        ///  *****      (left)        4
        ///      *****  (left)        5
        /// -----------
        /// *******     (left)        6
        ///  *****      (left)        7
        ///      ****** (right)       8
        ///       ****  (right)       9
        /// -----------
        /// ******      (left)        10
        ///      ****** (right)       11
        /// ******      (left)        12
        ///      ****** (right)       13
        /// </example>
        /// <param name="lyrics">Lyrics</param>
        private void assignLayoutArrangement(IList<Lyric> lyrics)
        {
            // Force change to new line if lyric has long time
            const int new_lyric_line_time = 15000;

            // Apply layout index
            for (int i = 0; i < lyrics.Count; i++)
            {
                var previousCycleLyric = i >= number_of_line ? lyrics[i - number_of_line] : null;
                var previousLyric = i >= 1 ? lyrics[i - 1] : null;
                var lyric = lyrics[i];

                // Force change layout
                if (lyric.StartTime - previousLyric?.EndTime > new_lyric_line_time)
                    lyric.LayoutIndex = 1;
                // Change to next layout
                else if (previousLyric?.LayoutIndex == 1)
                    lyric.LayoutIndex = 0;
                // Change to first layout index
                else
                    lyric.LayoutIndex = 1;
            }
        }

        private void assignLyricTime(IList<Lyric> lyrics)
        {
            // Reset working time
            lyrics.ForEach(h => h.InitialWorkingTime());

            // Apply start time
            for (int i = 0; i < lyrics.Count; i++)
            {
                var lastLyricLine = i >= number_of_line ? lyrics[i - number_of_line] : null;
                var lyricLine = lyrics[i];

                if (lastLyricLine == null)
                    continue;

                // Adjust start time and end time
                var lyricEndTime = lyricLine.EndTime;
                lyricLine.StartTime = lastLyricLine.EndTime + 1000;

                // Should re-assign duration here
                lyricLine.Duration = lyricEndTime - lyricLine.StartTime;
            }
        }
    }
}
