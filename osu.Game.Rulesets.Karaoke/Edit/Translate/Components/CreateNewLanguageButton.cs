// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Globalization;
using System.Linq;
using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Extensions;
using osu.Framework.Graphics.Cursor;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Graphics.UserInterface;
using osu.Game.Graphics.UserInterface;
using osu.Game.Rulesets.Karaoke.Edit.ChangeHandlers.Languages;
using osu.Game.Rulesets.Karaoke.Edit.Components.UserInterfaceV2;

namespace osu.Game.Rulesets.Karaoke.Edit.Translate.Components
{
    public class CreateNewLanguageButton : IconButton, IHasPopover
    {
        [Resolved]
        private ILanguagesChangeHandler languagesChangeHandler { get; set; }

        private readonly Bindable<CultureInfo> currentLanguage = new();

        public CreateNewLanguageButton()
        {
            Icon = FontAwesome.Solid.Plus;
            Action = this.ShowPopover;

            currentLanguage.BindValueChanged(e =>
            {
                var newLanguage = e.NewValue;
                if (newLanguage == null)
                    return;

                if (!languagesChangeHandler.Languages.Contains(newLanguage))
                {
                    languagesChangeHandler.Add(newLanguage);
                }

                // after selected the language, should always hide the popover.
                this.HidePopover();

                // Should clear the bindable after selected.
                currentLanguage.Value = null;
            });
        }

        public Popover GetPopover()
            => new LanguageSelectorPopover(currentLanguage);
    }
}
