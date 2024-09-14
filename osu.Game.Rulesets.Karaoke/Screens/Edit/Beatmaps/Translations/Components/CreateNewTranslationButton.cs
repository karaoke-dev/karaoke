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
using osu.Game.Rulesets.Karaoke.Edit.ChangeHandlers.Beatmaps;
using osu.Game.Rulesets.Karaoke.Graphics.UserInterfaceV2;

namespace osu.Game.Rulesets.Karaoke.Screens.Edit.Beatmaps.Translations.Components;

public partial class CreateNewTranslationButton : IconButton, IHasPopover
{
    [Resolved]
    private IBeatmapTranslationsChangeHandler beatmapTranslationsChangeHandler { get; set; } = null!;

    private readonly Bindable<CultureInfo?> currentLanguage = new();

    public CreateNewTranslationButton()
    {
        Icon = FontAwesome.Solid.Plus;
        Action = this.ShowPopover;

        currentLanguage.BindValueChanged(e =>
        {
            var newLanguage = e.NewValue;
            if (newLanguage == null)
                return;

            if (!beatmapTranslationsChangeHandler.Languages.Contains(newLanguage))
            {
                beatmapTranslationsChangeHandler.Add(newLanguage);
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
