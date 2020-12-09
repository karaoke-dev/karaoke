// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using osu.Framework.Graphics.Sprites;
using osu.Game.Overlays.Dialog;
using osu.Game.Rulesets.Karaoke.Skinning.Components;

namespace osu.Game.Rulesets.Karaoke.Edit.Layout.Components
{
    public class DeleteLayoutDialog : PopupDialog
    {
        public DeleteLayoutDialog(KaraokeLayout layout, Action deleteAction)
        {
            HeaderText = "Confirm deletion of";
            BodyText = $"Are you sure you wany to delete layout {layout.Name}?";

            Icon = FontAwesome.Regular.TrashAlt;

            Buttons = new PopupDialogButton[]
            {
                new PopupDialogOkButton
                {
                    Text = @"Yes. Go for it.",
                    Action = deleteAction
                },
                new PopupDialogCancelButton
                {
                    Text = @"No! Abort mission!",
                },
            };
        }
    }
}
