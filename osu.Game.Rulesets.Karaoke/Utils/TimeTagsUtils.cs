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
        public static Tuple<TimeTagIndex, double?>[] Sort(Tuple<TimeTagIndex, double?>[] timeTags)
        {
            return timeTags?.OrderBy(x => x.Item1.Index)
                           .ThenByDescending(x => x.Item1.State)
                           .ThenBy(x => x.Item2).ToArray();
        }

        /// <summary>
        /// Find invalid time tags.
        /// </summary>
        /// <param name="timeTags">Time tags</param>
        /// <returns>List of invalid time tags</returns>
        public static Tuple<TimeTagIndex, double?>[] FindInvalid(Tuple<TimeTagIndex, double?>[] timeTags, FindWay other = FindWay.BasedOnStart, FindWay self = FindWay.BasedOnStart)
        {
            var sortedTimeTags = Sort(timeTags);
            var groupedTimeTags = sortedTimeTags.GroupBy(x => x.Item1.Index);

            foreach (var groupedTimeTag in groupedTimeTags)
            {
                // todo : find the time larger then normal time tag.
            }

            return new Tuple<TimeTagIndex, double?>[] { };
        }

        /// <summary>
        /// Auto fix invalid time tags.
        /// </summary>
        /// <param name="timeTags">Time tags</param>
        /// <param name="fixWay">Fix way</param>
        /// <returns>Fixed time tags.</returns>
        public static Tuple<TimeTagIndex, double?>[] FixInvalid(Tuple<TimeTagIndex, double?>[] timeTags, FixWay fixWay)
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
        /// Convert list of time tag to dictionary.
        /// </summary>
        /// <param name="timeTags">Time tags</param>
        /// <param name="applyFix">Should auto-fix or not</param>
        /// <returns>Time tags with dictionary format.</returns>
        public static IReadOnlyDictionary<TimeTagIndex, double> ToDictionary(Tuple<TimeTagIndex, double?>[] timeTags, bool applyFix = true)
        {
            // sorted value
            var sortedTimeTags = applyFix ? FixInvalid(timeTags, FixWay.Merge) : Sort(timeTags);

            // convert to dictionary, will get start's smallest time and end's largest time.
            return sortedTimeTags.Where(x => x.Item2 != null).GroupBy(x => x.Item1).Select(x =>
            {
                if (x.Key.State == TimeTagIndex.IndexState.Start)
                    return x.FirstOrDefault();
                else
                    return x.LastOrDefault();
            }).ToDictionary(k => k.Item1, v => v.Item2 ?? throw new ArgumentNullException("Dictionaty should not have null value"));
        }

        /// <summary>
        /// Convert dictionary to list of time tags.
        /// </summary>
        /// <param name="dictionary">Dictionary.</param>
        /// <returns>Time tagd</returns>
        public static Tuple<TimeTagIndex, double?>[] ToTimeTagList(IReadOnlyDictionary<TimeTagIndex, double> dictionary)
        {
            return dictionary.Select(d => Create(d.Key, d.Value)).ToArray();
        }

        /// <summary>
        /// Get start time.
        /// </summary>
        /// <param name="timeTags">Time tags</param>
        /// <returns>Start time</returns>
        public static double? GetStartTime(Tuple<TimeTagIndex, double?>[] timeTags)
        {
            return ToDictionary(timeTags).FirstOrDefault().Value;
        }

        /// <summary>
        /// Get End time.
        /// </summary>
        /// <param name="timeTags">Time tags</param>
        /// <returns>End time</returns>
        public static double? GetEndTime(Tuple<TimeTagIndex, double?>[] timeTags)
        {
            return ToDictionary(timeTags).LastOrDefault().Value;
        }

        public static Tuple<TimeTagIndex, double?> Create(TimeTagIndex index, double? time) => Tuple.Create(index, time);
    }

    public enum FindWay
    {
        BasedOnStart,

        BasedOnEnd,
    }

    public enum FixWay
    {
        BasedOnStart,

        BasedOnEnd,

        Merge,
    }
}
