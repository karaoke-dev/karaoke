// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Collections.Generic;
using osu.Framework.Graphics.Shaders;
using osu.Framework.Graphics.Sprites;

namespace osu.Game.Rulesets.Karaoke.Skinning.Metadatas
{
    public class LyricStyle
    {
        public string Name { get; set; }

        /// <summary>
        ///  todo: should use <see cref="ICustomizedShader"/> instead because we should save <see cref="StepShader"/> also.
        /// </summary>
        public List<IShader> LeftLyricTextShaders = new();

        /// <summary>
        ///  todo: should use <see cref="ICustomizedShader"/> instead because we should save <see cref="StepShader"/> also.
        /// </summary>
        public List<IShader> RightLyricTextShaders = new();

        // todo: should be moved into config.
        public FontUsage MainTextFont { get; set; } = new("Torus", 48, "Bold");

        // todo: should be moved into config.
        public FontUsage RubyTextFont { get; set; } = new("Torus", 20, "Bold");

        // todo: should be moved into config.
        public FontUsage RomajiTextFont { get; set; } = new("Torus", 20, "Bold");
    }
}
