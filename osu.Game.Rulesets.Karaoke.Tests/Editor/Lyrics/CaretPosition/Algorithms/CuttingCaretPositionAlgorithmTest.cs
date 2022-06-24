// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using osu.Game.Rulesets.Karaoke.Edit.Lyrics.CaretPosition;
using osu.Game.Rulesets.Karaoke.Edit.Lyrics.CaretPosition.Algorithms;
using osu.Game.Rulesets.Karaoke.Objects;

namespace osu.Game.Rulesets.Karaoke.Tests.Editor.Lyrics.CaretPosition.Algorithms
{
    [TestFixture]
    public class CuttingCaretPositionAlgorithmTest : BaseCaretPositionAlgorithmTest<CuttingCaretPositionAlgorithm, TextCaretPosition>
    {
        [TestCase(nameof(singleLyric), 0, 1, true)]
        [TestCase(nameof(singleLyric), 0, 3, true)]
        [TestCase(nameof(singleLyric), 0, 0, false)]
        [TestCase(nameof(singleLyric), 0, 4, false)]
        [TestCase(nameof(singleLyric), 0, 5, false)]
        [TestCase(nameof(singleLyric), 0, -1, false)]
        [TestCase(nameof(singleLyricWithNoText), 0, 0, true)] // It's not cuttable if no lyric text.
        [TestCase(nameof(singleLyricWithNoText), 0, 1, false)]
        [TestCase(nameof(singleLyricWithNoText), 0, -1, false)]
        public void TestPositionMovable(string sourceName, int lyricIndex, int index, bool movable)
        {
            var lyrics = GetLyricsByMethodName(sourceName);
            var caret = createTextCaretPosition(lyrics, lyricIndex, index);

            // Check is movable
            TestPositionMovable(lyrics, caret, movable);
        }

        [TestCase(nameof(singleLyric), 0, 1, null, null)] // cannot move up if at top index.
        [TestCase(nameof(twoLyricsWithText), 1, 1, 0, 1)]
        [TestCase(nameof(threeLyricsWithSpacing), 2, 1, 0, 1)]
        [TestCase(nameof(threeLyricsWithSpacing), 2, 2, 0, 2)]
        public void TestMoveUp(string sourceName, int lyricIndex, int index, int? expectedLyricIndex, int? expectedIndex)
        {
            var lyrics = GetLyricsByMethodName(sourceName);
            var caret = createTextCaretPosition(lyrics, lyricIndex, index);
            var expected = createExpectedTextCaretPosition(lyrics, expectedLyricIndex, expectedIndex);

            // Check is movable
            TestMoveUp(lyrics, caret, expected);
        }

        [TestCase(nameof(singleLyric), 0, 1, null, null)] // cannot move down if at bottom index.
        [TestCase(nameof(twoLyricsWithText), 0, 1, 1, 1)]
        [TestCase(nameof(twoLyricsWithText), 0, 3, 1, 2)]
        [TestCase(nameof(threeLyricsWithSpacing), 0, 1, 2, 1)]
        [TestCase(nameof(threeLyricsWithSpacing), 0, 3, 2, 2)]
        public void TestMoveDown(string sourceName, int lyricIndex, int index, int? expectedLyricIndex, int? expectedIndex)
        {
            var lyrics = GetLyricsByMethodName(sourceName);
            var caret = createTextCaretPosition(lyrics, lyricIndex, index);
            var expected = createExpectedTextCaretPosition(lyrics, expectedLyricIndex, expectedIndex);

            // Check is movable
            TestMoveDown(lyrics, caret, expected);
        }

        [TestCase(nameof(singleLyric), 0, 1, null, null)]
        [TestCase(nameof(twoLyricsWithText), 1, 1, 0, 3)]
        [TestCase(nameof(threeLyricsWithSpacing), 2, 1, 0, 3)]
        [TestCase(nameof(threeLyricsWithSpacing), 2, 3, 2, 2)]
        public void TestMoveLeft(string sourceName, int lyricIndex, int index, int? expectedLyricIndex, int? expectedIndex)
        {
            var lyrics = GetLyricsByMethodName(sourceName);
            var caret = createTextCaretPosition(lyrics, lyricIndex, index);
            var expected = createExpectedTextCaretPosition(lyrics, expectedLyricIndex, expectedIndex);

            // Check is movable
            TestMoveLeft(lyrics, caret, expected);
        }

        [TestCase(nameof(singleLyric), 0, 3, null, null)]
        [TestCase(nameof(twoLyricsWithText), 0, 3, 1, 1)]
        [TestCase(nameof(threeLyricsWithSpacing), 0, 3, 2, 1)]
        [TestCase(nameof(threeLyricsWithSpacing), 0, 2, 0, 3)]
        public void TestMoveRight(string sourceName, int lyricIndex, int index, int? expectedLyricIndex, int? expectedIndex)
        {
            var lyrics = GetLyricsByMethodName(sourceName);
            var caret = createTextCaretPosition(lyrics, lyricIndex, index);
            var expected = createExpectedTextCaretPosition(lyrics, expectedLyricIndex, expectedIndex);

            // Check is movable
            TestMoveRight(lyrics, caret, expected);
        }

        [TestCase(nameof(singleLyric), 0, 1)]
        [TestCase(nameof(singleLyricWithNoText), null, null)]
        [TestCase(nameof(twoLyricsWithText), 0, 1)]
        [TestCase(nameof(threeLyricsWithSpacing), 0, 1)]
        public void TestMoveToFirst(string sourceName, int? expectedLyricIndex, int? expectedIndex)
        {
            var lyrics = GetLyricsByMethodName(sourceName);
            var expected = createExpectedTextCaretPosition(lyrics, expectedLyricIndex, expectedIndex);

            // Check first position
            TestMoveToFirst(lyrics, expected);
        }

        [TestCase(nameof(singleLyric), 0, 3)]
        [TestCase(nameof(singleLyricWithNoText), null, null)]
        [TestCase(nameof(twoLyricsWithText), 1, 2)]
        [TestCase(nameof(threeLyricsWithSpacing), 2, 2)]
        public void TestMoveToLast(string sourceName, int? expectedLyricIndex, int? expectedIndex)
        {
            var lyrics = GetLyricsByMethodName(sourceName);
            var expected = createExpectedTextCaretPosition(lyrics, expectedLyricIndex, expectedIndex);

            // Check last position
            TestMoveToLast(lyrics, expected);
        }

        [TestCase(nameof(singleLyric), 0)]
        [TestCase(nameof(singleLyricWithNoText), 0)]
        public void TestMoveToTarget(string sourceName, int expectedLyricIndex)
        {
            var lyrics = GetLyricsByMethodName(sourceName);
            var lyric = lyrics[expectedLyricIndex];
            var expected = createExpectedTextCaretPosition(lyrics, expectedLyricIndex, 1);

            // Check move to target position.
            TestMoveToTarget(lyrics, lyric, expected);
        }

        protected override void AssertEqual(TextCaretPosition expected, TextCaretPosition actual)
        {
            Assert.AreEqual(expected.Lyric, actual.Lyric);
            Assert.AreEqual(expected.Index, actual.Index);
        }

        private static TextCaretPosition createTextCaretPosition(IEnumerable<Lyric> lyrics, int lyricIndex, int index)
        {
            var lyric = lyrics.ElementAtOrDefault(lyricIndex);
            if (lyric == null)
                throw new ArgumentNullException();

            return new TextCaretPosition(lyric, index);
        }

        private static TextCaretPosition? createExpectedTextCaretPosition(IEnumerable<Lyric> lyrics, int? lyricIndex, int? index)
        {
            if (lyricIndex == null || index == null)
                return null;

            return createTextCaretPosition(lyrics, lyricIndex.Value, index.Value);
        }

        #region source

        private static Lyric[] singleLyric => new[]
        {
            new Lyric
            {
                Text = "カラオケ"
            }
        };

        private static Lyric[] singleLyricWithNoText => new[]
        {
            new Lyric()
        };

        private static Lyric[] twoLyricsWithText => new[]
        {
            new Lyric
            {
                Text = "カラオケ"
            },
            new Lyric
            {
                Text = "大好き"
            }
        };

        private static Lyric[] threeLyricsWithSpacing => new[]
        {
            new Lyric
            {
                Text = "カラオケ"
            },
            new Lyric(),
            new Lyric
            {
                Text = "大好き"
            }
        };

        #endregion
    }
}
