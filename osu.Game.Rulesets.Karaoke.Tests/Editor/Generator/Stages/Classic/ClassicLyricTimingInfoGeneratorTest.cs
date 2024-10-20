// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using osu.Game.Rulesets.Karaoke.Beatmaps;
using osu.Game.Rulesets.Karaoke.Edit.Generator.Stages.Classic;
using osu.Game.Rulesets.Karaoke.Objects;
using osu.Game.Rulesets.Karaoke.Stages.Infos.Classic;
using osu.Game.Rulesets.Karaoke.Tests.Helper;

namespace osu.Game.Rulesets.Karaoke.Tests.Editor.Generator.Stages.Classic;

public class ClassicLyricTimingInfoGeneratorTest
    : BaseStageInfoPropertyGeneratorTest<ClassicLyricTimingInfoGenerator, ClassicLyricTimingInfo, ClassicLyricTimingInfoGeneratorConfig>
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

        var expected = new ClassicLyricTimingInfo();
        var timing1 = expected.AddTimingPoint(x => x.Time = 0); // should show the lyric at screen when loaded.
        expected.AddToMapping(timing1, lyric1); // show
        expected.AddToMapping(timing1, lyric2); // show

        var timing2 = expected.AddTimingPoint(x => x.Time = 3000); // it's time to hide lyric1 and show lyric3.
        expected.AddToMapping(timing2, lyric1); // hide
        expected.AddToMapping(timing2, lyric3); // show

        var timing3 = expected.AddTimingPoint(x => x.Time = 6000); // it's time to hide lyric2 and show lyric4.
        expected.AddToMapping(timing3, lyric2); // hide
        expected.AddToMapping(timing3, lyric4); // show

        var timing4 = expected.AddTimingPoint(x => x.Time = 12000); // it's time to hide lyric3 and lyric4.
        expected.AddToMapping(timing4, lyric3); // hide
        expected.AddToMapping(timing4, lyric4); // hide

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

        var expected = new ClassicLyricTimingInfo();
        var timing1 = expected.AddTimingPoint(x => x.Time = 0);
        expected.AddToMapping(timing1, lyric1); // show
        expected.AddToMapping(timing1, lyric2); // show
        expected.AddToMapping(timing1, lyric3); // show

        var timing2 = expected.AddTimingPoint(x => x.Time = 3000);
        expected.AddToMapping(timing2, lyric1); // hide
        expected.AddToMapping(timing2, lyric4); // show

        var timing3 = expected.AddTimingPoint(x => x.Time = 6000);
        expected.AddToMapping(timing3, lyric2); // hide
        expected.AddToMapping(timing3, lyric5); // show

        var timing4 = expected.AddTimingPoint(x => x.Time = 12000);
        expected.AddToMapping(timing4, lyric3); // hide
        expected.AddToMapping(timing4, lyric4); // hide
        expected.AddToMapping(timing4, lyric5); // hide

        CheckGenerateResult(beatmap, expected, config);
    }

    protected override void AssertEqual(ClassicLyricTimingInfo expected, ClassicLyricTimingInfo actual)
    {
        Assert.AreEqual(expected.Timings.Select(x => x.Time), actual.Timings.Select(x => x.Time));

        // because we cannot check the id in the mapping value, so just check the key.
        Assert.AreEqual(expected.Mappings.Keys, actual.Mappings.Keys);
    }
}
