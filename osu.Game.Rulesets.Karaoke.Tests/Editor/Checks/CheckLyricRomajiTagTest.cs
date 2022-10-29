// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using NUnit.Framework;
using osu.Game.Rulesets.Karaoke.Edit.Checks;
using osu.Game.Rulesets.Karaoke.Objects;
using osu.Game.Rulesets.Karaoke.Tests.Helper;
using static osu.Game.Rulesets.Karaoke.Edit.Checks.CheckLyricRomajiTag;

namespace osu.Game.Rulesets.Karaoke.Tests.Editor.Checks
{
    public class CheckLyricRomajiTagTest : HitObjectCheckTest<Lyric, CheckLyricRomajiTag>
    {
        [TestCase("karaoke", new[] { "[0,2]:ka", "[2,4]:ra", "[4,5]:o", "[5,7]:ke" })]
        [TestCase("karaoke", new[] { "[0,7]:karaoke" })]
        public void TestCheck(string text, string[] romajies)
        {
            var lyric = new Lyric
            {
                Text = text,
                RomajiTags = TestCaseTagHelper.ParseRomajiTags(romajies)
            };

            AssertOk(lyric);
        }

        [TestCase("karaoke", new[] { "[-1,2]:ka" })]
        [TestCase("karaoke", new[] { "[7,8]:ke" })]
        public void TestCheckRomajiOutOfRange(string text, string[] romajies)
        {
            var lyric = new Lyric
            {
                Text = text,
                RomajiTags = TestCaseTagHelper.ParseRomajiTags(romajies)
            };

            AssertNotOk<IssueTemplateLyricRomajiOutOfRange>(lyric);
        }

        [TestCase("karaoke", new[] { "[0,2]:ka", "[1,3]:ra" })]
        [TestCase("karaoke", new[] { "[0,3]:ka", "[1,2]:ra" })]
        public void TestCheckRomajiOverlapping(string text, string[] romajies)
        {
            var lyric = new Lyric
            {
                Text = text,
                RomajiTags = TestCaseTagHelper.ParseRomajiTags(romajies)
            };

            AssertNotOk<IssueTemplateLyricRomajiOverlapping>(lyric);
        }

        [TestCase("karaoke", new[] { "[0,3]:" })]
        [TestCase("karaoke", new[] { "[0,3]: " })]
        [TestCase("karaoke", new[] { "[0,3]:　" })]
        public void TestCheckRomajiEmptyText(string text, string[] romajies)
        {
            var lyric = new Lyric
            {
                Text = text,
                RomajiTags = TestCaseTagHelper.ParseRomajiTags(romajies)
            };

            AssertNotOk<IssueTemplateLyricRomajiEmptyText>(lyric);
        }
    }
}
