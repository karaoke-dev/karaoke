// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Sprites;
using osu.Game.Overlays.Settings;
using osu.Game.Rulesets.Karaoke.Configuration;

namespace osu.Game.Rulesets.Karaoke.Screens.Config.Sections.Graphics
{
    public class LyricFontSettings : KaraokeSettingsSubsection
    {
        protected override string Header => "Lyric font";

        [BackgroundDependencyLoader]
        private void load()
        {
            Children = new Drawable[]
            {
                new SettingsSlider<double>
                {
                    LabelText = "Font scale",
                    Current = Config.GetBindable<double>(KaraokeRulesetSetting.LyricScale),
                    KeyboardStep = 0.01f,
                    DisplayAsPercentage = true
                },
                new SettingsFont
                {
                    LabelText = "Default main font",
                    Current = Config.GetBindable<FontUsage>(KaraokeRulesetSetting.MainFont)
                },
                new SettingsFont
                {
                    LabelText = "Default ruby font",
                    Current = Config.GetBindable<FontUsage>(KaraokeRulesetSetting.RubyFont)
                },
                new SettingsFont
                {
                    LabelText = "Default romaji font",
                    Current = Config.GetBindable<FontUsage>(KaraokeRulesetSetting.RomajiFont)
                },
                new SettingsCheckbox
                {
                    LabelText = "Force use default lyric font.",
                    TooltipText = "Force use default font even has customize font in skin or beatmap.",
                    Current = Config.GetBindable<bool>(KaraokeRulesetSetting.ForceUseDefaultFont)
                },
                new SettingsFont
                {
                    LabelText = "Translate font",
                    Current = Config.GetBindable<FontUsage>(KaraokeRulesetSetting.TranslateFont)
                },
                new SettingsCheckbox
                {
                    LabelText = "Force use default translate font.",
                    TooltipText = "Force use default font even has customize font in skin or beatmap.",
                    Current = Config.GetBindable<bool>(KaraokeRulesetSetting.ForceUseDefaultTranslateFont)
                }
            };
        }
    }
}
