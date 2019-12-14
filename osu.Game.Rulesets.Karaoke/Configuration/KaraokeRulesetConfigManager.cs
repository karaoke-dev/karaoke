// Copyright (c) ppy Pty Ltd <contact@ppy.sh>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Configuration.Tracking;
using osu.Game.Configuration;
using osu.Game.Rulesets.Configuration;
using osu.Game.Rulesets.Karaoke.UI;

namespace osu.Game.Rulesets.Karaoke.Configuration
{
    public class KaraokeRulesetConfigManager : RulesetConfigManager<KaraokeRulesetSetting>
    {
        public KaraokeRulesetConfigManager(SettingsStore settings, RulesetInfo ruleset, int? variant = null)
            : base(settings, ruleset, variant)
        {
        }

        protected override void InitialiseDefaults()
        {
            base.InitialiseDefaults();

            Set(KaraokeRulesetSetting.ScrollTime, 5000.0, 1000.0, 10000.0, 100.0);
            Set(KaraokeRulesetSetting.ScrollDirection, KaraokeScrollingDirection.Left);
            Set(KaraokeRulesetSetting.DisplayAlternativeText, false);
            Set(KaraokeRulesetSetting.UseTranslate, true);
            Set(KaraokeRulesetSetting.PerferLanguage, "en-US");
            Set(KaraokeRulesetSetting.ShowCursor, false);
        }

        public override TrackedSettings CreateTrackedSettings() => new TrackedSettings
        {
            new TrackedSetting<double>(KaraokeRulesetSetting.ScrollTime, v => new SettingDescription(v, "Scroll Time", $"{v}ms"))
        };
    }

    public enum KaraokeRulesetSetting
    {
        ScrollTime,
        ScrollDirection,
        DisplayAlternativeText,
        UseTranslate,
        PerferLanguage,
        ShowCursor,
    }
}
