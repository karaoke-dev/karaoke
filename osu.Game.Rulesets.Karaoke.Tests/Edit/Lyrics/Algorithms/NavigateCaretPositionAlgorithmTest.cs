// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Linq;
using NUnit.Framework;
using osu.Game.Rulesets.Karaoke.Edit.Lyrics.Algorithms;
using osu.Game.Rulesets.Karaoke.Edit.Lyrics.CaretPosition;
using osu.Game.Rulesets.Karaoke.Objects;

namespace osu.Game.Rulesets.Karaoke.Tests.Edit.Lyrics.Algorithms
{
    public class NavigateCaretPositionAlgorithmTest : BaseCaretPositionAlgorithmTest<NavigateCaretPositionAlgorithm, NavigateCaretPosition>
    {
        [TestCase(nameof(singleLyric), 0, true)]
        [TestCase(nameof(singleLyricWithNoText), 0, true)]
        public void TestPositionMovable(string sourceName, int lyricIndex, bool movable)
        {
            var lyrics = GetLyricsByMethodName(sourceName);
            var caretPosition = CreateEditNoteCaretPosition(lyrics, lyricIndex);

            // Check is movable, will always be true in this algorithm.
            TestPositionMovable(lyrics, caretPosition, movable);
        }

        [TestCase(nameof(singleLyric), 0, NOT_EXIST)] // cannot move up if at top index.
        [TestCase(nameof(singleLyricWithNoText), 0, NOT_EXIST)]
        [TestCase(nameof(twoLyricsWithText), 1, 0)]
        [TestCase(nameof(threeLyricsWithSpacing), 2, 1)]
        public void TestMoveUp(string sourceName, int lyricIndex, int newLyricIndex)
        {
            var lyrics = GetLyricsByMethodName(sourceName);
            var caretPosition = CreateEditNoteCaretPosition(lyrics, lyricIndex);
            var newCaretPosition = CreateEditNoteCaretPosition(lyrics, newLyricIndex);

            // Check is movable
            TestMoveUp(lyrics, caretPosition, newCaretPosition);
        }

        [TestCase(nameof(singleLyric), 0, NOT_EXIST)] // cannot move down if at bottom index.
        [TestCase(nameof(singleLyricWithNoText), 0, NOT_EXIST)]
        [TestCase(nameof(twoLyricsWithText), 0, 1)]
        [TestCase(nameof(threeLyricsWithSpacing), 0, 1)]
        public void TestMoveDown(string sourceName, int lyricIndex, int newLyricIndex)
        {
            var lyrics = GetLyricsByMethodName(sourceName);
            var caretPosition = CreateEditNoteCaretPosition(lyrics, lyricIndex);
            var newCaretPosition = CreateEditNoteCaretPosition(lyrics, newLyricIndex);

            // Check is movable
            TestMoveDown(lyrics, caretPosition, newCaretPosition);
        }

        [TestCase(nameof(singleLyric), 0, NOT_EXIST)]
        [TestCase(nameof(singleLyricWithNoText), 0, NOT_EXIST)]
        [TestCase(nameof(twoLyricsWithText), 0, NOT_EXIST)]
        [TestCase(nameof(threeLyricsWithSpacing), 0, NOT_EXIST)]
        public void TestMoveLeft(string sourceName, int lyricIndex, int newLyricIndex)
        {
            var lyrics = GetLyricsByMethodName(sourceName);
            var caretPosition = CreateEditNoteCaretPosition(lyrics, lyricIndex);
            var newCaretPosition = CreateEditNoteCaretPosition(lyrics, newLyricIndex);

            // Check is movable
            TestMoveLeft(lyrics, caretPosition, newCaretPosition);
        }

        [TestCase(nameof(singleLyric), 0, NOT_EXIST)]
        [TestCase(nameof(singleLyricWithNoText), 0, NOT_EXIST)]
        [TestCase(nameof(twoLyricsWithText), 0, NOT_EXIST)]
        [TestCase(nameof(threeLyricsWithSpacing), 0, NOT_EXIST)]
        public void TestMoveRight(string sourceName, int lyricIndex, int newLyricIndex)
        {
            var lyrics = GetLyricsByMethodName(sourceName);
            var caretPosition = CreateEditNoteCaretPosition(lyrics, lyricIndex);
            var newCaretPosition = CreateEditNoteCaretPosition(lyrics, newLyricIndex);

            // Check is movable
            TestMoveRight(lyrics, caretPosition, newCaretPosition);
        }

        [TestCase(nameof(singleLyric), 0)]
        [TestCase(nameof(singleLyricWithNoText), 0)]
        [TestCase(nameof(twoLyricsWithText), 0)]
        [TestCase(nameof(threeLyricsWithSpacing), 0)]
        public void TestMoveToFirst(string sourceName, int lyricIndex)
        {
            var lyrics = GetLyricsByMethodName(sourceName);
            var caretPosition = CreateEditNoteCaretPosition(lyrics, lyricIndex);

            // Check first position
            TestMoveToFirst(lyrics, caretPosition);
        }

        [TestCase(nameof(singleLyric), 0)]
        [TestCase(nameof(singleLyricWithNoText), 0)]
        [TestCase(nameof(twoLyricsWithText), 1)]
        [TestCase(nameof(threeLyricsWithSpacing), 2)]
        public void TestMoveToLast(string sourceName, int lyricIndex)
        {
            var lyrics = GetLyricsByMethodName(sourceName);
            var caretPosition = CreateEditNoteCaretPosition(lyrics, lyricIndex);

            // Check last position
            TestMoveToLast(lyrics, caretPosition);
        }

        [TestCase(nameof(singleLyric), 0)]
        [TestCase(nameof(singleLyricWithNoText), 0)]
        public void TestMoveToTarget(string sourceName, int lyricIndex)
        {
            var lyrics = GetLyricsByMethodName(sourceName);
            var lyric = lyrics[lyricIndex];
            var caretPosition = CreateEditNoteCaretPosition(lyrics, lyricIndex);

            // Check move to target position.
            TestMoveToTarget(lyrics, lyric, caretPosition);
        }

        protected override void AssertEqual(NavigateCaretPosition compare, NavigateCaretPosition actual)
        {
            if (compare == null)
            {
                Assert.IsNull(actual);
            }
            else
            {
                Assert.AreEqual(compare.Lyric, actual.Lyric);
            }
        }

        protected NavigateCaretPosition CreateEditNoteCaretPosition(Lyric[] lyrics, int lyricIndex)
        {
            if (lyricIndex == NOT_EXIST)
                return null;

            var lyric = lyrics.ElementAtOrDefault(lyricIndex);
            return new NavigateCaretPosition(lyric);
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
