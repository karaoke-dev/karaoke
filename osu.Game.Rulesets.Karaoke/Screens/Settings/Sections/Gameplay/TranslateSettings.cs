// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

#nullable disable

using System.Globalization;
using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Localisation;
using osu.Game.Overlays.Settings;
using osu.Game.Rulesets.Karaoke.Configuration;
using osu.Game.Rulesets.Karaoke.Screens.Settings.Previews;
using osu.Game.Rulesets.Karaoke.Screens.Settings.Previews.Gameplay;

namespace osu.Game.Rulesets.Karaoke.Screens.Settings.Sections.Gameplay
{
    public class TranslateSettings : KaraokeSettingsSubsection
    {
        protected override LocalisableString Header => "Translate";

        public override SettingsSubsectionPreview CreatePreview() => new LyricPreview();

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
