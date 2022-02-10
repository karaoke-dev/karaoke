// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using NUnit.Framework;
using osu.Game.Rulesets.Karaoke.Utils;

namespace osu.Game.Rulesets.Karaoke.Tests.Utils
{
    [TestFixture]
    public class SingerUtilsTest
    {
        [TestCase(null, 0)]
        [TestCase(new[] { 1 }, 1)]
        [TestCase(new[] { 1, 2, 3 }, 7)]
        [TestCase(new[] { 1, 4, 5 }, 25)]
        public void TestGetShiftingStyleIndex(int[] singerIndexes, int expected)
        {
            int actual = SingerUtils.GetShiftingStyleIndex(singerIndexes);
            Assert.AreEqual(expected, actual);
        }

        [TestCase(-1, new int[] { })]
        [TestCase(0, new int[] { })]
        [TestCase(1, new[] { 1 })]
        [TestCase(7, new[] { 1, 2, 3 })]
        [TestCase(25, new[] { 1, 4, 5 })]
        public void TestGetSingersIndex(int styleIndex, int[] expected)
        {
            int[] actual = SingerUtils.GetSingersIndex(styleIndex);
            Assert.AreEqual(expected, actual);
        }
    }
}
