// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using NUnit.Framework;
using osu.Game.Rulesets.Karaoke.Edit.Checks;
using osu.Game.Rulesets.Karaoke.Edit.Checks.Issues;
using osu.Game.Rulesets.Karaoke.Objects;
using osu.Game.Rulesets.Karaoke.Tests.Helper;
using static osu.Game.Rulesets.Karaoke.Edit.Checks.CheckLyricRomajiTag;

namespace osu.Game.Rulesets.Karaoke.Tests.Editor.Checks;

public class CheckLyricRomajiTagTest : HitObjectCheckTest<Lyric, CheckLyricRomajiTag>
{
    [TestCase("karaoke", new[] { "[0,1]:ka", "[2,3]:ra", "[4]:o", "[5,6]:ke" })]
    [TestCase("karaoke", new[] { "[0,6]:karaoke" })]
    public void TestCheck(string text, string[] romajies)
    {
        var lyric = new Lyric
        {
            Text = text,
            RomajiTags = TestCaseTagHelper.ParseRomajiTags(romajies)
        };

        AssertOk(lyric);
    }

    [TestCase("karaoke", new[] { "[-1]:ka" })]
    [TestCase("karaoke", new[] { "[7]:ke" })]
    public void TestCheckRomajiOutOfRange(string text, string[] romajies)
    {
        var lyric = new Lyric
        {
            Text = text,
            RomajiTags = TestCaseTagHelper.ParseRomajiTags(romajies)
        };

        AssertNotOk<LyricRomajiTagIssue, IssueTemplateLyricRomajiOutOfRange>(lyric);
    }

    [TestCase("karaoke", new[] { "[0,1]:ka", "[1,2]:ra" })]
    [TestCase("karaoke", new[] { "[0,2]:ka", "[1]:ra" })]
    public void TestCheckRomajiOverlapping(string text, string[] romajies)
    {
        var lyric = new Lyric
        {
            Text = text,
            RomajiTags = TestCaseTagHelper.ParseRomajiTags(romajies)
        };

        AssertNotOk<LyricRomajiTagIssue, IssueTemplateLyricRomajiOverlapping>(lyric);
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

        AssertNotOk<LyricRomajiTagIssue, IssueTemplateLyricRomajiEmptyText>(lyric);
    }
}
