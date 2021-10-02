// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using osu.Framework.Graphics.Shaders;

namespace osu.Game.Rulesets.Karaoke.Extensions
{
    public static class ShaderManagerExtensions
    {
        public static T LocalCustomizedShader<T>(this ShaderManager shaderManager) where T : class, ICustomizedShader
        {
            var type = typeof(T);

            return type switch
            {
                Type _ when type == typeof(OutlineShader) => new OutlineShader(shaderManager.Load(VertexShaderDescriptor.TEXTURE_2, OutlineShader.SHADER_NAME)) as T,
                Type _ when type == typeof(RainbowShader) => new RainbowShader(shaderManager.Load(VertexShaderDescriptor.TEXTURE_2, RainbowShader.SHADER_NAME)) as T,
                Type _ when type == typeof(ShadowShader) => new ShadowShader(shaderManager.Load(VertexShaderDescriptor.TEXTURE_2, ShadowShader.SHADER_NAME)) as T,
                _ => throw new NotImplementedException()
            };
        }
    }
}
