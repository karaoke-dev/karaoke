// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Graphics.Sprites;
using osu.Game.Overlays.Dialog;
using System;

namespace osu.Game.Rulesets.Karaoke.Edit.ImportLyric.GenerateRuby
{
    public class UseAutoGenerateRubyPopupDialog : PopupDialog
    {
        public UseAutoGenerateRubyPopupDialog(Action<bool> okAction = null)
        {
            Icon = FontAwesome.Solid.Globe;
            HeaderText = "Auto generate ruby";
            BodyText = $"Would you like to use language detector to auto generate each lyric's ruby?";
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
