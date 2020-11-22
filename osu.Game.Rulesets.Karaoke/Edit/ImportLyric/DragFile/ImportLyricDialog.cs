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
        private ImportLyricManager importManager { get; set; }

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
                // Check file is exist
                if (!info.Exists)
                {
                    dialogOverlay.Push(createFileNotFoundDialog());
                    return false;
                }

                // Check format is match
                if (!ImportLyricManager.LyricFormatExtensions.Contains(info.Extension))
                {
                    dialogOverlay.Push(createFormatNotMatchDialog());
                    return false;
                }

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
                    case FileNotFoundException _:
                        dialogOverlay.Push(createFileNotFoundDialog());
                        break;

                    case FileLoadException loadException:
                        dialogOverlay.Push(createLoadExceptionDialog(loadException));
                        break;
                    default:
                        dialogOverlay.Push(createUnknownExceptionDialog());
                        break;
                }

                return false;
            }
        }

        private PopupDialog createFileNotFoundDialog()
            => new OkPopupDialog
            {
                Icon = FontAwesome.Regular.QuestionCircle,
                HeaderText = "Seems file is not exist",
                BodyText = $"Drag the file then drop again.",
            };

        private PopupDialog createFormatNotMatchDialog()
            => new OkPopupDialog
            {
                Icon = FontAwesome.Solid.ExclamationTriangle,
                HeaderText = "This type of file is not supported",
                BodyText = $"May sure this type of file is supported.",
            };

        private PopupDialog createLoadExceptionDialog(FileLoadException loadException)
            => new OkPopupDialog
            {
                Icon = FontAwesome.Solid.Bug,
                HeaderText = @"File loading error",
                BodyText = loadException.Message,
            };

        private PopupDialog createUnknownExceptionDialog()
            => new OkPopupDialog
            {
                Icon = FontAwesome.Solid.Bug,
                HeaderText = @"Unknown error",
                BodyText = @"Unknown error QAQa."
            };
    }
}
