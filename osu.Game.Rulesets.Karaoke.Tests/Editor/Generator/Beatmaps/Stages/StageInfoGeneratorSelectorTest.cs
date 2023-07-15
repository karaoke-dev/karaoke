// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Collections.Generic;
using NUnit.Framework;
using osu.Game.Rulesets.Karaoke.Beatmaps;
using osu.Game.Rulesets.Karaoke.Beatmaps.Stages;
using osu.Game.Rulesets.Karaoke.Beatmaps.Stages.Classic;
using osu.Game.Rulesets.Karaoke.Beatmaps.Stages.Preview;
using osu.Game.Rulesets.Karaoke.Edit.Generator.Beatmaps.Stages;
using osu.Game.Rulesets.Karaoke.Objects;
using osu.Game.Rulesets.Karaoke.Tests.Helper;

namespace osu.Game.Rulesets.Karaoke.Tests.Editor.Generator.Beatmaps.Stages;

[TestFixture(typeof(ClassicStageInfo))]
[TestFixture(typeof(PreviewStageInfo))]
public class StageInfoGeneratorSelectorTest<TStageInfo> : BaseGeneratorSelectorTest<StageInfoGeneratorSelector<TStageInfo>, KaraokeBeatmap, StageInfo>
    where TStageInfo : StageInfo, new()
{
    [Test]
    public void TestCanGenerate()
    {
        var selector = CreateSelector();
        var beatmap = createBeatmap();

        CheckCanGenerate(beatmap, true, selector);
    }

    [Test]
    public void TestGenerate()
    {
        var selector = CreateSelector();
        var beatmap = createBeatmap();

        var expected = new TStageInfo();
        CheckGenerateResult(beatmap, expected, selector);
    }

    protected override void AssertEqual(StageInfo expected, StageInfo actual)
    {
        // There's no need to check the content in the stage info.
        // Just make sure that the type in the test case is supported.
        Assert.AreEqual(expected.GetType(), actual.GetType());
    }

    private KaraokeBeatmap createBeatmap()
    {
        return new KaraokeBeatmap
        {
            HitObjects = new List<KaraokeHitObject>
            {
                TestCaseTagHelper.ParseLyric("[1000,3000]:lyric1"),
                TestCaseTagHelper.ParseLyric("[4000,6000]:lyric2"),
                TestCaseTagHelper.ParseLyric("[7000,9000]:lyric3"),
                TestCaseTagHelper.ParseLyric("[10000,12000]:lyric4"),
            },
        };
    }
}
