// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Globalization;
using System.Linq;
using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Game.Overlays.Settings;
using osu.Game.Rulesets.Karaoke.Configuration;
using osu.Game.Rulesets.Karaoke.Extensions;
using osu.Game.Rulesets.Karaoke.Graphics.UserInterface;

namespace osu.Game.Rulesets.Karaoke.Screens.Config.Sections.Gameplay
{
    public class TranslateSettings : KaraokeSettingsSubsection
    {
        private LanguageSelectionDialog languageSelectionDialog;

        [Resolved(canBeNull: true)]
        protected OsuGame Game { get; private set; }

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
                                    Current = Config.GetBindable<CultureInfo>(KaraokeRulesetSetting.PreferLanguage)
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
            };
        }
    }
}
