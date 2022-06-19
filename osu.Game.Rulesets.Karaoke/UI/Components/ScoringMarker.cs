// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osu.Game.Graphics;
using osuTK;

namespace osu.Game.Rulesets.Karaoke.UI.Components
{
    public class ScoringMarker : CompositeDrawable
    {
        private const float triangle_width = 20;
        private const float triangle_height = 20;

        public ScoringMarker()
        {
            AutoSizeAxes = Axes.Both;
            InternalChildren = new Drawable[]
            {
                new Triangle
                {
                    Size = new Vector2(triangle_width, triangle_height),
                    Rotation = 90
                }
            };
        }

        [BackgroundDependencyLoader]
        private void load(OsuColour colours)
        {
            Colour = colours.Yellow;
        }
    }
}
