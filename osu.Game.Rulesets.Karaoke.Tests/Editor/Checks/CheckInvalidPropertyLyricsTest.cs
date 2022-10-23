// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using NUnit.Framework;
using osu.Game.Rulesets.Karaoke.Edit.Checks;
using osu.Game.Rulesets.Karaoke.Objects;
using static osu.Game.Rulesets.Karaoke.Edit.Checks.CheckInvalidPropertyLyrics;

namespace osu.Game.Rulesets.Karaoke.Tests.Editor.Checks
{
    [TestFixture]
    public class CheckInvalidPropertyLyricsTest : HitObjectCheckTest<Lyric, CheckInvalidPropertyLyrics>
    {
        [Ignore("Not implement.")]
        [TestCase(0, true)]
        [TestCase(1, false)]
        [TestCase(-1, false)]
        [TestCase(-2, false)] // in here, not check is layout actually exist.
        [TestCase(null, true)]
        public void TestCheckLayout(int? layout, bool hasIssue)
        {
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
