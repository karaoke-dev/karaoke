// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using osu.Framework.Graphics.Shaders;

namespace osu.Game.Rulesets.Karaoke.Extensions
{
    /// <summary>
    /// Holds extension methods for <see cref="IShader"/>.
    /// </summary>
    public static class ShaderExtension
    {
        /// <summary>
        /// Adjusts specified properties of a <see cref="IShader"/>.
        /// </summary>
        /// <param name="drawable">The <see cref="IShader"/> whose properties should be adjusted.</param>
        /// <param name="adjustment">The adjustment function.</param>
        /// <returns>The given <see cref="IShader"/>.</returns>
        public static T With<T>(this T drawable, Action<T> adjustment)
            where T : IShader
        {
            adjustment?.Invoke(drawable);
            return drawable;
        }
    }
}
