// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using NUnit.Framework;
using osu.Game.Rulesets.Karaoke.Edit.Checks;
using osu.Game.Rulesets.Karaoke.Objects;
using osu.Game.Rulesets.Karaoke.Tests.Helper;
using static osu.Game.Rulesets.Karaoke.Edit.Checks.CheckLyricRubyTag;

namespace osu.Game.Rulesets.Karaoke.Tests.Editor.Checks
{
    public class CheckLyricRubyTagTest : HitObjectCheckTest<Lyric, CheckLyricRubyTag>
    {
        [TestCase("カラオケ", new[] { "[0,1]:か", "[1,2]:ら", "[2,3]:お", "[3,4]:け" })]
        [TestCase("カラオケ", new[] { "[0,4]:からおけ" })]
        public void TestCheck(string text, string[] rubies)
        {
            var lyric = new Lyric
            {
                Text = text,
                RubyTags = TestCaseTagHelper.ParseRubyTags(rubies)
            };

            AssertOk(lyric);
        }

        [TestCase("カラオケ", new[] { "[-1,1]:か" })]
        [TestCase("カラオケ", new[] { "[4,5]:け" })]
        public void TestCheckRubyOutOfRange(string text, string[] rubies)
        {
            var lyric = new Lyric
            {
                Text = text,
                RubyTags = TestCaseTagHelper.ParseRubyTags(rubies)
            };

            AssertNotOk<IssueTemplateLyricRubyOutOfRange>(lyric);
        }

        [TestCase("カラオケ", new[] { "[0,1]:か", "[0,1]:ら" })]
        [TestCase("カラオケ", new[] { "[0,3]:か", "[1,2]:ら" })]
        public void TestCheckRubyOverlapping(string text, string[] rubies)
        {
            var lyric = new Lyric
            {
                Text = text,
                RubyTags = TestCaseTagHelper.ParseRubyTags(rubies)
            };

            AssertNotOk<IssueTemplateLyricRubyOverlapping>(lyric);
        }

        [TestCase("カラオケ", new[] { "[0,3]:" })]
        [TestCase("カラオケ", new[] { "[0,3]: " })]
        [TestCase("カラオケ", new[] { "[0,3]:　" })]
        public void TestCheckRubyEmptyText(string text, string[] rubies)
        {
            var lyric = new Lyric
            {
                Text = text,
                RubyTags = TestCaseTagHelper.ParseRubyTags(rubies)
            };

            AssertNotOk<IssueTemplateLyricRubyEmptyText>(lyric);
        }
    }
}
