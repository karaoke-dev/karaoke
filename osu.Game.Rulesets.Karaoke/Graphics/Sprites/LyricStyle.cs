// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using System.Collections.Generic;
using osu.Framework.Extensions.Color4Extensions;
using osu.Framework.Graphics.Shaders;
using osuTK;

namespace osu.Game.Rulesets.Karaoke.Graphics.Sprites;

public class LyricStyle
{
    public static LyricStyle CreateDefault() => new()
    {
        LeftLyricTextShaders = new ICustomizedShader[]
        {
            new StepShader
            {
                Name = "Step shader",
                StepShaders = new ICustomizedShader[]
                {
                    new OutlineShader
                    {
                        Radius = 3,
                        OutlineColour = Color4Extensions.FromHex("#CCA532"),
                    },
                    new ShadowShader
                    {
                        ShadowColour = Color4Extensions.FromHex("#6B5B2D"),
                        ShadowOffset = new Vector2(3),
                    },
                },
            },
        },
        RightLyricTextShaders = new ICustomizedShader[]
        {
            new StepShader
            {
                Name = "Step shader",
                StepShaders = new ICustomizedShader[]
                {
                    new OutlineShader
                    {
                        Radius = 3,
                        OutlineColour = Color4Extensions.FromHex("#5932CC"),
                    },
                    new ShadowShader
                    {
                        ShadowColour = Color4Extensions.FromHex("#3D2D6B"),
                        ShadowOffset = new Vector2(3),
                    },
                },
            },
        },
    };

    public IReadOnlyList<ICustomizedShader> LeftLyricTextShaders = Array.Empty<ICustomizedShader>();

    public IReadOnlyList<ICustomizedShader> RightLyricTextShaders = Array.Empty<ICustomizedShader>();
}
