// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using osu.Framework.Graphics.Sprites;
using osu.Game.Overlays.Dialog;

namespace osu.Game.Rulesets.Karaoke.Edit.ImportLyric.GenerateRubyRomaji
{
    public class UseAutoGenerateRomajiPopupDialog : PopupDialog
    {
        public UseAutoGenerateRomajiPopupDialog(Action<bool> okAction = null)
        {
            Icon = FontAwesome.Solid.Globe;
            HeaderText = "Auto generate romaji";
            BodyText = "Would you like to use romaji generator to auto generate each lyric's romaji?";
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
