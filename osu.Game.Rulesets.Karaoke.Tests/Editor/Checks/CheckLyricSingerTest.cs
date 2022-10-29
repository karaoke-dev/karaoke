// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using NUnit.Framework;
using osu.Game.Rulesets.Karaoke.Edit.Checks;
using osu.Game.Rulesets.Karaoke.Objects;
using static osu.Game.Rulesets.Karaoke.Edit.Checks.CheckLyricSinger;

namespace osu.Game.Rulesets.Karaoke.Tests.Editor.Checks
{
    [TestFixture]
    public class CheckLyricSingerTest : HitObjectCheckTest<Lyric, CheckLyricSinger>
    {
        [TestCase(new[] { 1, 2, 3 })]
        [TestCase(new[] { 1 })]
        [TestCase(new[] { 100 })] // although singer is not exist, but should not check in this test case.
        public void TestCheck(int[] singers)
        {
            var lyric = new Lyric
            {
                Singers = singers
            };

            AssertOk(lyric);
        }

        [TestCase(new int[] { })]
        public void TestCheckNoSinger(int[] singers)
        {
            var lyric = new Lyric
            {
                Singers = singers
            };

            AssertNotOk<IssueTemplateLyricNoSinger>(lyric);
        }
    }
}
