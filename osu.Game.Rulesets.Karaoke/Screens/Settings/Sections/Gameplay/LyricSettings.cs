// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

#nullable disable

using osu.Framework.Allocation;
using osu.Framework.Localisation;
using osu.Game.Overlays.Settings;
using osu.Game.Rulesets.Karaoke.Configuration;
using osu.Game.Rulesets.Karaoke.Screens.Settings.Previews;
using osu.Game.Rulesets.Karaoke.Screens.Settings.Previews.Gameplay;

namespace osu.Game.Rulesets.Karaoke.Screens.Settings.Sections.Gameplay
{
    public class LyricSettings : KaraokeSettingsSubsection
    {
        protected override LocalisableString Header => "Lyric";

        public override SettingsSubsectionPreview CreatePreview() => new LyricPreview();

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
