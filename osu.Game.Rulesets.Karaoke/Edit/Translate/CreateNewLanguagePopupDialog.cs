// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using osu.Framework.Graphics.Sprites;
using osu.Game.Overlays.Dialog;

namespace osu.Game.Rulesets.Karaoke.Edit.Translate
{
    public class CreateNewLanguagePopupDialog : PopupDialog
    {
        public CreateNewLanguagePopupDialog(Action<bool> okAction = null)
        {
            Icon = FontAwesome.Solid.Globe;
            HeaderText = "Create first language.";
            BodyText = "Seems has no translate language in your beatmap, would you like to create one?";
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
