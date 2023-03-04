// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Game.Rulesets.Karaoke.Beatmaps;

namespace osu.Game.Rulesets.Karaoke.Edit.Generator.Beatmaps
{
    /// <summary>
    /// Base interface of the auto-generator.
    /// </summary>
    /// <typeparam name="TProperty"></typeparam>
    /// <typeparam name="TConfig"></typeparam>
    public abstract class BeatmapPropertyGenerator<TProperty, TConfig> : PropertyGenerator<KaraokeBeatmap, TProperty, TConfig>
        where TConfig : IHasConfig, new()
    {
        protected BeatmapPropertyGenerator(TConfig config)
            : base(config)
        {
        }
    }
}
