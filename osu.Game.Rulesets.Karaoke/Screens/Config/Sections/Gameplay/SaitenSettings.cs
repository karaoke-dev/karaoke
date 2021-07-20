// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Localisation;
using osu.Game.Graphics.UserInterface;
using osu.Game.Overlays.Settings;
using osu.Game.Rulesets.Karaoke.Configuration;

namespace osu.Game.Rulesets.Karaoke.Screens.Config.Sections.Gameplay
{
    public class SaitenSettings : KaraokeSettingsSubsection
    {
        protected override LocalisableString Header => "Saiten";

        [BackgroundDependencyLoader]
        private void load()
        {
            // todo : should separate saiten and pitch part?
            Children = new Drawable[]
            {
                new SettingsCheckbox
                {
                    LabelText = "Override pitch at gameplay",
                    Current = Config.GetBindable<bool>(KaraokeRulesetSetting.OverridePitchAtGameplay)
                },
                new SettingsSlider<int, PitchSlider>
                {
                    LabelText = "Pitch",
                    Current = Config.GetBindable<int>(KaraokeRulesetSetting.Pitch)
                },
                new SettingsCheckbox
                {
                    LabelText = "Override vocal pitch at gameplay",
                    Current = Config.GetBindable<bool>(KaraokeRulesetSetting.OverrideVocalPitchAtGameplay)
                },
                new SettingsSlider<int, PitchSlider>
                {
                    LabelText = "Vocal pitch",
                    Current = Config.GetBindable<int>(KaraokeRulesetSetting.VocalPitch)
                },
                new SettingsCheckbox
                {
                    LabelText = "Override saiten pitch at gameplay",
                    Current = Config.GetBindable<bool>(KaraokeRulesetSetting.OverrideSaitenPitchAtGameplay)
                },
                new SettingsSlider<int, PitchSlider>
                {
                    LabelText = "Saiten pitch",
                    Current = Config.GetBindable<int>(KaraokeRulesetSetting.SaitenPitch)
                },
            };
        }

        private class PitchSlider : OsuSliderBar<int>
        {
            public override LocalisableString TooltipText => (Current.Value >= 0 ? "+" : "") + Current.Value.ToString("N0");
        }
    }
}
