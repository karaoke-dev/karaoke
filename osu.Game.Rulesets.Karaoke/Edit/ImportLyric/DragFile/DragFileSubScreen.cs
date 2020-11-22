// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Graphics;
using osu.Framework.Graphics.Sprites;
using osu.Game.Database;
using osu.Game.Rulesets.Karaoke.Edit.ImportLyric.DragFile.Components;
using osuTK;
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
            Schedule(() =>
            {
                var fileInfo = new FileInfo(paths.First());
                DialogOverlay.Push(new ImportLyricDialog(fileInfo, success =>
                {
                    if (success)
                    {
                        Complete();
                    }
                }));
            });
            return Task.CompletedTask;
        }

        public override void Complete()
        {
            ScreenStack.Push(ImportLyricStep.AssignLanguage);
        }
    }
}
