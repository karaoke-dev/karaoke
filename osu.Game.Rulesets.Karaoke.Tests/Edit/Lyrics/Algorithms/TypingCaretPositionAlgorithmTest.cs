// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Linq;
using NUnit.Framework;
using osu.Game.Rulesets.Karaoke.Edit.Lyrics.Algorithms;
using osu.Game.Rulesets.Karaoke.Edit.Lyrics.CaretPosition;
using osu.Game.Rulesets.Karaoke.Objects;

namespace osu.Game.Rulesets.Karaoke.Tests.Edit.Lyrics.Algorithms
{
    [TestFixture]
    public class TypingCaretPositionAlgorithmTest : BaseCaretPositionAlgorithmTest<TypingCaretPositionAlgorithm, TextCaretPosition>
    {
        protected const int INDEX_EXIST_TAG = -1;

        [TestCase(nameof(singleLyric), 0, 0, true)]
        [TestCase(nameof(singleLyric), 0, 4, true)]
        [TestCase(nameof(singleLyric), 0, 5, false)]
        [TestCase(nameof(singleLyric), 0, -1, false)]
        [TestCase(nameof(singleLyricWithNoText), 0, 0, true)] // it's still movable even text is empty or null.
        [TestCase(nameof(singleLyricWithNoText), 0, 1, false)]
        public void TestPositionMovable(string sourceName, int lyricIndex, int index, bool movable)
        {
            var lyrics = GetLyricsByMethodName(sourceName);
            var caretPosition = CreateTextCaretPosition(lyrics, lyricIndex, index);

            // Check is movable
            TestPositionMovable(lyrics, caretPosition, movable);
        }

        [TestCase(nameof(singleLyric), 0, 0, NOT_EXIST, INDEX_EXIST_TAG)] // cannot move up if at top index.
        [TestCase(nameof(singleLyricWithNoText), 0, 0, NOT_EXIST, INDEX_EXIST_TAG)]
        [TestCase(nameof(twoLyricsWithText), 1, 0, 0, 0)]
        [TestCase(nameof(threeLyricsWithSpacing), 2, 0, 1, 0)]
        [TestCase(nameof(threeLyricsWithSpacing), 2, 3, 1, 0)]
        public void TestMoveUp(string sourceName, int lyricIndex, int index, int newLyricIndex, int newIndex)
        {
            var lyrics = GetLyricsByMethodName(sourceName);
            var caretPosition = CreateTextCaretPosition(lyrics, lyricIndex, index);
            var newCaretPosition = CreateTextCaretPosition(lyrics, newLyricIndex, newIndex);

            // Check is movable
            TestMoveUp(lyrics, caretPosition, newCaretPosition);
        }

        [TestCase(nameof(singleLyric), 0, 0, NOT_EXIST, INDEX_EXIST_TAG)] // cannot move down if at bottom index.
        [TestCase(nameof(singleLyricWithNoText), 0, 0, NOT_EXIST, INDEX_EXIST_TAG)]
        [TestCase(nameof(twoLyricsWithText), 0, 0, 1, 0)]
        [TestCase(nameof(threeLyricsWithSpacing), 0, 0, 1, 0)]
        [TestCase(nameof(threeLyricsWithSpacing), 0, 4, 1, 0)]
        public void TestMoveDown(string sourceName, int lyricIndex, int index, int newLyricIndex, int newIndex)
        {
            var lyrics = GetLyricsByMethodName(sourceName);
            var caretPosition = CreateTextCaretPosition(lyrics, lyricIndex, index);
            var newCaretPosition = CreateTextCaretPosition(lyrics, newLyricIndex, newIndex);

            // Check is movable
            TestMoveDown(lyrics, caretPosition, newCaretPosition);
        }

        [TestCase(nameof(singleLyric), 0, 0, NOT_EXIST, INDEX_EXIST_TAG)]
        [TestCase(nameof(singleLyricWithNoText), 0, 0, NOT_EXIST, INDEX_EXIST_TAG)]
        [TestCase(nameof(twoLyricsWithText), 1, 0, 0, 4)]
        [TestCase(nameof(threeLyricsWithSpacing), 2, 0, 1, 0)]
        [TestCase(nameof(threeLyricsWithSpacing), 2, 3, 2, 2)]
        public void TestMoveLeft(string sourceName, int lyricIndex, int index, int newLyricIndex, int newIndex)
        {
            var lyrics = GetLyricsByMethodName(sourceName);
            var caretPosition = CreateTextCaretPosition(lyrics, lyricIndex, index);
            var newCaretPosition = CreateTextCaretPosition(lyrics, newLyricIndex, newIndex);

            // Check is movable
            TestMoveLeft(lyrics, caretPosition, newCaretPosition);
        }

        [TestCase(nameof(singleLyric), 0, 4, NOT_EXIST, INDEX_EXIST_TAG)]
        [TestCase(nameof(singleLyricWithNoText), 0, 0, NOT_EXIST, INDEX_EXIST_TAG)]
        [TestCase(nameof(twoLyricsWithText), 0, 4, 1, 0)]
        [TestCase(nameof(threeLyricsWithSpacing), 0, 4, 1, 0)]
        [TestCase(nameof(threeLyricsWithSpacing), 0, 3, 0, 4)]
        public void TestMoveRight(string sourceName, int lyricIndex, int index, int newLyricIndex, int newIndex)
        {
            var lyrics = GetLyricsByMethodName(sourceName);
            var caretPosition = CreateTextCaretPosition(lyrics, lyricIndex, index);
            var newCaretPosition = CreateTextCaretPosition(lyrics, newLyricIndex, newIndex);

            // Check is movable
            TestMoveRight(lyrics, caretPosition, newCaretPosition);
        }

        [TestCase(nameof(singleLyric), 0, 0)]
        [TestCase(nameof(singleLyricWithNoText), 0, 0)]
        [TestCase(nameof(twoLyricsWithText), 0, 0)]
        [TestCase(nameof(threeLyricsWithSpacing), 0, 0)]
        public void TestMoveToFirst(string sourceName, int lyricIndex, int index)
        {
            var lyrics = GetLyricsByMethodName(sourceName);
            var caretPosition = CreateTextCaretPosition(lyrics, lyricIndex, index);

            // Check first position
            TestMoveToFirst(lyrics, caretPosition);
        }

        [TestCase(nameof(singleLyric), 0, 4)]
        [TestCase(nameof(singleLyricWithNoText), 0, 0)]
        [TestCase(nameof(twoLyricsWithText), 1, 3)]
        [TestCase(nameof(threeLyricsWithSpacing), 2, 3)]
        public void TestMoveToLast(string sourceName, int lyricIndex, int index)
        {
            var lyrics = GetLyricsByMethodName(sourceName);
            var caretPosition = CreateTextCaretPosition(lyrics, lyricIndex, index);

            // Check last position
            TestMoveToLast(lyrics, caretPosition);
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

        protected TextCaretPosition CreateTextCaretPosition(Lyric[] lyrics, int lyricIndex, int index)
        {
            if (lyricIndex == NOT_EXIST)
                return null;

            var lyric = lyrics.ElementAtOrDefault(lyricIndex);
            return new TextCaretPosition(lyric, index);
        }

        #region source

        private Lyric[] singleLyric => new[]
        {
            new Lyric
            {
                Text = "カラオケ"
            }
        };

        private Lyric[] singleLyricWithNoText => new[]
        {
            new Lyric()
        };

        private Lyric[] twoLyricsWithText => new[]
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

        private Lyric[] threeLyricsWithSpacing => new[]
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
