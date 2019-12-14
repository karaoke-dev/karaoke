// Copyright (c) ppy Pty Ltd <contact@ppy.sh>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Game.Beatmaps;
using osu.Game.Rulesets.Karaoke.Objects;
using System.Collections.Generic;
using System.Linq;

namespace osu.Game.Rulesets.Karaoke.Beatmaps
{
    public class KaraokeBeatmapProcessor : BeatmapProcessor
    {
        public KaraokeBeatmapProcessor(IBeatmap beatmap)
            : base(beatmap)
        {
        }

        public override void PostProcess()
        {
            base.PostProcess();

            var lyrics = Beatmap.HitObjects.OfType<LyricLine>().ToList();

            if (!lyrics.Any())
                return;

            // re-arrange layout
            layoutArrangement(lyrics);
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
        ///      *****  (reft)        5
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
        /// <param name="lyrics"></param>
        /// <param name="bottomOnly"></param>
        private void layoutArrangement(IList<LyricLine> lyrics, bool bottomOnly = false)
        {
            // Force change to new line if lyric has long time
            const int new_lyric_line_time = 15000;
            const int number_of_line = 2;

            // Applay layout index
            for (int i = 0; i < lyrics.Count; i++)
            {
                var periousCycleLyric = (i >= number_of_line) ? lyrics[i - number_of_line] : null;
                var perviousLyric = (i >= 1) ? lyrics[i - 1] : null;
                var lyric = lyrics[i];

                // Force change layout
                if ((lyric.StartTime - perviousLyric?.EndTime) > new_lyric_line_time)
                    lyric.LayoutIndex = 1;
                // Change to next layout
                else if (perviousLyric?.LayoutIndex == 1)
                    lyric.LayoutIndex = 0;
                // Change to first layout index
                else
                    lyric.LayoutIndex = 1;
            }

            // Apply start time
            for (int i = 0; i < lyrics.Count; i++)
            {
                var lastLyricLine = i >= number_of_line ? lyrics[i - number_of_line] : null;
                var lyricLine = lyrics[i];

                if (lastLyricLine != null)
                {
                    // Adjust start time and end time
                    lyricLine.StartTime = lastLyricLine.EndTime + 1000;
                }
            }

            // TODO : Apply end time?
        }
    }
}
