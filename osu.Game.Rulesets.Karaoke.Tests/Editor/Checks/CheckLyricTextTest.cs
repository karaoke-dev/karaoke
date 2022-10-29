// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using NUnit.Framework;
using osu.Game.Rulesets.Karaoke.Edit.Checks;
using osu.Game.Rulesets.Karaoke.Objects;
using static osu.Game.Rulesets.Karaoke.Edit.Checks.CheckLyricText;

namespace osu.Game.Rulesets.Karaoke.Tests.Editor.Checks
{
    [TestFixture]
    public class CheckLyricTextTest : HitObjectCheckTest<Lyric, CheckLyricText>
    {
        [TestCase("karaoke")]
        [TestCase("k")] // not limit min size for now.
        [TestCase("カラオケ")] // not limit language.
        public void TestCheck(string text)
        {
            var lyric = new Lyric
            {
                Text = text
            };

            AssertOk(lyric);
        }

        [TestCase(" ")] // but should not be empty or white space.
        [TestCase("　")] // but should not be empty or white space.
        [TestCase("")]
        [TestCase(null)]
        public void TestCheckNoText(string text)
        {
            var lyric = new Lyric
            {
                Text = text
            };

            AssertNotOk<IssueTemplateLyricNoText>(lyric);
        }
    }
}
