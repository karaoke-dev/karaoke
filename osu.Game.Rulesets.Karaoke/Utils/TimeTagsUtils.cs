// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Graphics.Sprites;
using System;
using System.Collections.Generic;
using System.Linq;

namespace osu.Game.Rulesets.Karaoke.Utils
{
    public static class TimeTagsUtils
    {
        /// <summary>
        /// Sort list of time tags by index and time.
        /// </summary>
        /// <param name="timeTags">Time tags</param>
        /// <returns>Sorted time tags</returns>
        public static List<Tuple<TimeTagIndex, double>> Sort(List<Tuple<TimeTagIndex, double>> timeTags)
        {
            return timeTags.OrderBy(x => x.Item1.Index).ThenByDescending(x => x.Item1.State).ThenBy(x => x.Item2).ToList();
        }

        /// <summary>
        /// Find invalid time tags.
        /// </summary>
        /// <param name="timeTags">Time tags</param>
        /// <returns>List of invalid time tags</returns>
        public static List<Tuple<TimeTagIndex, double>> FindInvalid(List<Tuple<TimeTagIndex, double>> timeTags)
        {
            var sortedTimeTags = Sort(timeTags);

            // todo : find the time larger then normal time tag.
            throw new Exception();
        }

        /// <summary>
        /// Auto fix invalid time tags.
        /// </summary>
        /// <param name="timeTags">Time tags</param>
        /// <param name="fixWay">Fix way</param>
        /// <returns>Fixed time tags.</returns>
        public static List<Tuple<TimeTagIndex, double>> FixInvalid(List<Tuple<TimeTagIndex, double>> timeTags, FixWay fixWay)
        {
            var sortedTimeTags = Sort(timeTags);
            var invalidTimeTags = FindInvalid(timeTags);

            foreach (var timetag in invalidTimeTags)
            {
                // todo : delete or delete with merge?
            }

            return sortedTimeTags;
        }

        /// <summary>
        /// Convert list of time tag to index.
        /// </summary>
        /// <param name="timeTags">Time tags</param>
        /// <param name="applyFix">Should auto-fix or not</param>
        /// <returns>Time tags with dictionary format.</returns>
        public static IReadOnlyDictionary<TimeTagIndex, double> ToDictionary(List<Tuple<TimeTagIndex, double>> timeTags, bool applyFix = true)
        {
            // sorted value
            var sortedTimeTags = applyFix ? FixInvalid(timeTags, FixWay.Merge) :  Sort(timeTags);

            // todo : convert to dictionary, will get start's smallest time and end's largest time.
            throw new Exception();

        }
    }

    public enum FixWay
    {
        RemoveSmaller,

        RemoveLarger,

        Merge
    }
}
