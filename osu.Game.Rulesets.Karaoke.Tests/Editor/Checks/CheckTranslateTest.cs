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

namespace osu.Game.Rulesets.Karaoke.Tests.Editor.Checks
{
    [TestFixture]
    public class CheckTranslateTest
    {
        private CheckTranslate check = null!;

        [SetUp]
        public void Setup()
        {
            check = new CheckTranslate();
        }

        [Test]
        public void TestNoLyricAndNoLanguage()
        {
            // test no lyric and no default language. (should not show alert)
            var beatmap = createTestingBeatmap(null, null);
            var result = check.Run(getContext(beatmap));

            int actual = result.Count();
            Assert.AreEqual(0, actual);
        }

        [Test]
        public void TestNoLyricButHaveLanguage()
        {
            // test no lyric and have language. (should not show alert)
            var translateLanguages = new List<CultureInfo> { new("Ja-jp") };
            var beatmap = createTestingBeatmap(translateLanguages, null);
            var result = check.Run(getContext(beatmap));

            int actual = result.Count();
            Assert.AreEqual(0, actual);
        }

        [Test]
        public void TestHaveLyricButNoLanguage()
        {
            // test have lyric and no language. (should not show alert)
            var lyrics = new[] { new Lyric() };
            var beatmap = createTestingBeatmap(null, lyrics);
            var result = check.Run(getContext(beatmap));

            int actual = result.Count();
            Assert.AreEqual(0, actual);
        }

        [Test]
        public void TestHaveLyricAndLanguage()
        {
            // no lyric with translate string. (should have issue)
            var translateLanguages = new List<CultureInfo> { new("Ja-jp") };
            var beatmap = createTestingBeatmap(translateLanguages, new[]
            {
                createLyric(),
                createLyric(),
            });
            Assert.AreEqual(1, check.Run(getContext(beatmap)).Count());

            // no lyric with translate string. (should have issue)
            var beatmap2 = createTestingBeatmap(translateLanguages, new[]
            {
                createLyric(new CultureInfo("Ja-jp")),
                createLyric(),
            });
            Assert.AreEqual(1, check.Run(getContext(beatmap2)).Count());

            // no lyric with translate string. (should have issue)
            var beatmap3 = createTestingBeatmap(translateLanguages, new[]
            {
                createLyric(new CultureInfo("Ja-jp")),
                createLyric(new CultureInfo("Ja-jp"), string.Empty),
            });
            Assert.AreEqual(1, check.Run(getContext(beatmap3)).Count());

            // some lyric with translate string. (should have issue)
            var beatmap4 = createTestingBeatmap(translateLanguages, new[]
            {
                createLyric(new CultureInfo("Ja-jp"), "translate1"),
                createLyric(new CultureInfo("Ja-jp")),
            });
            Assert.AreEqual(1, check.Run(getContext(beatmap4)).Count());

            // every lyric with translate string. (should not have issue)
            var beatmap5 = createTestingBeatmap(translateLanguages, new[]
            {
                createLyric(new CultureInfo("Ja-jp"), "translate1"),
                createLyric(new CultureInfo("Ja-jp"), "translate2"),
            });
            Assert.AreEqual(0, check.Run(getContext(beatmap5)).Count());

            // lyric translate not listed. (should have issue)
            var beatmap6 = createTestingBeatmap(null, new[]
            {
                createLyric(new CultureInfo("en-US"), "translate1"),
            });
            Assert.AreEqual(1, check.Run(getContext(beatmap6)).Count());

            static Lyric createLyric(CultureInfo? cultureInfo = null, string translate = null!)
            {
                var lyric = new Lyric();
                if (cultureInfo == null)
                    return lyric;

                lyric.Translates.Add(cultureInfo, translate);
                return lyric;
            }
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
                HitObjects = lyrics?.OfType<KaraokeHitObject>().ToList() ?? new List<KaraokeHitObject>()
            };
            return new EditorBeatmap(karaokeBeatmap);
        }

        private static BeatmapVerifierContext getContext(IBeatmap beatmap)
            => new(beatmap, new TestWorkingBeatmap(beatmap));
    }
}
