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
        public void TestToStringIndex(int index, TextIndex.IndexState state, int expected)
        {
            var textIndex = new TextIndex(index, state);

            int actual = TextIndexUtils.ToStringIndex(textIndex);
            Assert.AreEqual(expected, actual);
        }

        [TestCase(0, false, 0, TextIndex.IndexState.Start)]
        [TestCase(1, true, 0, TextIndex.IndexState.End)]
        [TestCase(0, true, -1, TextIndex.IndexState.End)] // In utils not checking is index out of range
        public void TestFromStringIndex(int textIndex, bool end, int expectedIndex, TextIndex.IndexState expectedState)
        {
            var expected = new TextIndex(expectedIndex, expectedState);
            var actual = TextIndexUtils.FromStringIndex(textIndex, end);
            Assert.AreEqual(expected, actual);
        }

        [TestCase(TextIndex.IndexState.Start, TextIndex.IndexState.End)]
        [TestCase(TextIndex.IndexState.End, TextIndex.IndexState.Start)]
        public void TestReverseState(TextIndex.IndexState state, TextIndex.IndexState expected)
        {
            var actual = TextIndexUtils.ReverseState(state);
            Assert.AreEqual(expected, actual);
        }

        [TestCase(1, TextIndex.IndexState.End, 1, TextIndex.IndexState.Start)]
        [TestCase(1, TextIndex.IndexState.Start, 0, TextIndex.IndexState.End)]
        [TestCase(0, TextIndex.IndexState.Start, -1, TextIndex.IndexState.End)] // didn't care about negative value.
        [TestCase(-1, TextIndex.IndexState.End, -1, TextIndex.IndexState.Start)] // didn't care about negative value.
        public void TestGetPreviousIndex(int index, TextIndex.IndexState state, int expectedIndex, TextIndex.IndexState expectedState)
        {
            var textIndex = new TextIndex(index, state);

            var expected = new TextIndex(expectedIndex, expectedState);
            var actual = TextIndexUtils.GetPreviousIndex(textIndex);
            Assert.AreEqual(expected, actual);
        }

        [TestCase(0, TextIndex.IndexState.Start, 0, TextIndex.IndexState.End)]
        [TestCase(0, TextIndex.IndexState.End, 1, TextIndex.IndexState.Start)]
        [TestCase(-1, TextIndex.IndexState.Start, -1, TextIndex.IndexState.End)] // didn't care about negative value.
        [TestCase(-1, TextIndex.IndexState.End, 0, TextIndex.IndexState.Start)] // didn't care about negative value.
        public void TestGetNextIndex(int index, TextIndex.IndexState state, int expectedIndex, TextIndex.IndexState expectedState)
        {
            var textIndex = new TextIndex(index, state);

            var expected = new TextIndex(expectedIndex, expectedState);
            var actual = TextIndexUtils.GetNextIndex(textIndex);
            Assert.AreEqual(expected, actual);
        }

        [TestCase(0, TextIndex.IndexState.Start, 1, 1, TextIndex.IndexState.Start)]
        [TestCase(0, TextIndex.IndexState.End, 1, 1, TextIndex.IndexState.End)]
        [TestCase(0, TextIndex.IndexState.Start, -1, -1, TextIndex.IndexState.Start)]
        [TestCase(0, TextIndex.IndexState.End, -1, -1, TextIndex.IndexState.End)]
        public void TestShiftingIndex(int index, TextIndex.IndexState state, int offset, int expectedIndex, TextIndex.IndexState expectedState)
        {
            var textIndex = new TextIndex(index, state);

            var expected = new TextIndex(expectedIndex, expectedState);
            var actual = TextIndexUtils.ShiftingIndex(textIndex, offset);
            Assert.AreEqual(expected, actual);
        }

        [TestCase(0, TextIndex.IndexState.Start, "karaoke", false)]
        [TestCase(0, TextIndex.IndexState.End, "karaoke", false)]
        [TestCase(-1, TextIndex.IndexState.Start, "karaoke", true)]
        [TestCase(-1, TextIndex.IndexState.End, "karaoke", true)]
        [TestCase(0, TextIndex.IndexState.Start, "", true)] // should be counted as out of range if lyric is empty
        [TestCase(0, TextIndex.IndexState.End, "", true)]
        [TestCase(0, TextIndex.IndexState.Start, null, true)] // should be counted as out of range if lyric is null
        [TestCase(0, TextIndex.IndexState.End, null, true)]
        public void TestOutOfRange(int index, TextIndex.IndexState state, string lyric, bool expected)
        {
            var textIndex = new TextIndex(index, state);

            bool actual = TextIndexUtils.OutOfRange(textIndex, lyric);
            Assert.AreEqual(expected, actual);
        }

        [TestCase(TextIndex.IndexState.Start, 1, -1, 1)]
        [TestCase(TextIndex.IndexState.End, 1, -1, -1)]
        [TestCase(TextIndex.IndexState.Start, "1", "-1", "1")]
        [TestCase(TextIndex.IndexState.End, "1", "-1", "-1")]
        public void TestGetValueByState(TextIndex.IndexState state, object startValue, object endValue, object expected)
        {
            var textIndex = new TextIndex(0, state);

            object actual = TextIndexUtils.GetValueByState(textIndex, startValue, endValue);
            Assert.AreEqual(expected, actual);
        }

        [TestCase(0, TextIndex.IndexState.Start, "0")]
        [TestCase(0, TextIndex.IndexState.End, "0(end)")]
        [TestCase(-1, TextIndex.IndexState.Start, "-1")]
        [TestCase(-1, TextIndex.IndexState.End, "-1(end)")]
        public void TestPositionFormattedString(int index, TextIndex.IndexState state, string expected)
        {
            var textIndex = new TextIndex(index, state);

            string actual = TextIndexUtils.PositionFormattedString(textIndex);
            Assert.AreEqual(expected, actual);
        }
    }
}
