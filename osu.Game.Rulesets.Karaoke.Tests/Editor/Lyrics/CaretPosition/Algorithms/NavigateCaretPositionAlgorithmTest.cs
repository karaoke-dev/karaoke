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
    public class NavigateCaretPositionAlgorithmTest : BaseCaretPositionAlgorithmTest<NavigateCaretPositionAlgorithm, NavigateCaretPosition>
    {
        [TestCase(nameof(singleLyric), 0, true)]
        [TestCase(nameof(singleLyricWithNoText), 0, true)]
        public void TestPositionMovable(string sourceName, int lyricIndex, bool movable)
        {
            var lyrics = GetLyricsByMethodName(sourceName);
            var caret = createNavigateCaretPosition(lyrics, lyricIndex);

            // Check is movable, will always be true in this algorithm.
            TestPositionMovable(lyrics, caret, movable);
        }

        [TestCase(nameof(singleLyric), 0, null)] // cannot move up if at top index.
        [TestCase(nameof(singleLyricWithNoText), 0, null)]
        [TestCase(nameof(twoLyricsWithText), 1, 0)]
        [TestCase(nameof(threeLyricsWithSpacing), 2, 1)]
        public void TestMoveUp(string sourceName, int lyricIndex, int? expectedLyricIndex)
        {
            var lyrics = GetLyricsByMethodName(sourceName);
            var caret = createNavigateCaretPosition(lyrics, lyricIndex);
            var expected = createExpectedNavigateCaretPosition(lyrics, expectedLyricIndex);

            // Check is movable
            TestMoveUp(lyrics, caret, expected);
        }

        [TestCase(nameof(singleLyric), 0, null)] // cannot move down if at bottom index.
        [TestCase(nameof(singleLyricWithNoText), 0, null)]
        [TestCase(nameof(twoLyricsWithText), 0, 1)]
        [TestCase(nameof(threeLyricsWithSpacing), 0, 1)]
        public void TestMoveDown(string sourceName, int lyricIndex, int? expectedLyricIndex)
        {
            var lyrics = GetLyricsByMethodName(sourceName);
            var caret = createNavigateCaretPosition(lyrics, lyricIndex);
            var expected = createExpectedNavigateCaretPosition(lyrics, expectedLyricIndex);

            // Check is movable
            TestMoveDown(lyrics, caret, expected);
        }

        [TestCase(nameof(singleLyric), 0, null)]
        [TestCase(nameof(singleLyricWithNoText), 0, null)]
        [TestCase(nameof(twoLyricsWithText), 0, null)]
        [TestCase(nameof(threeLyricsWithSpacing), 0, null)]
        public void TestMoveLeft(string sourceName, int lyricIndex, int? expectedLyricIndex)
        {
            var lyrics = GetLyricsByMethodName(sourceName);
            var caret = createNavigateCaretPosition(lyrics, lyricIndex);
            var expected = createExpectedNavigateCaretPosition(lyrics, expectedLyricIndex);

            // Check is movable
            TestMoveLeft(lyrics, caret, expected);
        }

        [TestCase(nameof(singleLyric), 0, null)]
        [TestCase(nameof(singleLyricWithNoText), 0, null)]
        [TestCase(nameof(twoLyricsWithText), 0, null)]
        [TestCase(nameof(threeLyricsWithSpacing), 0, null)]
        public void TestMoveRight(string sourceName, int lyricIndex, int? expectedLyricIndex)
        {
            var lyrics = GetLyricsByMethodName(sourceName);
            var caret = createNavigateCaretPosition(lyrics, lyricIndex);
            var expected = createExpectedNavigateCaretPosition(lyrics, expectedLyricIndex);

            // Check is movable
            TestMoveRight(lyrics, caret, expected);
        }

        [TestCase(nameof(singleLyric), 0)]
        [TestCase(nameof(singleLyricWithNoText), 0)]
        [TestCase(nameof(twoLyricsWithText), 0)]
        [TestCase(nameof(threeLyricsWithSpacing), 0)]
        public void TestMoveToFirst(string sourceName, int? expectedLyricIndex)
        {
            var lyrics = GetLyricsByMethodName(sourceName);
            var expected = createExpectedNavigateCaretPosition(lyrics, expectedLyricIndex);

            // Check first position
            TestMoveToFirst(lyrics, expected);
        }

        [TestCase(nameof(singleLyric), 0)]
        [TestCase(nameof(singleLyricWithNoText), 0)]
        [TestCase(nameof(twoLyricsWithText), 1)]
        [TestCase(nameof(threeLyricsWithSpacing), 2)]
        public void TestMoveToLast(string sourceName, int? expectedLyricIndex)
        {
            var lyrics = GetLyricsByMethodName(sourceName);
            var expected = createExpectedNavigateCaretPosition(lyrics, expectedLyricIndex);

            // Check last position
            TestMoveToLast(lyrics, expected);
        }

        [TestCase(nameof(singleLyric), 0)]
        [TestCase(nameof(singleLyricWithNoText), 0)]
        public void TestMoveToTarget(string sourceName, int expectedLyricIndex)
        {
            var lyrics = GetLyricsByMethodName(sourceName);
            var lyric = lyrics[expectedLyricIndex];
            var expected = createExpectedNavigateCaretPosition(lyrics, expectedLyricIndex);

            // Check move to target position.
            TestMoveToTarget(lyrics, lyric, expected);
        }

        protected override void AssertEqual(NavigateCaretPosition expected, NavigateCaretPosition actual)
        {
            Assert.AreEqual(expected.Lyric, actual.Lyric);
        }

        private static NavigateCaretPosition createNavigateCaretPosition(IEnumerable<Lyric> lyrics, int lyricIndex)
        {
            var lyric = lyrics.ElementAtOrDefault(lyricIndex);
            if (lyric == null)
                throw new ArgumentNullException();

            return new NavigateCaretPosition(lyric);
        }

        private static NavigateCaretPosition? createExpectedNavigateCaretPosition(IEnumerable<Lyric> lyrics, int? lyricIndex)
        {
            if (lyricIndex == null)
                return null;

            return createNavigateCaretPosition(lyrics, lyricIndex.Value);
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
