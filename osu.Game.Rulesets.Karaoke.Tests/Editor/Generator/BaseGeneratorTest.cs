// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using osu.Game.Rulesets.Karaoke.Edit.Generator;

namespace osu.Game.Rulesets.Karaoke.Tests.Editor.Generator;

public class BaseGeneratorTest<TConfig> where TConfig : IHasConfig<TConfig>, new()
{
    protected static TConfig GeneratorConfig(Action<TConfig>? action = null)
    {
        var config = new TConfig();
        action?.Invoke(config);
        return config;
    }

    protected static TConfig GeneratorDefaultConfig(Action<TConfig>? action = null)
    {
        var config = new TConfig().CreateDefaultConfig();
        action?.Invoke(config);
        return config;
    }
}
