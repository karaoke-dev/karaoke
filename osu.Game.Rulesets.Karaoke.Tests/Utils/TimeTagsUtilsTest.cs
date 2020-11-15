// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

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

        [TestCase(nameof(InvalidTimeTagWithOneStartLargerThenAllEnd), new double[] { })]
        [TestCase(nameof(InvalidTimeTagWithMultiStartLargerThenAllEnd), new double[] { })]
        [TestCase(nameof(InvalidTimeTagWithOneEndLargerThenAllStart), new double[] { })]
        [TestCase(nameof(InvalidTimeTagWithMultiEndLargerThenAllStart), new double[] { })]
        [TestCase(nameof(InvalidTimeTagWithSomeStartLargerThenSomeEnd), new double[] { })]
        [TestCase(nameof(InvalidTimeTagWithSomeEndLargerThenSomeStart), new double[] { })]
        public void TestFindInvalid(string testCase, double[] results)
        {
            // todo : run all and find error amount and index.
        }

        public void TestFixInvalid()
        {
            // todo : run all valid and check do not fixing

            // todo : run all invalid then check which part is fixed, using list of time to check result.
        }

        [TestCase(nameof(InvalidTimeTagWithOneStartLargerThenAllEnd), new double[] { })]
        [TestCase(nameof(InvalidTimeTagWithMultiStartLargerThenAllEnd), new double[] { })]
        [TestCase(nameof(InvalidTimeTagWithOneEndLargerThenAllStart), new double[] { })]
        [TestCase(nameof(InvalidTimeTagWithMultiEndLargerThenAllStart), new double[] { })]
        [TestCase(nameof(InvalidTimeTagWithSomeStartLargerThenSomeEnd), new double[] { })]
        [TestCase(nameof(InvalidTimeTagWithSomeEndLargerThenSomeStart), new double[] { })]
        [TestCase(nameof(ValidTimeTagWithUnsortedAndAllEmpty), new double[] { })]
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

        public static Tuple<TimeTagIndex, double?>[] InvalidTimeTagWithOneStartLargerThenAllEnd()
             => new Tuple<TimeTagIndex, double?>[]
             {
                 // 1. one start larger then all end.
             };

        public static Tuple<TimeTagIndex, double?>[] InvalidTimeTagWithMultiStartLargerThenAllEnd()
             => new Tuple<TimeTagIndex, double?>[]
             {
                 // 2. multi start larger then all end.
             };

        public static Tuple<TimeTagIndex, double?>[] InvalidTimeTagWithOneEndLargerThenAllStart()
             => new Tuple<TimeTagIndex, double?>[]
             {
                 // 3. one end larger then all start.
             };

        public static Tuple<TimeTagIndex, double?>[] InvalidTimeTagWithMultiEndLargerThenAllStart()
             => new Tuple<TimeTagIndex, double?>[]
             {
                 // 4. multi start larger then all end.
             };

        public static Tuple<TimeTagIndex, double?>[] InvalidTimeTagWithSomeStartLargerThenSomeEnd()
             => new Tuple<TimeTagIndex, double?>[]
             {
                 // 5. some start larger then some end.
             };

        public static Tuple<TimeTagIndex, double?>[] InvalidTimeTagWithSomeEndLargerThenSomeStart()
             => new Tuple<TimeTagIndex, double?>[]
             {
                 // 6. some end larger then some start.
             };

        #endregion
    }
}
