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
using osu.Game.Rulesets.Karaoke.Graphics.UserInterface;
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

        [Resolved]
        protected OsuGame Geme { get; private set; }

        private KaraokeChangelogOverlay changelogOverlay;
        private LanguageSelectionDialog languageSelectionDialog;

        [BackgroundDependencyLoader]
        private void load()
        {
            var config = (KaraokeRulesetConfigManager)Config;
            var microphoneManager = new MicrophoneManager();

            Children = new Drawable[]
            {
                // Visual
                new SettingsEnumDropdown<KaraokeScrollingDirection>
                {
                    LabelText = "Scrolling direction",
                    Current = config.GetBindable<KaraokeScrollingDirection>(KaraokeRulesetSetting.ScrollDirection)
                },
                new SettingsSlider<double, TimeSlider>
                {
                    LabelText = "Scroll speed",
                    Current = config.GetBindable<double>(KaraokeRulesetSetting.ScrollTime)
                },
                new SettingsCheckbox
                {
                    LabelText = "Display alternative text",
                    Current = config.GetBindable<bool>(KaraokeRulesetSetting.DisplayAlternativeText)
                },
                new SettingsCheckbox
                {
                    LabelText = "Show cursor while playing",
                    Current = config.GetBindable<bool>(KaraokeRulesetSetting.ShowCursor)
                },
                // Translate
                new SettingsCheckbox
                {
                    LabelText = "Translate",
                    Current = config.GetBindable<bool>(KaraokeRulesetSetting.UseTranslate)
                },
                new SettingsButton
                {
                    Text = "Prefer language",
                    TooltipText = "Select perfer translate language.",
                    Action = () =>
                    {
                        try
                        {
                            if (DisplayContainer == null)
                                return;

                            if (languageSelectionDialog == null && !DisplayContainer.Children.OfType<LanguageSelectionDialog>().Any())
                                DisplayContainer.Add(languageSelectionDialog = new LanguageSelectionDialog
                                {
                                    Current = config.GetBindable<CultureInfo>(KaraokeRulesetSetting.PreferLanguage)
                                });

                            languageSelectionDialog?.Show();
                        }
                        catch
                        {
                            // maybe this overlay has been moved into internal.
                        }
                    }
                    
                },
                // Pitch
                new SettingsCheckbox
                {
                    LabelText = "Override pitch at gameplay",
                    Current = config.GetBindable<bool>(KaraokeRulesetSetting.OverridePitchAtGameplay)
                },
                new MicrophoneDeviceSettingsDropdown
                {
                    LabelText = "Microphone devices",
                    Items = microphoneManager.MicrophoneDeviceNames,
                    Current = config.GetBindable<string>(KaraokeRulesetSetting.MicrophoneDevice)
                },
                new SettingsSlider<int, PitchSlider>
                {
                    LabelText = "Pitch",
                    Current = config.GetBindable<int>(KaraokeRulesetSetting.Pitch)
                },
                new SettingsCheckbox
                {
                    LabelText = "Override vocal pitch at gameplay",
                    Current = config.GetBindable<bool>(KaraokeRulesetSetting.OverrideVocalPitchAtGameplay)
                },
                new SettingsSlider<int, PitchSlider>
                {
                    LabelText = "Vocal pitch",
                    Current = config.GetBindable<int>(KaraokeRulesetSetting.VocalPitch)
                },
                new SettingsCheckbox
                {
                    LabelText = "Override saiten pitch at gameplay",
                    Current = config.GetBindable<bool>(KaraokeRulesetSetting.OverrideSaitenPitchAtGameplay)
                },
                new SettingsSlider<int, PitchSlider>
                {
                    LabelText = "Saiten pitch",
                    Current = config.GetBindable<int>(KaraokeRulesetSetting.SaitenPitch)
                },
                // Practice
                new SettingsSlider<double, TimeSlider>
                {
                    LabelText = "Practice preempt time",
                    Current = config.GetBindable<double>(KaraokeRulesetSetting.PracticePreemptTime)
                },
                new SettingsButton
                {
                    Text = "Change log",
                    TooltipText = "Let's see what karaoke! changed.",
                    Action = () =>
                    {
                        try
                        {
                            if (DisplayContainer == null)
                                return;

                            if (changelogOverlay == null && !DisplayContainer.Children.OfType<KaraokeChangelogOverlay>().Any())
                                DisplayContainer.Add(changelogOverlay = new KaraokeChangelogOverlay("karaoke-dev"));

                            changelogOverlay?.Show();
                        }
                        catch
                        {
                            // maybe this overlay has been moved into internal.
                        }
                    }
                }
            };
        }

        protected Container DisplayContainer => Geme?.Children[3] as Container;

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
