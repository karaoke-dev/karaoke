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
    public class TypingCaretPositionAlgorithmTest : BaseIndexCaretPositionAlgorithmTest<TypingCaretPositionAlgorithm, TypingCaretPosition>
    {
        #region Lyric

        [TestCase(nameof(singleLyric), 0, 0, true)]
        [TestCase(nameof(singleLyric), 0, 4, true)]
        [TestCase(nameof(singleLyric), 0, 5, false)]
        [TestCase(nameof(singleLyric), 0, -1, false)]
        [TestCase(nameof(singleLyricWithNoText), 0, 0, true)] // it's still movable even text is empty or null.
        [TestCase(nameof(singleLyricWithNoText), 0, 1, false)]
        public void TestPositionMovable(string sourceName, int lyricIndex, int index, bool movable)
        {
            var lyrics = GetLyricsByMethodName(sourceName);
            var caret = createCaretPosition(lyrics, lyricIndex, index);

            // Check is movable
            TestPositionMovable(lyrics, caret, movable);
        }

        [TestCase(nameof(singleLyric), 0, 0, null, null)] // cannot move up if at top index.
        [TestCase(nameof(singleLyricWithNoText), 0, 0, null, null)]
        [TestCase(nameof(twoLyricsWithText), 1, 0, 0, 0)]
        [TestCase(nameof(threeLyricsWithSpacing), 2, 0, 1, 0)]
        [TestCase(nameof(threeLyricsWithSpacing), 2, 3, 1, 0)]
        public void TestMoveToPreviousLyric(string sourceName, int lyricIndex, int index, int? expectedLyricIndex, int? expectedIndex)
        {
            var lyrics = GetLyricsByMethodName(sourceName);
            var caret = createCaretPosition(lyrics, lyricIndex, index);
            var expected = createExpectedCaretPosition(lyrics, expectedLyricIndex, expectedIndex);

            // Check is movable
            TestMoveToPreviousLyric(lyrics, caret, expected);
        }

        [TestCase(nameof(singleLyric), 0, 0, null, null)] // cannot move down if at bottom index.
        [TestCase(nameof(singleLyricWithNoText), 0, 0, null, null)]
        [TestCase(nameof(twoLyricsWithText), 0, 0, 1, 0)]
        [TestCase(nameof(threeLyricsWithSpacing), 0, 0, 1, 0)]
        [TestCase(nameof(threeLyricsWithSpacing), 0, 4, 1, 0)]
        public void TestMoveToNextLyric(string sourceName, int lyricIndex, int index, int? expectedLyricIndex, int? expectedIndex)
        {
            var lyrics = GetLyricsByMethodName(sourceName);
            var caret = createCaretPosition(lyrics, lyricIndex, index);
            var expected = createExpectedCaretPosition(lyrics, expectedLyricIndex, expectedIndex);

            // Check is movable
            TestMoveToNextLyric(lyrics, caret, expected);
        }

        [TestCase(nameof(singleLyric), 0, 0)]
        [TestCase(nameof(singleLyricWithNoText), 0, 0)]
        [TestCase(nameof(twoLyricsWithText), 0, 0)]
        [TestCase(nameof(threeLyricsWithSpacing), 0, 0)]
        public void TestMoveToFirstLyric(string sourceName, int? expectedLyricIndex, int? expectedIndex)
        {
            var lyrics = GetLyricsByMethodName(sourceName);
            var expected = createExpectedCaretPosition(lyrics, expectedLyricIndex, expectedIndex);

            // Check first position
            TestMoveToFirstLyric(lyrics, expected);
        }

        [TestCase(nameof(singleLyric), 0, 4)]
        [TestCase(nameof(singleLyricWithNoText), 0, 0)]
        [TestCase(nameof(twoLyricsWithText), 1, 3)]
        [TestCase(nameof(threeLyricsWithSpacing), 2, 3)]
        public void TestMoveToLastLyric(string sourceName, int? expectedLyricIndex, int? expectedIndex)
        {
            var lyrics = GetLyricsByMethodName(sourceName);
            var expected = createExpectedCaretPosition(lyrics, expectedLyricIndex, expectedIndex);

            // Check last position
            TestMoveToLastLyric(lyrics, expected);
        }

        [TestCase(nameof(singleLyric), 0, 0)]
        [TestCase(nameof(singleLyricWithNoText), 0, 0)]
        public void TestMoveToTargetLyric(string sourceName, int expectedLyricIndex, int? expectedIndex)
        {
            var lyrics = GetLyricsByMethodName(sourceName);
            var lyric = lyrics[expectedLyricIndex];
            var expected = createExpectedCaretPosition(lyrics, expectedLyricIndex, expectedIndex);

            // Check move to target position.
            TestMoveToTargetLyric(lyrics, lyric, expected);
        }

        #endregion

        #region Lyric index

        [TestCase(nameof(singleLyric), 0, 0, null, null)]
        [TestCase(nameof(singleLyricWithNoText), 0, 0, null, null)]
        [TestCase(nameof(twoLyricsWithText), 1, 0, 0, 4)]
        [TestCase(nameof(threeLyricsWithSpacing), 2, 0, 1, 0)]
        [TestCase(nameof(threeLyricsWithSpacing), 2, 3, 2, 2)]
        public void TestMoveToPreviousIndex(string sourceName, int lyricIndex, int index, int? expectedLyricIndex, int? expectedIndex)
        {
            var lyrics = GetLyricsByMethodName(sourceName);
            var caret = createCaretPosition(lyrics, lyricIndex, index);
            var expected = createExpectedCaretPosition(lyrics, expectedLyricIndex, expectedIndex);

            // Check is movable
            TestMoveToPreviousIndex(lyrics, caret, expected);
        }

        [TestCase(nameof(singleLyric), 0, 4, null, null)]
        [TestCase(nameof(singleLyricWithNoText), 0, 0, null, null)]
        [TestCase(nameof(twoLyricsWithText), 0, 4, 1, 0)]
        [TestCase(nameof(threeLyricsWithSpacing), 0, 4, 1, 0)]
        [TestCase(nameof(threeLyricsWithSpacing), 0, 3, 0, 4)]
        public void TestMoveToNextIndex(string sourceName, int lyricIndex, int index, int? expectedLyricIndex, int? expectedIndex)
        {
            var lyrics = GetLyricsByMethodName(sourceName);
            var caret = createCaretPosition(lyrics, lyricIndex, index);
            var expected = createExpectedCaretPosition(lyrics, expectedLyricIndex, expectedIndex);

            // Check is movable
            TestMoveToNextIndex(lyrics, caret, expected);
        }

        [TestCase(nameof(singleLyric), 0, 0, 0)]
        [TestCase(nameof(singleLyricWithNoText), 0, 0, 0)]
        public void TestMoveToFirstIndex(string sourceName, int lyricIndex, int? expectedLyricIndex, int? expectedIndex)
        {
            var lyrics = GetLyricsByMethodName(sourceName);
            var lyric = lyrics[lyricIndex];
            var expected = createExpectedCaretPosition(lyrics, expectedLyricIndex, expectedIndex);

            // Check is movable
            TestMoveToFirstIndex(lyrics, lyric, expected);
        }

        [TestCase(nameof(singleLyric), 0, 0, 4)]
        [TestCase(nameof(singleLyricWithNoText), 0, 0, 0)]
        public void TestMoveToLastIndex(string sourceName, int lyricIndex, int? expectedLyricIndex, int? expectedIndex)
        {
            var lyrics = GetLyricsByMethodName(sourceName);
            var lyric = lyrics[lyricIndex];
            var expected = createExpectedCaretPosition(lyrics, expectedLyricIndex, expectedIndex);

            // Check is movable
            TestMoveToLastIndex(lyrics, lyric, expected);
        }

        #endregion

        protected override void AssertEqual(TypingCaretPosition expected, TypingCaretPosition actual)
        {
            Assert.AreEqual(expected.Lyric, actual.Lyric);
            Assert.AreEqual(expected.Index, actual.Index);
        }

        private static TypingCaretPosition createCaretPosition(IEnumerable<Lyric> lyrics, int lyricIndex, int index)
        {
            var lyric = lyrics.ElementAtOrDefault(lyricIndex);
            if (lyric == null)
                throw new ArgumentNullException();

            return new TypingCaretPosition(lyric, index);
        }

        private static TypingCaretPosition? createExpectedCaretPosition(IEnumerable<Lyric> lyrics, int? lyricIndex, int? index)
        {
            if (lyricIndex == null || index == null)
                return null;

            return createCaretPosition(lyrics, lyricIndex.Value, index.Value);
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
