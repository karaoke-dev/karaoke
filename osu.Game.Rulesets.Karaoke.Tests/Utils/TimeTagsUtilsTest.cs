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
                var generatedTimeTag = TimeTagsUtils.GenerateCenterTimeTag(
                    TestCaseTagHelper.ParseTimeTag(startTag),
                    TestCaseTagHelper.ParseTimeTag(endTag),
                    index);

                var actualTimeTag = TestCaseTagHelper.ParseTimeTag(result);
                Assert.AreEqual(generatedTimeTag.Index, actualTimeTag.Index);
                Assert.AreEqual(generatedTimeTag.Time, actualTimeTag.Time);
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
        public void TestSort(string[] timeTagTexts, string[] actualTimeTags)
        {
            var timeTags = TestCaseTagHelper.ParseTimeTags(timeTagTexts);
            var sortedTimeTag = TimeTagsUtils.Sort(timeTags);
            TimeTagAssert.ArePropertyEqual(sortedTimeTag, TestCaseTagHelper.ParseTimeTags(actualTimeTags));
        }

        [TestCase("カラオケ", new[] { "[0,start]:1000", "[1,start]:2000", "[2,start]:3000", "[3,start]:4000", "[3,end]:5000" }, new string[] { })]
        [TestCase("カラオケ", new[] { "[0,start]:1000", "[3,end]:5000" }, new string[] { })]
        [TestCase("カラオケ", new[] { "[-1,start]:1000" }, new[] { "[-1,start]:1000" })]
        [TestCase("カラオケ", new[] { "[4,start]:4000" }, new[] { "[4,start]:4000" })]
        public void TestFindOutOfRange(string text, string[] timeTagTexts, string[] invalidTimeTags)
        {
            var timeTags = TestCaseTagHelper.ParseTimeTags(timeTagTexts);
            var outOfRangeTimeTags = TimeTagsUtils.FindOutOfRange(timeTags, text);
            TimeTagAssert.ArePropertyEqual(outOfRangeTimeTags, TestCaseTagHelper.ParseTimeTags(invalidTimeTags));
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
        public void TestFindOverlapping(string[] timeTagTexts, GroupCheck other, SelfCheck self, int[] errorIndex)
        {
            // run all and find overlapping indexes.
            var timeTags = TestCaseTagHelper.ParseTimeTags(timeTagTexts);
            var overlappingTimeTags = TimeTagsUtils.FindOverlapping(timeTags, other, self);
            var overlappingTimeTagIndexed = overlappingTimeTags.Select(v => timeTags.IndexOf(v)).ToArray();
            Assert.AreEqual(overlappingTimeTagIndexed, errorIndex);
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
        public void TestFixOverlapping(string[] timeTagTexts, GroupCheck other, SelfCheck self, string[] actualTimeTagTexts)
        {
            // check which part is fixed, using list of time to check result.
            var timeTags = TestCaseTagHelper.ParseTimeTags(timeTagTexts);
            var fixedTimeTag = TimeTagsUtils.FixOverlapping(timeTags, other, self);
            TimeTagAssert.ArePropertyEqual(fixedTimeTag, TestCaseTagHelper.ParseTimeTags(actualTimeTagTexts));
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
        public void TestToDictionary(string[] timeTagTexts, double[] actualTimes)
        {
            var timeTags = TestCaseTagHelper.ParseTimeTags(timeTagTexts);
            var dictionary = TimeTagsUtils.ToDictionary(timeTags);
            Assert.AreEqual(dictionary.Values.ToArray(), actualTimes);
        }

        [TestCase(new[] { "[0,start]:1100", "[0,end]:2000", "[1,start]:2100", "[1,end]:3000" }, 1100)]
        [TestCase(new[] { "[1,end]:3000", "[1,start]:2100", "[0,end]:2000", "[0,start]:1100" }, 1100)]
        [TestCase(new[] { "[0,start]:", "[0,start]:", "[0,end]:2000", "[0,start]:1100" }, 1100)]
        public void TestGetStartTime(string[] timeTagTexts, double? actualStartTime)
        {
            var timeTags = TestCaseTagHelper.ParseTimeTags(timeTagTexts);
            var startTime = TimeTagsUtils.GetStartTime(timeTags);
            Assert.AreEqual(startTime, actualStartTime);
        }

        [TestCase(new[] { "[0,start]:1100", "[0,end]:2000", "[1,start]:2100", "[1,end]:3000" }, 3000)]
        [TestCase(new[] { "[1,end]:3000", "[1,start]:2100", "[0,end]:2000", "[0,start]:1100" }, 3000)]
        [TestCase(new[] { "[0,start]:", "[0,start]:", "[0,end]:2000", "[0,start]:1100" }, 2000)]
        public void TestGetEndTime(string[] timeTagTexts, double? actualEndTime)
        {
            var timeTags = TestCaseTagHelper.ParseTimeTags(timeTagTexts);
            var endTime = TimeTagsUtils.GetEndTime(timeTags);
            Assert.AreEqual(endTime, actualEndTime);
        }
    }
}
