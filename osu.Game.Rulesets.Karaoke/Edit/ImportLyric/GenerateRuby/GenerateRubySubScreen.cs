// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Sprites;

namespace osu.Game.Rulesets.Karaoke.Edit.ImportLyric.GenerateRuby
{
    public class GenerateRubySubScreen : ImportLyricSubScreenWithTopNavigation
    {
        public override string Title => "Generate ruby";

        public override string ShortTitle => "Generate ruby";

        public override ImportLyricStep Step => ImportLyricStep.GenerateRuby;

        public override IconUsage Icon => FontAwesome.Solid.Gem;

        protected override TopNavigation CreateNavigation()
           => new GenerateRubyNavigation(this);

        protected override Drawable CreateContent()
            => new Container();

        public override void Complete()
        {
            ScreenStack.Push(ImportLyricStep.GenerateTimeTag);
        }

        public class GenerateRubyNavigation : TopNavigation
        {
            public GenerateRubyNavigation(ImportLyricSubScreen screen)
                : base(screen)
            {
            }

            protected override void UpdateState(NavigationState value)
            {
                base.UpdateState(value);

                // todo : update text
            }
        }
    }
}
