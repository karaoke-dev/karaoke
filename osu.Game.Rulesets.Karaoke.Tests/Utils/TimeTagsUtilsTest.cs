// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore.Internal;
using NUnit.Framework;
using osu.Framework.Graphics.Sprites;
using osu.Game.Rulesets.Karaoke.Objects;
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

        [TestCase(nameof(ValidTimeTagWithSorted), new double[] { 1100, 2000, 2100, 3000 })]
        [TestCase(nameof(ValidTimeTagWithUnsorted), new double[] { 1100, 2000, 2100, 3000 })]
        [TestCase(nameof(ValidTimeTagWithUnsortedAndDuplicatedWithNoValue), new double[] { 1100, 2000 })]
        [TestCase(nameof(ValidTimeTagWithUnsortedAndDuplicatedWithValue), new double[] { 1000, 1100, 1100, 2000 })]
        [TestCase(nameof(ValidTimeTagWithUnsortedAndAllEmpty), new double[] { })]
        public void TestSort(string testCase, double[] results)
        {
            var timeTags = getValueByMethodName(testCase);

            // run all then using time(nullable double) to check.
            var sortedTimeTag = TimeTagsUtils.Sort(timeTags);
            Assert.AreEqual(getSortedTime(sortedTimeTag), results);
        }

        [TestCase(nameof(InvalidTimeTagWithStartLargerThenEnd), GroupCheck.Asc, SelfCheck.BasedOnStart, new[] { 1 })]
        [TestCase(nameof(InvalidTimeTagWithStartLargerThenEnd), GroupCheck.Asc, SelfCheck.BasedOnEnd, new[] { 0 })]
        [TestCase(nameof(InvalidTimeTagWithEndLargerThenNextStart), GroupCheck.Asc, SelfCheck.BasedOnStart, new[] { 2 })]
        [TestCase(nameof(InvalidTimeTagWithEndLargerThenNextStart), GroupCheck.Desc, SelfCheck.BasedOnStart, new[] { 1 })]
        [TestCase(nameof(InvalidTimeTagWithEndLargerThenNextEnd), GroupCheck.Asc, SelfCheck.BasedOnStart, new[] { 2, 3 })]
        [TestCase(nameof(InvalidTimeTagWithEndLargerThenNextEnd), GroupCheck.Desc, SelfCheck.BasedOnStart, new[] { 1 })]
        [TestCase(nameof(InvalidTimeTagWithStartSmallerThenPreviousStart), GroupCheck.Asc, SelfCheck.BasedOnStart, new[] { 2 })]
        [TestCase(nameof(InvalidTimeTagWithStartSmallerThenPreviousStart), GroupCheck.Desc, SelfCheck.BasedOnStart, new[] { 0, 1 })]
        [TestCase(nameof(InvalidTimeTagWithAllInverse), GroupCheck.Asc, SelfCheck.BasedOnStart, new[] { 1, 2, 3 })]
        [TestCase(nameof(InvalidTimeTagWithAllInverse), GroupCheck.Asc, SelfCheck.BasedOnEnd, new[] { 0, 2, 3 })]
        [TestCase(nameof(InvalidTimeTagWithAllInverse), GroupCheck.Desc, SelfCheck.BasedOnStart, new[] { 0, 1, 3 })]
        [TestCase(nameof(InvalidTimeTagWithAllInverse), GroupCheck.Desc, SelfCheck.BasedOnEnd, new[] { 0, 1, 2 })]
        public void TestFindInvalid(string testCase, GroupCheck other, SelfCheck self, int[] errorIndex)
        {
            var timeTags = getValueByMethodName(testCase);

            // run all and find invalid indexes.
            var invalidTimeTag = TimeTagsUtils.FindInvalid(timeTags, other, self);
            var invalidIndexes = invalidTimeTag.Select(v => timeTags.IndexOf(v)).ToArray();
            Assert.AreEqual(invalidIndexes, errorIndex);
        }

        [TestCase(nameof(InvalidTimeTagWithStartLargerThenEnd), GroupCheck.Asc, SelfCheck.BasedOnStart, new double[] { 2000, 2000 })]
        [TestCase(nameof(InvalidTimeTagWithStartLargerThenEnd), GroupCheck.Asc, SelfCheck.BasedOnEnd, new double[] { 1000, 1000 })]
        [TestCase(nameof(InvalidTimeTagWithEndLargerThenNextStart), GroupCheck.Asc, SelfCheck.BasedOnStart, new double[] { 1100, 2100, 2100, 3000 })]
        [TestCase(nameof(InvalidTimeTagWithEndLargerThenNextStart), GroupCheck.Desc, SelfCheck.BasedOnStart, new double[] { 1100, 2000, 2000, 3000 })]
        [TestCase(nameof(InvalidTimeTagWithEndLargerThenNextEnd), GroupCheck.Asc, SelfCheck.BasedOnStart, new double[] { 1000, 5000, 5000, 5000 })]
        [TestCase(nameof(InvalidTimeTagWithEndLargerThenNextEnd), GroupCheck.Desc, SelfCheck.BasedOnStart, new double[] { 1000, 2000, 2000, 3000 })]
        [TestCase(nameof(InvalidTimeTagWithStartSmallerThenPreviousStart), GroupCheck.Asc, SelfCheck.BasedOnStart, new double[] { 1000, 2000, 2000, 3000 })]
        [TestCase(nameof(InvalidTimeTagWithStartSmallerThenPreviousStart), GroupCheck.Desc, SelfCheck.BasedOnStart, new double[] { 0, 0, 0, 3000 })]
        //[TestCase(nameof(InvalidTimeTagWithAllInverse), GroupCheck.Asc, SelfCheck.BasedOnStart, new double[] { 4000, 4000, 4000, 4000 })]
        //[TestCase(nameof(InvalidTimeTagWithAllInverse), GroupCheck.Asc, SelfCheck.BasedOnEnd, new double[] { 3000, 3000, 3000, 3000 })]
        //[TestCase(nameof(InvalidTimeTagWithAllInverse), GroupCheck.Desc, SelfCheck.BasedOnStart, new double[] { 2000, 2000, 2000, 2000 })]
        //[TestCase(nameof(InvalidTimeTagWithAllInverse), GroupCheck.Desc, SelfCheck.BasedOnEnd, new double[] { 1000, 1000, 1000, 1000 })]
        public void TestFixInvalid(string testCase, GroupCheck other, SelfCheck self, double[] results)
        {
            var timeTags = getValueByMethodName(testCase);

            // check which part is fixed, using list of time to check result.
            var fixedTimeTag = TimeTagsUtils.FixInvalid(timeTags, other, self);
            Assert.AreEqual(getSortedTime(fixedTimeTag), results);
        }

        [TestCase(nameof(ValidTimeTagWithSorted), new double[] { 1100, 2000, 2100, 3000 })]
        [TestCase(nameof(ValidTimeTagWithUnsorted), new double[] { 1100, 2000, 2100, 3000 })]
        [TestCase(nameof(ValidTimeTagWithUnsortedAndDuplicatedWithNoValue), new double[] { 1100, 2000 })]
        [TestCase(nameof(ValidTimeTagWithUnsortedAndDuplicatedWithValue), new double[] { 1000, 2000 })]
        [TestCase(nameof(ValidTimeTagWithUnsortedAndAllEmpty), new double[] { })]
        [TestCase(nameof(InvalidTimeTagWithStartLargerThenEnd), new double[] { 2000, 2000 })]
        [TestCase(nameof(InvalidTimeTagWithEndLargerThenNextStart), new double[] { 1100, 2100, 2100, 3000 })]
        [TestCase(nameof(InvalidTimeTagWithEndLargerThenNextEnd), new double[] { 1000, 5000, 5000, 5000 })]
        [TestCase(nameof(InvalidTimeTagWithStartSmallerThenPreviousStart), new double[] { 1000, 2000, 2000, 3000 })]
        //[TestCase(nameof(InvalidTimeTagWithAllInverse), new double[] { 4000, 4000, 4000, 4000 })]
        public void TestToDictionary(string testCase, double[] results)
        {
            var timeTags = getValueByMethodName(testCase);

            // todo : using list of time to check result.
            var dictionary = TimeTagsUtils.ToDictionary(timeTags);
            Assert.AreEqual(getSortedTime(dictionary), results);
        }

        private double[] getSortedTime(TimeTag[] timeTags)
            => timeTags.Where(x => x.Time != null).Select(x => x.Time ?? 0)
                       .OrderBy(x => x).ToArray();

        private double[] getSortedTime(IReadOnlyDictionary<TimeTagIndex, double> dictionary)
            => dictionary.Select(x => x.Value).ToArray();

        private TimeTag[] getValueByMethodName(string methodName)
        {
            Type thisType = GetType();
            var theMethod = thisType.GetMethod(methodName);
            if (theMethod == null)
                throw new MissingMethodException("Test method is not exist.");

            return theMethod.Invoke(this, null) as TimeTag[];
        }

        #region valid source

        public static TimeTag[] ValidTimeTagWithSorted()
            => new[]
            {
                new TimeTag(new TimeTagIndex(0), 1100),
                new TimeTag(new TimeTagIndex(0, TimeTagIndex.IndexState.End), 2000),
                new TimeTag(new TimeTagIndex(1), 2100),
                new TimeTag(new TimeTagIndex(1, TimeTagIndex.IndexState.End), 3000),
            };

        public static TimeTag[] ValidTimeTagWithUnsorted()
            => ValidTimeTagWithSorted().Reverse().ToArray();

        public static TimeTag[] ValidTimeTagWithUnsortedAndDuplicatedWithNoValue()
            => new[]
            {
                new TimeTag(new TimeTagIndex(0)),
                new TimeTag(new TimeTagIndex(0)),
                new TimeTag(new TimeTagIndex(0, TimeTagIndex.IndexState.End), 2000), // this time tag is not in order.
                new TimeTag(new TimeTagIndex(0), 1100),
            };

        public static TimeTag[] ValidTimeTagWithUnsortedAndDuplicatedWithValue()
            => new[]
            {
                // not sorted + duplicated time tag(with value)
                new TimeTag(new TimeTagIndex(0), 1000),
                new TimeTag(new TimeTagIndex(0), 1100),
                new TimeTag(new TimeTagIndex(0, TimeTagIndex.IndexState.End), 2000), // this time tag is not in order.
                new TimeTag(new TimeTagIndex(0), 1100),
            };

        public static TimeTag[] ValidTimeTagWithUnsortedAndAllEmpty()
            => new[]
            {
                new TimeTag(new TimeTagIndex(0)),
                new TimeTag(new TimeTagIndex(0, TimeTagIndex.IndexState.End)),
                new TimeTag(new TimeTagIndex(0)), // this time tag is not sorted.
                new TimeTag(new TimeTagIndex(1)),
                new TimeTag(new TimeTagIndex(1, TimeTagIndex.IndexState.End)),
            };

        #endregion

        #region invalid source

        public static TimeTag[] InvalidTimeTagWithStartLargerThenEnd()
            => new[]
            {
                new TimeTag(new TimeTagIndex(0), 2000), // Start is larger then end.
                new TimeTag(new TimeTagIndex(0, TimeTagIndex.IndexState.End), 1000),
            };

        public static TimeTag[] InvalidTimeTagWithEndLargerThenNextStart()
            => new[]
            {
                new TimeTag(new TimeTagIndex(0), 1100),
                new TimeTag(new TimeTagIndex(0, TimeTagIndex.IndexState.End), 2100), // End is larger than second start.
                new TimeTag(new TimeTagIndex(1), 2000),
                new TimeTag(new TimeTagIndex(1, TimeTagIndex.IndexState.End), 3000),
            };

        public static TimeTag[] InvalidTimeTagWithEndLargerThenNextEnd()
            => new[]
            {
                new TimeTag(new TimeTagIndex(0), 1000),
                new TimeTag(new TimeTagIndex(0, TimeTagIndex.IndexState.End), 5000), // End is larger than second end.
                new TimeTag(new TimeTagIndex(1), 2000),
                new TimeTag(new TimeTagIndex(1, TimeTagIndex.IndexState.End), 3000),
            };

        public static TimeTag[] InvalidTimeTagWithStartSmallerThenPreviousStart()
            => new[]
            {
                new TimeTag(new TimeTagIndex(0), 1000),
                new TimeTag(new TimeTagIndex(0, TimeTagIndex.IndexState.End), 2000),
                new TimeTag(new TimeTagIndex(1), 0), // Start is smaller than previous start.
                new TimeTag(new TimeTagIndex(1, TimeTagIndex.IndexState.End), 3000),
            };

        public static TimeTag[] InvalidTimeTagWithAllInverse()
            => new[]
            {
                new TimeTag(new TimeTagIndex(0), 4000),
                new TimeTag(new TimeTagIndex(0, TimeTagIndex.IndexState.End), 3000),
                new TimeTag(new TimeTagIndex(1), 2000),
                new TimeTag(new TimeTagIndex(1, TimeTagIndex.IndexState.End), 1000),
            };

        #endregion
    }
}
