// Copyright (c) ppy Pty Ltd <contact@ppy.sh>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Game.Graphics.UserInterface;
using osu.Game.Overlays.Settings;
using osu.Game.Rulesets.Karaoke.Configuration;

namespace osu.Game.Rulesets.Karaoke.UI
{
    public class KaraokeSettingsSubsection : RulesetSettingsSubsection
    {
        protected override string Header => "osu!karaoke";

        public KaraokeSettingsSubsection(KaraokeRuleset ruleset)
            : base(ruleset)
        {
        }

        [BackgroundDependencyLoader]
        private void load()
        {
            var config = (KaraokeRulesetConfigManager)Config;

            Children = new Drawable[]
            {
                new SettingsEnumDropdown<KaraokeScrollingDirection>
                {
                    LabelText = "Scrolling direction",
                    Bindable = config.GetBindable<KaraokeScrollingDirection>(KaraokeRulesetSetting.ScrollDirection)
                },
                new SettingsSlider<double, TimeSlider>
                {
                    LabelText = "Scroll speed",
                    Bindable = config.GetBindable<double>(KaraokeRulesetSetting.ScrollTime)
                },
                new SettingsCheckbox
                {
                    LabelText = "Display alternative text",
                    Bindable = config.GetBindable<bool>(KaraokeRulesetSetting.DisplayAlternativeText)
                },
                new SettingsCheckbox
                {
                    LabelText = "Translate",
                    Bindable = config.GetBindable<bool>(KaraokeRulesetSetting.UseTranslate)
                },
                new SettingsTextBox
                {
                    LabelText = "Perfer language",
                    Bindable = config.GetBindable<string>(KaraokeRulesetSetting.PerferLanguage)
                },
                new SettingsCheckbox
                {
                    LabelText = "Show cursor while playing",
                    Bindable = config.GetBindable<bool>(KaraokeRulesetSetting.ShowCursor)
                }
            };
        }

        private class TimeSlider : OsuSliderBar<double>
        {
            public override string TooltipText => Current.Value.ToString("N0") + "ms";
        }
    }
}
