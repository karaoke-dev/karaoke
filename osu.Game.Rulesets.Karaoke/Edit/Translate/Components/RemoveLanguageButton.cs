// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Globalization;
using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Graphics.Sprites;
using osu.Game.Graphics.UserInterface;
using osu.Game.Overlays;
using osu.Game.Rulesets.Karaoke.Edit.ChangeHandlers.Languages;

namespace osu.Game.Rulesets.Karaoke.Edit.Translate.Components
{
    public class RemoveLanguageButton : IconButton
    {
        [Resolved]
        private ILanguagesChangeHandler languagesChangeHandler { get; set; }

        [Resolved]
        private IDialogOverlay dialogOverlay { get; set; }

        [Resolved]
        private IBindable<CultureInfo> currentLanguage { get; set; }

        public RemoveLanguageButton()
        {
            Icon = FontAwesome.Solid.Trash;
            Action = () =>
            {
                if (languagesChangeHandler.IsLanguageContainsTranslate(currentLanguage.Value))
                {
                    dialogOverlay.Push(new DeleteLanguagePopupDialog(currentLanguage.Value, isOk =>
                    {
                        if (isOk)
                            languagesChangeHandler.Remove(currentLanguage.Value);
                    }));
                }
                else
                {
                    languagesChangeHandler.Remove(currentLanguage.Value);
                }
            };
        }
    }
}
