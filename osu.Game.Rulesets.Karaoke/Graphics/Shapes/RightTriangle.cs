// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using osu.Framework.Graphics;
using osu.Framework.Graphics.OpenGL;
using osu.Framework.Graphics.OpenGL.Vertices;
using osu.Framework.Graphics.Primitives;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Graphics.Textures;
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
            Texture = Texture.WhitePixel;
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

            protected override void Blit(Action<TexturedVertex2D> vertexAction)
            {
                DrawTriangle(Texture, toTriangle(ScreenSpaceDrawQuad, rightAngleDirection), DrawColourInfo.Colour, null, null,
                    new Vector2(InflationAmount.X / DrawRectangle.Width, InflationAmount.Y / DrawRectangle.Height), TextureCoords);
            }

            protected override void BlitOpaqueInterior(Action<TexturedVertex2D> vertexAction)
            {
                var triangle = toTriangle(ConservativeScreenSpaceDrawQuad, rightAngleDirection);

                if (GLWrapper.IsMaskingActive)
                    DrawClipped(ref triangle, Texture, DrawColourInfo.Colour, vertexAction: vertexAction);
                else
                    DrawTriangle(Texture, triangle, DrawColourInfo.Colour, vertexAction: vertexAction);
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
