// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using NUnit.Framework;
using osu.Game.Rulesets.Karaoke.Beatmaps;
using osu.Game.Rulesets.Karaoke.Beatmaps.Stages;
using osu.Game.Rulesets.Karaoke.Edit.Generator;
using osu.Game.Rulesets.Karaoke.Edit.Generator.Beatmaps;
using osu.Game.Rulesets.Karaoke.Objects;

namespace osu.Game.Rulesets.Karaoke.Tests.Editor.Generator.Beatmaps.Stages;

public abstract class BaseLyricStageElementCategoryGeneratorTest<TGenerator, TObject, TStageElement, TConfig>
    : BaseStageElementCategoryGeneratorTest<TGenerator, TObject, TStageElement, Lyric, TConfig>
    where TGenerator : BeatmapPropertyGenerator<TObject, TConfig>
    where TObject : StageElementCategory<TStageElement, Lyric>
    where TStageElement : StageElement, IComparable<TStageElement>, new()
    where TConfig : GeneratorConfig, new()
{
}

public abstract class BaseStageElementCategoryGeneratorTest<TGenerator, TObject, TStageElement, THitObject, TConfig>
    : BaseBeatmapGeneratorTest<TGenerator, TObject, TConfig>
    where TGenerator : BeatmapPropertyGenerator<TObject, TConfig>
    where TObject : StageElementCategory<TStageElement, THitObject>
    where TStageElement : StageElement, IComparable<TStageElement>, new()
    where THitObject : KaraokeHitObject, IHasPrimaryKey
    where TConfig : GeneratorConfig, new()
{
    protected sealed override void AssertEqual(TObject expected, TObject actual)
    {
        for (int i = 0; i < expected.AvailableElements.Count; i++)
        {
            var expectedElement = expected.AvailableElements[i];
            var actualElement = actual.AvailableElements[i];

            AssertEqual(expectedElement, actualElement);
        }

        Assert.AreEqual(expected.Mappings, actual.Mappings);
    }

    protected abstract void AssertEqual(TStageElement expected, TStageElement actual);
}
