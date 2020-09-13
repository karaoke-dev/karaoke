// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osu.Game.Graphics.Containers;
using osuTK;
using osuTK.Graphics;

namespace osu.Game.Rulesets.Karaoke.UI.HUD
{
    /// <summary>
    /// Present setting at right side
    /// </summary>
    public abstract class RightSideOverlay : OsuFocusedOverlayContainer
    {
        public const float SETTING_MARGIN = 20;
        public const float SETTING_SPACING = 20;
        public const float TRANSITION_LENGTH = 600;

        protected override bool DimMainContent => false;

        protected override Container<Drawable> Content => content;

        private readonly FillFlowContainer<Drawable> content;

        public RightSideOverlay()
        {
            InternalChildren = new Drawable[]
            {
                new Box
                {
                    Name = "Background",
                    RelativeSizeAxes = Axes.Both,
                    Colour = Color4.Black,
                    Alpha = 0.6f
                },
                new Container
                {
                    RelativeSizeAxes = Axes.Y,
                    AutoSizeAxes = Axes.X,
                    Child = content = new FillFlowContainer<Drawable>
                    {
                        AutoSizeAxes = Axes.Both,
                        Direction = FillDirection.Vertical,
                        Spacing = new Vector2(SETTING_SPACING),
                        Margin = new MarginPadding(SETTING_MARGIN),
                    }
                }
            };
        }

        [BackgroundDependencyLoader]
        private void load()
        {
            AutoSizeAxes = Axes.X;
        }

        protected override void PopIn()
        {
            base.PopIn();

            this.MoveToX(0, TRANSITION_LENGTH, Easing.OutQuint);
            this.FadeTo(1, TRANSITION_LENGTH, Easing.OutQuint);
        }

        protected override void PopOut()
        {
            base.PopOut();

            var width = DrawWidth;
            this.MoveToX(width, TRANSITION_LENGTH, Easing.OutQuint);
            this.FadeTo(0, TRANSITION_LENGTH, Easing.OutQuint);
        }
    }
}
