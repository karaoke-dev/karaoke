// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Graphics.Sprites;

namespace osu.Game.Rulesets.Karaoke.Edit.ImportLyric.GenerateRuby
{
    public class GenerateRubySubScreen : ImportLyricSubScreenWithTopNavigation
    {
        public override string Title => "Generate ruby";

        public override string ShortTitle => "Generate ruby";

        public override ImportLyricStep Step => ImportLyricStep.GenerateRuby;

        public override IconUsage Icon => FontAwesome.Solid.Gem;

        public override void Complete()
        {
            ScreenStack.Push(ImportLyricStep.GenerateTimeTag);
        }
    }
}
