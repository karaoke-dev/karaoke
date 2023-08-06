// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using osu.Game.Rulesets.Karaoke.Edit.Generator.Beatmaps.Stages;
using osu.Game.Rulesets.Karaoke.Stages;

namespace osu.Game.Rulesets.Karaoke.Tests.Editor.Generator.Beatmaps.Stages;

public abstract class BaseStageInfoGeneratorTest<TGenerator, TStageInfo, TConfig>
    : BaseBeatmapGeneratorTest<TGenerator, StageInfo, TConfig>
    where TStageInfo : StageInfo
    where TGenerator : StageInfoGenerator<TConfig>
    where TConfig : StageInfoGeneratorConfig, new()
{
    protected sealed override void AssertEqual(StageInfo expected, StageInfo actual)
    {
        if (expected is not TStageInfo expectedStageInfo)
            throw new ArgumentException($"Expected type is not {typeof(TStageInfo).Name}", nameof(expected));

        if (actual is not TStageInfo actualStageInfo)
            throw new ArgumentException($"Actual type is not {typeof(TStageInfo).Name}", nameof(actual));

        AssertEqual(expectedStageInfo, actualStageInfo);
    }

    protected abstract void AssertEqual(TStageInfo expected, TStageInfo actual);
}
