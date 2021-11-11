﻿// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework;
using osu.Framework.Extensions;
using osu.Framework.Graphics.Shaders;
using osu.Game.Rulesets.Karaoke.Skinning.Metadatas.Fonts;
using osuTK.Graphics;

namespace osu.Game.Rulesets.Karaoke.Skinning.Tools
{
    // it's the temp logic to collect logic.
    public static class SkinConvertorTool
    {
        public static IShader[] ConvertLeftSideShader(ShaderManager shaderManager, LyricFont lyricFont)
        {
            if (shaderManager == null)
                return null;

            // todo: do not use outline shader in other platform because seems osx use OpenGL 2.X
            // which will cause shader compile failed.
            if (RuntimeInfo.OS != RuntimeInfo.Platform.Windows)
                return null;

            var edgeSize = (int)lyricFont.LyricTextFontInfo.EdgeSize;
            var frontColor = Color4.Blue; // todo: use real colour.
            return new IShader[]
            {
                shaderManager.LocalInternalShader<OutlineShader>()
                             .With(x =>
                             {
                                 x.Radius = edgeSize;
                                 x.OutlineColour = frontColor;
                             })
            };
        }

        public static IShader[] ConvertRightSideShader(ShaderManager shaderManager, LyricFont lyricFont)
        {
            if (shaderManager == null)
                return null;

            // todo: do not use outline shader in other platform because seems osx use OpenGL 2.X
            // which will cause shader compile failed.
            if (RuntimeInfo.OS != RuntimeInfo.Platform.Windows)
                return null;

            var edgeSize = (int)lyricFont.LyricTextFontInfo.EdgeSize;
            var backColor = Color4.DarkGreen; // todo: use real colour.
            return new IShader[]
            {
                shaderManager.LocalInternalShader<OutlineShader>()
                             .With(x =>
                             {
                                 x.Radius = edgeSize;
                                 x.OutlineColour = backColor;
                             })
            };
        }
    }
}
