// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Game.Rulesets.Karaoke.Beatmaps;
using osu.Game.Rulesets.Karaoke.Edit.Generator;
using osu.Game.Rulesets.Karaoke.Edit.Generator.Beatmaps;

namespace osu.Game.Rulesets.Karaoke.Tests.Editor.Generator.Beatmaps;

public abstract class BaseBeatmapGeneratorTest<TGenerator, TObject, TConfig>
    : BasePropertyGeneratorTest<TGenerator, KaraokeBeatmap, TObject, TConfig>
    where TGenerator : BeatmapPropertyGenerator<TObject, TConfig>
    where TConfig : GeneratorConfig, new()
{
}
