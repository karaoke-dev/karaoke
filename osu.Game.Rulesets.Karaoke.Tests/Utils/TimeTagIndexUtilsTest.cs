// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using NUnit.Framework;
using osu.Framework.Graphics.Sprites;
using osu.Game.Rulesets.Karaoke.Utils;

namespace osu.Game.Rulesets.Karaoke.Tests.Utils
{
    [TestFixture]
    public class TimeTagIndexUtilsTest
    {
        [TestCase(0, TimeTagIndex.IndexState.Start, 0)]
        [TestCase(0, TimeTagIndex.IndexState.End, 1)]
        [TestCase(-1, TimeTagIndex.IndexState.Start, -1)] // In utils not checking is index out of range
        [TestCase(-1, TimeTagIndex.IndexState.End, 0)]
        public void TestToLyricIndex(int index, TimeTagIndex.IndexState state, int actualIndex)
        {
            var timeTagIndex = new TimeTagIndex(index, state);
            Assert.AreEqual(TimeTagIndexUtils.ToLyricIndex(timeTagIndex), actualIndex);
        }

        [TestCase(TimeTagIndex.IndexState.Start, TimeTagIndex.IndexState.End)]
        [TestCase(TimeTagIndex.IndexState.End, TimeTagIndex.IndexState.Start)]
        public void TestReverseState(TimeTagIndex.IndexState state, TimeTagIndex.IndexState actualState)
        {
            Assert.AreEqual(TimeTagIndexUtils.ReverseState(state), actualState);
        }

        [TestCase(0, TimeTagIndex.IndexState.Start, 1, 1, TimeTagIndex.IndexState.Start)]
        [TestCase(0, TimeTagIndex.IndexState.End, 1, 1, TimeTagIndex.IndexState.End)]
        [TestCase(0, TimeTagIndex.IndexState.Start, -1, -1, TimeTagIndex.IndexState.Start)]
        [TestCase(0, TimeTagIndex.IndexState.End, -1, -1, TimeTagIndex.IndexState.End)]
        public void TestShiftingTimeTagIndex(int index, TimeTagIndex.IndexState state, int shifting, int actualIndex, TimeTagIndex.IndexState actualState)
        {
            var timeTagIndex = new TimeTagIndex(index, state);
            var actualTimeTagIndex = new TimeTagIndex(actualIndex, actualState);
            Assert.AreEqual(TimeTagIndexUtils.ShiftingTimeTagIndex(timeTagIndex, shifting), actualTimeTagIndex);
        }
    }
}
