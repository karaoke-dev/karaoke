// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using Microsoft.EntityFrameworkCore.Internal;
using NUnit.Framework;
using osu.Framework.Graphics.Sprites;
using osu.Game.Rulesets.Karaoke.Utils;
using System;
using System.Collections.Generic;
using System.Linq;

namespace osu.Game.Rulesets.Karaoke.Tests.Utils
{
    [TestFixture]
    public class TimeTagsUtilsTest
    {
        [TestCase(nameof(ValidTimeTagWithSorted), new double[] { 1100, 2000, 2100, 3000 })]
        [TestCase(nameof(ValidTimeTagWithUnsorted), new double[] { 1100, 2000, 2100, 3000 })]
        [TestCase(nameof(ValidTimeTagWithUnsortedAndDuplicatedWithNoValue), new double[] { 1100, 2000 })]
        [TestCase(nameof(ValidTimeTagWithUnsortedAndDuplicatedWithValue), new double[] { 1000, 1100, 1100, 2000 })]
        [TestCase(nameof(ValidTimeTagWithUnsortedAndAllEmpty), new double[] { })]
        public void TestSort(string testCase, double[] results)
        {
            var timeTags = getvalueByMethodName(testCase);

            // run all then using time(nullable double) to check.
            var sortedTimeTag = TimeTagsUtils.Sort(timeTags);
            Assert.AreEqual(getSortedTime(sortedTimeTag), results);
        }

        [TestCase(nameof(InvalidTimeTagWithStartLargerThenEnd), GroupCheck.Asc, SelfCheck.BasedOnStart, new int[] { 1 })]
        [TestCase(nameof(InvalidTimeTagWithStartLargerThenEnd), GroupCheck.Asc, SelfCheck.BasedOnEnd, new int[] { 0 })]
        [TestCase(nameof(InvalidTimeTagWithEndLargerThenNextStart), GroupCheck.Asc, SelfCheck.BasedOnStart, new int[] { 2 })]
        [TestCase(nameof(InvalidTimeTagWithEndLargerThenNextStart), GroupCheck.Desc, SelfCheck.BasedOnStart, new int[] { 1 })]
        [TestCase(nameof(InvalidTimeTagWithEndLargerThenNextEnd), GroupCheck.Asc, SelfCheck.BasedOnStart, new int[] { 2, 3 })]
        [TestCase(nameof(InvalidTimeTagWithEndLargerThenNextEnd), GroupCheck.Desc, SelfCheck.BasedOnStart, new int[] { 1 })]
        [TestCase(nameof(InvalidTimeTagWithStartSmallerThenPerviousStart), GroupCheck.Asc, SelfCheck.BasedOnStart, new int[] { 2 })]
        [TestCase(nameof(InvalidTimeTagWithStartSmallerThenPerviousStart), GroupCheck.Desc, SelfCheck.BasedOnStart, new int[] { 0, 1 })]
        [TestCase(nameof(InvalidTimeTagWithAllInverse), GroupCheck.Asc, SelfCheck.BasedOnStart, new int[] { 1, 2, 3 })]
        [TestCase(nameof(InvalidTimeTagWithAllInverse), GroupCheck.Asc, SelfCheck.BasedOnEnd, new int[] { 0, 2, 3 })]
        [TestCase(nameof(InvalidTimeTagWithAllInverse), GroupCheck.Desc, SelfCheck.BasedOnStart, new int[] { 0, 1, 3 })]
        [TestCase(nameof(InvalidTimeTagWithAllInverse), GroupCheck.Desc, SelfCheck.BasedOnEnd, new int[] { 0, 1, 2 })]
        public void TestFindInvalid(string testCase, GroupCheck other, SelfCheck self, int[] errorIndex)
        {
            var timeTags = getvalueByMethodName(testCase);

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
        [TestCase(nameof(InvalidTimeTagWithStartSmallerThenPerviousStart), GroupCheck.Asc, SelfCheck.BasedOnStart, new double[] { 1000, 2000, 2000, 3000 })]
        [TestCase(nameof(InvalidTimeTagWithStartSmallerThenPerviousStart), GroupCheck.Desc, SelfCheck.BasedOnStart, new double[] { 0, 0, 0, 3000 })]
        //[TestCase(nameof(InvalidTimeTagWithAllInverse), GroupCheck.Asc, SelfCheck.BasedOnStart, new double[] { 4000, 4000, 4000, 4000 })]
        //[TestCase(nameof(InvalidTimeTagWithAllInverse), GroupCheck.Asc, SelfCheck.BasedOnEnd, new double[] { 3000, 3000, 3000, 3000 })]
        //[TestCase(nameof(InvalidTimeTagWithAllInverse), GroupCheck.Desc, SelfCheck.BasedOnStart, new double[] { 2000, 2000, 2000, 2000 })]
        //[TestCase(nameof(InvalidTimeTagWithAllInverse), GroupCheck.Desc, SelfCheck.BasedOnEnd, new double[] { 1000, 1000, 1000, 1000 })]
        public void TestFixInvalid(string testCase, GroupCheck other, SelfCheck self, double[] results)
        {
            var timeTags = getvalueByMethodName(testCase);

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
        [TestCase(nameof(InvalidTimeTagWithStartSmallerThenPerviousStart), new double[] { 1000, 2000, 2000, 3000 })]
        //[TestCase(nameof(InvalidTimeTagWithAllInverse), new double[] { 4000, 4000, 4000, 4000 })]
        public void TestToDictionary(string testCase, double[] results)
        {
            var timeTags = getvalueByMethodName(testCase);

            // todo : using list of time to check result.
            var dictionary = TimeTagsUtils.ToDictionary(timeTags);
            Assert.AreEqual(getSortedTime(dictionary), results);
        }

        private double[] getSortedTime(Tuple<TimeTagIndex, double?>[] timeTags)
            => timeTags.Where(x => x.Item2 != null).Select(x => x.Item2 ?? 0)
                       .OrderBy(x => x).ToArray();

        private double[] getSortedTime(IReadOnlyDictionary<TimeTagIndex, double> dictionary)
            => dictionary.Select(x => x.Value).ToArray();

        private Tuple<TimeTagIndex, double?>[] getvalueByMethodName(string methodName)
        {
            Type thisType = GetType();
            var theMethod = thisType.GetMethod(methodName);
            if (theMethod == null)
                throw new MissingMethodException("Test method is not exist.");

            return theMethod.Invoke(this, null) as Tuple<TimeTagIndex, double?>[];
        }

        #region valid source

        public static Tuple<TimeTagIndex, double?>[] ValidTimeTagWithSorted()
            => new[]
            {
                TimeTagsUtils.Create(new TimeTagIndex(0, TimeTagIndex.IndexState.Start), 1100),
                TimeTagsUtils.Create(new TimeTagIndex(0, TimeTagIndex.IndexState.End), 2000),
                TimeTagsUtils.Create(new TimeTagIndex(1, TimeTagIndex.IndexState.Start), 2100),
                TimeTagsUtils.Create(new TimeTagIndex(1, TimeTagIndex.IndexState.End), 3000),
            };

        public static Tuple<TimeTagIndex, double?>[] ValidTimeTagWithUnsorted()
            => ValidTimeTagWithSorted().Reverse().ToArray();

        public static Tuple<TimeTagIndex, double?>[] ValidTimeTagWithUnsortedAndDuplicatedWithNoValue()
            => new Tuple<TimeTagIndex, double?>[]
            {
                TimeTagsUtils.Create(new TimeTagIndex(0, TimeTagIndex.IndexState.Start), null),
                TimeTagsUtils.Create(new TimeTagIndex(0, TimeTagIndex.IndexState.Start), null),
                TimeTagsUtils.Create(new TimeTagIndex(0, TimeTagIndex.IndexState.End), 2000), // this time tag is not in order.
                TimeTagsUtils.Create(new TimeTagIndex(0, TimeTagIndex.IndexState.Start), 1100),
            };

        public static Tuple<TimeTagIndex, double?>[] ValidTimeTagWithUnsortedAndDuplicatedWithValue()
            => new Tuple<TimeTagIndex, double?>[]
            {
                // not sorted + duliicated time tag(with value)
                TimeTagsUtils.Create(new TimeTagIndex(0, TimeTagIndex.IndexState.Start), 1000),
                TimeTagsUtils.Create(new TimeTagIndex(0, TimeTagIndex.IndexState.Start), 1100),
                TimeTagsUtils.Create(new TimeTagIndex(0, TimeTagIndex.IndexState.End), 2000), // this time tag is not in order.
                TimeTagsUtils.Create(new TimeTagIndex(0, TimeTagIndex.IndexState.Start), 1100),
            };

        public static Tuple<TimeTagIndex, double?>[] ValidTimeTagWithUnsortedAndAllEmpty()
            => new Tuple<TimeTagIndex, double?>[]
            {
                TimeTagsUtils.Create(new TimeTagIndex(0, TimeTagIndex.IndexState.Start), null),
                TimeTagsUtils.Create(new TimeTagIndex(0, TimeTagIndex.IndexState.End), null),
                TimeTagsUtils.Create(new TimeTagIndex(0, TimeTagIndex.IndexState.Start), null), // this time tag is not sorted.
                TimeTagsUtils.Create(new TimeTagIndex(1, TimeTagIndex.IndexState.Start), null),
                TimeTagsUtils.Create(new TimeTagIndex(1, TimeTagIndex.IndexState.End), null),
            };

        #endregion

        #region invalid source

        public static Tuple<TimeTagIndex, double?>[] InvalidTimeTagWithStartLargerThenEnd()
            => new[]
            {
                TimeTagsUtils.Create(new TimeTagIndex(0, TimeTagIndex.IndexState.Start), 2000), // Start is larger then end.
                TimeTagsUtils.Create(new TimeTagIndex(0, TimeTagIndex.IndexState.End), 1000),
            };

        public static Tuple<TimeTagIndex, double?>[] InvalidTimeTagWithEndLargerThenNextStart()
            => new[]
            {
                TimeTagsUtils.Create(new TimeTagIndex(0, TimeTagIndex.IndexState.Start), 1100),
                TimeTagsUtils.Create(new TimeTagIndex(0, TimeTagIndex.IndexState.End), 2100), // End is larger than second start.
                TimeTagsUtils.Create(new TimeTagIndex(1, TimeTagIndex.IndexState.Start), 2000),
                TimeTagsUtils.Create(new TimeTagIndex(1, TimeTagIndex.IndexState.End), 3000),
            };

        public static Tuple<TimeTagIndex, double?>[] InvalidTimeTagWithEndLargerThenNextEnd()
            => new[]
            {
                TimeTagsUtils.Create(new TimeTagIndex(0, TimeTagIndex.IndexState.Start), 1000),
                TimeTagsUtils.Create(new TimeTagIndex(0, TimeTagIndex.IndexState.End), 5000), // End is larger than second end.
                TimeTagsUtils.Create(new TimeTagIndex(1, TimeTagIndex.IndexState.Start), 2000),
                TimeTagsUtils.Create(new TimeTagIndex(1, TimeTagIndex.IndexState.End), 3000),
            };

        public static Tuple<TimeTagIndex, double?>[] InvalidTimeTagWithStartSmallerThenPerviousStart()
            => new[]
            {
                TimeTagsUtils.Create(new TimeTagIndex(0, TimeTagIndex.IndexState.Start), 1000),
                TimeTagsUtils.Create(new TimeTagIndex(0, TimeTagIndex.IndexState.End), 2000),
                TimeTagsUtils.Create(new TimeTagIndex(1, TimeTagIndex.IndexState.Start), 0),// Start is smaller than pervious start.
                TimeTagsUtils.Create(new TimeTagIndex(1, TimeTagIndex.IndexState.End), 3000),
            };

        public static Tuple<TimeTagIndex, double?>[] InvalidTimeTagWithAllInverse()
            => new[]
            {
                TimeTagsUtils.Create(new TimeTagIndex(0, TimeTagIndex.IndexState.Start), 4000),
                TimeTagsUtils.Create(new TimeTagIndex(0, TimeTagIndex.IndexState.End), 3000),
                TimeTagsUtils.Create(new TimeTagIndex(1, TimeTagIndex.IndexState.Start), 2000),
                TimeTagsUtils.Create(new TimeTagIndex(1, TimeTagIndex.IndexState.End), 1000),
            };

        #endregion
    }
}
