// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using System.Globalization;
using osu.Framework.Graphics.Sprites;
using osu.Game.Overlays.Dialog;
using osu.Game.Rulesets.Karaoke.Utils;

namespace osu.Game.Rulesets.Karaoke.Screens.Edit.Beatmaps.Translations;

public partial class DeleteLanguagePopupDialog : PopupDialog
{
    public DeleteLanguagePopupDialog(CultureInfo currentLanguage, Action<bool> okAction)
    {
        Icon = FontAwesome.Regular.TrashAlt;
        HeaderText = $"Confirm deletion of language {CultureInfoUtils.GetLanguageDisplayText(currentLanguage)}?";
        BodyText = "It will also remove the translations.";
        Buttons = new PopupDialogButton[]
        {
            new PopupDialogOkButton
            {
                Text = "Yes. Go for it.",
                Action = () => okAction(true),
            },
            new PopupDialogCancelButton
            {
                Text = "No! Abort mission!",
                Action = () => okAction(false),
            },
        };
    }
}
