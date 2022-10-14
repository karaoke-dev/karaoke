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
    public class ClickingCaretPositionAlgorithmTest : BaseCaretPositionAlgorithmTest<ClickingCaretPositionAlgorithm, ClickingCaretPosition>
    {
        [TestCase(nameof(singleLyric), 0, true)]
        [TestCase(nameof(singleLyricWithNoText), 0, true)]
        public void TestPositionMovable(string sourceName, int lyricIndex, bool movable)
        {
            var lyrics = GetLyricsByMethodName(sourceName);
            var caret = createCaretPosition(lyrics, lyricIndex);

            // Check is movable, will always be true in this algorithm.
            TestPositionMovable(lyrics, caret, movable);
        }

        [TestCase(nameof(singleLyric), 0, null)] // should always not movable.
        [TestCase(nameof(singleLyricWithNoText), 0, null)]
        [TestCase(nameof(twoLyricsWithText), 1, null)]
        [TestCase(nameof(threeLyricsWithSpacing), 2, null)]
        public void TestMoveUp(string sourceName, int lyricIndex, int? expectedLyricIndex)
        {
            var lyrics = GetLyricsByMethodName(sourceName);
            var caret = createCaretPosition(lyrics, lyricIndex);
            var expected = createExpectedCaretPosition(lyrics, expectedLyricIndex);

            // Check is movable
            TestMoveUp(lyrics, caret, expected);
        }

        [TestCase(nameof(singleLyric), 0, null)] // should always not movable.
        [TestCase(nameof(singleLyricWithNoText), 0, null)]
        [TestCase(nameof(twoLyricsWithText), 0, null)]
        [TestCase(nameof(threeLyricsWithSpacing), 0, null)]
        public void TestMoveDown(string sourceName, int lyricIndex, int? expectedLyricIndex)
        {
            var lyrics = GetLyricsByMethodName(sourceName);
            var caret = createCaretPosition(lyrics, lyricIndex);
            var expected = createExpectedCaretPosition(lyrics, expectedLyricIndex);

            // Check is movable
            TestMoveDown(lyrics, caret, expected);
        }

        [TestCase(nameof(singleLyric), null)] // should always not movable.
        [TestCase(nameof(singleLyricWithNoText), null)]
        [TestCase(nameof(twoLyricsWithText), null)]
        [TestCase(nameof(threeLyricsWithSpacing), null)]
        public void TestMoveToFirst(string sourceName, int? expectedLyricIndex)
        {
            var lyrics = GetLyricsByMethodName(sourceName);
            var expected = createExpectedCaretPosition(lyrics, expectedLyricIndex);

            // Check first position
            TestMoveToFirst(lyrics, expected);
        }

        [TestCase(nameof(singleLyric), null)] // should always not movable.
        [TestCase(nameof(singleLyricWithNoText), null)]
        [TestCase(nameof(twoLyricsWithText), null)]
        [TestCase(nameof(threeLyricsWithSpacing), null)]
        public void TestMoveToLast(string sourceName, int? expectedLyricIndex)
        {
            var lyrics = GetLyricsByMethodName(sourceName);
            var expected = createExpectedCaretPosition(lyrics, expectedLyricIndex);

            // Check last position
            TestMoveToLast(lyrics, expected);
        }

        [TestCase(nameof(singleLyric), 0)]
        [TestCase(nameof(singleLyricWithNoText), 0)]
        public void TestMoveToTarget(string sourceName, int expectedLyricIndex)
        {
            var lyrics = GetLyricsByMethodName(sourceName);
            var lyric = lyrics[expectedLyricIndex];
            var expected = createExpectedCaretPosition(lyrics, expectedLyricIndex);

            // Check move to target position.
            TestMoveToTarget(lyrics, lyric, expected);
        }

        protected override void AssertEqual(ClickingCaretPosition expected, ClickingCaretPosition actual)
        {
            Assert.AreEqual(expected.Lyric, actual.Lyric);
        }

        private static ClickingCaretPosition createCaretPosition(IEnumerable<Lyric> lyrics, int lyricIndex)
        {
            var lyric = lyrics.ElementAtOrDefault(lyricIndex);
            if (lyric == null)
                throw new ArgumentNullException();

            return new ClickingCaretPosition(lyric);
        }

        private static ClickingCaretPosition? createExpectedCaretPosition(IEnumerable<Lyric> lyrics, int? lyricIndex)
        {
            if (lyricIndex == null)
                return null;

            return createCaretPosition(lyrics, lyricIndex.Value);
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
