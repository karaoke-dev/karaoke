// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Sprites;

namespace osu.Game.Rulesets.Karaoke.Edit.ImportLyric.GenerateTimeTag
{
    public class GenerateTimeTagSubScreen : ImportLyricSubScreenWithTopNavigation
    {
        public override string Title => "Generate time tag";

        public override string ShortTitle => "Generate time tag";

        public override ImportLyricStep Step => ImportLyricStep.GenerateTimeTag;

        public override IconUsage Icon => FontAwesome.Solid.Tag;

        protected override TopNavigation CreateNavigation()
            => new GenerateTimeTagNavigation(this);

        protected override Drawable CreateContent()
            => new Container();

        public override void Complete()
        {
            ScreenStack.Push(ImportLyricStep.Success);
        }

        public class GenerateTimeTagNavigation : TopNavigation
        {
            public GenerateTimeTagNavigation(ImportLyricSubScreen screen)
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
