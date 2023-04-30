// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

#nullable disable

using System.Globalization;
using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Graphics.Sprites;
using osu.Game.Graphics.UserInterface;
using osu.Game.Overlays;
using osu.Game.Rulesets.Karaoke.Edit.ChangeHandlers.Beatmaps;

namespace osu.Game.Rulesets.Karaoke.Screens.Edit.Beatmaps.Translate.Components;

public partial class RemoveLanguageButton : IconButton
{
    [Resolved]
    private IBeatmapLanguagesChangeHandler beatmapLanguagesChangeHandler { get; set; }

    [Resolved]
    private IDialogOverlay dialogOverlay { get; set; }

    [Resolved]
    private IBindable<CultureInfo> currentLanguage { get; set; }

    public RemoveLanguageButton()
    {
        Icon = FontAwesome.Solid.Trash;
        Action = () =>
        {
            if (beatmapLanguagesChangeHandler.IsLanguageContainsTranslate(currentLanguage.Value))
            {
                dialogOverlay.Push(new DeleteLanguagePopupDialog(currentLanguage.Value, isOk =>
                {
                    if (isOk)
                        beatmapLanguagesChangeHandler.Remove(currentLanguage.Value);
                }));
            }
            else
            {
                beatmapLanguagesChangeHandler.Remove(currentLanguage.Value);
            }
        };
    }
}
