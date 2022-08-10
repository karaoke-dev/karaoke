// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Primitives;
using osu.Framework.Graphics.Rendering;
using osu.Framework.Graphics.Sprites;
using osuTK;

namespace osu.Game.Rulesets.Karaoke.Graphics.Shapes
{
    public class RightTriangle : Sprite
    {
        /// <summary>
        /// Creates a new right triangle with a white pixel as texture.
        /// </summary>
        public RightTriangle()
        {
            // Setting the texture would normally set a size of (1, 1), but since the texture is set from BDL it needs to be set here instead.
            // RelativeSizeAxes may not behave as expected if this is not done.
            Size = Vector2.One;
        }

        [BackgroundDependencyLoader]
        private void load(IRenderer renderer)
        {
            Texture ??= renderer.WhitePixel;
        }

        public override RectangleF BoundingBox => toTriangle(ToParentSpace(LayoutRectangle), RightAngleDirection).AABBFloat;

        private TriangleRightAngleDirection rightAngleDirection = TriangleRightAngleDirection.BottomLeft;

        public TriangleRightAngleDirection RightAngleDirection
        {
            get => rightAngleDirection;
            set
            {
                rightAngleDirection = value;
                Invalidate();
            }
        }

        private static Triangle toTriangle(Quad q, TriangleRightAngleDirection rightAngleDirection) =>
            rightAngleDirection switch
            {
                TriangleRightAngleDirection.TopLeft => new Triangle(q.TopLeft, q.TopRight, q.BottomLeft),
                TriangleRightAngleDirection.TopRight => new Triangle(q.TopLeft, q.TopRight, q.BottomRight),
                TriangleRightAngleDirection.BottomLeft => new Triangle(q.TopLeft, q.BottomLeft, q.BottomRight),
                TriangleRightAngleDirection.BottomRight => new Triangle(q.TopRight, q.BottomLeft, q.BottomRight),
                _ => throw new ArgumentOutOfRangeException(nameof(rightAngleDirection), rightAngleDirection, null)
            };

        public override bool Contains(Vector2 screenSpacePos) => toTriangle(ScreenSpaceDrawQuad, RightAngleDirection).Contains(screenSpacePos);

        protected override DrawNode CreateDrawNode() => new TriangleDrawNode(this);

        private class TriangleDrawNode : SpriteDrawNode
        {
            protected new RightTriangle Source => (RightTriangle)base.Source;

            private TriangleRightAngleDirection rightAngleDirection;

            public TriangleDrawNode(RightTriangle source)
                : base(source)
            {
            }

            public override void ApplyState()
            {
                base.ApplyState();

                rightAngleDirection = Source.RightAngleDirection;
            }

            protected override void Blit(IRenderer renderer)
            {
                if (DrawRectangle.Width == 0 || DrawRectangle.Height == 0)
                    return;

                renderer.DrawTriangle(Texture, toTriangle(ScreenSpaceDrawQuad, rightAngleDirection), DrawColourInfo.Colour, null, null,
                    new Vector2(InflationAmount.X / DrawRectangle.Width, InflationAmount.Y / DrawRectangle.Height), TextureCoords);
            }

            protected override void BlitOpaqueInterior(IRenderer renderer)
            {
                if (DrawRectangle.Width == 0 || DrawRectangle.Height == 0)
                    return;

                var triangle = toTriangle(ConservativeScreenSpaceDrawQuad, rightAngleDirection);

                if (renderer.IsMaskingActive)
                    renderer.DrawClipped(ref triangle, Texture, DrawColourInfo.Colour);
                else
                    renderer.DrawTriangle(Texture, triangle, DrawColourInfo.Colour);
            }
        }
    }

    public enum TriangleRightAngleDirection
    {
        TopLeft,

        TopRight,

        BottomLeft,

        BottomRight,
    }
}
