// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

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

        public LyricCheckReport CheckLyric(Lyric lyric, LyricCheckProperty checkProperty = LyricCheckProperty.All)
        {
            var report = new LyricCheckReport();

            if (checkProperty.HasFlag(LyricCheckProperty.Time))
                report.TimeInvalid = invalidLyricTime(lyric);

            if (checkProperty.HasFlag(LyricCheckProperty.Time))
                report.InvalidTimeTag = checkInvalidTimeTagTime(lyric);

            if (checkProperty.HasFlag(LyricCheckProperty.Time))
                report.InvalidRubyTag = checkInvalidRubyRange(lyric);

            if (checkProperty.HasFlag(LyricCheckProperty.Time))
                report.InvalidRomajiTag = checkInvalidRomajiRange(lyric);

            return report;
        }


        private bool invalidLyricTime(Lyric lyric)
        {
            // todo : apply utils with enum key.
            return false;
        }

        private TimeTag[] checkInvalidTimeTagTime(Lyric lyric)
        {
            var groupCheck = config.TimeTagTimeGroupCheck;
            var selfCheck = config.TimeTagTimeSelfCheck;
            return TimeTagsUtils.FindInvalid(lyric.TimeTags, groupCheck, selfCheck);
        }

        private RubyTag[] checkInvalidRubyRange(Lyric lyric)
        {
            return TextTagsUtils.FindOutOfRange(lyric.RubyTags, lyric.Text);
        }

        // todo : will use in future
        private RubyTag[] checkOverlappingRubyPosition(Lyric lyric)
        {
            var sorting = config.RubyPositionSorting;
            return TextTagsUtils.FindOverlapping(lyric.RubyTags, sorting);
        }

        private RomajiTag[] checkInvalidRomajiRange(Lyric lyric)
        {
            return TextTagsUtils.FindOutOfRange(lyric.RomajiTags, lyric.Text);
        }

        // todo : will use in future
        private RomajiTag[] checkOverlappingRomajiPosition(Lyric lyric)
        {
            var sorting = config.RomajiPositionSorting;
            return TextTagsUtils.FindOverlapping(lyric.RomajiTags, sorting);
        }
    }
}
