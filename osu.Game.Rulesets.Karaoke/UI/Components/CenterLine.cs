// Copyright (c) ppy Pty Ltd <contact@ppy.sh>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osu.Game.Graphics;

namespace osu.Game.Rulesets.Karaoke.UI.Components
{
    public class CenterLine : Container
    {
        private readonly Box centerLineBox;

        public CenterLine()
        {
            RelativeSizeAxes = Axes.X;
            Height = ColumnBackground.COLUMN_HEIGHT;

            Child = centerLineBox = new Box
            {
                RelativeSizeAxes = Axes.Both,
                Alpha = 0.1f
            };
        }

        [BackgroundDependencyLoader]
        private void load(OsuColour colours)
        {
            centerLineBox.Colour = colours.Red;
        }
    }
}
