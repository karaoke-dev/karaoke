// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

namespace osu.Game.Rulesets.Karaoke.Edit.ImportLyric.GenerateRuby
{
    public class GenerateRubySubScreen : ImportLyricSubScreen
    {
        public override string Title => "Generate ruby";

        public override string ShortTitle => "Generate ruby";

        public override ImportLyricStep Step => ImportLyricStep.GenerateRuby;

        public override void Complete()
        {
            ScreenStack.Push(ImportLyricStep.GenerateTimeTag);
        }
    }
}
