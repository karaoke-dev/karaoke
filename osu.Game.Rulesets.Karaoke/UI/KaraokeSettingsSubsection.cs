// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Globalization;
using System.Linq;
using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Localisation;
using osu.Game.Graphics.UserInterface;
using osu.Game.Overlays.Settings;
using osu.Game.Rulesets.Karaoke.Configuration;
using osu.Game.Rulesets.Karaoke.Extensions;
using osu.Game.Rulesets.Karaoke.Localisation;
using osu.Game.Rulesets.Karaoke.Overlays;
using osu.Game.Rulesets.Karaoke.Screens.Settings;

namespace osu.Game.Rulesets.Karaoke.UI
{
    public class KaraokeSettingsSubsection : RulesetSettingsSubsection
    {
        protected override LocalisableString Header => CommonStrings.RulesetName;

        public KaraokeSettingsSubsection(Ruleset ruleset)
            : base(ruleset)
        {
        }

        [Resolved]
        protected OsuGame Game { get; private set; }

        private KaraokeChangelogOverlay changelogOverlay;

        [BackgroundDependencyLoader]
        private void load()
        {
            var config = (KaraokeRulesetConfigManager)Config;

            Children = new Drawable[]
            {
                // Scrolling
                new SettingsSlider<double, TimeSlider>
                {
                    LabelText = KaraokeSettingsSubsectionStrings.ScrollSpeed,
                    Current = config.GetBindable<double>(KaraokeRulesetSetting.ScrollTime)
                },
                new SettingsCheckbox
                {
                    LabelText = KaraokeSettingsSubsectionStrings.ShowCursorWhilePlaying,
                    Current = config.GetBindable<bool>(KaraokeRulesetSetting.ShowCursor)
                },
                // Translate
                new SettingsCheckbox
                {
                    LabelText = KaraokeSettingsSubsectionStrings.Translate,
                    Current = config.GetBindable<bool>(KaraokeRulesetSetting.UseTranslate)
                },
                new SettingsLanguage
                {
                    LabelText = KaraokeSettingsSubsectionStrings.PreferLanguage,
                    TooltipText = KaraokeSettingsSubsectionStrings.SelectPreferTranslateLanguage,
                    Current = config.GetBindable<CultureInfo>(KaraokeRulesetSetting.PreferLanguage)
                },
                new SettingsMicrophoneDeviceDropdown
                {
                    LabelText = KaraokeSettingsSubsectionStrings.MicrophoneDevices,
                    Current = config.GetBindable<string>(KaraokeRulesetSetting.MicrophoneDevice)
                },
                // Practice
                new SettingsSlider<double, TimeSlider>
                {
                    LabelText = KaraokeSettingsSubsectionStrings.PracticePreemptTime,
                    Current = config.GetBindable<double>(KaraokeRulesetSetting.PracticePreemptTime)
                },
                new DangerousSettingsButton
                {
                    Text = KaraokeSettingsSubsectionStrings.OpenRulesetSettings,
                    TooltipText = KaraokeSettingsSubsectionStrings.OpenRulesetSettingsForAdjustingMoreConfigs,
                    Action = () =>
                    {
                        try
                        {
                            var screenStake = Game.GetScreenStack();
                            var settingOverlay = Game.GetSettingsOverlay();
                            screenStake?.Push(new KaraokeSettings());
                            settingOverlay?.Hide();
                        }
                        catch
                        {
                        }
                    }
                },
                new SettingsButton
                {
                    Text = KaraokeSettingsSubsectionStrings.ChangeLog,
                    TooltipText = KaraokeSettingsSubsectionStrings.LetsSeeWhatKaraokeChanged,
                    Action = () =>
                    {
                        try
                        {
                            var displayContainer = Game.GetChangelogPlacementContainer();
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
            public override LocalisableString TooltipText => Current.Value.ToString("N0") + "ms";
        }
    }
}
