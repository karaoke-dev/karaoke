// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Game.Rulesets.Karaoke.Beatmaps;

namespace osu.Game.Rulesets.Karaoke.Edit.Generator.Beatmaps
{
    /// <summary>
    /// Base interface of the detector.
    /// </summary>
    /// <typeparam name="TProperty"></typeparam>
    /// <typeparam name="TConfig"></typeparam>
    public abstract class BeatmapPropertyDetector<TProperty, TConfig> : PropertyDetector<KaraokeBeatmap, TProperty, TConfig>
        where TConfig : IHasConfig<TConfig>, new()
    {
        protected BeatmapPropertyDetector(TConfig config)
            : base(config)
        {
        }
    }
}
