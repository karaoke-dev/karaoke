// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using osu.Game.Overlays.Dialog;

namespace osu.Game.Rulesets.Karaoke.Screens.Edit.Import.Lyrics;

public partial class RollBackPopupDialog : PopupDialog
{
    public RollBackPopupDialog(ILyricImporterStepScreen screen, Action<bool> okAction)
    {
        Icon = screen.Icon;
        HeaderText = screen.ShortTitle;
        BodyText = $"Will roll-back to step '{screen.Title}'";
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
