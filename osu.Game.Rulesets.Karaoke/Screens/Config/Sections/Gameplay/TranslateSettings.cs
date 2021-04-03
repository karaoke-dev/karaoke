// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Globalization;
using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Game.Overlays.Settings;
using osu.Game.Rulesets.Karaoke.Configuration;

namespace osu.Game.Rulesets.Karaoke.Screens.Config.Sections.Gameplay
{
    public class TranslateSettings : KaraokeSettingsSubsection
    {
        protected override string Header => "Translate";

        [BackgroundDependencyLoader]
        private void load()
        {
            Children = new Drawable[]
            {
                new SettingsCheckbox
                {
                    LabelText = "Translate",
                    Current = Config.GetBindable<bool>(KaraokeRulesetSetting.UseTranslate)
                },
                new SettingsLanguage
                {
                    LabelText = "Prefer language",
                    TooltipText = "Select prefer translate language.",
                    Current = Config.GetBindable<CultureInfo>(KaraokeRulesetSetting.PreferLanguage)
                },
            };
        }
    }
}
