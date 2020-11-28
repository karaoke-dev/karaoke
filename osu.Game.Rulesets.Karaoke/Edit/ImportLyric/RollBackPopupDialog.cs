// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Game.Overlays.Dialog;
using System;

namespace osu.Game.Rulesets.Karaoke.Edit.ImportLyric
{
    public class RollBackPopupDialog : PopupDialog
    {
        public RollBackPopupDialog(IImportLyricSubScreen screen, Action<bool> okAction = null)
        {
            Icon = screen.Icon;
            HeaderText = screen.ShortTitle;
            BodyText = $"Will roll-back to step '{screen.Title}'";
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
