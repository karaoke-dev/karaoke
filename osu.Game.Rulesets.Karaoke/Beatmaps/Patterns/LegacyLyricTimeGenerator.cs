// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Collections.Generic;
using System.Linq;
using osu.Framework.Extensions.IEnumerableExtensions;
using osu.Game.Rulesets.Karaoke.Objects;

namespace osu.Game.Rulesets.Karaoke.Beatmaps.Patterns
{
    /// <summary>
    /// Note: this class will be replaced by another lyric-time generator.
    /// </summary>
    public class LegacyLyricTimeGenerator : IPatternGenerator<Lyric>
    {
        private const int number_of_line = 2;

        public void Generate(IEnumerable<Lyric> hitObjects)
        {
            var lyrics = hitObjects.ToList();

            // Re-assign time
            assignLyricTime(lyrics);
        }

        private void assignLyricTime(IList<Lyric> lyrics)
        {
            // Reset working time
            lyrics.ForEach(h => h.InitialWorkingTime());

            // Apply start time
            for (int i = 0; i < lyrics.Count; i++)
            {
                var lastLyric = i >= number_of_line ? lyrics[i - number_of_line] : null;
                var lyric = lyrics[i];

                if (lastLyric == null)
                    continue;

                // Adjust start time and end time
                double lyricEndTime = lyric.EndTime;
                lyric.StartTime = lastLyric.EndTime + 1000;

                // Should re-assign duration here
                lyric.Duration = lyricEndTime - lyric.StartTime;
            }
        }
    }
}
