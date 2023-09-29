// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using osu.Framework.Graphics.Sprites;
using osu.Game.Overlays.Dialog;

namespace osu.Game.Rulesets.Karaoke.Screens.Edit.Import.Lyrics.GenerateRuby;

public partial class UseAutoGenerateRubyPopupDialog : PopupDialog
{
    public UseAutoGenerateRubyPopupDialog(Action<bool> okAction)
    {
        Icon = FontAwesome.Solid.Globe;
        HeaderText = "Auto generate ruby";
        BodyText = "Would you like to use ruby generator to auto generate each lyric's ruby?";
        Buttons = new PopupDialogButton[]
        {
            new PopupDialogOkButton
            {
                Text = "OK",
                Action = () => okAction(true),
            },
            new PopupDialogCancelButton
            {
                Text = "Cancel",
                Action = () => okAction(false),
            },
        };
    }
}
