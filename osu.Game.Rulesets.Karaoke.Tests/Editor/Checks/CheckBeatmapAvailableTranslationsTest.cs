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
using static osu.Game.Rulesets.Karaoke.Edit.Checks.CheckBeatmapAvailableTranslations;

namespace osu.Game.Rulesets.Karaoke.Tests.Editor.Checks;

[TestFixture]
public class CheckBeatmapAvailableTranslationsTest : BeatmapPropertyCheckTest<CheckBeatmapAvailableTranslations>
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
        var translationLanguages = new List<CultureInfo> { new("Ja-jp") };
        var beatmap = createTestingBeatmap(translationLanguages, null);

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
    public void TestEveryLyricContainsTranslation()
    {
        var translationLanguages = new List<CultureInfo> { new("Ja-jp") };
        var beatmap = createTestingBeatmap(translationLanguages, new[]
        {
            createLyric(new CultureInfo("Ja-jp"), "translation1"),
            createLyric(new CultureInfo("Ja-jp"), "translation2"),
        });

        AssertOk(getContext(beatmap));
    }

    [Test]
    public void TestCheckMissingTranslation()
    {
        // no lyric with translation string. (should have issue)
        var translationLanguages = new List<CultureInfo> { new("Ja-jp") };
        var beatmap = createTestingBeatmap(translationLanguages, new[]
        {
            createLyric(),
            createLyric(),
        });
        AssertNotOk<IssueTemplateMissingTranslation>(getContext(beatmap));

        // no lyric with translation string. (should have issue)
        var beatmap2 = createTestingBeatmap(translationLanguages, new[]
        {
            createLyric(new CultureInfo("Ja-jp")),
            createLyric(),
        });
        AssertNotOk<IssueTemplateMissingTranslation>(getContext(beatmap2));

        // no lyric with translation string. (should have issue)
        var beatmap3 = createTestingBeatmap(translationLanguages, new[]
        {
            createLyric(new CultureInfo("Ja-jp")),
            createLyric(new CultureInfo("Ja-jp"), string.Empty),
        });
        AssertNotOk<IssueTemplateMissingTranslation>(getContext(beatmap3));
    }

    [Test]
    public void TestCheckMissingPartialTranslation()
    {
        // some lyric with translation string. (should have issue)
        var translationLanguages = new List<CultureInfo> { new("Ja-jp") };
        var beatmap4 = createTestingBeatmap(translationLanguages, new[]
        {
            createLyric(new CultureInfo("Ja-jp"), "translation1"),
            createLyric(new CultureInfo("Ja-jp")),
        });
        AssertNotOk<IssueTemplateMissingPartialTranslation>(getContext(beatmap4));
    }

    [Test]
    public void TestCheckTranslationNotInListedLanguage()
    {
        // lyric translation not listed. (should have issue)
        var beatmap6 = createTestingBeatmap(null, new[]
        {
            createLyric(new CultureInfo("en-US"), "translation1"),
        });
        AssertNotOk<IssueTemplateTranslationNotInListedLanguage>(getContext(beatmap6));
    }

    private static IBeatmap createTestingBeatmap(List<CultureInfo>? translationLanguages, IEnumerable<Lyric>? lyrics)
    {
        var karaokeBeatmap = new KaraokeBeatmap
        {
            BeatmapInfo =
            {
                Ruleset = new KaraokeRuleset().RulesetInfo,
            },
            AvailableTranslationLanguages = translationLanguages ?? new List<CultureInfo>(),
            HitObjects = lyrics?.OfType<KaraokeHitObject>().ToList() ?? new List<KaraokeHitObject>(),
        };
        return new EditorBeatmap(karaokeBeatmap);
    }

    private static BeatmapVerifierContext getContext(IBeatmap beatmap)
        => new(beatmap, new TestWorkingBeatmap(beatmap));

    private static Lyric createLyric(CultureInfo? cultureInfo = null, string translation = null!)
    {
        var lyric = new Lyric();
        if (cultureInfo == null)
            return lyric;

        lyric.Translations.Add(cultureInfo, translation);
        return lyric;
    }
}
