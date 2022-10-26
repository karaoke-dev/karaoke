// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using NUnit.Framework;
using osu.Game.Rulesets.Karaoke.Edit.Checks;
using osu.Game.Rulesets.Karaoke.Objects;
using osu.Game.Rulesets.Karaoke.Tests.Helper;
using static osu.Game.Rulesets.Karaoke.Edit.Checks.CheckLyricTimeTag;

namespace osu.Game.Rulesets.Karaoke.Tests.Editor.Checks
{
    public class CheckLyricTimeTagTest : HitObjectCheckTest<Lyric, CheckLyricTimeTag>
    {
        [TestCase("カラオケ", new[] { "[0,start]:1000", "[1,start]:2000", "[2,start]:3000", "[3,start]:4000", "[3,end]:5000" })]
        [TestCase("カラオケ", new[] { "[0,start]:1000", "[3,end]:5000" })]
        [TestCase("カラオケ", new[] { "[0,start]:1000", "[3,end]:5000" })]
        public void TestCheck(string text, string[] timeTags)
        {
            var lyric = new Lyric
            {
                Text = text,
                TimeTags = TestCaseTagHelper.ParseTimeTags(timeTags)
            };

            AssertOk(lyric);
        }

        [TestCase("カラオケ", new string[] { })]
        public void TestCheckMissingNoTimeTag(string text, string[] timeTags)
        {
            var lyric = new Lyric
            {
                Text = text,
                TimeTags = TestCaseTagHelper.ParseTimeTags(timeTags)
            };

            AssertNotOk<IssueTemplateLyricEmptyTimeTag>(lyric);
        }

        [TestCase("カラオケ", new[] { "[3,end]:5000" })]
        public void TestCheckMissingFirstTimeTag(string text, string[] timeTags)
        {
            var lyric = new Lyric
            {
                Text = text,
                TimeTags = TestCaseTagHelper.ParseTimeTags(timeTags)
            };

            AssertNotOk<IssueTemplateLyricMissingFirstTimeTag>(lyric);
        }

        [TestCase("カラオケ", new[] { "[0,start]:5000" })]
        public void TestCheckMissingLastTimeTag(string text, string[] timeTags)
        {
            var lyric = new Lyric
            {
                Text = text,
                TimeTags = TestCaseTagHelper.ParseTimeTags(timeTags)
            };

            AssertNotOk<IssueTemplateLyricMissingLastTimeTag>(lyric);
        }

        [TestCase("カラオケ", new[] { "[-1,start]:1000" })]
        [TestCase("カラオケ", new[] { "[4,start]:4000" })]
        public void TestCheckOutOfRange(string text, string[] timeTags)
        {
            var lyric = new Lyric
            {
                Text = text,
                TimeTags = TestCaseTagHelper.ParseTimeTags(timeTags)
            };

            AssertNotOk<IssueTemplateLyricTimeTagOutOfRange>(lyric);
        }

        [TestCase("カラオケ", new[] { "[0,start]:5000", "[3,end]:1000" })]
        public void TestCheckOverlapping(string text, string[] timeTags)
        {
            var lyric = new Lyric
            {
                Text = text,
                TimeTags = TestCaseTagHelper.ParseTimeTags(timeTags)
            };

            AssertNotOk<IssueTemplateLyricTimeTagOverlapping>(lyric);
        }

        [TestCase("カラオケ", new[] { "[0,start]:" })]
        public void TestCheckEmptyTime(string text, string[] timeTags)
        {
            var lyric = new Lyric
            {
                Text = text,
                TimeTags = TestCaseTagHelper.ParseTimeTags(timeTags)
            };

            AssertNotOk<IssueTemplateLyricTimeTagEmptyTime>(lyric);
        }
    }
}
