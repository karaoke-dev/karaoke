// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using osu.Game.Overlays.Dialog;

namespace osu.Game.Rulesets.Karaoke.Screens.Edit.Import.Lyrics;

public partial class RollBackResetPopupDialog : PopupDialog
{
    public RollBackResetPopupDialog(ILyricImporterStepScreen screen, Action<bool> okAction)
    {
        Icon = screen.Icon;
        HeaderText = "Really sure?";
        BodyText = $"Are you really sure you want to roll-back to step '{screen.Title}'? You might lost every change you made.";
        Buttons = new PopupDialogButton[]
        {
            new PopupDialogDangerousButton
            {
                Text = "Forget all changes",
                Action = () => okAction(true),
            },
            new PopupDialogCancelButton
            {
                Text = "Let me think about it",
                Action = () => okAction(false),
            },
        };
    }
}
