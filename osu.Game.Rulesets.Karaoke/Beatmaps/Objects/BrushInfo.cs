// Copyright (c) ppy Pty Ltd <contact@ppy.sh>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Collections.Generic;
using osuTK.Graphics;

namespace osu.Game.Rulesets.Karaoke.Beatmaps.Objects
{
    public class BrushInfo
    {
        public BrushType Type { get; set; }

        /// <summary>
        /// Used in <see cref="BrushType.Solid"/>
        /// </summary>
        public Color4 SolidColor { get; set; }

        /// <summary>
        /// Used in <see cref="BrushType.Gradient"/> and <see cref="BrushType.MilleFeuille"/>
        /// </summary>
        public List<BrushGradient> BrushGradients { get; set; }

        public class BrushGradient
        {
            public float XPosition { get; set; }

            public Color4 Color { get; set; }
        }
    }
}
