// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using osu.Game.Rulesets.Karaoke.Beatmaps;
using osu.Game.Rulesets.Karaoke.Beatmaps.Metadatas;
using osu.Game.Rulesets.Karaoke.Edit.Checks;
using osu.Game.Rulesets.Karaoke.Edit.Generator.Beatmaps.Pages;
using osu.Game.Rulesets.Karaoke.Objects;
using osu.Game.Rulesets.Karaoke.Tests.Helper;

namespace osu.Game.Rulesets.Karaoke.Tests.Editor.Generator.Beatmaps.Pages;

[TestFixture]
public class PageGeneratorTest : BaseBeatmapGeneratorTest<PageGenerator, Page[], PageGeneratorConfig>
{
    private const double min_interval = CheckBeatmapPageInfo.MIN_INTERVAL;
    private const double max_interval = CheckBeatmapPageInfo.MAX_INTERVAL;

    [TestCase(new[] { "[1000,3000]:karaoke" }, true)]
    [TestCase(new[] { "[1000,3000]:karaoke", "[4000,6000]:karaoke" }, true)]
    [TestCase(new[] { "[1000,3000]:karaoke", "[1000,3000]:karaoke" }, true)] // should still runnable even if lyric is overlapping.
    [TestCase(new string[] { }, false)]
    public void TestCanGenerate(string[] lyrics, bool canGenerate)
    {
        var config = GeneratorConfig();
        var beatmap = new KaraokeBeatmap
        {
            HitObjects = TestCaseTagHelper.ParseLyrics(lyrics).OfType<KaraokeHitObject>().ToList(),
        };

        CheckCanGenerate(beatmap, canGenerate, config);
    }

    [Test]
    public void TestGenerateWithZeroLyric()
    {
        var config = GeneratorConfig();
        var beatmap = new KaraokeBeatmap
        {
            HitObjects = new List<KaraokeHitObject>()
        };

        var expectedPages = Array.Empty<Page>();

        CheckGenerateResult(beatmap, expectedPages, config);
    }

    [TestCase("[1000,3000]:karaoke", new double[] { 1000, 3000 })]
    [TestCase("[1000,23000]:karaoke", new[] { 1000, 1000 + max_interval, 1000 + max_interval * 2, 23000 })]
    public void TestGenerateWithSingleLyric(string lyric, double[] expectedTimes)
    {
        var config = GeneratorConfig();
        var beatmap = new KaraokeBeatmap
        {
            HitObjects = new List<KaraokeHitObject>
            {
                TestCaseTagHelper.ParseLyric(lyric)
            }
        };

        var expectedPages = expectedTimes.Select(x => new Page
        {
            Time = x
        }).ToArray();

        CheckGenerateResult(beatmap, expectedPages, config);
    }

    [TestCase("[1000,4000]:karaoke", "[4000,7000]:karaoke", new double[] { 1000, 4000, 7000 })]
    [TestCase("[1000,4000]:karaoke", "[5000,8000]:karaoke", new double[] { 1000, 4500, 8000 })]
    [TestCase("[1000,3000]:karaoke", "[1000,3000]:karaoke", new double[] { 1000, 3000 })] //should deal with overlapping lyric.
    [TestCase("[1000,23000]:karaoke", "[1000,23000]:karaoke", new[] { 1000, 1000 + max_interval, 1000 + max_interval * 2, 23000 })] //should deal with overlapping lyric with long time.
    [TestCase("[1000,23000]:karaoke", "[3000,4000]:karaoke", new[] { 1000, 1000 + max_interval, 1000 + max_interval * 2, 23000 })] // should ignore second lyric.
    public void TestGenerateWithTwoLyrics(string firstLyric, string secondLyric, double[] expectedTimes)
    {
        var config = GeneratorConfig();
        var beatmap = new KaraokeBeatmap
        {
            HitObjects = new List<KaraokeHitObject>
            {
                TestCaseTagHelper.ParseLyric(firstLyric),
                TestCaseTagHelper.ParseLyric(secondLyric)
            }
        };

        var expectedPages = expectedTimes.Select(x => new Page
        {
            Time = x
        }).ToArray();

        CheckGenerateResult(beatmap, expectedPages, config);
    }

    [Ignore("Waiting for implementation.")]
    public void TestGenerateWithSingleLyricWithPage()
    {
    }

    [Ignore("Waiting for implementation.")]
    public void TestGenerateWithTwoLyricsWithPage()
    {
    }

    protected override void AssertEqual(Page[] expected, Page[] actual)
    {
        string expectedTimes = string.Join(",", expected.Select(x => x.Time));
        string actualTimes = string.Join(",", actual.Select(x => x.Time));
        Assert.AreEqual(expectedTimes, actualTimes);
    }
}
