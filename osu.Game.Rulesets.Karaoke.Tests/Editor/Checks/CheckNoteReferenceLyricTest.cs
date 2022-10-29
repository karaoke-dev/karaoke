// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using NUnit.Framework;
using osu.Game.Rulesets.Karaoke.Edit.Checks;
using osu.Game.Rulesets.Karaoke.Objects;
using osu.Game.Rulesets.Karaoke.Tests.Helper;
using osu.Game.Rulesets.Objects;
using static osu.Game.Rulesets.Karaoke.Edit.Checks.CheckNoteReferenceLyric;

namespace osu.Game.Rulesets.Karaoke.Tests.Editor.Checks
{
    [TestFixture]
    public class CheckNoteReferenceLyricTest : HitObjectCheckTest<Note, CheckNoteReferenceLyric>
    {
        [TestCase(0, new[] { "[0,start]:1000", "[3,end]:5000" })]
        [TestCase(0, new[] { "[0,start]:1000", "[1,start]:2000", "[2,start]:3000", "[3,start]:4000", "[3,end]:5000" })]
        [TestCase(1, new[] { "[0,start]:1000", "[1,start]:2000", "[2,start]:3000", "[3,start]:4000", "[3,end]:5000" })]
        [TestCase(2, new[] { "[0,start]:1000", "[1,start]:2000", "[2,start]:3000", "[3,start]:4000", "[3,end]:5000" })]
        [TestCase(3, new[] { "[0,start]:1000", "[1,start]:2000", "[2,start]:3000", "[3,start]:4000", "[3,end]:5000" })]
        public void TestCheck(int referenceTimeTagIndex, string[] timeTags)
        {
            var lyric = new Lyric
            {
                Text = "カラオケ",
                TimeTags = TestCaseTagHelper.ParseTimeTags(timeTags)
            };
            var note = new Note
            {
                ReferenceLyric = lyric,
                ReferenceTimeTagIndex = referenceTimeTagIndex
            };

            AssertOk(new HitObject[] { lyric, note });
        }

        [Test]
        public void TestCheckNullReferenceLyric()
        {
            var note = new Note
            {
                ReferenceLyric = null // reference should not be null.
            };

            AssertNotOk<IssueTemplateNullReferenceLyric>(note);
        }

        [Test]
        public void TestCheckInvalidReferenceLyric()
        {
            var lyric = new Lyric
            {
                Text = "カラオケ",
                TimeTags = TestCaseTagHelper.ParseTimeTags(new[] { "[0,start]:1000", "[3,end]:5000" })
            };
            var note = new Note
            {
                ReferenceLyric = lyric, // reference lyric should be in the beatmap.
                ReferenceTimeTagIndex = 0,
            };

            AssertNotOk<IssueTemplateInvalidReferenceLyric>(note);
        }

        [TestCase(0, new string[] { })]
        [TestCase(0, new[] { "[0,start]:1000" })]
        [TestCase(-1, new[] { "[0,start]:1000" })] // should not show other error if time-tag amount is not enough.
        [TestCase(2, new[] { "[0,start]:1000" })]
        public void TestCheckReferenceLyricHasLessThanTwoTimeTag(int referenceTimeTagIndex, string[] timeTags)
        {
            var lyric = new Lyric
            {
                Text = "カラオケ",
                TimeTags = TestCaseTagHelper.ParseTimeTags(timeTags)
            };
            var note = new Note
            {
                ReferenceLyric = lyric,
                ReferenceTimeTagIndex = referenceTimeTagIndex,
            };

            AssertNotOk<IssueTemplateReferenceLyricHasLessThanTwoTimeTag>(new HitObject[] { lyric, note });
        }

        [TestCase(-1, new[] { "[0,start]:1000", "[3,end]:5000" })]
        [TestCase(-1, new[] { "[0,start]:1000", "[1,start]:2000", "[2,start]:3000", "[3,start]:4000", "[3,end]:5000" })]
        public void TestCheckMissingStartReferenceTimeTag(int referenceTimeTagIndex, string[] timeTags)
        {
            var lyric = new Lyric
            {
                Text = "カラオケ",
                TimeTags = TestCaseTagHelper.ParseTimeTags(timeTags)
            };
            var note = new Note
            {
                ReferenceLyric = lyric,
                ReferenceTimeTagIndex = referenceTimeTagIndex
            };

            AssertNotOk<IssueTemplateMissingStartReferenceTimeTag>(new HitObject[] { lyric, note });
        }

        [TestCase(0, new[] { "[0,start]:", "[3,end]:5000" })]
        [TestCase(0, new[] { "[0,start]:", "[1,start]:2000", "[2,start]:3000", "[3,start]:4000", "[3,end]:5000" })]
        [TestCase(1, new[] { "[0,start]:1000", "[1,start]:", "[2,start]:3000", "[3,start]:4000", "[3,end]:5000" })]
        public void TestCheckStartReferenceTimeTagMissingTime(int referenceTimeTagIndex, string[] timeTags)
        {
            var lyric = new Lyric
            {
                Text = "カラオケ",
                TimeTags = TestCaseTagHelper.ParseTimeTags(timeTags)
            };
            var note = new Note
            {
                ReferenceLyric = lyric,
                ReferenceTimeTagIndex = referenceTimeTagIndex
            };

            AssertNotOk<IssueTemplateStartReferenceTimeTagMissingTime>(new HitObject[] { lyric, note });
        }

        [TestCase(1, new[] { "[0,start]:1000", "[3,end]:5000" })]
        [TestCase(4, new[] { "[0,start]:1000", "[1,start]:2000", "[2,start]:3000", "[3,start]:4000", "[3,end]:5000" })]
        public void TestCheckMissingEndReferenceTimeTag(int referenceTimeTagIndex, string[] timeTags)
        {
            var lyric = new Lyric
            {
                Text = "カラオケ",
                TimeTags = TestCaseTagHelper.ParseTimeTags(timeTags)
            };
            var note = new Note
            {
                ReferenceLyric = lyric,
                ReferenceTimeTagIndex = referenceTimeTagIndex
            };

            AssertNotOk<IssueTemplateMissingEndReferenceTimeTag>(new HitObject[] { lyric, note });
        }

        [TestCase(0, new[] { "[0,start]:1000", "[3,end]:" })]
        [TestCase(3, new[] { "[0,start]:1000", "[1,start]:2000", "[2,start]:3000", "[3,start]:4000", "[3,end]:" })]
        [TestCase(2, new[] { "[0,start]:1000", "[1,start]:2000", "[2,start]:3000", "[3,start]:", "[3,end]:5000" })]
        public void TestCheckEndReferenceTimeTagMissingTime(int referenceTimeTagIndex, string[] timeTags)
        {
            var lyric = new Lyric
            {
                Text = "カラオケ",
                TimeTags = TestCaseTagHelper.ParseTimeTags(timeTags)
            };
            var note = new Note
            {
                ReferenceLyric = lyric,
                ReferenceTimeTagIndex = referenceTimeTagIndex
            };

            AssertNotOk<IssueTemplateEndReferenceTimeTagMissingTime>(new HitObject[] { lyric, note });
        }
    }
}
