// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using NUnit.Framework;
using osu.Game.Beatmaps;
using osu.Game.Rulesets.Edit;
using osu.Game.Rulesets.Karaoke.Beatmaps;
using osu.Game.Rulesets.Karaoke.Edit.Checks;
using osu.Game.Rulesets.Karaoke.Objects;
using osu.Game.Screens.Edit;
using osu.Game.Tests.Beatmaps;
using static osu.Game.Rulesets.Karaoke.Edit.Checks.CheckBeatmapAvailableTranslates;

namespace osu.Game.Rulesets.Karaoke.Tests.Editor.Checks;

[TestFixture]
public class CheckBeatmapAvailableTranslatesTest : BeatmapPropertyCheckTest<CheckBeatmapAvailableTranslates>
{
    [Test]
    public void TestNoLyricAndNoLanguage()
    {
        // test no lyric and no default language. (should not show alert)
        var beatmap = createTestingBeatmap(null, null);

        AssertOk(getContext(beatmap));
    }

    [Test]
    public void TestNoLyricButHaveLanguage()
    {
        // test no lyric and have language. (should not show alert)
        var translateLanguages = new List<CultureInfo> { new("Ja-jp") };
        var beatmap = createTestingBeatmap(translateLanguages, null);

        AssertOk(getContext(beatmap));
    }

    [Test]
    public void TestHaveLyricButNoLanguage()
    {
        // test have lyric and no language. (should not show alert)
        var lyrics = new[] { new Lyric() };
        var beatmap = createTestingBeatmap(null, lyrics);

        AssertOk(getContext(beatmap));
    }

    [Test]
    public void TestEveryLyricContainsTranslate()
    {
        var translateLanguages = new List<CultureInfo> { new("Ja-jp") };
        var beatmap = createTestingBeatmap(translateLanguages, new[]
        {
            createLyric(new CultureInfo("Ja-jp"), "translate1"),
            createLyric(new CultureInfo("Ja-jp"), "translate2"),
        });

        AssertOk(getContext(beatmap));
    }

    [Test]
    public void TestCheckMissingTranslate()
    {
        // no lyric with translate string. (should have issue)
        var translateLanguages = new List<CultureInfo> { new("Ja-jp") };
        var beatmap = createTestingBeatmap(translateLanguages, new[]
        {
            createLyric(),
            createLyric(),
        });
        AssertNotOk<IssueTemplateMissingTranslate>(getContext(beatmap));

        // no lyric with translate string. (should have issue)
        var beatmap2 = createTestingBeatmap(translateLanguages, new[]
        {
            createLyric(new CultureInfo("Ja-jp")),
            createLyric(),
        });
        AssertNotOk<IssueTemplateMissingTranslate>(getContext(beatmap2));

        // no lyric with translate string. (should have issue)
        var beatmap3 = createTestingBeatmap(translateLanguages, new[]
        {
            createLyric(new CultureInfo("Ja-jp")),
            createLyric(new CultureInfo("Ja-jp"), string.Empty),
        });
        AssertNotOk<IssueTemplateMissingTranslate>(getContext(beatmap3));
    }

    [Test]
    public void TestCheckMissingPartialTranslate()
    {
        // some lyric with translate string. (should have issue)
        var translateLanguages = new List<CultureInfo> { new("Ja-jp") };
        var beatmap4 = createTestingBeatmap(translateLanguages, new[]
        {
            createLyric(new CultureInfo("Ja-jp"), "translate1"),
            createLyric(new CultureInfo("Ja-jp")),
        });
        AssertNotOk<IssueTemplateMissingPartialTranslate>(getContext(beatmap4));
    }

    [Test]
    public void TestCheckContainsNotListedLanguage()
    {
        // lyric translate not listed. (should have issue)
        var beatmap6 = createTestingBeatmap(null, new[]
        {
            createLyric(new CultureInfo("en-US"), "translate1"),
        });
        AssertNotOk<IssueTemplateContainsNotListedLanguage>(getContext(beatmap6));
    }

    private static IBeatmap createTestingBeatmap(List<CultureInfo>? translateLanguage, IEnumerable<Lyric>? lyrics)
    {
        var karaokeBeatmap = new KaraokeBeatmap
        {
            BeatmapInfo =
            {
                Ruleset = new KaraokeRuleset().RulesetInfo,
            },
            AvailableTranslates = translateLanguage ?? new List<CultureInfo>(),
            HitObjects = lyrics?.OfType<KaraokeHitObject>().ToList() ?? new List<KaraokeHitObject>(),
        };
        return new EditorBeatmap(karaokeBeatmap);
    }

    private static BeatmapVerifierContext getContext(IBeatmap beatmap)
        => new(beatmap, new TestWorkingBeatmap(beatmap));

    private static Lyric createLyric(CultureInfo? cultureInfo = null, string translate = null!)
    {
        var lyric = new Lyric();
        if (cultureInfo == null)
            return lyric;

        lyric.Translates.Add(cultureInfo, translate);
        return lyric;
    }
}
