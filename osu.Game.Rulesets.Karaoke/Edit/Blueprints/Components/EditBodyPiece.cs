// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osu.Game.Graphics;
using osu.Game.Rulesets.Karaoke.Objects.Drawables.Pieces;

namespace osu.Game.Rulesets.Karaoke.Edit.Blueprints.Components
{
    public class EditBodyPiece : Container
    {
        [BackgroundDependencyLoader]
        private void load(OsuColour colours)
        {
            Masking = true;
            BorderColour = colours.Yellow;
            BorderThickness = 2;
            CornerRadius = DefaultBorderBodyPiece.CORNER_RADIUS;
            Child = new Box
            {
                RelativeSizeAxes = Axes.Both,
                AlwaysPresent = true,
                Alpha = 0
            };
        }
    }
}
