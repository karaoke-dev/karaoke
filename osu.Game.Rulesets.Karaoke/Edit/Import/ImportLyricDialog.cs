// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using System.IO;
using osu.Framework.Allocation;
using osu.Framework.Graphics.Sprites;
using osu.Game.Overlays;
using osu.Game.Overlays.Dialog;
using osu.Game.Rulesets.Karaoke.Graphics.Overlays.Dialog;

namespace osu.Game.Rulesets.Karaoke.Edit.Import
{
    public class ImportLyricDialog : PopupDialog
    {
        [Resolved]
        private ImportManager importManager { get; set; }

        [Resolved]
        private DialogOverlay dialogOverlay { get; set; }

        public ImportLyricDialog(FileInfo info, Action<bool> resetAction = null)
        {
            BodyText = "Import lyric file will clean-up all exist lyric.";

            Icon = FontAwesome.Regular.TrashAlt;
            HeaderText = @"Confirm import lyric file?";
            Buttons = new PopupDialogButton[]
            {
                new PopupDialogOkButton
                {
                    Text = @"Yes. Go for it.",
                    Action = () =>
                    {
                        var success = processImport(info);
                        resetAction?.Invoke(success);
                    }
                },
                new PopupDialogCancelButton
                {
                    Text = @"No! Abort mission!",
                },
            };
        }

        private bool processImport(FileInfo info)
        {
            try
            {
                importManager.ImportLrcFile(info);
                dialogOverlay.Push(new OkPopupDialog
                {
                    Icon = FontAwesome.Regular.CheckCircle,
                    HeaderText = @"Import success",
                    BodyText = "Lyrics has been imported."
                });
                return true;
            }
            catch (Exception ex)
            {
                switch (ex)
                {
                    case FileNotFoundException fileNotFoundException:
                        dialogOverlay.Push(new OkPopupDialog
                        {
                            Icon = FontAwesome.Regular.QuestionCircle,
                            HeaderText = @"File not found",
                            BodyText = fileNotFoundException.Message,
                        });
                        break;

                    case FileLoadException loadException:
                        dialogOverlay.Push(new OkPopupDialog
                        {
                            Icon = FontAwesome.Regular.QuestionCircle,
                            HeaderText = @"File not found",
                            BodyText = loadException.Message,
                        });
                        break;
                }

                return false;
            }
        }
    }
}
