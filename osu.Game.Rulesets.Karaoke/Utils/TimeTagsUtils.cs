// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using System.Collections.Generic;
using System.Linq;
using osu.Framework.Graphics.Sprites;
using osu.Game.Rulesets.Karaoke.Extensions;
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
            if (startTimeTag == null || endTimeTag == null)
                throw new ArgumentNullException($"{nameof(startTimeTag)} or {nameof(endTimeTag)} cannot be null.");

            if (startTimeTag.Index > endTimeTag.Index)
                throw new InvalidOperationException($"{nameof(endTimeTag.Index)} cannot larger than {startTimeTag.Index}");

            if (index < startTimeTag.Index || index > endTimeTag.Index)
                throw new InvalidOperationException($"{nameof(endTimeTag.Index)} cannot larger than {startTimeTag.Index}");

            if (startTimeTag.Time == null || endTimeTag.Time == null)
                return new TimeTag(index);

            var diffFromStartToEnd = getTimeCalculationIndex(endTimeTag.Index) - getTimeCalculationIndex(startTimeTag.Index);
            var diffFromStartToNow = getTimeCalculationIndex(index) - getTimeCalculationIndex(startTimeTag.Index);
            if (diffFromStartToEnd == 0 || diffFromStartToNow == 0)
                return new TimeTag(index, startTimeTag.Time);

            var time = startTimeTag.Time +
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
        public static TimeTag[] Sort(TimeTag[] timeTags)
        {
            return timeTags?.OrderBy(x => x.Index)
                           .ThenBy(x => x.Time).ToArray();
        }

        /// <summary>
        /// Find out of range time-tag.
        /// </summary>
        /// <param name="timeTags"></param>
        /// <param name="lyric"></param>
        /// <returns></returns>
        public static TimeTag[] FindOutOfRange(TimeTag[] timeTags, string lyric)
        {
            return timeTags?.Where(x => x.Index.Index < 0 || x.Index.Index >= lyric.Length).ToArray();
        }

        /// <summary>
        /// Find invalid time tags.
        /// </summary>
        /// <param name="timeTags">Time tags</param>
        /// <param name="other">Check way</param>
        /// <param name="self">Check way</param>
        /// <returns>List of invalid time tags</returns>
        public static TimeTag[] FindInvalid(TimeTag[] timeTags, GroupCheck other = GroupCheck.Asc, SelfCheck self = SelfCheck.BasedOnStart)
        {
            if (timeTags == null)
                return null;

            var sortedTimeTags = Sort(timeTags);
            var groupedTimeTags = sortedTimeTags.GroupBy(x => x.Index.Index);

            var invalidList = new List<TimeTag>();

            foreach (var groupedTimeTag in groupedTimeTags)
            {
                var startTimeGroup = groupedTimeTag.Where(x => x.Index.State == TextIndex.IndexState.Start && x.Time != null);
                var endTimeGroup = groupedTimeTag.Where(x => x.Index.State == TextIndex.IndexState.End && x.Time != null);

                // add invalid group into list.
                var groupInvalid = findGroupInvalid();
                if (groupInvalid != null)
                    invalidList.AddRange(groupInvalid);

                // add invalid self into list.
                var selfInvalid = findSelfInvalid();
                if (selfInvalid != null)
                    invalidList.AddRange(selfInvalid);

                List<TimeTag> findGroupInvalid()
                {
                    switch (other)
                    {
                        case GroupCheck.Asc:
                            // mark next is invalid if smaller then self
                            var groupMaxTime = groupedTimeTag.Max(x => x.Time);
                            if (groupMaxTime == null)
                                return null;

                            return sortedTimeTags.Where(x => x.Index.Index > groupedTimeTag.Key && x.Time < groupMaxTime).ToList();

                        case GroupCheck.Desc:
                            // mark previous is invalid if larger then self
                            var groupMinTime = groupedTimeTag.Min(x => x.Time);
                            if (groupMinTime == null)
                                return null;

                            return sortedTimeTags.Where(x => x.Index.Index < groupedTimeTag.Key && x.Time > groupMinTime).ToList();

                        default:
                            return null;
                    }
                }

                List<TimeTag> findSelfInvalid()
                {
                    switch (self)
                    {
                        case SelfCheck.BasedOnStart:
                            var maxStartTime = startTimeGroup.Max(x => x.Time);
                            if (maxStartTime == null)
                                return null;

                            return endTimeGroup.Where(x => x.Time != null && x.Time.Value < maxStartTime.Value).ToList();

                        case SelfCheck.BasedOnEnd:
                            var minEndTime = endTimeGroup.Min(x => x.Time);
                            if (minEndTime == null)
                                return null;

                            return startTimeGroup.Where(x => x.Time != null && x.Time.Value > minEndTime.Value).ToList();

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
        /// <param name="other">Fix way</param>
        /// <param name="self">Fix way</param>
        /// <returns>Fixed time tags.</returns>
        public static TimeTag[] FixInvalid(TimeTag[] timeTags, GroupCheck other = GroupCheck.Asc, SelfCheck self = SelfCheck.BasedOnStart)
        {
            if (timeTags == null || timeTags.Length == 0)
                return timeTags;

            var sortedTimeTags = Sort(timeTags);
            var groupedTimeTags = sortedTimeTags.GroupBy(x => x.Index.Index);

            var invalidTimeTags = FindInvalid(timeTags, other, self);
            var validTimeTags = sortedTimeTags.Except(invalidTimeTags);

            foreach (var invalidTimeTag in invalidTimeTags)
            {
                var listIndex = sortedTimeTags.IndexOf(invalidTimeTag);
                var timeTag = invalidTimeTag.Index;

                // fix self-invalid
                var groupedTimeTag = groupedTimeTags.FirstOrDefault(x => x.Key == timeTag.Index).ToList();
                var startTimeGroup = groupedTimeTag.Where(x => x.Index.State == TextIndex.IndexState.Start && x.Time != null);
                var endTimeGroup = groupedTimeTag.Where(x => x.Index.State == TextIndex.IndexState.End && x.Time != null);

                switch (timeTag.State)
                {
                    case TextIndex.IndexState.Start:
                        var minEndTime = endTimeGroup.Min(x => x.Time);

                        if (minEndTime != null && minEndTime < invalidTimeTag.Time)
                        {
                            sortedTimeTags[listIndex] = new TimeTag(timeTag, minEndTime);
                            continue;
                        }

                        break;

                    case TextIndex.IndexState.End:
                        var maxStartTime = startTimeGroup.Max(x => x.Time);

                        if (maxStartTime != null && maxStartTime > invalidTimeTag.Time)
                        {
                            sortedTimeTags[listIndex] = new TimeTag(timeTag, maxStartTime);
                            continue;
                        }

                        break;

                    default:
                        throw new ArgumentOutOfRangeException();
                }

                // fix previous or next value to apply
                switch (other)
                {
                    case GroupCheck.Asc:
                        // find previous value to apply.
                        var previousValidValue = sortedTimeTags.Reverse().FirstOrDefault(x => x.Index.Index < timeTag.Index && x.Time != null)?.Time;
                        sortedTimeTags[listIndex] = new TimeTag(timeTag, previousValidValue);
                        break;

                    case GroupCheck.Desc:
                        // find next value to apply.
                        var nextValidValue = sortedTimeTags.FirstOrDefault(x => x.Index.Index > timeTag.Index && x.Time != null)?.Time;
                        sortedTimeTags[listIndex] = new TimeTag(timeTag, nextValidValue);
                        break;

                    default:
                        throw new InvalidOperationException(nameof(other));
                }
            }

            return sortedTimeTags;
        }

        /// <summary>
        /// Convert list of time tag to dictionary.
        /// </summary>
        /// <param name="timeTags">Time tags</param>
        /// <param name="applyFix">Should auto-fix or not</param>
        /// <param name="other">Fix way</param>
        /// <param name="self">Fix way</param>
        /// <returns>Time tags with dictionary format.</returns>
        public static IReadOnlyDictionary<TextIndex, double> ToDictionary(TimeTag[] timeTags, bool applyFix = true, GroupCheck other = GroupCheck.Asc,
                                                                          SelfCheck self = SelfCheck.BasedOnStart)
        {
            if (timeTags == null)
                return new Dictionary<TextIndex, double>();

            // sorted value
            var sortedTimeTags = applyFix ? FixInvalid(timeTags, other, self) : Sort(timeTags);

            // convert to dictionary, will get start's smallest time and end's largest time.
            return sortedTimeTags.Where(x => x.Time != null).GroupBy(x => x.Index).Select(x =>
            {
                if (x.Key.State == TextIndex.IndexState.Start)
                    return x.FirstOrDefault();

                return x.LastOrDefault();
            }).ToDictionary(k => k?.Index ?? throw new ArgumentNullException(nameof(k)), v => v?.Time ?? throw new ArgumentNullException(nameof(v)));
        }

        /// <summary>
        /// Convert dictionary to list of time tags.
        /// </summary>
        /// <param name="dictionary">Dictionary.</param>
        /// <returns>Time tags</returns>
        public static TimeTag[] ToTimeTagList(IReadOnlyDictionary<TextIndex, double> dictionary)
        {
            return dictionary.Select(d => new TimeTag(d.Key, d.Value)).ToArray();
        }

        /// <summary>
        /// Get start time.
        /// </summary>
        /// <param name="timeTags">Time tags</param>
        /// <returns>Start time</returns>
        public static double? GetStartTime(TimeTag[] timeTags)
        {
            return ToDictionary(timeTags).FirstOrDefault().Value;
        }

        /// <summary>
        /// Get End time.
        /// </summary>
        /// <param name="timeTags">Time tags</param>
        /// <returns>End time</returns>
        public static double? GetEndTime(TimeTag[] timeTags)
        {
            return ToDictionary(timeTags).LastOrDefault().Value;
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
