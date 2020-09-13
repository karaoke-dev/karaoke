// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Game.Graphics.Sprites;
using osu.Game.Graphics.UserInterface;
using osu.Game.Rulesets.Karaoke.Configuration;
using osu.Game.Rulesets.Karaoke.Objects;
using osu.Game.Screens.Play.PlayerSettings;

namespace osu.Game.Rulesets.Karaoke.UI.PlayerSettings
{
    public class TranslateSettings : PlayerSettingsGroup
    {
        private readonly PlayerCheckbox translateCheckBox;
        private readonly OsuSpriteText translateText;
        private readonly OsuDropdown<string> translateDropDown;

        public TranslateSettings(PropertyDictionary dictionary)
            : base("Translate")
        {
            Children = new Drawable[]
            {
                translateCheckBox = new PlayerCheckbox
                {
                    LabelText = "Translate"
                },
                translateText = new OsuSpriteText
                {
                    Text = "Translate language"
                },
                translateDropDown = new OsuDropdown<string>
                {
                    RelativeSizeAxes = Axes.X,
                    Items = dictionary.Translates.Keys
                },
            };
        }

        [BackgroundDependencyLoader]
        private void load(KaraokeSessionStatics session)
        {
            // Translate
            translateCheckBox.Current = session.GetBindable<bool>(KaraokeRulesetSession.UseTranslate);
            translateDropDown.Current = session.GetBindable<string>(KaraokeRulesetSession.PreferLanguage);

            // hidden dropdown if not translate
            translateCheckBox.Current.BindValueChanged(value =>
            {
                if (value.NewValue)
                {
                    translateText.Show();
                    translateDropDown.Show();
                }
                else
                {
                    translateText.Hide();
                    translateDropDown.Hide();
                }
            }, true);
        }
    }
}
