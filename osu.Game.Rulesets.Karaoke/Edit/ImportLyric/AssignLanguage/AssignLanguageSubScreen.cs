// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Graphics.Sprites;

namespace osu.Game.Rulesets.Karaoke.Edit.ImportLyric.AssignLanguage
{
    public class AssignLanguageSubScreen : ImportLyricSubScreen
    {
        public override string Title => "Language";

        public override string ShortTitle => "Language";

        public override ImportLyricStep Step => ImportLyricStep.AssignLanguage;

        public override IconUsage Icon => FontAwesome.Solid.Globe;

        public override void Complete()
        {
            ScreenStack.Push(ImportLyricStep.GenerateRuby);
        }
    }
}
