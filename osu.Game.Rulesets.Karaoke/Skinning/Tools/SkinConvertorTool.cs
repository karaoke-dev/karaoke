// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Linq;
using osu.Framework;
using osu.Framework.Graphics.Shaders;
using osu.Game.Rulesets.Karaoke.Skinning.Metadatas;

namespace osu.Game.Rulesets.Karaoke.Skinning.Tools
{
    // it's the temp logic to collect logic.
    public static class SkinConvertorTool
    {
        public static IShader[] ConvertLeftSideShader(ShaderManager shaderManager, LyricStyle lyricStyle)
        {
            if (shaderManager == null)
                return null;

            // todo: do not use outline shader in other platform because seems osx use OpenGL 2.X
            // which will cause shader compile failed.
            if (RuntimeInfo.OS != RuntimeInfo.Platform.Windows)
                return null;

            var shaders = lyricStyle.LeftLyricTextShaders.ToArray();
            attachShaders(shaderManager, shaders);

            return shaders;
        }

        public static IShader[] ConvertRightSideShader(ShaderManager shaderManager, LyricStyle lyricStyle)
        {
            if (shaderManager == null)
                return null;

            // todo: do not use outline shader in other platform because seems osx use OpenGL 2.X
            // which will cause shader compile failed.
            if (RuntimeInfo.OS != RuntimeInfo.Platform.Windows)
                return null;

            var shaders = lyricStyle.RightLyricTextShaders.ToArray();
            attachShaders(shaderManager, shaders);

            return shaders;
        }

        private static void attachShaders(ShaderManager shaderManager, IShader[] shaders)
        {
            var internalShaders = shaders.OfType<InternalShader>().ToArray();

            foreach (var internalShader in internalShaders)
            {
                var shader = shaderManager.Load(VertexShaderDescriptor.TEXTURE_2, internalShader.ShaderName);
                internalShader.AttachOriginShader(shader);
            }
        }
    }
}
