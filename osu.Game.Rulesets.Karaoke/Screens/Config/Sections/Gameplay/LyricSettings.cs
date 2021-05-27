// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Allocation;
using osu.Game.Overlays.Settings;
using osu.Game.Rulesets.Karaoke.Configuration;

namespace osu.Game.Rulesets.Karaoke.Screens.Config.Sections.Gameplay
{
    public class LyricSettings : KaraokeSettingsSubsection
    {
        protected override string Header => "Lyric";

        [BackgroundDependencyLoader]
        private void load()
        {
            Children = new[]
            {
                new SettingsCheckbox
                {
                    LabelText = "Display ruby",
                    Current = Config.GetBindable<bool>(KaraokeRulesetSetting.DisplayRuby)
                },
                new SettingsCheckbox
                {
                    LabelText = "Display romaji",
                    Current = Config.GetBindable<bool>(KaraokeRulesetSetting.DisplayRomaji)
                },
            };
        }
    }
}
