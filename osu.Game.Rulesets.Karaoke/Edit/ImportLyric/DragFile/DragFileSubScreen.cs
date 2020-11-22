// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Graphics.Sprites;

namespace osu.Game.Rulesets.Karaoke.Edit.ImportLyric.DragFile
{
    public class DragFileSubScreen : ImportLyricSubScreen
    {
        public override string Title => "Import";

        public override string ShortTitle => "Import";

        public override ImportLyricStep Step => ImportLyricStep.ImportLyric;

        public override IconUsage Icon => FontAwesome.Solid.Upload;

        public override void Complete()
        {
            ScreenStack.Push(ImportLyricStep.AssignLanguage);
        }
    }
}
