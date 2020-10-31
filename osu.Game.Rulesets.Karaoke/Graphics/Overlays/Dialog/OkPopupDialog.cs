// Copyright (c) ppy Pty Ltd <contact@ppy.sh>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Game.Overlays.Dialog;

namespace osu.Game.Rulesets.Karaoke.Graphics.Overlays.Dialog
{
    public class OkPopupDialog : PopupDialog
    {
        public OkPopupDialog()
        {
            Buttons = new PopupDialogButton[]
            {
                new PopupDialogCancelButton
                {
                    Text = @"OK",
                },
            };
        }
    }
}
