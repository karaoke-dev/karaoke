// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Game.Configuration;
using osu.Game.Rulesets.Karaoke.Graphics.Sprites;

namespace osu.Game.Rulesets.Karaoke.Configuration;

public class KaraokeRulesetEditConfigManager : InMemoryConfigManager<KaraokeRulesetEditSetting>
{
    protected override void InitialiseDefaults()
    {
        base.InitialiseDefaults();

        // Lyric display.
        SetDefault(KaraokeRulesetEditSetting.DisplayType, LyricDisplayType.Lyric);
        SetDefault(KaraokeRulesetEditSetting.DisplayProperty, LyricDisplayProperty.Both);
        SetDefault(KaraokeRulesetEditSetting.DisplayTranslate, true);
    }
}

public enum KaraokeRulesetEditSetting
{
    // Lyric display type
    DisplayType,
    DisplayProperty,
    DisplayTranslate,
}
