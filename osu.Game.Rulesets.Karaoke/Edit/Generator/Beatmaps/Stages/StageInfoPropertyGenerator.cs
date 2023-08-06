// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Game.Rulesets.Karaoke.Beatmaps;

namespace osu.Game.Rulesets.Karaoke.Edit.Generator.Beatmaps.Stages;

/// <summary>
/// Base interface of the auto-generator.
/// </summary>
/// <typeparam name="TProperty"></typeparam>
/// <typeparam name="TConfig"></typeparam>
public abstract class StageInfoPropertyGenerator<TProperty, TConfig> : PropertyGenerator<KaraokeBeatmap, TProperty, TConfig>
    where TConfig : GeneratorConfig, new()
{
    protected StageInfoPropertyGenerator(TConfig config)
        : base(config)
    {
    }
}
