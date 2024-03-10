// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using osu.Framework.Graphics.Sprites;
using osu.Game.Overlays.Dialog;

namespace osu.Game.Rulesets.Karaoke.Screens.Edit.Import.Lyrics.GenerateTimeTag;

public partial class UseAutoGenerateRomanisationPopupDialog : PopupDialog
{
    public UseAutoGenerateRomanisationPopupDialog(Action<bool> okAction)
    {
        Icon = FontAwesome.Solid.Globe;
        HeaderText = "Auto generate romanisation";
        BodyText = "Would you like to use generator to auto generate each lyric's romanisation?";
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
