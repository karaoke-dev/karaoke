// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Sprites;
using osu.Game.Database;
using osu.Game.Overlays.Dialog;
using osu.Game.Rulesets.Karaoke.Edit.ImportLyric.DragFile.Components;
using osu.Game.Rulesets.Karaoke.Graphics.Overlays.Dialog;
using osuTK;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace osu.Game.Rulesets.Karaoke.Edit.ImportLyric.DragFile
{
    public class DragFileSubScreen : ImportLyricSubScreen, ICanAcceptFiles
    {
        public override string Title => "Import";
        public override string ShortTitle => "Import";
        public override ImportLyricStep Step => ImportLyricStep.ImportLyric;
        public override IconUsage Icon => FontAwesome.Solid.Upload;

        public IEnumerable<string> HandledExtensions => ImportLyricManager.LyricFormatExtensions;

        [Resolved]
        private ImportLyricManager importManager { get; set; }

        public DragFileSubScreen()
        {
            InternalChild = new DrawableDragFile
            {
                Anchor = Anchor.Centre,
                Origin = Anchor.Centre,
                Size = new Vector2(300, 120)
            };
        }

        public Task Import(params string[] paths)
        {
            var fileInfo = new FileInfo(paths.First());
            ImportLyricFile(fileInfo);
            return Task.CompletedTask;
        }

        public void ImportLyricFile(FileInfo fileInfo)
        {
            Schedule(() =>
            {
                // Check if file exists
                if (!fileInfo.Exists)
                {
                    DialogOverlay.Push(createFileNotFoundDialog());
                    return;
                }

                // Check if format matches
                if (!ImportLyricManager.LyricFormatExtensions.Contains(fileInfo.Extension))
                {
                    DialogOverlay.Push(createFormatNotMatchDialog());
                    return;
                }

                DialogOverlay.Push(new ImportLyricDialog(execute =>
                {
                    if (!execute)
                        return;

                    try
                    {
                        importManager.ImportLrcFile(fileInfo);
                        DialogOverlay.Push(createCompleteDialog());
                    }
                    catch (Exception ex)
                    {
                        switch (ex)
                        {
                            case FileNotFoundException _:
                                DialogOverlay.Push(createFileNotFoundDialog());
                                break;

                            case FileLoadException loadException:
                                DialogOverlay.Push(createLoadExceptionDialog(loadException));
                                break;

                            default:
                                DialogOverlay.Push(createUnknownExceptionDialog());
                                break;
                        }
                    }
                }));
            });
        }

        public override void Complete()
        {
            ScreenStack.Push(ImportLyricStep.AssignLanguage);
        }

        private PopupDialog createFileNotFoundDialog()
            => new OkPopupDialog
            {
                Icon = FontAwesome.Regular.QuestionCircle,
                HeaderText = "Seems file is not exist",
                BodyText = "Drag the file then drop again.",
            };

        private PopupDialog createFormatNotMatchDialog()
            => new OkPopupDialog
            {
                Icon = FontAwesome.Solid.ExclamationTriangle,
                HeaderText = "This type of file is not supported",
                BodyText = "May sure this type of file is supported.",
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

        private PopupDialog createCompleteDialog()
            => new OkPopupDialog(ok =>
            {
                Complete();
            })
            {
                Icon = FontAwesome.Regular.CheckCircle,
                HeaderText = @"Import success",
                BodyText = "Lyrics has been imported."
            };
    }
}
