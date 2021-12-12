// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

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
        private const int index_exist_tag = -1;

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
            var caretPosition = createTextCaretPosition(lyrics, lyricIndex, index);

            // Check is movable
            TestPositionMovable(lyrics, caretPosition, movable);
        }

        [TestCase(nameof(singleLyric), 0, 1, NOT_EXIST, NOT_EXIST)] // cannot move up if at top index.
        [TestCase(nameof(twoLyricsWithText), 1, 1, 0, 1)]
        [TestCase(nameof(threeLyricsWithSpacing), 2, 1, 0, 1)]
        [TestCase(nameof(threeLyricsWithSpacing), 2, 2, 0, 2)]
        public void TestMoveUp(string sourceName, int lyricIndex, int index, int newLyricIndex, int newIndex)
        {
            var lyrics = GetLyricsByMethodName(sourceName);
            var caretPosition = createTextCaretPosition(lyrics, lyricIndex, index);
            var newCaretPosition = createTextCaretPosition(lyrics, newLyricIndex, newIndex);

            // Check is movable
            TestMoveUp(lyrics, caretPosition, newCaretPosition);
        }

        [TestCase(nameof(singleLyric), 0, 1, NOT_EXIST, index_exist_tag)] // cannot move down if at bottom index.
        [TestCase(nameof(twoLyricsWithText), 0, 1, 1, 1)]
        [TestCase(nameof(twoLyricsWithText), 0, 3, 1, 2)]
        [TestCase(nameof(threeLyricsWithSpacing), 0, 1, 2, 1)]
        [TestCase(nameof(threeLyricsWithSpacing), 0, 3, 2, 2)]
        public void TestMoveDown(string sourceName, int lyricIndex, int index, int newLyricIndex, int newIndex)
        {
            var lyrics = GetLyricsByMethodName(sourceName);
            var caretPosition = createTextCaretPosition(lyrics, lyricIndex, index);
            var newCaretPosition = createTextCaretPosition(lyrics, newLyricIndex, newIndex);

            // Check is movable
            TestMoveDown(lyrics, caretPosition, newCaretPosition);
        }

        [TestCase(nameof(singleLyric), 0, 1, NOT_EXIST, index_exist_tag)]
        [TestCase(nameof(twoLyricsWithText), 1, 1, 0, 3)]
        [TestCase(nameof(threeLyricsWithSpacing), 2, 1, 0, 3)]
        [TestCase(nameof(threeLyricsWithSpacing), 2, 3, 2, 2)]
        public void TestMoveLeft(string sourceName, int lyricIndex, int index, int newLyricIndex, int newIndex)
        {
            var lyrics = GetLyricsByMethodName(sourceName);
            var caretPosition = createTextCaretPosition(lyrics, lyricIndex, index);
            var newCaretPosition = createTextCaretPosition(lyrics, newLyricIndex, newIndex);

            // Check is movable
            TestMoveLeft(lyrics, caretPosition, newCaretPosition);
        }

        [TestCase(nameof(singleLyric), 0, 3, NOT_EXIST, index_exist_tag)]
        [TestCase(nameof(twoLyricsWithText), 0, 3, 1, 1)]
        [TestCase(nameof(threeLyricsWithSpacing), 0, 3, 2, 1)]
        [TestCase(nameof(threeLyricsWithSpacing), 0, 2, 0, 3)]
        public void TestMoveRight(string sourceName, int lyricIndex, int index, int newLyricIndex, int newIndex)
        {
            var lyrics = GetLyricsByMethodName(sourceName);
            var caretPosition = createTextCaretPosition(lyrics, lyricIndex, index);
            var newCaretPosition = createTextCaretPosition(lyrics, newLyricIndex, newIndex);

            // Check is movable
            TestMoveRight(lyrics, caretPosition, newCaretPosition);
        }

        [TestCase(nameof(singleLyric), 0, 1)]
        [TestCase(nameof(singleLyricWithNoText), NOT_EXIST, index_exist_tag)]
        [TestCase(nameof(twoLyricsWithText), 0, 1)]
        [TestCase(nameof(threeLyricsWithSpacing), 0, 1)]
        public void TestMoveToFirst(string sourceName, int lyricIndex, int index)
        {
            var lyrics = GetLyricsByMethodName(sourceName);
            var caretPosition = createTextCaretPosition(lyrics, lyricIndex, index);

            // Check first position
            TestMoveToFirst(lyrics, caretPosition);
        }

        [TestCase(nameof(singleLyric), 0, 3)]
        [TestCase(nameof(singleLyricWithNoText), NOT_EXIST, index_exist_tag)]
        [TestCase(nameof(twoLyricsWithText), 1, 2)]
        [TestCase(nameof(threeLyricsWithSpacing), 2, 2)]
        public void TestMoveToLast(string sourceName, int lyricIndex, int index)
        {
            var lyrics = GetLyricsByMethodName(sourceName);
            var caretPosition = createTextCaretPosition(lyrics, lyricIndex, index);

            // Check last position
            TestMoveToLast(lyrics, caretPosition);
        }

        [TestCase(nameof(singleLyric), 0)]
        [TestCase(nameof(singleLyricWithNoText), 0)]
        public void TestMoveToTarget(string sourceName, int lyricIndex)
        {
            var lyrics = GetLyricsByMethodName(sourceName);
            var lyric = lyrics[lyricIndex];
            var caretPosition = createTextCaretPosition(lyrics, lyricIndex, 1);

            // Check move to target position.
            TestMoveToTarget(lyrics, lyric, caretPosition);
        }

        protected override void AssertEqual(TextCaretPosition compare, TextCaretPosition actual)
        {
            if (compare == null)
            {
                Assert.IsNull(actual);
            }
            else
            {
                Assert.AreEqual(compare.Lyric, actual.Lyric);
                Assert.AreEqual(compare.Index, actual.Index);
            }
        }

        private static TextCaretPosition createTextCaretPosition(IEnumerable<Lyric> lyrics, int lyricIndex, int index)
        {
            if (lyricIndex == NOT_EXIST)
                return null;

            var lyric = lyrics.ElementAtOrDefault(lyricIndex);
            return new TextCaretPosition(lyric, index);
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
