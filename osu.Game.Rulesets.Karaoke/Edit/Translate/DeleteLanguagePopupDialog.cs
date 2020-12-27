// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using System.Globalization;
using osu.Framework.Graphics.Sprites;
using osu.Game.Overlays.Dialog;

namespace osu.Game.Rulesets.Karaoke.Edit.Translate
{
    public class DeleteLanguagePopupDialog : PopupDialog
    {
        public DeleteLanguagePopupDialog(CultureInfo currentLanguage, Action<bool> okAction = null)
        {
            Icon = FontAwesome.Regular.TrashAlt;
            HeaderText = "Confirm delete language?";
            BodyText = $"Are you really sure you wants to delete language '{currentLanguage.Name}'?";
            Buttons = new PopupDialogButton[]
            {
                new PopupDialogOkButton
                {
                    Text = @"Sure",
                    Action = () => okAction?.Invoke(true),
                },
                new PopupDialogCancelButton
                {
                    Text = @"Let me think about it",
                    Action = () => okAction?.Invoke(false),
                },
            };
        }
    }
}
