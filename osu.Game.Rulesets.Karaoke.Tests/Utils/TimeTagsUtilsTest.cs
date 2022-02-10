// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Linq;
using NUnit.Framework;
using osu.Game.Rulesets.Karaoke.Extensions;
using osu.Game.Rulesets.Karaoke.Tests.Asserts;
using osu.Game.Rulesets.Karaoke.Tests.Helper;
using osu.Game.Rulesets.Karaoke.Utils;

namespace osu.Game.Rulesets.Karaoke.Tests.Utils
{
    [TestFixture]
    public class TimeTagsUtilsTest
    {
        [TestCase("[1,start]:1000", "[3,start]:3000", 2, "[2,start]:2000")]
        [TestCase("[1,start]:1000", "[3,end]:4000", 2, "[2,start]:2000")]
        [TestCase("[1,end]:2000", "[3,start]:3000", 2, "[2,start]:2000")]
        [TestCase("[1,start]:", "[3,start]:3000", 2, "[2,start]:")]
        [TestCase("[1,start]:1000", "[3,start]:", 2, "[2,start]:")]
        [TestCase("[1,start]:", "[3,start]:", 2, "[2,start]:")]
        [TestCase("[0,start]:", "[0,start]:", 0, "[0,start]:")] // edge case, but it's valid.
        [TestCase("[1,start]:", "[3,start]:", 10, null)] // new index should be in the range.
        [TestCase("[10,start]:", "[3,start]:", 10, null)] // start index should be smaller then end index.
        [TestCase(null, "[3,start]:", 10, null)] // should not be null.
        [TestCase("[1,start]:", null, 2, null)] // should not be null.
        [TestCase(null, null, 2, null)] // should not be null.
        public void GenerateTimeTag(string startTag, string endTag, int index, string result)
        {
            try
            {
                var expectedTimeTag = TestCaseTagHelper.ParseTimeTag(result);
                var actualTimeTag = TimeTagsUtils.GenerateCenterTimeTag(
                    TestCaseTagHelper.ParseTimeTag(startTag),
                    TestCaseTagHelper.ParseTimeTag(endTag),
                    index);

                Assert.AreEqual(expectedTimeTag.Index, actualTimeTag.Index);
                Assert.AreEqual(expectedTimeTag.Time, actualTimeTag.Time);
            }
            catch
            {
                Assert.IsNull(result);
            }
        }

        [TestCase(new[] { "[0,start]:1100", "[0,end]:2000", "[1,start]:2100", "[1,end]:3000" }, new[] { "[0,start]:1100", "[0,end]:2000", "[1,start]:2100", "[1,end]:3000" })]
        [TestCase(new[] { "[1,end]:3000", "[1,start]:2100", "[0,end]:2000", "[0,start]:1100" }, new[] { "[0,start]:1100", "[0,end]:2000", "[1,start]:2100", "[1,end]:3000" })]
        [TestCase(new[] { "[0,start]:", "[0,start]:", "[0,end]:2000", "[0,start]:1100" }, new[] { "[0,start]:", "[0,start]:", "[0,start]:1100", "[0,end]:2000" })]
        [TestCase(new[] { "[0,start]:1000", "[0,start]:1100", "[0,end]:2000", "[0,start]:1100" }, new[] { "[0,start]:1000", "[0,start]:1100", "[0,start]:1100", "[0,end]:2000" })]
        [TestCase(new[] { "[0,start]:", "[0,end]:", "[0,start]:", "[1,start]:", "[1,end]:" }, new[] { "[0,start]:", "[0,start]:", "[0,end]:", "[1,start]:", "[1,end]:" })]
        public void TestSort(string[] timeTagTexts, string[] expectedTimeTags)
        {
            var timeTags = TestCaseTagHelper.ParseTimeTags(timeTagTexts);

            var expected = TestCaseTagHelper.ParseTimeTags(expectedTimeTags);
            var actual = TimeTagsUtils.Sort(timeTags);
            TimeTagAssert.ArePropertyEqual(expected, actual);
        }

        [TestCase("カラオケ", new[] { "[0,start]:1000", "[1,start]:2000", "[2,start]:3000", "[3,start]:4000", "[3,end]:5000" }, new string[] { })]
        [TestCase("カラオケ", new[] { "[0,start]:1000", "[3,end]:5000" }, new string[] { })]
        [TestCase("カラオケ", new[] { "[-1,start]:1000" }, new[] { "[-1,start]:1000" })]
        [TestCase("カラオケ", new[] { "[4,start]:4000" }, new[] { "[4,start]:4000" })]
        public void TestFindOutOfRange(string text, string[] timeTagTexts, string[] invalidTimeTags)
        {
            var timeTags = TestCaseTagHelper.ParseTimeTags(timeTagTexts);

            var expected = TestCaseTagHelper.ParseTimeTags(invalidTimeTags);
            var actual = TimeTagsUtils.FindOutOfRange(timeTags, text);
            TimeTagAssert.ArePropertyEqual(expected, actual);
        }

        [TestCase(new[] { "[0,start]:1000", "[1,start]:", "[2,start]:3000", "[3,start]:", "[3,end]:5000" }, new[] { "[1,start]:", "[3,start]:" })]
        [TestCase(new[] { "[0,start]:", "[3,end]:" }, new[] { "[0,start]:", "[3,end]:" })]
        public void TestFindNoneTime(string[] timeTagTexts, string[] invalidTimeTags)
        {
            var timeTags = TestCaseTagHelper.ParseTimeTags(timeTagTexts);

            var expected = TestCaseTagHelper.ParseTimeTags(invalidTimeTags);
            var actual = TimeTagsUtils.FindNoneTime(timeTags);
            TimeTagAssert.ArePropertyEqual(expected, actual);
        }

        [TestCase("カラオケ", new[] { "[0,start]:1000", "[3,end]:2000" }, true)]
        [TestCase("カラオケ", new[] { "[0,start]:2000", "[3,end]:3000", "[3,end]:4000" }, true)]
        [TestCase("カラオケ", new[] { "[0,start]:3000", "[0,start]:4000" }, true)] // multiple start time-tag is ok.
        [TestCase("カラオケ", new[] { "[3,end]:1000" }, false)]
        [TestCase("カラオケ", new[] { "[-1,start]:1000", "[3,end]:2000" }, false)] // out of range end time-tag should be count as missing.
        [TestCase("", new[] { "[0,start]:1000", "[0,end]:2000" }, false)] // empty lyric should always count as missing.
        [TestCase("カラオケ", null, false)] // empty time-tag should always count as missing.
        public void TestHasStartTimeTagInLyric(string text, string[] timeTagTexts, bool expected)
        {
            var timeTags = TestCaseTagHelper.ParseTimeTags(timeTagTexts);

            bool actual = TimeTagsUtils.HasStartTimeTagInLyric(timeTags, text);
            Assert.AreEqual(expected, actual);
        }

        [TestCase("カラオケ", new[] { "[0,start]:1000", "[3,end]:2000" }, true)]
        [TestCase("カラオケ", new[] { "[3,start]:2000", "[3,start]:3000", "[3,end]:4000" }, true)]
        [TestCase("カラオケ", new[] { "[3,end]:3000", "[3,end]:4000" }, true)] // multiple end time-tag is ok.
        [TestCase("カラオケ", new[] { "[0,start]:1000" }, false)]
        [TestCase("カラオケ", new[] { "[0,start]:1000", "[5,end]:2000" }, false)] // out of range end time-tag should be count as missing.
        [TestCase("", new[] { "[0,start]:1000", "[0,end]:2000" }, false)] // empty lyric should always count as missing.
        public void TestHasEndTimeTagInLyric(string text, string[] timeTagTexts, bool expected)
        {
            var timeTags = TestCaseTagHelper.ParseTimeTags(timeTagTexts);

            bool actual = TimeTagsUtils.HasEndTimeTagInLyric(timeTags, text);
            Assert.AreEqual(expected, actual);
        }

        [TestCase(new[] { "[0,start]:2000", "[0,end]:1000" }, GroupCheck.Asc, SelfCheck.BasedOnStart, new[] { 1 })]
        [TestCase(new[] { "[0,start]:2000", "[0,end]:1000" }, GroupCheck.Asc, SelfCheck.BasedOnEnd, new[] { 0 })]
        [TestCase(new[] { "[0,start]:1100", "[0,end]:2100", "[1,start]:2000", "[1,end]:3000" }, GroupCheck.Asc, SelfCheck.BasedOnStart, new[] { 2 })]
        [TestCase(new[] { "[0,start]:1100", "[0,end]:2100", "[1,start]:2000", "[1,end]:3000" }, GroupCheck.Desc, SelfCheck.BasedOnStart, new[] { 1 })]
        [TestCase(new[] { "[0,start]:1000", "[0,end]:5000", "[1,start]:2000", "[1,end]:3000" }, GroupCheck.Asc, SelfCheck.BasedOnStart, new[] { 2, 3 })]
        [TestCase(new[] { "[0,start]:1000", "[0,end]:5000", "[1,start]:2000", "[1,end]:3000" }, GroupCheck.Desc, SelfCheck.BasedOnStart, new[] { 1 })]
        [TestCase(new[] { "[0,start]:1000", "[0,end]:2000", "[1,start]:0", "[1,end]:3000" }, GroupCheck.Asc, SelfCheck.BasedOnStart, new[] { 2 })]
        [TestCase(new[] { "[0,start]:1000", "[0,end]:2000", "[1,start]:0", "[1,end]:3000" }, GroupCheck.Desc, SelfCheck.BasedOnStart, new[] { 0, 1 })]
        [TestCase(new[] { "[0,start]:4000", "[0,end]:3000", "[1,start]:2000", "[1,end]:1000" }, GroupCheck.Asc, SelfCheck.BasedOnStart, new[] { 1, 2, 3 })]
        [TestCase(new[] { "[0,start]:4000", "[0,end]:3000", "[1,start]:2000", "[1,end]:1000" }, GroupCheck.Asc, SelfCheck.BasedOnEnd, new[] { 0, 2, 3 })]
        [TestCase(new[] { "[0,start]:4000", "[0,end]:3000", "[1,start]:2000", "[1,end]:1000" }, GroupCheck.Desc, SelfCheck.BasedOnStart, new[] { 0, 1, 3 })]
        [TestCase(new[] { "[0,start]:4000", "[0,end]:3000", "[1,start]:2000", "[1,end]:1000" }, GroupCheck.Desc, SelfCheck.BasedOnEnd, new[] { 0, 1, 2 })]
        public void TestFindOverlapping(string[] timeTagTexts, GroupCheck other, SelfCheck self, int[] expected)
        {
            // run all and find overlapping indexes.
            var timeTags = TestCaseTagHelper.ParseTimeTags(timeTagTexts);
            var overlappingTimeTags = TimeTagsUtils.FindOverlapping(timeTags, other, self);

            int[] actual = overlappingTimeTags.Select(v => timeTags.IndexOf(v)).ToArray();
            Assert.AreEqual(expected, actual);
        }

        [TestCase(new[] { "[0,start]:2000", "[0,end]:1000" }, GroupCheck.Asc, SelfCheck.BasedOnStart, new[] { "[0,start]:2000", "[0,end]:2000" })]
        [TestCase(new[] { "[0,start]:2000", "[0,end]:1000" }, GroupCheck.Asc, SelfCheck.BasedOnEnd, new[] { "[0,start]:1000", "[0,end]:1000" })]
        [TestCase(new[] { "[0,start]:1100", "[0,end]:2100", "[1,start]:2000", "[1,end]:3000" }, GroupCheck.Asc, SelfCheck.BasedOnStart,
            new[] { "[0,start]:1100", "[0,end]:2100", "[1,start]:2100", "[1,end]:3000" })]
        [TestCase(new[] { "[0,start]:1100", "[0,end]:2100", "[1,start]:2000", "[1,end]:3000" }, GroupCheck.Desc, SelfCheck.BasedOnStart,
            new[] { "[0,start]:1100", "[0,end]:2000", "[1,start]:2000", "[1,end]:3000" })]
        [TestCase(new[] { "[0,start]:1000", "[0,end]:5000", "[1,start]:2000", "[1,end]:3000" }, GroupCheck.Asc, SelfCheck.BasedOnStart,
            new[] { "[0,start]:1000", "[0,end]:5000", "[1,start]:5000", "[1,end]:5000" })]
        [TestCase(new[] { "[0,start]:1000", "[0,end]:5000", "[1,start]:2000", "[1,end]:3000" }, GroupCheck.Desc, SelfCheck.BasedOnStart,
            new[] { "[0,start]:1000", "[0,end]:2000", "[1,start]:2000", "[1,end]:3000" })]
        [TestCase(new[] { "[0,start]:1000", "[0,end]:2000", "[1,start]:0", "[1,end]:3000" }, GroupCheck.Asc, SelfCheck.BasedOnStart,
            new[] { "[0,start]:1000", "[0,end]:2000", "[1,start]:2000", "[1,end]:3000" })]
        [TestCase(new[] { "[0,start]:1000", "[0,end]:2000", "[1,start]:0", "[1,end]:3000" }, GroupCheck.Desc, SelfCheck.BasedOnStart,
            new[] { "[0,start]:0", "[0,end]:0", "[1,start]:0", "[1,end]:3000" })]
        //[TestCase(new[] { "[0,start]:4000", "[0,end]:3000", "[1,start]:2000", "[1,end]:1000" }, GroupCheck.Asc, SelfCheck.BasedOnStart, new double[] { "[0,start]:4000", "[0,end]:4000", "[1,start]:4000", "[1,end]:4000" })]
        //[TestCase(new[] { "[0,start]:4000", "[0,end]:3000", "[1,start]:2000", "[1,end]:1000" }, GroupCheck.Asc, SelfCheck.BasedOnEnd, new double[] { "[0,start]:3000", "[0,end]:3000", "[1,start]:3000", "[1,end]:3000" })]
        //[TestCase(new[] { "[0,start]:4000", "[0,end]:3000", "[1,start]:2000", "[1,end]:1000" }, GroupCheck.Desc, SelfCheck.BasedOnStart, new double[] { "[0,start]:2000", "[0,end]:2000", "[1,start]:2000", "[1,end]:2000" })]
        //[TestCase(new[] { "[0,start]:4000", "[0,end]:3000", "[1,start]:2000", "[1,end]:1000" }, GroupCheck.Desc, SelfCheck.BasedOnEnd, new double[] { "[0,start]:1000", "[0,end]:1000", "[1,start]:1000", "[1,end]:1000" })]
        public void TestFixOverlapping(string[] timeTagTexts, GroupCheck other, SelfCheck self, string[] expectedTimeTagTexts)
        {
            // check which part is fixed, using list of time to check result.
            var timeTags = TestCaseTagHelper.ParseTimeTags(timeTagTexts);

            var expected = TestCaseTagHelper.ParseTimeTags(expectedTimeTagTexts);
            var actual = TimeTagsUtils.FixOverlapping(timeTags, other, self);
            TimeTagAssert.ArePropertyEqual(expected, actual);
        }

        [TestCase(new[] { "[0,start]:1100", "[0,end]:2000", "[1,start]:2100", "[1,end]:3000" }, new double[] { 1100, 2000, 2100, 3000 })]
        [TestCase(new[] { "[1,end]:3000", "[1,start]:2100", "[0,end]:2000", "[0,start]:1100" }, new double[] { 1100, 2000, 2100, 3000 })]
        [TestCase(new[] { "[0,start]:", "[0,start]:", "[0,end]:2000", "[0,start]:1100" }, new double[] { 1100, 2000 })]
        [TestCase(new[] { "[0,start]:1000", "[0,start]:1100", "[0,end]:2000", "[0,start]:1100" }, new double[] { 1000, 2000 })]
        [TestCase(new[] { "[0,start]:", "[0,end]:", "[0,start]:", "[1,start]:", "[1,end]:" }, new double[] { })]
        [TestCase(new[] { "[0,start]:2000", "[0,end]:1000" }, new double[] { 2000, 2000 })]
        [TestCase(new[] { "[0,start]:1100", "[0,end]:2100", "[1,start]:2000", "[1,end]:3000" }, new double[] { 1100, 2100, 2100, 3000 })]
        [TestCase(new[] { "[0,start]:1000", "[0,end]:5000", "[1,start]:2000", "[1,end]:3000" }, new double[] { 1000, 5000, 5000, 5000 })]
        [TestCase(new[] { "[0,start]:1000", "[0,end]:2000", "[1,start]:0", "[1,end]:3000" }, new double[] { 1000, 2000, 2000, 3000 })]
        //[TestCase(new[] { "[0,start]:4000", "[0,end]:3000", "[1,start]:2000", "[1,end]:1000" }, new double[] { 4000, 4000, 4000, 4000 })]
        public void TestToDictionary(string[] timeTagTexts, double[] expected)
        {
            var timeTags = TestCaseTagHelper.ParseTimeTags(timeTagTexts);

            double[] actual = TimeTagsUtils.ToDictionary(timeTags).Values.ToArray();
            Assert.AreEqual(expected, actual);
        }

        [TestCase(new[] { "[0,start]:1100", "[0,end]:2000", "[1,start]:2100", "[1,end]:3000" }, 1100)]
        [TestCase(new[] { "[1,end]:3000", "[1,start]:2100", "[0,end]:2000", "[0,start]:1100" }, 1100)]
        [TestCase(new[] { "[0,start]:", "[0,start]:", "[0,end]:2000", "[0,start]:1100" }, 1100)]
        public void TestGetStartTime(string[] timeTagTexts, double? expected)
        {
            var timeTags = TestCaseTagHelper.ParseTimeTags(timeTagTexts);

            double? actual = TimeTagsUtils.GetStartTime(timeTags);
            Assert.AreEqual(expected, actual);
        }

        [TestCase(new[] { "[0,start]:1100", "[0,end]:2000", "[1,start]:2100", "[1,end]:3000" }, 3000)]
        [TestCase(new[] { "[1,end]:3000", "[1,start]:2100", "[0,end]:2000", "[0,start]:1100" }, 3000)]
        [TestCase(new[] { "[0,start]:", "[0,start]:", "[0,end]:2000", "[0,start]:1100" }, 2000)]
        public void TestGetEndTime(string[] timeTagTexts, double? expected)
        {
            var timeTags = TestCaseTagHelper.ParseTimeTags(timeTagTexts);

            double? actual = TimeTagsUtils.GetEndTime(timeTags);
            Assert.AreEqual(expected, actual);
        }
    }
}
