// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using NUnit.Framework;
using osu.Framework.Graphics.Sprites;
using osu.Game.Rulesets.Karaoke.Edit.Checks;
using osu.Game.Rulesets.Karaoke.Edit.Checks.Issues;
using osu.Game.Rulesets.Karaoke.Objects;
using osu.Game.Rulesets.Karaoke.Tests.Helper;
using static osu.Game.Rulesets.Karaoke.Edit.Checks.CheckLyricTimeTag;

namespace osu.Game.Rulesets.Karaoke.Tests.Editor.Checks;

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
            TimeTags = TestCaseTagHelper.ParseTimeTags(timeTags),
        };

        AssertOk(lyric);
    }

    [TestCase("カラオケ", new string[] { })]
    public void TestCheckMissingNoTimeTag(string text, string[] timeTags)
    {
        var lyric = new Lyric
        {
            Text = text,
            TimeTags = TestCaseTagHelper.ParseTimeTags(timeTags),
        };

        AssertNotOk<LyricIssue, IssueTemplateLyricEmptyTimeTag>(lyric);
    }

    [TestCase("カラオケ", new[] { "[3,end]:5000" })]
    public void TestCheckMissingFirstTimeTag(string text, string[] timeTags)
    {
        var lyric = new Lyric
        {
            Text = text,
            TimeTags = TestCaseTagHelper.ParseTimeTags(timeTags),
        };

        AssertNotOk<LyricIssue, IssueTemplateLyricMissingFirstTimeTag>(lyric);
    }

    [TestCase("カラオケ", new[] { "[0,start]:5000" })]
    public void TestCheckMissingLastTimeTag(string text, string[] timeTags)
    {
        var lyric = new Lyric
        {
            Text = text,
            TimeTags = TestCaseTagHelper.ParseTimeTags(timeTags),
        };

        AssertNotOk<LyricIssue, IssueTemplateLyricMissingLastTimeTag>(lyric);
    }

    [TestCase("カラオケ", new[] { "[-1,start]:0", "[0,start]:1000", "[3,end]:1000" })] // out-of range start time-tag time.
    [TestCase("カラオケ", new[] { "[0,start]:1000", "[3,end]:1000", "[4,start]:2000" })] // out-of range end time-tag time.
    public void TestCheckOutOfRange(string text, string[] timeTags)
    {
        var lyric = new Lyric
        {
            Text = text,
            TimeTags = TestCaseTagHelper.ParseTimeTags(timeTags),
        };

        AssertNotOk<LyricTimeTagIssue, IssueTemplateLyricTimeTagOutOfRange>(lyric);
    }

    [TestCase("カラオケ", new[] { "[0,start]:5000", "[3,end]:1000" })]
    public void TestCheckOverlapping(string text, string[] timeTags)
    {
        var lyric = new Lyric
        {
            Text = text,
            TimeTags = TestCaseTagHelper.ParseTimeTags(timeTags),
        };

        AssertNotOk<LyricTimeTagIssue, IssueTemplateLyricTimeTagOverlapping>(lyric);
    }

    [TestCase("カラオケ", new[] { "[0,start]", "[3,end]:1000" })] // empty start time-tag time.
    [TestCase("カラオケ", new[] { "[0,start]:1000", "[3,end]" })] // empty end time-tag time.
    [TestCase("カラオケ", new[] { "[0,start]:1000", "[1,start]", "[3,end]:2000" })] // empty center time-tag time.
    public void TestCheckEmptyTime(string text, string[] timeTags)
    {
        var lyric = new Lyric
        {
            Text = text,
            TimeTags = TestCaseTagHelper.ParseTimeTags(timeTags),
        };

        AssertNotOk<LyricTimeTagIssue, IssueTemplateLyricTimeTagEmptyTime>(lyric);
    }

    [TestCase("カラオケ", "")] // should not be empty.
    [TestCase("カラオケ", " ")] // should not be white-space only.
    [TestCase("カラオケ", "卡拉OK")] // should be within latin.
    public void TestCheckTimeTagRomajiInvalidText(string text, string romanisedSyllable)
    {
        var lyric = new Lyric
        {
            Text = text,
            TimeTags = new[]
            {
                new TimeTag(new TextIndex())
                {
                    RomanisedSyllable = romanisedSyllable,
                    Time = 1000,
                },
                new TimeTag(new TextIndex(3, TextIndex.IndexState.End))
                {
                    Time = 2000,
                },
            },
        };

        AssertNotOk<LyricTimeTagIssue, IssueTemplateLyricTimeTagRomajiInvalidText>(lyric);
    }

    [TestCase("カラオケ", null)] // should not be white-space only.
    public void TestCheckRomajiEmptyTextIfFirst(string text, string romanisedSyllable)
    {
        var lyric = new Lyric
        {
            Text = text,
            TimeTags = new[]
            {
                new TimeTag(new TextIndex())
                {
                    RomanisedSyllable = romanisedSyllable,
                    FirstSyllable = true,
                    Time = 1000,
                },
                new TimeTag(new TextIndex(3, TextIndex.IndexState.End))
                {
                    Time = 2000,
                },
            },
        };

        AssertNotOk<LyricTimeTagIssue, IssueTemplateLyricTimeTagRomajiInvalidTextIfFirst>(lyric);
    }

    [TestCase("カラオケ", "")] // should not have empty text if end.
    [TestCase("カラオケ", " ")] // should not have empty text if end.
    [TestCase("カラオケ", "123")] // should not have empty text if end.
    public void TestRomajiNotHaveEmptyTextIfEnd(string text, string romanisedSyllable)
    {
        var lyric = new Lyric
        {
            Text = text,
            TimeTags = new[]
            {
                new TimeTag(new TextIndex())
                {
                    Time = 1000,
                },
                new TimeTag(new TextIndex(3, TextIndex.IndexState.End))
                {
                    RomanisedSyllable = romanisedSyllable,
                    Time = 2000,
                },
            },
        };

        AssertNotOk<LyricTimeTagIssue, IssueTemplateLyricTimeTagRomajiNotHaveEmptyTextIfEnd>(lyric);
    }

    [Test]
    public void TestRomajiNotFistRomajiTextIfEnd()
    {
        var lyric = new Lyric
        {
            Text = "カラオケ",
            TimeTags = new[]
            {
                new TimeTag(new TextIndex())
                {
                    Time = 1000,
                },
                new TimeTag(new TextIndex(3, TextIndex.IndexState.End))
                {
                    FirstSyllable = true, // is invalid.
                    Time = 2000,
                },
            },
        };

        AssertNotOk<LyricTimeTagIssue, IssueTemplateLyricTimeTagRomajiNotFistRomajiTextIfEnd>(lyric);
    }
}
