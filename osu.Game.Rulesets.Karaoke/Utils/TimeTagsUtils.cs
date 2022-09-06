﻿// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using osu.Framework.Graphics.Sprites;
using osu.Game.Rulesets.Karaoke.Objects;

namespace osu.Game.Rulesets.Karaoke.Utils
{
    public static class TimeTagsUtils
    {
        /// <summary>
        /// Generate center time-tag with time.
        /// </summary>
        /// <param name="startTimeTag"></param>
        /// <param name="endTimeTag"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        public static TimeTag GenerateCenterTimeTag(TimeTag startTimeTag, TimeTag endTimeTag, TextIndex index)
        {
            if (startTimeTag == null)
                throw new ArgumentNullException(nameof(startTimeTag));

            if (endTimeTag == null)
                throw new ArgumentNullException(nameof(endTimeTag));

            if (startTimeTag.Index > endTimeTag.Index)
                throw new InvalidOperationException($"{nameof(endTimeTag.Index)} cannot larger than {startTimeTag.Index}");

            if (index < startTimeTag.Index || index > endTimeTag.Index)
                throw new InvalidOperationException($"{nameof(endTimeTag.Index)} cannot larger than {startTimeTag.Index}");

            if (startTimeTag.Time == null || endTimeTag.Time == null)
                return new TimeTag(index);

            int diffFromStartToEnd = getTimeCalculationIndex(endTimeTag.Index) - getTimeCalculationIndex(startTimeTag.Index);
            int diffFromStartToNow = getTimeCalculationIndex(index) - getTimeCalculationIndex(startTimeTag.Index);
            if (diffFromStartToEnd == 0 || diffFromStartToNow == 0)
                return new TimeTag(index, startTimeTag.Time);

            double? time = startTimeTag.Time +
                           (endTimeTag.Time - startTimeTag.Time)
                           / diffFromStartToEnd
                           * diffFromStartToNow;

            return new TimeTag(index, time);

            static int getTimeCalculationIndex(TextIndex calculationIndex)
                => TextIndexUtils.ToStringIndex(calculationIndex);
        }

        public static TimeTag GenerateCenterTimeTag(TimeTag startTimeTag, TimeTag endTimeTag, int index)
            => GenerateCenterTimeTag(startTimeTag, endTimeTag, new TextIndex(index));

        /// <summary>
        /// Sort list of time tags by index and time.
        /// </summary>
        /// <param name="timeTags">Time tags</param>
        /// <returns>Sorted time tags</returns>
        public static IEnumerable<TimeTag> Sort(IEnumerable<TimeTag> timeTags)
        {
            return timeTags.OrderBy(x => x.Index)
                           .ThenBy(x => x.Time);
        }

        /// <summary>
        /// Find out of range time-tag.
        /// </summary>
        /// <param name="timeTags"></param>
        /// <param name="lyric"></param>
        /// <returns></returns>
        public static TimeTag[] FindOutOfRange(IEnumerable<TimeTag> timeTags, string lyric)
        {
            return timeTags.Where(x => x.Index.Index < 0 || x.Index.Index >= lyric.Length).ToArray();
        }

        /// <summary>
        /// Find time-tag that has no time.
        /// </summary>
        /// <param name="timeTags"></param>
        /// <returns></returns>
        public static TimeTag[] FindNoneTime(IEnumerable<TimeTag> timeTags)
            => timeTags.Where(x => x.Time == null).ToArray();

        /// <summary>
        /// Check lyric has start time-tag
        /// </summary>
        /// <param name="timeTags"></param>
        /// <param name="lyric"></param>
        public static bool HasStartTimeTagInLyric(IEnumerable<TimeTag> timeTags, string lyric)
            => !string.IsNullOrEmpty(lyric) && timeTags.Any(x => x.Index.State == TextIndex.IndexState.Start && x.Index.Index == 0);

        /// <summary>
        /// Check lyric has end time-tag
        /// </summary>
        /// <param name="timeTags"></param>
        /// <param name="lyric"></param>
        public static bool HasEndTimeTagInLyric(IEnumerable<TimeTag> timeTags, string lyric)
            => timeTags.Any(x => x.Index.State == TextIndex.IndexState.End && x.Index.Index == lyric.Length - 1);

        /// <summary>
        /// Find overlapping time tags.
        /// </summary>
        /// <param name="timeTags">Time tags</param>
        /// <param name="other">Check way</param>
        /// <param name="self">Check way</param>
        /// <returns>List of overlapping time tags</returns>
        public static IList<TimeTag> FindOverlapping(IEnumerable<TimeTag> timeTags, GroupCheck other = GroupCheck.Asc, SelfCheck self = SelfCheck.BasedOnStart)
        {
            var sortedTimeTags = Sort(timeTags);
            var groupedTimeTags = sortedTimeTags.GroupBy(x => x.Index.Index);

            var overlappingTimeTagList = new List<TimeTag>();

            foreach (var groupedTimeTag in groupedTimeTags)
            {
                var startTimeGroup = groupedTimeTag.Where(x => x.Index.State == TextIndex.IndexState.Start && x.Time != null);
                var endTimeGroup = groupedTimeTag.Where(x => x.Index.State == TextIndex.IndexState.End && x.Time != null);

                // add overlapping group into list.
                var groupOverlapping = findGroupOverlapping();
                overlappingTimeTagList.AddRange(groupOverlapping);

                // add overlapping self into list.
                var selfOverlapping = findSelfOverlapping();
                overlappingTimeTagList.AddRange(selfOverlapping);

                IEnumerable<TimeTag> findGroupOverlapping()
                {
                    switch (other)
                    {
                        case GroupCheck.Asc:
                            // mark next is overlapping if smaller then self
                            double? groupMaxTime = groupedTimeTag.Max(x => x.Time);
                            if (groupMaxTime == null)
                                return Array.Empty<TimeTag>();

                            return sortedTimeTags.Where(x => x.Index.Index > groupedTimeTag.Key && x.Time < groupMaxTime).ToList();

                        case GroupCheck.Desc:
                            // mark previous is overlapping if larger then self
                            double? groupMinTime = groupedTimeTag.Min(x => x.Time);
                            if (groupMinTime == null)
                                return Array.Empty<TimeTag>();

                            return sortedTimeTags.Where(x => x.Index.Index < groupedTimeTag.Key && x.Time > groupMinTime).ToList();

                        default:
                            return Array.Empty<TimeTag>();
                    }
                }

                IEnumerable<TimeTag> findSelfOverlapping()
                {
                    switch (self)
                    {
                        case SelfCheck.BasedOnStart:
                            double? maxStartTime = startTimeGroup.Max(x => x.Time);
                            if (maxStartTime == null)
                                return Array.Empty<TimeTag>();

                            return endTimeGroup.Where(x => x.Time != null && x.Time.Value < maxStartTime.Value).ToList();

                        case SelfCheck.BasedOnEnd:
                            double? minEndTime = endTimeGroup.Min(x => x.Time);
                            if (minEndTime == null)
                                return Array.Empty<TimeTag>();

                            return startTimeGroup.Where(x => x.Time != null && x.Time.Value > minEndTime.Value).ToList();

                        default:
                            return Array.Empty<TimeTag>();
                    }
                }
            }

            return Sort(overlappingTimeTagList.Distinct()).ToArray();
        }

        /// <summary>
        /// Auto fix overlapping time tags.
        /// </summary>
        /// <param name="timeTags">Time tags</param>
        /// <param name="other">Fix way</param>
        /// <param name="self">Fix way</param>
        /// <returns>Fixed time tags.</returns>
        public static IList<TimeTag> FixOverlapping(IList<TimeTag> timeTags, GroupCheck other = GroupCheck.Asc, SelfCheck self = SelfCheck.BasedOnStart)
        {
            if (!timeTags.Any())
                return timeTags;

            var sortedTimeTags = Sort(timeTags).ToArray();
            var groupedTimeTags = sortedTimeTags.GroupBy(x => x.Index.Index).ToArray();

            var overlappingTimeTags = FindOverlapping(timeTags, other, self);
            var validTimeTags = sortedTimeTags.Except(overlappingTimeTags);

            foreach (var overlappingTimeTag in overlappingTimeTags)
            {
                int listIndex = Array.IndexOf(sortedTimeTags, overlappingTimeTag);
                var timeTag = overlappingTimeTag.Index;

                // fix self-overlapping
                var groupedTimeTag = groupedTimeTags.FirstOrDefault(x => x.Key == timeTag.Index)?.ToList();
                var startTimeGroup = groupedTimeTag?.Where(x => x.Index.State == TextIndex.IndexState.Start && x.Time != null);
                var endTimeGroup = groupedTimeTag?.Where(x => x.Index.State == TextIndex.IndexState.End && x.Time != null);

                switch (timeTag.State)
                {
                    case TextIndex.IndexState.Start:
                        double? minEndTime = endTimeGroup?.Min(x => x.Time);

                        if (minEndTime != null && minEndTime < overlappingTimeTag.Time)
                        {
                            sortedTimeTags[listIndex] = new TimeTag(timeTag, minEndTime);
                            continue;
                        }

                        break;

                    case TextIndex.IndexState.End:
                        double? maxStartTime = startTimeGroup?.Max(x => x.Time);

                        if (maxStartTime != null && maxStartTime > overlappingTimeTag.Time)
                        {
                            sortedTimeTags[listIndex] = new TimeTag(timeTag, maxStartTime);
                            continue;
                        }

                        break;

                    default:
                        throw new InvalidEnumArgumentException(nameof(timeTag.State));
                }

                // fix previous or next value to apply
                switch (other)
                {
                    case GroupCheck.Asc:
                        // find previous value to apply.
                        double? previousValidValue = sortedTimeTags.Reverse().FirstOrDefault(x => x.Index.Index < timeTag.Index && x.Time != null)?.Time;
                        sortedTimeTags[listIndex] = new TimeTag(timeTag, previousValidValue);
                        break;

                    case GroupCheck.Desc:
                        // find next value to apply.
                        double? nextValidValue = sortedTimeTags.FirstOrDefault(x => x.Index.Index > timeTag.Index && x.Time != null)?.Time;
                        sortedTimeTags[listIndex] = new TimeTag(timeTag, nextValidValue);
                        break;

                    default:
                        throw new InvalidEnumArgumentException(nameof(other));
                }
            }

            return sortedTimeTags;
        }

        /// <summary>
        /// Convert list of time tag to dictionary.
        /// WIll sort by the time.
        /// </summary>
        /// <param name="timeTags">Time tags</param>
        /// <returns>Time tags with dictionary format.</returns>
        public static IReadOnlyDictionary<double, TextIndex> ToTimeBasedDictionary(IList<TimeTag> timeTags)
        {
            // convert to dictionary, will get start's smallest time and end's largest time.
            return timeTags.Where(x => x.Time != null)
                           .OrderBy(x => x.Time)
                           .GroupBy(x => x.Time)
                           .Select(x =>
                               // will always get the first time-tag for now.
                               x.FirstOrDefault())
                           .ToDictionary(k => k?.Time ?? throw new ArgumentNullException(nameof(k)),
                               v => v?.Index ?? throw new ArgumentNullException(nameof(v)));
        }

        /// <summary>
        /// Get start time.
        /// </summary>
        /// <param name="timeTags">Time tags</param>
        /// <returns>Start time</returns>
        public static double? GetStartTime(IList<TimeTag> timeTags)
        {
            var dictionary = ToTimeBasedDictionary(timeTags);
            if (!dictionary.Any())
                return null;

            return dictionary.First().Key;
        }

        /// <summary>
        /// Get End time.
        /// </summary>
        /// <param name="timeTags">Time tags</param>
        /// <returns>End time</returns>
        public static double? GetEndTime(IList<TimeTag> timeTags)
        {
            var dictionary = ToTimeBasedDictionary(timeTags);
            if (!dictionary.Any())
                return null;

            return dictionary.Last().Key;
        }
    }

    public enum GroupCheck
    {
        /// <summary>
        /// Mark next time tag is error if conflict.
        /// </summary>
        Asc,

        /// <summary>
        /// Mark previous tag is error if conflict.
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
