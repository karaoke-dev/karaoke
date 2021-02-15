// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Game.Rulesets.Karaoke.Configuration;
using osu.Game.Rulesets.Karaoke.Objects;
using osu.Game.Rulesets.Karaoke.Utils;
using osu.Game.Screens.Edit;

namespace osu.Game.Rulesets.Karaoke.Edit.Lyrics
{
    public class LyricInvalidChecker : Component
    {
        [Resolved]
        private KaraokeRulesetEditConfigManager configManager { get; set; }

        [Resolved]
        private EditorBeatmap beatmap { get; set; }

        public Lyric[] InvalidTimeLyrics()
        {
            // todo : implement.
            return null;
        }

        public bool InvalidLyricTime(Lyric lyric)
        {
            // todo : apply utils with enum key.
            return false;
        }

        public TimeTag[] CheckInvalidTimeTagTime(Lyric lyric)
        {
            var groupCheck = configManager.Get<GroupCheck>(KaraokeRulesetEditSetting.CheckInvalidTimeTagTimeGroupCheck);
            var selfCheck = configManager.Get<SelfCheck>(KaraokeRulesetEditSetting.CheckInvalidTimeTagTimeSelfCheck);
            return TimeTagsUtils.FindInvalid(lyric.TimeTags, groupCheck, selfCheck);
        }

        public RubyTag[] CheckInvalidRubyRange(Lyric lyric)
        {
            return TextTagsUtils.FindOutOfRange(lyric.RubyTags, lyric.Text);
        }

        public RubyTag[] CheckOverlappingRubyPosition(Lyric lyric)
        {
            var sorting = configManager.Get<TextTagsUtils.Sorting>(KaraokeRulesetEditSetting.CheckRubyPositionSorting);
            return TextTagsUtils.FindOverlapping(lyric.RubyTags, sorting);
        }

        public RomajiTag[] CheckInvalidRomajiRange(Lyric lyric)
        {
            return TextTagsUtils.FindOutOfRange(lyric.RomajiTags, lyric.Text);
        }

        public RomajiTag[] CheckOverlappingRomajiPosition(Lyric lyric)
        {
            var sorting = configManager.Get<TextTagsUtils.Sorting>(KaraokeRulesetEditSetting.CheckRomajiPositionSorting);
            return TextTagsUtils.FindOverlapping(lyric.RomajiTags, sorting);
        }
    }
}
