// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using osu.Framework.Graphics.Sprites;
using osu.Game.Overlays.Dialog;

namespace osu.Game.Rulesets.Karaoke.Screens.Edit.Import.Lyrics.GenerateTimeTag;

public partial class UseAutoGenerateTimeTagPopupDialog : PopupDialog
{
    public UseAutoGenerateTimeTagPopupDialog(Action<bool> okAction)
    {
        Icon = FontAwesome.Solid.Globe;
        HeaderText = "Auto generate time tag";
        BodyText = "Would you like to use time-tag to auto generate each lyric's time tag?";
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
