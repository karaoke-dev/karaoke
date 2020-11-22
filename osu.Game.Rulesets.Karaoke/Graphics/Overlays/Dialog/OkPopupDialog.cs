// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Game.Overlays.Dialog;
using System;

namespace osu.Game.Rulesets.Karaoke.Graphics.Overlays.Dialog
{
    public class OkPopupDialog : PopupDialog
    {
        public OkPopupDialog(Action<bool> okAction = null)
        {
            Buttons = new PopupDialogButton[]
            {
                new PopupDialogOkButton
                {
                    Text = @"OK",
                    Action = () => okAction?.Invoke(true),
                },
            };
        }
    }
}
