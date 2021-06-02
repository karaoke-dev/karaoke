// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osu.Game.Graphics;
using osuTK;

namespace osu.Game.Rulesets.Karaoke.Edit.Lyrics.Rows.Extends.Components.TimeTagEditor
{
    public class CurrentTimeMarker : CompositeDrawable
    {
        private const float triangle_width = 15;
        private const float triangle_height = 10;
        private const float bar_width = 2;

        public CurrentTimeMarker()
        {
            RelativeSizeAxes = Axes.Y;
            Size = new Vector2(triangle_width, 1);

            InternalChildren = new Drawable[]
            {
                new Box
                {
                    Anchor = Anchor.Centre,
                    Origin = Anchor.Centre,
                    RelativeSizeAxes = Axes.Y,
                    Width = bar_width,
                },
                new Triangle
                {
                    Anchor = Anchor.TopCentre,
                    Origin = Anchor.BottomCentre,
                    Size = new Vector2(triangle_width, triangle_height),
                    Scale = new Vector2(1, -1)
                },
                new Triangle
                {
                    Anchor = Anchor.BottomCentre,
                    Origin = Anchor.BottomCentre,
                    Size = new Vector2(triangle_width, triangle_height),
                }
            };
        }

        [BackgroundDependencyLoader]
        private void load(OsuColour colours)
        {
            Colour = colours.RedDark;
        }
    }
}
