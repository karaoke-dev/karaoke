// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using System.Collections.Generic;
using System.Linq;
using osu.Framework;
using osu.Framework.Extensions;
using osu.Framework.Graphics.Shaders;
using osu.Game.Rulesets.Karaoke.Skinning.Metadatas;

namespace osu.Game.Rulesets.Karaoke.Skinning.Tools
{
    // it's the temp logic to collect logic.
    public static class SkinConvertorTool
    {
        public static ICustomizedShader[] ConvertLeftSideShader(ShaderManager shaderManager, LyricStyle lyricStyle)
        {
            if (shaderManager == null)
                return null;

            // todo: do not use outline shader in other platform because seems osx use OpenGL 2.X
            // which will cause shader compile failed.
            if (RuntimeInfo.OS != RuntimeInfo.Platform.Windows)
                return null;

            var shaders = lyricStyle.LeftLyricTextShaders?.ToArray() ?? Array.Empty<ICustomizedShader>();
            attachShaders(shaderManager, shaders);

            return shaders;
        }

        public static ICustomizedShader[] ConvertRightSideShader(ShaderManager shaderManager, LyricStyle lyricStyle)
        {
            if (shaderManager == null)
                return null;

            // todo: do not use outline shader in other platform because seems osx use OpenGL 2.X
            // which will cause shader compile failed.
            if (RuntimeInfo.OS != RuntimeInfo.Platform.Windows)
                return null;

            var shaders = lyricStyle.RightLyricTextShaders?.ToArray() ?? Array.Empty<ICustomizedShader>();
            attachShaders(shaderManager, shaders);

            return shaders;
        }

        private static void attachShaders(ShaderManager shaderManager, IEnumerable<ICustomizedShader> shaders)
        {
            foreach (var shader in shaders)
            {
                switch (shader)
                {
                    case InternalShader internalShader:
                        shaderManager.AttachShader(internalShader);
                        break;

                    case StepShader stepShader:
                        attachShaders(shaderManager, stepShader.StepShaders.ToArray());
                        break;

                    default:
                        throw new InvalidCastException($"{shader?.GetType()} cannot attach shader.");
                }
            }
        }
    }
}
