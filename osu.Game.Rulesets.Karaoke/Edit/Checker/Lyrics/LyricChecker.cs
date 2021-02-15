// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Collections.Generic;
using osu.Game.Rulesets.Karaoke.Objects;
using osu.Game.Rulesets.Karaoke.Utils;

namespace osu.Game.Rulesets.Karaoke.Edit.Checker.Lyrics
{
    /// <summary>
    /// This checker is focus on checking and give report, and should be testable.
    /// </summary>
    public class LyricChecker
    {
        private readonly LyricCheckerConfig config;

        public LyricChecker(LyricCheckerConfig config)
        {
            this.config = config;
        }

        public TimeInvalid[] CheckInvalidLyricTime(Lyric lyric)
        {
            var result = new List<TimeInvalid>();

            if (LyricUtils.CheckIsTimeOverlapping(lyric))
                result.Add(TimeInvalid.Overlapping);

            if (LyricUtils.CheckIsStartTimeInvalid(lyric))
                result.Add(TimeInvalid.StartTimeInvalid);

            if (LyricUtils.CheckIsEndTimeInvalid(lyric))
                result.Add(TimeInvalid.EndTimeInvalid);

            return result.ToArray();
        }

        public Dictionary<TimeTagInvalid, TimeTag[]> CheckInvalidTimeTags(Lyric lyric)
        {
            var result = new Dictionary<TimeTagInvalid, TimeTag[]>();

            // todo : check out of range.
            var outOfRangeTags = TimeTagsUtils.FindOutOfRange(lyric.TimeTags, lyric.Text);
            if (outOfRangeTags?.Length > 0)
                result.Add(TimeTagInvalid.OutOfRange, outOfRangeTags);

            // Check overlapping.
            var groupCheck = config.TimeTagTimeGroupCheck;
            var selfCheck = config.TimeTagTimeSelfCheck;
            var invalidTimeTags = TimeTagsUtils.FindInvalid(lyric.TimeTags, groupCheck, selfCheck);
            if (invalidTimeTags?.Length > 0)
                result.Add(TimeTagInvalid.Overlapping, invalidTimeTags);

            return result;
        }

        public Dictionary<RubyTagInvalid, RubyTag[]> CheckInvalidRubyTags(Lyric lyric)
        {
            var result = new Dictionary<RubyTagInvalid, RubyTag[]>();

            // Checking out of range tags.
            var outOfRangeTags = TextTagsUtils.FindOutOfRange(lyric.RubyTags, lyric.Text);
            if (outOfRangeTags?.Length > 0)
                result.Add(RubyTagInvalid.OutOfRange, outOfRangeTags);

            // Checking overlapping.
            var sorting = config.RomajiPositionSorting;
            var overlappingTags = TextTagsUtils.FindOverlapping(lyric.RubyTags, sorting);
            if (overlappingTags?.Length > 0)
                result.Add(RubyTagInvalid.Overlapping, overlappingTags);

            return result;
        }

        public Dictionary<RomajiTagInvalid, RomajiTag[]> CheckInvalidRomajiTags(Lyric lyric)
        {
            var result = new Dictionary<RomajiTagInvalid, RomajiTag[]>();

            // Checking out of range tags.
            var outOfRangeTags = TextTagsUtils.FindOutOfRange(lyric.RomajiTags, lyric.Text);
            if (outOfRangeTags?.Length > 0)
                result.Add(RomajiTagInvalid.OutOfRange, outOfRangeTags);

            // Checking overlapping.
            var sorting = config.RomajiPositionSorting;
            var overlappingTags = TextTagsUtils.FindOverlapping(lyric.RomajiTags, sorting);
            if (overlappingTags?.Length > 0)
                result.Add(RomajiTagInvalid.Overlapping, overlappingTags);

            return result;
        }
    }
}
