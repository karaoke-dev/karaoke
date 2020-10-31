// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Cursor;
using osu.Framework.Graphics.Shapes;
using osu.Game.Graphics;
using osuTK;

namespace osu.Game.Rulesets.Karaoke.Graphics.Cursor
{
    public abstract class BackgroundToolTip : VisibilityContainer, ITooltip
    {
        private readonly Box background;
        private readonly Container content;

        protected override Container<Drawable> Content => content;

        protected BackgroundToolTip()
        {
            AutoSizeAxes = Axes.Both;
            Masking = true;
            CornerRadius = 5;

            InternalChildren = new Drawable[]
            {
                background = new Box
                {
                    RelativeSizeAxes = Axes.Both
                },
                content = new Container
                {
                    AutoSizeAxes = Axes.Both,
                    AutoSizeDuration = 200,
                    AutoSizeEasing = Easing.OutQuint,
                    Padding = new MarginPadding(10)
                }
            };
        }

        [BackgroundDependencyLoader]
        private void load(OsuColour colours)
        {
            background.Colour = colours.Gray3;
        }

        public abstract bool SetContent(object content);

        public void Move(Vector2 pos) => Position = pos;

        protected override void PopIn() => this.FadeIn(200, Easing.OutQuint);

        protected override void PopOut() => this.FadeOut(200, Easing.OutQuint);
    }
}
