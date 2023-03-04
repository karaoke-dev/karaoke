// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Game.Rulesets.Karaoke.Beatmaps;
using osu.Game.Rulesets.Karaoke.Edit.Generator;
using osu.Game.Rulesets.Karaoke.Edit.Generator.Beatmaps;

namespace osu.Game.Rulesets.Karaoke.Tests.Editor.Generator.Beatmaps
{
    public abstract class BaseBeatmapDetectorTest<TDetector, TObject, TConfig>
        : BasePropertyDetectorTest<TDetector, KaraokeBeatmap, TObject, TConfig>
        where TDetector : BeatmapPropertyDetector<TObject, TConfig>
        where TConfig : IHasConfig, new()
    {
    }
}
