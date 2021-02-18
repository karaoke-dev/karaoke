// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using NUnit.Framework;
using osu.Framework.Graphics.Sprites;
using osu.Game.Rulesets.Karaoke.Utils;

namespace osu.Game.Rulesets.Karaoke.Tests.Utils
{
    [TestFixture]
    public class TextIndexUtilsTest
    {
        [TestCase(0, TextIndex.IndexState.Start, 0)]
        [TestCase(0, TextIndex.IndexState.End, 1)]
        [TestCase(-1, TextIndex.IndexState.Start, -1)] // In utils not checking is index out of range
        [TestCase(-1, TextIndex.IndexState.End, 0)]
        public void TestToStringIndex(int index, TextIndex.IndexState state, int actualIndex)
        {
            var textIndex = new TextIndex(index, state);
            Assert.AreEqual(TextIndexUtils.ToStringIndex(textIndex), actualIndex);
        }

        [TestCase(TextIndex.IndexState.Start, TextIndex.IndexState.End)]
        [TestCase(TextIndex.IndexState.End, TextIndex.IndexState.Start)]
        public void TestReverseState(TextIndex.IndexState state, TextIndex.IndexState actualState)
        {
            Assert.AreEqual(TextIndexUtils.ReverseState(state), actualState);
        }

        [TestCase(0, TextIndex.IndexState.Start, 1, 1, TextIndex.IndexState.Start)]
        [TestCase(0, TextIndex.IndexState.End, 1, 1, TextIndex.IndexState.End)]
        [TestCase(0, TextIndex.IndexState.Start, -1, -1, TextIndex.IndexState.Start)]
        [TestCase(0, TextIndex.IndexState.End, -1, -1, TextIndex.IndexState.End)]
        public void TestShiftingIndex(int index, TextIndex.IndexState state, int shifting, int actualIndex, TextIndex.IndexState actualState)
        {
            var textIndex = new TextIndex(index, state);
            var actualTextIndex = new TextIndex(actualIndex, actualState);
            Assert.AreEqual(TextIndexUtils.ShiftingIndex(textIndex, shifting), actualTextIndex);
        }

        [TestCase(0, TextIndex.IndexState.Start, "0")]
        [TestCase(0, TextIndex.IndexState.End, "0(end)")]
        [TestCase(-1, TextIndex.IndexState.Start, "-1")]
        [TestCase(-1, TextIndex.IndexState.End, "-1(end)")]
        public void TestPositionFormattedString(int index, TextIndex.IndexState state, string actual)
        {
            var textIndex = new TextIndex(index, state);
            Assert.AreEqual(TextIndexUtils.PositionFormattedString(textIndex), actual);
        }
    }
}
