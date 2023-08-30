// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using osu.Framework.Graphics.Sprites;
using osu.Game.Overlays.Dialog;

namespace osu.Game.Rulesets.Karaoke.Screens.Edit.Import.Lyrics.DragFile;

public partial class ImportLyricDialog : PopupDialog
{
    public ImportLyricDialog(Action<bool> resetAction)
    {
        Icon = FontAwesome.Regular.TrashAlt;
        HeaderText = "Confirm import lyric file?";
        BodyText = "Import lyric file will clean-up all exist lyric.";

        Buttons = new PopupDialogButton[]
        {
            new PopupDialogOkButton
            {
                Text = "Yes. Go for it.",
                Action = () => resetAction(true),
            },
            new PopupDialogCancelButton
            {
                Text = "No! Abort mission!",
                Action = () => resetAction(false),
            },
        };
    }
}
