// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using NUnit.Framework;
using osu.Game.Rulesets.Karaoke.Edit.Checks;
using osu.Game.Rulesets.Karaoke.Edit.Checks.Issues;
using osu.Game.Rulesets.Karaoke.Objects;
using osu.Game.Rulesets.Karaoke.Tests.Helper;
using osu.Game.Rulesets.Objects;
using static osu.Game.Rulesets.Karaoke.Edit.Checks.CheckNoteTime;

namespace osu.Game.Rulesets.Karaoke.Tests.Editor.Checks;

public class CheckNoteTimeTest : HitObjectCheckTest<Note, CheckNoteTime>
{
    [TestCase(0, new[] { "[0,start]:1000", "[1,start]:2000", "[2,start]:3000", "[3,start]:4000", "[3,end]:5000" })]
    [TestCase(3, new[] { "[0,start]:1000", "[1,start]:2000", "[2,start]:3000", "[3,start]:4000", "[3,end]:5000" })]
    public void TestCheck(int referenceTimeTagIndex, string[] timeTags)
    {
        var referencedLyric = new Lyric
        {
            Text = "カラオケ",
            TimeTags = TestCaseTagHelper.ParseTimeTags(timeTags),
        };
        var note = new Note
        {
            ReferenceLyricId = referencedLyric.ID,
            ReferenceLyric = referencedLyric,
            ReferenceTimeTagIndex = referenceTimeTagIndex,
        };

        AssertOk(new HitObject[] { referencedLyric, note });
    }

    [Test]
    public void TestCheckWithNoReferenceLyric()
    {
        var note = new Note
        {
            Text = "karaoke",
            ReferenceLyricId = null,
            ReferenceLyric = null,
        };

        // should not have error because this check will be handled in other check.
        AssertOk(new HitObject[] { note });
    }

    [TestCase(3, new[] { "[0,start]:1000", "[1,start]:2000", "[2,start]:3000", "[3,start]:4000", "[3,end]" })] // will missing start time-tag.
    [TestCase(2, new[] { "[0,start]:1000", "[1,start]:2000", "[2,start]:3000", "[3,start]", "[3,end]:5000" })] // will missing end time-tag.
    public void TestCheckMissingStartOrEndTimeTag(int referenceTimeTagIndex, string[] timeTags)
    {
        var referencedLyric = new Lyric
        {
            Text = "カラオケ",
            TimeTags = TestCaseTagHelper.ParseTimeTags(timeTags),
        };
        var note = new Note
        {
            ReferenceLyricId = referencedLyric.ID,
            ReferenceLyric = referencedLyric,
            ReferenceTimeTagIndex = referenceTimeTagIndex,
        };

        // should not have error because this check will be handled in other check.
        AssertOk(new HitObject[] { referencedLyric, note });
    }

    [TestCase("[0,start]:2000", "[1,start]:1000")]
    [TestCase("[0,end]:2000", "[1,end]:1000")]
    [TestCase("[0,start]:2000", "[1,end]:1000")]
    [TestCase("[0,end]:2000", "[1,start]:1000")]
    [TestCase("[1,start]:2000", "[0,start]:1000")] // should have error even if time-tag index is not sorted. we did not care about the time-tag index in here.
    public void TestCheckInvalidReferenceTimeTagTime(string startTimeTag, string endTimeTag)
    {
        var referencedLyric = new Lyric
        {
            Text = "カラオケ",
            TimeTags = TestCaseTagHelper.ParseTimeTags(new[] { startTimeTag, endTimeTag }),
        };
        var note = new Note
        {
            ReferenceLyricId = referencedLyric.ID,
            ReferenceLyric = referencedLyric,
            ReferenceTimeTagIndex = 0,
        };

        AssertNotOk<NoteIssue, IssueTemplateNoteInvalidReferenceTimeTagTime>(new HitObject[] { referencedLyric, note });
    }

    [TestCase("[0,start]", "[1,start]")]
    [TestCase("[0,end]", "[1,end]")]
    [TestCase("[0,start]", "[1,end]")]
    [TestCase("[0,end]", "[1,start]")]
    [TestCase("[1,start]", "[0,start]")] // should have error even if time-tag index is not sorted. we did not care about the time-tag index in here.
    public void TestCheckDurationTooShort(string startTimeTag, string endTimeTag)
    {
        var referencedLyric = new Lyric
        {
            Text = "カラオケ",
            TimeTags = TestCaseTagHelper.ParseTimeTags(new[] { $"{startTimeTag}:0", $"{endTimeTag}:{MIN_DURATION - 1}" }),
        };
        var note = new Note
        {
            ReferenceLyricId = referencedLyric.ID,
            ReferenceLyric = referencedLyric,
            ReferenceTimeTagIndex = 0,
        };

        AssertNotOk<NoteIssue, IssueTemplateNoteDurationTooShort>(new HitObject[] { referencedLyric, note });
    }

    [TestCase("[0,start]", "[1,start]")]
    [TestCase("[0,end]", "[1,end]")]
    [TestCase("[0,start]", "[1,end]")]
    [TestCase("[0,end]", "[1,start]")]
    [TestCase("[1,start]", "[0,start]")] // should have error even if time-tag index is not sorted. we did not care about the time-tag index in here.
    public void TestCheckDurationTooLong(string startTimeTag, string endTimeTag)
    {
        var referencedLyric = new Lyric
        {
            Text = "カラオケ",
            TimeTags = TestCaseTagHelper.ParseTimeTags(new[] { $"{startTimeTag}:0", $"{endTimeTag}:{MAX_DURATION + 1}" }),
        };
        var note = new Note
        {
            ReferenceLyricId = referencedLyric.ID,
            ReferenceLyric = referencedLyric,
            ReferenceTimeTagIndex = 0,
        };

        AssertNotOk<NoteIssue, IssueTemplateNoteDurationTooLong>(new HitObject[] { referencedLyric, note });
    }
}
