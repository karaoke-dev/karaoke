// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Linq;
using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Input;
using osu.Game.Graphics.UserInterface;
using osu.Game.Overlays.Settings;
using osu.Game.Rulesets.Karaoke.Configuration;
using osu.Game.Rulesets.Karaoke.Overlays;

namespace osu.Game.Rulesets.Karaoke.UI
{
    public class KaraokeSettingsSubsection : RulesetSettingsSubsection
    {
        protected override string Header => "karaoke!";

        public KaraokeSettingsSubsection(Ruleset ruleset)
            : base(ruleset)
        {
        }

        private KaraokeChangelogOverlay changelogOverlay;

        [BackgroundDependencyLoader]
        private void load(OsuGame game)
        {
            var config = (KaraokeRulesetConfigManager)Config;
            var microphoneManager = new MicrophoneManager();

            Children = new Drawable[]
            {
                // Visual
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
                    LabelText = "Show cursor while playing",
                    Bindable = config.GetBindable<bool>(KaraokeRulesetSetting.ShowCursor)
                },
                // Translate
                new SettingsCheckbox
                {
                    LabelText = "Translate",
                    Bindable = config.GetBindable<bool>(KaraokeRulesetSetting.UseTranslate)
                },
                new SettingsTextBox
                {
                    LabelText = "Prefer language",
                    Bindable = config.GetBindable<string>(KaraokeRulesetSetting.PreferLanguage)
                },
                // Pitch
                new SettingsCheckbox
                {
                    LabelText = "Override pitch at gameplay",
                    Bindable = config.GetBindable<bool>(KaraokeRulesetSetting.OverridePitchAtGameplay)
                },
                new MicrophoneDeviceSettingsDropdown
                {
                    LabelText = "Microphone devices",
                    Items = microphoneManager.MicrophoneDeviceNames,
                    Bindable = config.GetBindable<string>(KaraokeRulesetSetting.MicrophoneDevice)
                },
                new SettingsSlider<int, PitchSlider>
                {
                    LabelText = "Pitch",
                    Bindable = config.GetBindable<int>(KaraokeRulesetSetting.Pitch)
                },
                new SettingsCheckbox
                {
                    LabelText = "Override vocal pitch at gameplay",
                    Bindable = config.GetBindable<bool>(KaraokeRulesetSetting.OverrideVocalPitchAtGameplay)
                },
                new SettingsSlider<int, PitchSlider>
                {
                    LabelText = "Vocal pitch",
                    Bindable = config.GetBindable<int>(KaraokeRulesetSetting.VocalPitch)
                },
                new SettingsCheckbox
                {
                    LabelText = "Override saiten pitch at gameplay",
                    Bindable = config.GetBindable<bool>(KaraokeRulesetSetting.OverrideSaitenPitchAtGameplay)
                },
                new SettingsSlider<int, PitchSlider>
                {
                    LabelText = "Saiten pitch",
                    Bindable = config.GetBindable<int>(KaraokeRulesetSetting.SaitenPitch)
                },
                // Practice
                new SettingsSlider<double, TimeSlider>
                {
                    LabelText = "Practice preempt time",
                    Bindable = config.GetBindable<double>(KaraokeRulesetSetting.PracticePreemptTime)
                },
                new SettingsButton
                {
                    Text = "Change log",
                    TooltipText = "Let's see what karaoke! changed.",
                    Action = () =>
                    {
                        var overlayContent = game.Children[3] as Container;

                        if (overlayContent == null)
                            return;

                        if (changelogOverlay == null && !overlayContent.Children.OfType<KaraokeChangelogOverlay>().Any())
                            overlayContent.Add(changelogOverlay = new KaraokeChangelogOverlay("karaoke-dev"));

                        changelogOverlay?.Show();
                    }
                }
            };
        }

        private class PitchSlider : OsuSliderBar<int>
        {
            public override string TooltipText => (Current.Value >= 0 ? "+" : "") + Current.Value.ToString("N0");
        }

        private class TimeSlider : OsuSliderBar<double>
        {
            public override string TooltipText => Current.Value.ToString("N0") + "ms";
        }

        private class MicrophoneDeviceSettingsDropdown : SettingsDropdown<string>
        {
            protected override OsuDropdown<string> CreateDropdown() => new MicrophoneDeviceDropdownControl();

            private class MicrophoneDeviceDropdownControl : DropdownControl
            {
                protected override string GenerateItemText(string item)
                    => string.IsNullOrEmpty(item) ? "Default" : base.GenerateItemText(item);
            }
        }
    }
}
