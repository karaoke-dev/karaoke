// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Game.Rulesets.Karaoke.Beatmaps;
using osu.Game.Rulesets.Karaoke.Edit.Generator;
using osu.Game.Rulesets.Karaoke.Edit.Generator.Beatmaps.Stages;

namespace osu.Game.Rulesets.Karaoke.Tests.Editor.Generator.Beatmaps.Stages;

public abstract class BaseStageInfoPropertyGeneratorTest<TGenerator, TObject, TConfig>
    : BasePropertyGeneratorTest<TGenerator, KaraokeBeatmap, TObject, TConfig>
    where TGenerator : StageInfoPropertyGenerator<TObject, TConfig>
    where TConfig : GeneratorConfig, new()
{
}
