// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Collections.Generic;
using osu.Framework.Graphics.Shaders;

namespace osu.Game.Rulesets.Karaoke.Skinning.Metadatas
{
    public class LyricStyle
    {
        public string Name { get; set; }

        /// <summary>
        ///  todo: should use <see cref="ICustomizedShader"/> instead because we should save <see cref="StepShader"/> also.
        /// </summary>
        public List<ICustomizedShader> LeftLyricTextShaders = new();

        /// <summary>
        ///  todo: should use <see cref="ICustomizedShader"/> instead because we should save <see cref="StepShader"/> also.
        /// </summary>
        public List<ICustomizedShader> RightLyricTextShaders = new();
    }
}
