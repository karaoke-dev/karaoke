// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Graphics.Sprites;
using osu.Game.Overlays.Dialog;
using System;

namespace osu.Game.Rulesets.Karaoke.Edit.ImportLyric.GenerateTimeTag
{
    public class UseAutoGenerateTimeTagPopupDialog : PopupDialog
    {
        public UseAutoGenerateTimeTagPopupDialog(Action<bool> okAction = null)
        {
            Icon = FontAwesome.Solid.Globe;
            HeaderText = "Auto generate time tag";
            BodyText = "Would you like to use time-tag to auto generate each lyric's time tag?";
            Buttons = new PopupDialogButton[]
            {
                new PopupDialogOkButton
                {
                    Text = @"OK",
                    Action = () => okAction?.Invoke(true),
                },
                new PopupDialogCancelButton
                {
                    Text = @"Cancel",
                    Action = () => okAction?.Invoke(false),
                },
            };
        }
    }
}
