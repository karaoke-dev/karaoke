// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

#nullable disable

using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using NUnit.Framework;
using osu.Game.Beatmaps;
using osu.Game.Rulesets.Edit;
using osu.Game.Rulesets.Edit.Checks.Components;
using osu.Game.Rulesets.Karaoke.Edit.Checks;
using osu.Game.Rulesets.Karaoke.Objects;
using osu.Game.Rulesets.Objects;
using osu.Game.Tests.Beatmaps;
using static osu.Game.Rulesets.Karaoke.Edit.Checks.CheckInvalidPropertyLyrics;

namespace osu.Game.Rulesets.Karaoke.Tests.Editor.Checks
{
    [TestFixture]
    public class CheckInvalidPropertyLyricsTest
    {
        private CheckInvalidPropertyLyrics check;

        [SetUp]
        public void Setup()
        {
            check = new CheckInvalidPropertyLyrics();
        }

        [TestCase("Ja-jp", false)]
        [TestCase("", false)] // should not have issue if CultureInfo accept it.
        [TestCase(null, true)]
        public void TestCheckLanguage(string language, bool expected)
        {
            var lyric = new Lyric
            {
                Language = language != null ? new CultureInfo(language) : null,
            };

            bool actual = run(lyric).Select(x => x.Template).OfType<IssueTemplateNotFillLanguage>().Any();
            Assert.AreEqual(expected, actual);
        }

        [Ignore("Not implement.")]
        [TestCase(0, true)]
        [TestCase(1, false)]
        [TestCase(-1, false)]
        [TestCase(-2, false)] // in here, not check is layout actually exist.
        [TestCase(null, true)]
        public void TestCheckLayout(int? layout, bool hasIssue)
        {
        }

        [TestCase("karaoke", false)]
        [TestCase("k", false)] // not limit min size for now.
        [TestCase("カラオケ", false)] // not limit language.
        [TestCase(" ", true)] // but should not be empty or white space.
        [TestCase("", true)]
        [TestCase(null, true)]
        public void TestCheckText(string text, bool expected)
        {
            var lyric = new Lyric
            {
                Text = text
            };

            bool actual = run(lyric).Select(x => x.Template).OfType<IssueTemplateNoText>().Any();
            Assert.AreEqual(expected, actual);
        }

        [TestCase(new[] { 1, 2, 3 }, false)]
        [TestCase(new[] { 1 }, false)]
        [TestCase(new[] { 100 }, false)] // although singer is not exist, but should not check in this test case.
        [TestCase(new int[] { }, true)]
        public void TestCheckNoSinger(int[] singers, bool expected)
        {
            var lyric = new Lyric
            {
                Singers = singers
            };

            bool actual = run(lyric).Select(x => x.Template).OfType<IssueTemplateNoSinger>().Any();
            Assert.AreEqual(expected, actual);
        }

        [Ignore("Not implement.")]
        public void TestCheckSingerInBeatmap(int[] singers, bool hasIssue)
        {
        }

        private IEnumerable<Issue> run(HitObject lyric)
        {
            var beatmap = new Beatmap
            {
                HitObjects = new List<HitObject>
                {
                    lyric
                }
            };
            var context = new BeatmapVerifierContext(beatmap, new TestWorkingBeatmap(beatmap));
            return check.Run(context);
        }
    }
}
