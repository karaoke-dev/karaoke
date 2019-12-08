// Copyright (c) ppy Pty Ltd <contact@ppy.sh>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using osuTK;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Shapes;
using osu.Game.Rulesets.Objects.Drawables;
using osuTK.Graphics;

namespace osu.Game.Rulesets.Karaoke.Objects.Drawables
{
    /// <summary>
    /// Visualises a <see cref="BarLine"/>. Although this derives DrawableKaraokeHitObject,
    /// this does not handle input/sound like a normal hit object.
    /// </summary>
    public class DrawableBarLine : DrawableKaraokeScrollingHitObject<BarLine>
    {
        /// <summary>
        /// Height of major bar line triangles.
        /// </summary>
        private const float triangle_width = 12;

        /// <summary>
        /// Offset of the major bar line triangles from the sides of the bar line.
        /// </summary>
        private const float triangle_offset = 9;

        public DrawableBarLine(BarLine barLine)
            : base(barLine)
        {
            RelativeSizeAxes = Axes.Y;
            Width = 2f;

            AddInternal(new Box
            {
                Name = "Bar line",
                Anchor = Anchor.CentreLeft,
                Origin = Anchor.CentreLeft,
                RelativeSizeAxes = Axes.Both,
                Colour = new Color4(255, 204, 33, 255),
            });

            if (barLine.Major)
            {
                AddInternal(new EquilateralTriangle
                {
                    Name = "Up triangle",
                    Anchor = Anchor.TopCentre,
                    Origin = Anchor.Centre,
                    Size = new Vector2(triangle_width),
                    Y = -triangle_offset,
                    Rotation = 180
                });

                AddInternal(new EquilateralTriangle
                {
                    Name = "Down triangle",
                    Anchor = Anchor.BottomCentre,
                    Origin = Anchor.Centre,
                    Size = new Vector2(triangle_width),
                    Y = triangle_offset,
                    Rotation = 0
                });
            }

            if (!barLine.Major)
                Alpha = 0.2f;
        }

        protected override void UpdateInitialTransforms()
        {
        }

        protected override void UpdateStateTransforms(ArmedState state)
        {
        }
    }
}
