// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

#nullable disable

using System;
using System.Globalization;
using osu.Framework.Graphics.Sprites;
using osu.Game.Overlays.Dialog;
using osu.Game.Rulesets.Karaoke.Utils;

namespace osu.Game.Rulesets.Karaoke.Edit.Translate
{
    public class DeleteLanguagePopupDialog : PopupDialog
    {
        public DeleteLanguagePopupDialog(CultureInfo currentLanguage, Action<bool> okAction = null)
        {
            Icon = FontAwesome.Regular.TrashAlt;
            HeaderText = $"Confirm deletion of language {CultureInfoUtils.GetLanguageDisplayText(currentLanguage)}?";
            BodyText = "It will also remove the translate.";
            Buttons = new PopupDialogButton[]
            {
                new PopupDialogOkButton
                {
                    Text = @"Yes. Go for it.",
                    Action = () => okAction?.Invoke(true),
                },
                new PopupDialogCancelButton
                {
                    Text = @"No! Abort mission!",
                    Action = () => okAction?.Invoke(false),
                },
            };
        }
    }
}
