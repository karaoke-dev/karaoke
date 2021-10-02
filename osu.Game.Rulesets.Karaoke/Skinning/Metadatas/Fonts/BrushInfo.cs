// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Collections.Generic;
using osuTK.Graphics;

namespace osu.Game.Rulesets.Karaoke.Skinning.Metadatas.Fonts
{
    // todo : this shit will be replaced by shader.
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
