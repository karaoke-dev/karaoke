// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using osu.Game.Overlays.Dialog;

namespace osu.Game.Rulesets.Karaoke.Edit.ImportLyric
{
    public class RollBackResetPopupDialog : PopupDialog
    {
        public RollBackResetPopupDialog(ILyricImporterStepScreen screen, Action<bool> okAction = null)
        {
            Icon = screen.Icon;
            HeaderText = "Really sure?";
            BodyText = $"Are you really sure you want to roll-back to step '{screen.Title}'? You might lost every change you made.";
            Buttons = new PopupDialogButton[]
            {
                new PopupDialogDangerousButton
                {
                    Text = @"Forget all changes",
                    Action = () => okAction?.Invoke(true),
                },
                new PopupDialogCancelButton
                {
                    Text = @"Let me think about it",
                    Action = () => okAction?.Invoke(false),
                },
            };
        }
    }
}
