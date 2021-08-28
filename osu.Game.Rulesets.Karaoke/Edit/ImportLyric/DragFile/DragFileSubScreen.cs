// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Screens;
using osu.Game.Database;
using osu.Game.Overlays.Dialog;
using osu.Game.Rulesets.Karaoke.Edit.ImportLyric.DragFile.Components;
using osu.Game.Rulesets.Karaoke.Graphics.Overlays.Dialog;

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

        [Resolved]
        private OsuGameBase game { get; set; }

        public DragFileSubScreen()
        {
            InternalChild = new DrawableDragFile
            {
                Anchor = Anchor.Centre,
                Origin = Anchor.Centre,
                RelativeSizeAxes = Axes.Both,
                Padding = new MarginPadding(64),
                Import = path =>
                {
                    Task.Factory.StartNew(async () =>
                    {
                        await Import(path).ConfigureAwait(false);
                    }, TaskCreationOptions.LongRunning);
                }
            };
        }

        public Task Import(params string[] paths)
        {
            var fileInfo = new FileInfo(paths.First());
            ImportLyricFile(fileInfo);
            return Task.CompletedTask;
        }

        public Task Import(params ImportTask[] tasks)
        {
            // todo : wail until really implement needed.
            throw new NotImplementedException("Report to https://github.com/karaoke-dev/karaoke and i will implement it.");
        }

        public void ImportLyricFile(FileInfo fileInfo)
        {
            Schedule(() =>
            {
                // Check file is exist
                if (!fileInfo.Exists)
                {
                    DialogOverlay.Push(createFileNotFoundDialog());
                    return;
                }

                // Check format is match
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
            ScreenStack.Push(ImportLyricStep.EditLyric);
        }

        public override void OnEntering(IScreen last)
        {
            game.RegisterImportHandler(this);
            base.OnEntering(last);
        }

        public override void OnResuming(IScreen last)
        {
            game.RegisterImportHandler(this);
            base.OnResuming(last);
        }

        public override void OnSuspending(IScreen next)
        {
            game.UnregisterImportHandler(this);
            base.OnSuspending(next);
        }

        public override bool OnExiting(IScreen next)
        {
            game.UnregisterImportHandler(this);
            return base.OnExiting(next);
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
            => new OkPopupDialog(ok => { Complete(); })
            {
                Icon = FontAwesome.Regular.CheckCircle,
                HeaderText = @"Import success",
                BodyText = "Lyrics has been imported."
            };
    }
}
