// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using NUnit.Framework;
using osu.Framework.Graphics.Sprites;
using System;
using System.Collections.Generic;

namespace osu.Game.Rulesets.Karaoke.Tests.Utils
{
    [TestFixture]
    public class TimeTagsUtilsTest
    {
        [TestCaseSource("ValidTimeTagWithSorted")]
        [TestCaseSource("ValidTimeTagWithUnsorted")]
        public void TestSort(IReadOnlyList<Tuple<TimeTagIndex, double>> timetags)
        {
            // run all then using time(nullable double) to check.
        }

        public void TestFindInvalid()
        {
            // run all and find error amount and index.
        }

        public void TestFixInvalid()
        {
            // run all valid and check do not fixing

            // run all invalid then check which part is fixed, using list of time to check result.
        }

        public void TestToDictionary()
        {
            // try all empty dictionary included.
        }

        #region valid source

        public static IReadOnlyList<Tuple<TimeTagIndex, double>> ValidTimeTagWithSorted()
            => new List<Tuple<TimeTagIndex, double>>
            {
                // todo : sorted list
            };

        public static IReadOnlyList<Tuple<TimeTagIndex, double>> ValidTimeTagWithUnsorted()
             => new List<Tuple<TimeTagIndex, double>>
             {
                 // todo : just not sorted.
             };

        public static IReadOnlyList<Tuple<TimeTagIndex, double>> ValidTimeTagWithUnsortedAndDuplicatedWithNoValue()
             => new List<Tuple<TimeTagIndex, double>>
             {
                 // not sorted + duliicated time tag(with no value)
             };

        public static IReadOnlyList<Tuple<TimeTagIndex, double>> ValidTimeTagWithUnsortedAndDuplicatedWithValue()
             => new List<Tuple<TimeTagIndex, double>>
             {
                 // not sorted + duliicated time tag(with value)
             };

        public static IReadOnlyList<Tuple<TimeTagIndex, double>> ValidTimeTagWithUnsortedAndAllEmpty()
             => new List<Tuple<TimeTagIndex, double>>
             {
                 // all empty
             };

        #endregion

        #region invalid source

        public static IReadOnlyList<Tuple<TimeTagIndex, double>> InvalidTimeTagWithOneStartLargerThenAllEnd()
             => new List<Tuple<TimeTagIndex, double>>
             {
                 // 1. one start larger then all end.
             };

        public static IReadOnlyList<Tuple<TimeTagIndex, double>> InvalidTimeTagWithMultiStartLargerThenAllEnd()
             => new List<Tuple<TimeTagIndex, double>>
             {
                 // 2. multi start larger then all end.
             };

        public static IReadOnlyList<Tuple<TimeTagIndex, double>> InvalidTimeTagWithOneEndLargerThenAllStart()
             => new List<Tuple<TimeTagIndex, double>>
             {
                 // 3. one end larger then all start.
             };

        public static IReadOnlyList<Tuple<TimeTagIndex, double>> InvalidTimeTagWithMultiEndLargerThenAllStart()
             => new List<Tuple<TimeTagIndex, double>>
             {
                 // 4. multi start larger then all end.
             };

        public static IReadOnlyList<Tuple<TimeTagIndex, double>> InvalidTimeTagWithSomeStartLargerThenSomeEnd()
             => new List<Tuple<TimeTagIndex, double>>
             {
                 // 5. some start larger then some end.
             };

        public static IReadOnlyList<Tuple<TimeTagIndex, double>> InvalidTimeTagWithSomeEndLargerThenSomeStart()
             => new List<Tuple<TimeTagIndex, double>>
             {
                 // 6. some end larger then some start.
             };

        #endregion
    }
}
