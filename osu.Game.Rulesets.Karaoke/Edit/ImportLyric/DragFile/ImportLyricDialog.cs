// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using System.IO;
using System.Linq;
using osu.Framework.Allocation;
using osu.Framework.Graphics.Sprites;
using osu.Game.Overlays;
using osu.Game.Overlays.Dialog;
using osu.Game.Rulesets.Karaoke.Graphics.Overlays.Dialog;

namespace osu.Game.Rulesets.Karaoke.Edit.ImportLyric.DragFile
{
    public class ImportLyricDialog : PopupDialog
    {
        [Resolved]
        private DialogOverlay dialogOverlay { get; set; }

        public ImportLyricDialog(FileInfo info, Action<bool> resetAction = null)
        {
            Icon = FontAwesome.Regular.TrashAlt;
            HeaderText = @"Confirm import lyric file?";
            BodyText = "Import lyric file will clean-up all exist lyric.";

            Buttons = new PopupDialogButton[]
            {
                new PopupDialogOkButton
                {
                    Text = @"Yes. Go for it.",
                    Action = () => resetAction?.Invoke(true),
                },
                new PopupDialogCancelButton
                {
                    Text = @"No! Abort mission!",
                    Action = () => resetAction?.Invoke(false),
                },
            };
        }
    }
}
