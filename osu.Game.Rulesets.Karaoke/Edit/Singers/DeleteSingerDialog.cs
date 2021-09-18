// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using osu.Framework.Graphics.Sprites;
using osu.Game.Overlays.Dialog;

namespace osu.Game.Rulesets.Karaoke.Edit.Singers
{
    public class DeleteSingerDialog : PopupDialog
    {
        public DeleteSingerDialog(Action<bool> okAction = null)
        {
            Icon = FontAwesome.Solid.Globe;
            HeaderText = "Confirm deletion of";
            BodyText = "singer"; //should change to singer name later
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
