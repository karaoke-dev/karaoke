// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

#nullable disable

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

            Child = centerLineBox = new Box
            {
                RelativeSizeAxes = Axes.Both,
            };
        }

        [BackgroundDependencyLoader]
        private void load(OsuColour colours)
        {
            centerLineBox.Colour = colours.Red;
        }
    }
}
