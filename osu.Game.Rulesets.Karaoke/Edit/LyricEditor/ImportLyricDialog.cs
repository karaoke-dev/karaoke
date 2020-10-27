// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Graphics.Sprites;
using osu.Game.Overlays.Dialog;
using System;

namespace osu.Game.Rulesets.Karaoke.Edit.LyricEditor
{
    public class ImportLyricDialog : PopupDialog
    {
        public ImportLyricDialog(Action resetAction)
        {
            BodyText = "Import lyric file will clean-up all exist lyric.";

            Icon = FontAwesome.Regular.TrashAlt;
            HeaderText = @"Confirm import lyric file?";
            Buttons = new PopupDialogButton[]
            {
                new PopupDialogOkButton
                {
                    Text = @"Yes. Go for it.",
                    Action = resetAction
                },
                new PopupDialogCancelButton
                {
                    Text = @"No! Abort mission!",
                },
            };
        }
    }
}
