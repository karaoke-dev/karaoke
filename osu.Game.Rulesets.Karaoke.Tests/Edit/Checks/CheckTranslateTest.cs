// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using NUnit.Framework;
using osu.Framework.Audio.Sample;
using osu.Framework.Bindables;
using osu.Framework.Graphics;
using osu.Framework.Graphics.OpenGL.Textures;
using osu.Framework.Graphics.Textures;
using osu.Game.Audio;
using osu.Game.Beatmaps;
using osu.Game.Rulesets.Karaoke.Beatmaps;
using osu.Game.Rulesets.Karaoke.Edit.Checks;
using osu.Game.Rulesets.Karaoke.Objects;
using osu.Game.Screens.Edit;
using osu.Game.Skinning;

namespace osu.Game.Rulesets.Karaoke.Tests.Edit.Checks
{
    [TestFixture]
    public class CheckTranslateTest
    {
        private CheckTranslate check;

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
            var result = check.Run(beatmap);
            Assert.AreEqual(result.Count(), 0);
        }

        [Test]
        public void TestNoLyricButHaveLanguage()
        {
            // test no lyric and have language. (should not show alert)
            var translateLanguages = new CultureInfo[] { new CultureInfo("Ja-jp") };
            var beatmap = createTestingBeatmap(translateLanguages, null);
            var result = check.Run(beatmap);
            Assert.AreEqual(result.Count(), 0);
        }

        [Test]
        public void TestHaveLyricButNoLanguage()
        {
            // test have lyric and no language. (should not show alert)
            var lyrics = new Lyric[] { new Lyric() };
            var beatmap = createTestingBeatmap(null, lyrics);
            var result = check.Run(beatmap);
            Assert.AreEqual(result.Count(), 0);
        }

        [Test]
        public void TestHaveLyricAndLanguage()
        {
            // no lyric with translate string. (should have issue)
            var translateLanguages = new CultureInfo[] { new CultureInfo("Ja-jp") };
            var beatmap = createTestingBeatmap(translateLanguages, new Lyric[]
            {
                createLyric(),
                createLyric(),
            });
            Assert.AreEqual(check.Run(beatmap).Count(), 1);

            // no lyric with translate string. (should have issue)
            var beatmap2 = createTestingBeatmap(translateLanguages, new Lyric[]
            {
                createLyric(new CultureInfo("Ja-jp")),
                createLyric(),
            });
            Assert.AreEqual(check.Run(beatmap2).Count(), 1);

            // no lyric with translate string. (should have issue)
            var beatmap3 = createTestingBeatmap(translateLanguages, new Lyric[]
            {
                createLyric(new CultureInfo("Ja-jp")),
                createLyric(new CultureInfo("Ja-jp"), ""),
            });
            Assert.AreEqual(check.Run(beatmap3).Count(), 1);

            // some lyric with translate string. (should have issue)
            var beatmap4 = createTestingBeatmap(translateLanguages, new Lyric[]
            {
                createLyric(new CultureInfo("Ja-jp"), "translate1"),
                createLyric(new CultureInfo("Ja-jp")),
            });
            Assert.AreEqual(check.Run(beatmap4).Count(), 1);

            // every lyric with translate string. (should not have issue)
            var beatmap5 = createTestingBeatmap(translateLanguages, new Lyric[]
            {
                createLyric(new CultureInfo("Ja-jp"), "translate1"),
                createLyric(new CultureInfo("Ja-jp"), "translate2"),
            });
            Assert.AreEqual(check.Run(beatmap5).Count(), 0);

            static Lyric createLyric(CultureInfo cultureInfo = null, string translate = null)
            {
                var lyric = new Lyric();
                if (cultureInfo == null)
                    return lyric;

                lyric.Translates.Add(cultureInfo, translate);
                return lyric;
            }
        }

        private IBeatmap createTestingBeatmap(CultureInfo[] translateLanguage, Lyric[] lyrics)
        {
            var karaokeBeatmap = new KaraokeBeatmap
            {
                AvailableTranslates = translateLanguage,
                HitObjects = lyrics?.OfType<KaraokeHitObject>().ToList() ?? new List<KaraokeHitObject>()
            };
            return new EditorBeatmap(karaokeBeatmap, null);
        }
    }
}
