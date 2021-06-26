// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using NUnit.Framework;
using osu.Game.Rulesets.Karaoke.Extensions;

namespace osu.Game.Rulesets.Karaoke.Tests.Extensions
{
    public class EnumerableExtensionsTest
    {
        [TestCase(new[] { 1, 2, 3, 4, 5, 6 }, 1, 3, 3)]
        [TestCase(new[] { 1, 3, 2, 4, 6, 5 }, 1, 6, 6)]
        [TestCase(new[] { 1, 2, 3, 4, 5, 6 }, 3, 1, 0)]
        [TestCase(new[] { 1, 2, 3, 4, 5, 6 }, 1, 7, 0)]
        public void TestGetNextMatch(int[] values, int startFrom, int matchCondition, int actual)
        {
            Assert.AreEqual(values.GetNextMatch(startFrom, x => x == matchCondition), actual);
        }

        [TestCase(new[] { 1, 2, 3, 4, 5, 6 }, 6, 3, 3)]
        [TestCase(new[] { 1, 3, 2, 4, 6, 5 }, 5, 3, 3)]
        [TestCase(new[] { 1, 2, 3, 4, 5, 6 }, 3, 6, 0)]
        [TestCase(new[] { 2, 3, 4, 5, 6 }, 6, 1, 0)]
        public void TestGetPreviousMatch(int[] values, int startFrom, int matchCondition, int actual)
        {
            Assert.AreEqual(values.GetPreviousMatch(startFrom, x => x == matchCondition), actual);
        }
    }
}
