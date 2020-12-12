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

        public override RectangleF BoundingBox => toTriangle(ToParentSpace(LayoutRectangle)).AABBFloat;

        private static Triangle toTriangle(Quad q) => new Triangle(
            q.TopLeft,
            q.BottomLeft,
            q.BottomRight);

        public override bool Contains(Vector2 screenSpacePos) => toTriangle(ScreenSpaceDrawQuad).Contains(screenSpacePos);

        protected override DrawNode CreateDrawNode() => new TriangleDrawNode(this);

        private class TriangleDrawNode : SpriteDrawNode
        {
            public TriangleDrawNode(RightTriangle source)
                : base(source)
            {
            }

            protected override void Blit(Action<TexturedVertex2D> vertexAction)
            {
                DrawTriangle(Texture, toTriangle(ScreenSpaceDrawQuad), DrawColourInfo.Colour, null, null,
                    new Vector2(InflationAmount.X / DrawRectangle.Width, InflationAmount.Y / DrawRectangle.Height), TextureCoords);
            }

            protected override void BlitOpaqueInterior(Action<TexturedVertex2D> vertexAction)
            {
                var triangle = toTriangle(ConservativeScreenSpaceDrawQuad);

                if (GLWrapper.IsMaskingActive)
                    DrawClipped(ref triangle, Texture, DrawColourInfo.Colour, vertexAction: vertexAction);
                else
                    DrawTriangle(Texture, triangle, DrawColourInfo.Colour, vertexAction: vertexAction);
            }
        }
    }
}
