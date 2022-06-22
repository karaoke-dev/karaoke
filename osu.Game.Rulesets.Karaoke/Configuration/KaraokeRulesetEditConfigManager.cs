// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

#nullable disable

using osu.Game.Configuration;

namespace osu.Game.Rulesets.Karaoke.Configuration
{
    public class KaraokeRulesetEditConfigManager : InMemoryConfigManager<KaraokeRulesetEditSetting>
    {
        protected override void InitialiseDefaults()
        {
            base.InitialiseDefaults();

            // Note editor
            SetDefault(KaraokeRulesetEditSetting.DisplayRuby, true);
            SetDefault(KaraokeRulesetEditSetting.DisplayRomaji, true);
            SetDefault(KaraokeRulesetEditSetting.DisplayTranslate, true);
        }
    }

    public enum KaraokeRulesetEditSetting
    {
        // Note editor
        DisplayRuby,
        DisplayRomaji,
        DisplayTranslate,
    }
}
