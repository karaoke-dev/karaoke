// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Collections.Generic;
using NUnit.Framework;
using osu.Game.Rulesets.Karaoke.Beatmaps;
using osu.Game.Rulesets.Karaoke.Edit.Generator.Stages.Classic;
using osu.Game.Rulesets.Karaoke.Objects;
using osu.Game.Rulesets.Karaoke.Stages.Infos.Classic;
using osu.Game.Rulesets.Karaoke.Tests.Helper;

namespace osu.Game.Rulesets.Karaoke.Tests.Editor.Generator.Stages.Classic;

public class ClassicLyricLayoutCategoryGeneratorTest
    : BaseLyricStageElementCategoryGeneratorTest<ClassicLyricLayoutCategoryGenerator, ClassicLyricLayoutCategory, ClassicLyricLayout, ClassicLyricLayoutCategoryGeneratorConfig>
{
    [Test]
    public void TestCanGenerate()
    {
        var config = GeneratorDefaultConfig();
        var beatmap = new KaraokeBeatmap
        {
            HitObjects = new List<KaraokeHitObject>
            {
                TestCaseTagHelper.ParseLyric("[1000,3000]:lyric1"),
                TestCaseTagHelper.ParseLyric("[4000,6000]:lyric2"),
                TestCaseTagHelper.ParseLyric("[7000,9000]:lyric3"),
                TestCaseTagHelper.ParseLyric("[10000,12000]:lyric4"),
            },
        };

        CheckCanGenerate(beatmap, true, config);
    }

    [Test]
    public void TestCanGenerateWithNonLyricBeatmap()
    {
        var config = GeneratorDefaultConfig();
        var beatmap = new KaraokeBeatmap();
        CheckCanGenerate(beatmap, false, config);
    }

    [Test]
    public void TestGenerate()
    {
        var config = GeneratorDefaultConfig();

        var lyric1 = TestCaseTagHelper.ParseLyric("[1000,3000]:lyric1", 1);
        var lyric2 = TestCaseTagHelper.ParseLyric("[4000,6000]:lyric2", 2);
        var lyric3 = TestCaseTagHelper.ParseLyric("[7000,9000]:lyric3", 3);
        var lyric4 = TestCaseTagHelper.ParseLyric("[10000,12000]:lyric4", 4);
        var beatmap = new KaraokeBeatmap
        {
            HitObjects = new List<KaraokeHitObject>
            {
                lyric1,
                lyric2,
                lyric3,
                lyric4,
            },
        };

        var expected = new ClassicLyricLayoutCategory();
        var layout1 = expected.AddElement(x => x.Alignment = ClassicLyricLayoutAlignment.Left);
        var layout2 = expected.AddElement(x => x.Alignment = ClassicLyricLayoutAlignment.Right);
        expected.AddToMapping(layout1, lyric1);
        expected.AddToMapping(layout2, lyric2);
        expected.AddToMapping(layout1, lyric3);
        expected.AddToMapping(layout2, lyric4);

        CheckGenerateResult(beatmap, expected, config);
    }

    [Test]
    public void TestGenerateWithThreeLyrics()
    {
        var config = GeneratorDefaultConfig(x => x.LyricRowAmount.Value = 3);

        var lyric1 = TestCaseTagHelper.ParseLyric("[1000,3000]:lyric1", 1);
        var lyric2 = TestCaseTagHelper.ParseLyric("[4000,6000]:lyric2", 2);
        var lyric3 = TestCaseTagHelper.ParseLyric("[7000,9000]:lyric3", 3);
        var lyric4 = TestCaseTagHelper.ParseLyric("[10000,12000]:lyric4", 4);
        var lyric5 = TestCaseTagHelper.ParseLyric("[10000,12000]:lyric5", 5);
        var beatmap = new KaraokeBeatmap
        {
            HitObjects = new List<KaraokeHitObject>
            {
                lyric1,
                lyric2,
                lyric3,
                lyric4,
                lyric5,
            },
        };

        var expected = new ClassicLyricLayoutCategory();
        var layout1 = expected.AddElement(x => x.Alignment = ClassicLyricLayoutAlignment.Left);
        var layout2 = expected.AddElement(x => x.Alignment = ClassicLyricLayoutAlignment.Center);
        var layout3 = expected.AddElement(x => x.Alignment = ClassicLyricLayoutAlignment.Right);
        expected.AddToMapping(layout1, lyric1);
        expected.AddToMapping(layout2, lyric2);
        expected.AddToMapping(layout3, lyric3);
        expected.AddToMapping(layout1, lyric4);
        expected.AddToMapping(layout2, lyric5);

        CheckGenerateResult(beatmap, expected, config);
    }

    [Test]
    public void TestGenerateWithNotMapping()
    {
        var config = GeneratorDefaultConfig(x => x.ApplyMappingToTheLyric.Value = false);

        var lyric1 = TestCaseTagHelper.ParseLyric("[1000,3000]:lyric1", 1);
        var lyric2 = TestCaseTagHelper.ParseLyric("[4000,6000]:lyric2", 2);
        var lyric3 = TestCaseTagHelper.ParseLyric("[7000,9000]:lyric3", 3);
        var lyric4 = TestCaseTagHelper.ParseLyric("[10000,12000]:lyric4", 4);
        var beatmap = new KaraokeBeatmap
        {
            HitObjects = new List<KaraokeHitObject>
            {
                lyric1,
                lyric2,
                lyric3,
                lyric4,
            },
        };

        var expected = new ClassicLyricLayoutCategory();
        expected.AddElement(x => x.Alignment = ClassicLyricLayoutAlignment.Left);
        expected.AddElement(x => x.Alignment = ClassicLyricLayoutAlignment.Right);

        CheckGenerateResult(beatmap, expected, config);
    }

    protected override void AssertEqual(ClassicLyricLayout expected, ClassicLyricLayout actual)
    {
        Assert.That(actual.Alignment, Is.EqualTo(expected.Alignment));
    }
}
