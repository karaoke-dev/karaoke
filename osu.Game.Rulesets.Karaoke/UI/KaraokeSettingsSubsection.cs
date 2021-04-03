// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Globalization;
using System.Linq;
using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Input;
using osu.Framework.Localisation;
using osu.Game.Graphics.UserInterface;
using osu.Game.Overlays.Settings;
using osu.Game.Rulesets.Karaoke.Configuration;
using osu.Game.Rulesets.Karaoke.Extensions;
using osu.Game.Rulesets.Karaoke.Graphics.UserInterface;
using osu.Game.Rulesets.Karaoke.Overlays;
using osu.Game.Rulesets.Karaoke.Screens.Config;

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
        protected OsuGame Game { get; private set; }

        private KaraokeChangelogOverlay changelogOverlay;
        private LanguageSelectionDialog languageSelectionDialog;

        [BackgroundDependencyLoader]
        private void load()
        {
            var config = (KaraokeRulesetConfigManager)Config;
            var microphoneManager = new MicrophoneManager();

            Children = new Drawable[]
            {
                // Scrolling
                new SettingsSlider<double, TimeSlider>
                {
                    LabelText = "Scroll speed",
                    Current = config.GetBindable<double>(KaraokeRulesetSetting.ScrollTime)
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
                    TooltipText = "Select prefer translate language.",
                    Action = () =>
                    {
                        try
                        {
                            var displayContainer = Game.GetDisplayContainer();
                            if (displayContainer == null)
                                return;

                            if (languageSelectionDialog == null && !displayContainer.Children.OfType<LanguageSelectionDialog>().Any())
                            {
                                displayContainer.Add(languageSelectionDialog = new LanguageSelectionDialog
                                {
                                    Current = config.GetBindable<CultureInfo>(KaraokeRulesetSetting.PreferLanguage)
                                });
                            }

                            languageSelectionDialog?.Show();
                        }
                        catch
                        {
                            // maybe this overlay has been moved into internal.
                        }
                    }
                },
                new MicrophoneDeviceSettingsDropdown
                {
                    LabelText = "Microphone devices",
                    Items = microphoneManager.MicrophoneDeviceNames,
                    Current = config.GetBindable<string>(KaraokeRulesetSetting.MicrophoneDevice)
                },
                // Practice
                new SettingsSlider<double, TimeSlider>
                {
                    LabelText = "Practice preempt time",
                    Current = config.GetBindable<double>(KaraokeRulesetSetting.PracticePreemptTime)
                },
                new SettingsButton
                {
                    Text = "Open config",
                    TooltipText = "Hello config",
                    Action = () =>
                    {
                        try
                        {
                            var screenStake = Game.GetScreenStack();
                            var settingOverlay = Game.GetSettingsOverlay();
                            screenStake?.Push(new KaraokeConfigScreen());
                            settingOverlay?.Hide();
                        }
                        catch
                        {

                        }
                    }
                },
                new SettingsButton
                {
                    Text = "Change log",
                    TooltipText = "Let's see what karaoke! changed.",
                    Action = () =>
                    {
                        try
                        {
                            var displayContainer = Game.GetDisplayContainer();
                            var settingOverlay = Game.GetSettingsOverlay();
                            if (displayContainer == null)
                                return;

                            if (changelogOverlay == null && !displayContainer.Children.OfType<KaraokeChangelogOverlay>().Any())
                                displayContainer.Add(changelogOverlay = new KaraokeChangelogOverlay("karaoke-dev"));

                            changelogOverlay?.Show();
                            settingOverlay?.Hide();
                        }
                        catch
                        {
                            // maybe this overlay has been moved into internal.
                        }
                    }
                }
            };
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
                protected override LocalisableString GenerateItemText(string item)
                    => string.IsNullOrEmpty(item) ? "Default" : base.GenerateItemText(item);
            }
        }
    }
}
