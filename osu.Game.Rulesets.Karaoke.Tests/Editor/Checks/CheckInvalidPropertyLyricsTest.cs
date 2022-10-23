// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Globalization;
using NUnit.Framework;
using osu.Game.Rulesets.Karaoke.Edit.Checks;
using osu.Game.Rulesets.Karaoke.Objects;
using static osu.Game.Rulesets.Karaoke.Edit.Checks.CheckInvalidPropertyLyrics;

namespace osu.Game.Rulesets.Karaoke.Tests.Editor.Checks
{
    [TestFixture]
    public class CheckInvalidPropertyLyricsTest : HitObjectCheckTest<Lyric, CheckInvalidPropertyLyrics>
    {
        [TestCase("Ja-jp")]
        [TestCase("")] // should not have issue if CultureInfo accept it.
        public void TestCheckLanguage(string language)
        {
            var lyric = new Lyric
            {
                Language = new CultureInfo(language),
            };

            AssertOk(lyric);
        }

        [TestCase(null)]
        public void TestCheckInvalidLanguage(string? language)
        {
            var lyric = new Lyric
            {
                Language = language != null ? new CultureInfo(language) : null,
            };

            AssertNotOk<IssueTemplateNotFillLanguage>(lyric);
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

        [TestCase("karaoke")]
        [TestCase("k")] // not limit min size for now.
        [TestCase("カラオケ")] // not limit language.
        public void TestCheckText(string text)
        {
            var lyric = new Lyric
            {
                Text = text
            };

            AssertOk(lyric);
        }

        [TestCase(" ")] // but should not be empty or white space.
        [TestCase("")]
        [TestCase(null)]
        public void TestCheckInvalidText(string text)
        {
            var lyric = new Lyric
            {
                Text = text
            };

            AssertNotOk<IssueTemplateNoText>(lyric);
        }

        [TestCase(new[] { 1, 2, 3 })]
        [TestCase(new[] { 1 })]
        [TestCase(new[] { 100 })] // although singer is not exist, but should not check in this test case.
        public void TestCheckSinger(int[] singers)
        {
            var lyric = new Lyric
            {
                Singers = singers
            };

            AssertOk(lyric);
        }

        [TestCase(new int[] { })]
        public void TestCheckInvalidSinger(int[] singers)
        {
            var lyric = new Lyric
            {
                Singers = singers
            };

            AssertNotOk<IssueTemplateNoSinger>(lyric);
        }

        [Ignore("Not implement.")]
        public void TestCheckSingerInBeatmap(int[] singers, bool hasIssue)
        {
        }
    }
}
