// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Game.Configuration;
using osu.Game.Rulesets.Karaoke.Objects.Utils;

namespace osu.Game.Rulesets.Karaoke.Configuration;

public class KaraokeRulesetEditCheckerConfigManager : InMemoryConfigManager<KaraokeRulesetEditCheckerSetting>
{
    protected override void InitialiseDefaults()
    {
        base.InitialiseDefaults();

        // Lyric
        SetDefault(KaraokeRulesetEditCheckerSetting.LyricRubyPositionSorting, TextTagsUtils.Sorting.Asc);
        SetDefault(KaraokeRulesetEditCheckerSetting.LyricRomajiPositionSorting, TextTagsUtils.Sorting.Asc);
        SetDefault(KaraokeRulesetEditCheckerSetting.LyricTimeTagTimeSelfCheck, SelfCheck.BasedOnStart);
        SetDefault(KaraokeRulesetEditCheckerSetting.LyricTimeTagTimeGroupCheck, GroupCheck.Asc);
    }
}

public enum KaraokeRulesetEditCheckerSetting
{
    LyricRubyPositionSorting,
    LyricRomajiPositionSorting,
    LyricTimeTagTimeSelfCheck,
    LyricTimeTagTimeGroupCheck
}
