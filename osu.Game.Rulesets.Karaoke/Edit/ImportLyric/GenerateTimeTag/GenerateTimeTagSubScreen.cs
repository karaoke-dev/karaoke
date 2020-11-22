// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Graphics.Sprites;

namespace osu.Game.Rulesets.Karaoke.Edit.ImportLyric.GenerateTimeTag
{
    public class GenerateTimeTagSubScreen : ImportLyricSubScreen
    {
        public override string Title => "Generate time tag";

        public override string ShortTitle => "Generate time tag";

        public override ImportLyricStep Step => ImportLyricStep.GenerateTimeTag;

        public override IconUsage Icon => FontAwesome.Solid.Tag;

        public override void Complete()
        {
            ScreenStack.Push(ImportLyricStep.Success);
        }
    }
}
