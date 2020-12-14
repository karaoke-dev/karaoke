// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osu.Game.Graphics;
using osuTK;

namespace osu.Game.Rulesets.Karaoke.Edit.Lyrics.Lyrics.Components
{
    public class DrawableLyricSplitterCursor : CompositeDrawable
    {
        public DrawableLyricSplitterCursor()
        {
            RelativeSizeAxes = Axes.Y;
            AutoSizeAxes = Axes.X;
            InternalChildren = new Drawable[]
            {
                new Triangle
                {
                    Anchor = Anchor.TopCentre,
                    Origin = Anchor.BottomCentre,
                    Scale = new Vector2(1, -1),
                    Size = new Vector2(10, 5),
                },
                new Triangle
                {
                    Anchor = Anchor.BottomCentre,
                    Origin = Anchor.BottomCentre,
                    Size = new Vector2(10, 5)
                },
                new Box
                {
                    Anchor = Anchor.Centre,
                    Origin = Anchor.Centre,
                    RelativeSizeAxes = Axes.Y,
                    Width = 2,
                    EdgeSmoothness = new Vector2(1, 0)
                }
            };
        }

        [BackgroundDependencyLoader]
        private void load(OsuColour colours) => Colour = colours.Red;
    }
}
