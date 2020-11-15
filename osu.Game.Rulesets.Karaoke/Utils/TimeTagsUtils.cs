// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using Microsoft.EntityFrameworkCore.Internal;
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
            return timeTags?.OrderBy(x => x.Item1)
                           .ThenBy(x => x.Item2).ToArray();
        }

        /// <summary>
        /// Find invalid time tags.
        /// </summary>
        /// <param name="timeTags">Time tags</param>
        /// <returns>List of invalid time tags</returns>
        public static Tuple<TimeTagIndex, double?>[] FindInvalid(Tuple<TimeTagIndex, double?>[] timeTags, GroupCheck other = GroupCheck.Asc, SelfCheck self = SelfCheck.BasedOnStart)
        {
            var sortedTimeTags = Sort(timeTags);
            var groupedTimeTags = sortedTimeTags.GroupBy(x => x.Item1.Index);

            var invalidList = new List<Tuple<TimeTagIndex, double?>>();
            foreach (var groupedTimeTag in groupedTimeTags)
            {
                var startTimeGroup = groupedTimeTag.Where(x => x.Item1.State == TimeTagIndex.IndexState.Start && x.Item2 != null);
                var endTimeGroup = groupedTimeTag.Where(x => x.Item1.State == TimeTagIndex.IndexState.End && x.Item2 != null);

                // add invalid group into list.
                var groupInvalid = findGroupInvalid();
                if (groupInvalid != null)
                    invalidList.AddRange(groupInvalid);

                // add invalid self into list.
                var selfInvalid = findSelfInvalid();
                if (selfInvalid != null)
                    invalidList.AddRange(selfInvalid);

                List<Tuple<TimeTagIndex, double?>> findGroupInvalid()
                {
                    switch (other)
                    {
                        case GroupCheck.Asc:
                            // mark next is invalid if smaller then self
                            var groupMaxTime = groupedTimeTag.Max(x => x.Item2);
                            if (groupMaxTime == null)
                                return null;

                            return sortedTimeTags.Where(x => x.Item1.Index > groupedTimeTag.Key && x.Item2 < groupMaxTime).ToList();

                        case GroupCheck.Desc:
                            // mark pervious is invalid if larger then self
                            var groupMinTime = groupedTimeTag.Min(x => x.Item2);
                            if (groupMinTime == null)
                                return null;

                            return sortedTimeTags.Where(x => x.Item1.Index < groupedTimeTag.Key && x.Item2 > groupMinTime).ToList();

                        default:
                            return null;
                    }
                }

                List<Tuple<TimeTagIndex, double?>> findSelfInvalid()
                {
                    switch (self)
                    {
                        case SelfCheck.BasedOnStart:
                            var maxStartTime = startTimeGroup.Max(x => x.Item2);
                            if (maxStartTime == null)
                                return null;
                            return endTimeGroup.Where(x => x.Item2.Value < maxStartTime.Value).ToList();

                        case SelfCheck.BasedOnEnd:
                            var minEndTime = endTimeGroup.Min(x => x.Item2);
                            if (minEndTime == null)
                                return null;
                            return startTimeGroup.Where(x => x.Item2.Value > minEndTime.Value).ToList();

                        default:
                            return null;
                    }
                }
            }

            return Sort(invalidList.Distinct().ToArray());
        }

        /// <summary>
        /// Auto fix invalid time tags.
        /// </summary>
        /// <param name="timeTags">Time tags</param>
        /// <param name="fixWay">Fix way</param>
        /// <returns>Fixed time tags.</returns>
        public static Tuple<TimeTagIndex, double?>[] FixInvalid(Tuple<TimeTagIndex, double?>[] timeTags, GroupCheck other = GroupCheck.Asc, SelfCheck self = SelfCheck.BasedOnStart)
        {
            var sortedTimeTags = Sort(timeTags);
            var groupedTimeTags = sortedTimeTags.GroupBy(x => x.Item1.Index);

            var invalidTimeTags = FindInvalid(timeTags, other, self);
            var validTimeTags = sortedTimeTags.Except(invalidTimeTags);

            foreach (var invalidTimeTag in invalidTimeTags)
            {
                var listIndex = sortedTimeTags.IndexOf(invalidTimeTag);
                var timeTag = invalidTimeTag.Item1;

                // fix self-invalid
                var groupedTimeTag = groupedTimeTags.FirstOrDefault(x => x.Key == timeTag.Index).ToList();
                var startTimeGroup = groupedTimeTag.Where(x => x.Item1.State == TimeTagIndex.IndexState.Start && x.Item2 != null);
                var endTimeGroup = groupedTimeTag.Where(x => x.Item1.State == TimeTagIndex.IndexState.End && x.Item2 != null);
                switch (timeTag.State)
                {
                    case TimeTagIndex.IndexState.Start:
                        var minEndTime = endTimeGroup.Min(x => x.Item2);
                        if (minEndTime != null && minEndTime < invalidTimeTag.Item2)
                        {
                            sortedTimeTags[listIndex] = new Tuple<TimeTagIndex, double?>(timeTag, minEndTime);
                            continue;
                        }
                            
                        break;

                    case TimeTagIndex.IndexState.End:
                        var maxStartTime = startTimeGroup.Max(x => x.Item2);
                        if (maxStartTime != null && maxStartTime > invalidTimeTag.Item2)
                        {
                            sortedTimeTags[listIndex] = new Tuple<TimeTagIndex, double?>(timeTag, maxStartTime);
                            continue;
                        }
                        break;
                }

                // fix pervious or next value to apply
                switch (other)
                {
                    case GroupCheck.Asc:
                        // find perviouls valiue to apply.
                        var perviousValidValue = sortedTimeTags.Reverse().FirstOrDefault(x => x.Item1.Index < timeTag.Index && x.Item2 != null)?.Item2;
                        sortedTimeTags[listIndex] = new Tuple<TimeTagIndex, double?>(timeTag, perviousValidValue);
                        break;

                    case GroupCheck.Desc:
                        // find next value to apply.
                        var nextValidValue = sortedTimeTags.FirstOrDefault(x => x.Item1.Index > timeTag.Index && x.Item2 != null)?.Item2;
                        sortedTimeTags[listIndex] = new Tuple<TimeTagIndex, double?>(timeTag, nextValidValue);
                        break;
                }
            }

            return sortedTimeTags;
        }

        /// <summary>
        /// Convert list of time tag to dictionary.
        /// </summary>
        /// <param name="timeTags">Time tags</param>
        /// <param name="applyFix">Should auto-fix or not</param>
        /// <returns>Time tags with dictionary format.</returns>
        public static IReadOnlyDictionary<TimeTagIndex, double> ToDictionary(Tuple<TimeTagIndex, double?>[] timeTags, bool applyFix = true, GroupCheck other = GroupCheck.Asc, SelfCheck self = SelfCheck.BasedOnStart)
        {
            // sorted value
            var sortedTimeTags = applyFix ? FixInvalid(timeTags, other, self) : Sort(timeTags);

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

    public enum GroupCheck
    {
        /// <summary>
        /// Mark next time tag is error if conflict.
        /// </summary>
        Asc,

        /// <summary>
        /// Mark pervious tag is error if conflict.
        /// </summary>
        Desc
    }

    public enum SelfCheck
    {
        /// <summary>
        /// Mark end time tag is error if conflict.
        /// </summary>
        BasedOnStart,

        /// <summary>
        /// Mark start time tag is error if conflict.
        /// </summary>
        BasedOnEnd,
    }
}
