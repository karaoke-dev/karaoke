// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Linq;
using NUnit.Framework;
using osu.Game.Rulesets.Karaoke.Edit.Lyrics;
using osu.Game.Rulesets.Karaoke.Edit.Lyrics.CaretPosition;
using osu.Game.Rulesets.Karaoke.Edit.Lyrics.CaretPosition.Algorithms;
using osu.Game.Rulesets.Karaoke.Objects;
using osu.Game.Rulesets.Karaoke.Tests.Helper;

namespace osu.Game.Rulesets.Karaoke.Tests.Edit.Lyrics.CaretPosition.Algorithms
{
    [TestFixture]
    public class TimeTagIndexCaretPositionAlgorithmTest : BaseCaretPositionAlgorithmTest<TimeTagIndexCaretPositionAlgorithm, TimeTagIndexCaretPosition>
    {
        protected const string NOT_EXIST_TAG = null;

        [TestCase(nameof(singleLyric), MovingTimeTagCaretMode.None, 0, "[0,start]", true)]
        [TestCase(nameof(singleLyric), MovingTimeTagCaretMode.OnlyStartTag, 0, "[0,start]", true)]
        [TestCase(nameof(singleLyric), MovingTimeTagCaretMode.OnlyEndTag, 0, "[0,start]", false)]
        [TestCase(nameof(singleLyric), MovingTimeTagCaretMode.None, 0, "[3,end]", true)]
        [TestCase(nameof(singleLyric), MovingTimeTagCaretMode.OnlyStartTag, 0, "[3,end]", false)]
        [TestCase(nameof(singleLyric), MovingTimeTagCaretMode.OnlyEndTag, 0, "[3,end]", true)]
        [TestCase(nameof(singleLyric), MovingTimeTagCaretMode.None, 0, "[4,start]", false)] // should not out of range.
        [TestCase(nameof(singleLyric), MovingTimeTagCaretMode.None, 0, "[-1,end]", false)]
        [TestCase(nameof(singleLyricWithNoText), MovingTimeTagCaretMode.None, 0, "[0,start]", false)] // it's not movable if no text
        [TestCase(nameof(singleLyricWithNoText), MovingTimeTagCaretMode.None, 0, "[0,end]", false)]
        public void TestPositionMovable(string sourceName, MovingTimeTagCaretMode mode, int lyricIndex, string timeTag, bool movable)
        {
            var lyrics = GetLyricsByMethodName(sourceName);
            var caretPosition = createTimeTagIndexCaretPosition(lyrics, lyricIndex, timeTag);

            // Check is movable
            TestPositionMovable(lyrics, caretPosition, movable, algorithms => algorithms.Mode = mode);
        }

        [TestCase(nameof(singleLyric), MovingTimeTagCaretMode.None, 0, "[0,start]", NOT_EXIST, NOT_EXIST_TAG)] // cannot move up if at top index.
        [TestCase(nameof(twoLyricsWithText), MovingTimeTagCaretMode.None, 1, "[0,start]", 0, "[0,start]")]
        [TestCase(nameof(twoLyricsWithText), MovingTimeTagCaretMode.OnlyStartTag, 1, "[0,start]", 0, "[0,start]")]
        [TestCase(nameof(twoLyricsWithText), MovingTimeTagCaretMode.OnlyEndTag, 1, "[0,start]", 0, "[0,end]")]
        [TestCase(nameof(threeLyricsWithSpacing), MovingTimeTagCaretMode.None, 2, "[0,start]", 0, "[0,start]")]
        [TestCase(nameof(threeLyricsWithSpacing), MovingTimeTagCaretMode.OnlyStartTag, 2, "[0,start]", 0, "[0,start]")]
        [TestCase(nameof(threeLyricsWithSpacing), MovingTimeTagCaretMode.OnlyEndTag, 2, "[0,start]", 0, "[0,end]")]
        [TestCase(nameof(threeLyricsWithSpacing), MovingTimeTagCaretMode.None, 2, "[2,end]", 0, "[2,end]")]
        [TestCase(nameof(threeLyricsWithSpacing), MovingTimeTagCaretMode.OnlyStartTag, 2, "[2,end]", 0, "[2,start]")]
        [TestCase(nameof(threeLyricsWithSpacing), MovingTimeTagCaretMode.OnlyEndTag, 2, "[2,end]", 0, "[2,end]")]
        public void TestMoveUp(string sourceName, MovingTimeTagCaretMode mode, int lyricIndex, string textTag, int newLyricIndex, string newTextTag)
        {
            var lyrics = GetLyricsByMethodName(sourceName);
            var caretPosition = createTimeTagIndexCaretPosition(lyrics, lyricIndex, textTag);
            var newCaretPosition = createTimeTagIndexCaretPosition(lyrics, newLyricIndex, newTextTag);

            // Check is movable
            TestMoveUp(lyrics, caretPosition, newCaretPosition, algorithms => algorithms.Mode = mode);
        }

        [TestCase(nameof(singleLyric), MovingTimeTagCaretMode.None, 0, "[0,start]", NOT_EXIST, NOT_EXIST_TAG)] // cannot move down if at bottom index.
        [TestCase(nameof(twoLyricsWithText), MovingTimeTagCaretMode.None, 0, "[0,start]", 1, "[0,start]")]
        [TestCase(nameof(twoLyricsWithText), MovingTimeTagCaretMode.OnlyStartTag, 0, "[0,start]", 1, "[0,start]")]
        [TestCase(nameof(twoLyricsWithText), MovingTimeTagCaretMode.OnlyEndTag, 0, "[0,start]", 1, "[0,end]")]
        [TestCase(nameof(threeLyricsWithSpacing), MovingTimeTagCaretMode.None, 0, "[0,start]", 2, "[0,start]")]
        [TestCase(nameof(threeLyricsWithSpacing), MovingTimeTagCaretMode.OnlyStartTag, 0, "[0,start]", 2, "[0,start]")]
        [TestCase(nameof(threeLyricsWithSpacing), MovingTimeTagCaretMode.OnlyEndTag, 0, "[0,start]", 2, "[0,end]")]
        [TestCase(nameof(threeLyricsWithSpacing), MovingTimeTagCaretMode.None, 0, "[3,start]", 2, "[2,start]")]
        [TestCase(nameof(threeLyricsWithSpacing), MovingTimeTagCaretMode.OnlyStartTag, 0, "[3,start]", 2, "[2,start]")]
        [TestCase(nameof(threeLyricsWithSpacing), MovingTimeTagCaretMode.OnlyEndTag, 0, "[3,start]", 2, "[2,end]")]
        [TestCase(nameof(threeLyricsWithSpacing), MovingTimeTagCaretMode.None, 0, "[3,end]", 2, "[2,end]")]
        public void TestMoveDown(string sourceName, MovingTimeTagCaretMode mode, int lyricIndex, string textTag, int newLyricIndex, string newTextTag)
        {
            var lyrics = GetLyricsByMethodName(sourceName);
            var caretPosition = createTimeTagIndexCaretPosition(lyrics, lyricIndex, textTag);
            var newCaretPosition = createTimeTagIndexCaretPosition(lyrics, newLyricIndex, newTextTag);

            // Check is movable
            TestMoveDown(lyrics, caretPosition, newCaretPosition, algorithms => algorithms.Mode = mode);
        }

        [TestCase(nameof(singleLyric), MovingTimeTagCaretMode.None, 0, "[0,start]", NOT_EXIST, NOT_EXIST_TAG)]
        [TestCase(nameof(singleLyric), MovingTimeTagCaretMode.None, 0, "[1,start]", 0, "[0,end]")]
        [TestCase(nameof(singleLyric), MovingTimeTagCaretMode.OnlyStartTag, 0, "[1,start]", 0, "[0,start]")]
        [TestCase(nameof(singleLyric), MovingTimeTagCaretMode.OnlyEndTag, 0, "[1,start]", 0, "[0,end]")]
        [TestCase(nameof(singleLyric), MovingTimeTagCaretMode.None, 0, "[1,end]", 0, "[1,start]")]
        [TestCase(nameof(singleLyric), MovingTimeTagCaretMode.OnlyStartTag, 0, "[1,end]", 0, "[1,start]")]
        [TestCase(nameof(singleLyric), MovingTimeTagCaretMode.OnlyEndTag, 0, "[1,end]", 0, "[0,end]")]
        [TestCase(nameof(twoLyricsWithText), MovingTimeTagCaretMode.None, 1, "[0,start]", 0, "[3,end]")]
        [TestCase(nameof(twoLyricsWithText), MovingTimeTagCaretMode.OnlyStartTag, 1, "[0,start]", 0, "[3,start]")]
        [TestCase(nameof(twoLyricsWithText), MovingTimeTagCaretMode.OnlyEndTag, 1, "[0,start]", 0, "[3,end]")]
        [TestCase(nameof(threeLyricsWithSpacing), MovingTimeTagCaretMode.None, 2, "[0,start]", 0, "[3,end]")]
        [TestCase(nameof(threeLyricsWithSpacing), MovingTimeTagCaretMode.OnlyStartTag, 2, "[0,start]", 0, "[3,start]")]
        [TestCase(nameof(threeLyricsWithSpacing), MovingTimeTagCaretMode.OnlyEndTag, 2, "[0,start]", 0, "[3,end]")]
        public void TestMoveLeft(string sourceName, MovingTimeTagCaretMode mode, int lyricIndex, string textTag, int newLyricIndex, string newTextTag)
        {
            var lyrics = GetLyricsByMethodName(sourceName);
            var caretPosition = createTimeTagIndexCaretPosition(lyrics, lyricIndex, textTag);
            var newCaretPosition = createTimeTagIndexCaretPosition(lyrics, newLyricIndex, newTextTag);

            // Check is movable
            TestMoveLeft(lyrics, caretPosition, newCaretPosition, algorithms => algorithms.Mode = mode);
        }

        [TestCase(nameof(singleLyric), MovingTimeTagCaretMode.None, 0, "[3,end]", NOT_EXIST, NOT_EXIST_TAG)]
        [TestCase(nameof(singleLyric), MovingTimeTagCaretMode.None, 0, "[1,start]", 0, "[1,end]")]
        [TestCase(nameof(singleLyric), MovingTimeTagCaretMode.OnlyStartTag, 0, "[1,start]", 0, "[2,start]")]
        [TestCase(nameof(singleLyric), MovingTimeTagCaretMode.OnlyEndTag, 0, "[1,start]", 0, "[1,end]")]
        [TestCase(nameof(singleLyric), MovingTimeTagCaretMode.None, 0, "[1,end]", 0, "[2,start]")]
        [TestCase(nameof(singleLyric), MovingTimeTagCaretMode.OnlyStartTag, 0, "[1,end]", 0, "[2,start]")]
        [TestCase(nameof(singleLyric), MovingTimeTagCaretMode.OnlyEndTag, 0, "[1,end]", 0, "[2,end]")]
        [TestCase(nameof(twoLyricsWithText), MovingTimeTagCaretMode.None, 0, "[3,end]", 1, "[0,start]")]
        [TestCase(nameof(twoLyricsWithText), MovingTimeTagCaretMode.OnlyStartTag, 0, "[3,end]", 1, "[0,start]")]
        [TestCase(nameof(twoLyricsWithText), MovingTimeTagCaretMode.OnlyEndTag, 0, "[3,end]", 1, "[0,end]")]
        [TestCase(nameof(threeLyricsWithSpacing), MovingTimeTagCaretMode.None, 0, "[3,end]", 2, "[0,start]")]
        [TestCase(nameof(threeLyricsWithSpacing), MovingTimeTagCaretMode.OnlyStartTag, 0, "[3,end]", 2, "[0,start]")]
        [TestCase(nameof(threeLyricsWithSpacing), MovingTimeTagCaretMode.OnlyEndTag, 0, "[3,end]", 2, "[0,end]")]
        public void TestMoveRight(string sourceName, MovingTimeTagCaretMode mode, int lyricIndex, string textTag, int newLyricIndex, string newTextTag)
        {
            var lyrics = GetLyricsByMethodName(sourceName);
            var caretPosition = createTimeTagIndexCaretPosition(lyrics, lyricIndex, textTag);
            var newCaretPosition = createTimeTagIndexCaretPosition(lyrics, newLyricIndex, newTextTag);

            // Check is movable
            TestMoveRight(lyrics, caretPosition, newCaretPosition, algorithms => algorithms.Mode = mode);
        }

        [TestCase(nameof(singleLyric), MovingTimeTagCaretMode.None, 0, "[0,start]")]
        [TestCase(nameof(singleLyric), MovingTimeTagCaretMode.OnlyStartTag, 0, "[0,start]")]
        [TestCase(nameof(singleLyric), MovingTimeTagCaretMode.OnlyEndTag, 0, "[0,end]")]
        [TestCase(nameof(singleLyricWithNoText), MovingTimeTagCaretMode.None, NOT_EXIST, NOT_EXIST_TAG)]
        [TestCase(nameof(twoLyricsWithText), 0, MovingTimeTagCaretMode.None, "[0,start]")]
        [TestCase(nameof(threeLyricsWithSpacing), 0, MovingTimeTagCaretMode.None, "[0,start]")]
        public void TestMoveToFirst(string sourceName, MovingTimeTagCaretMode mode, int lyricIndex, string textTag)
        {
            var lyrics = GetLyricsByMethodName(sourceName);
            var caretPosition = createTimeTagIndexCaretPosition(lyrics, lyricIndex, textTag);

            // Check first position
            TestMoveToFirst(lyrics, caretPosition, algorithms => algorithms.Mode = mode);
        }

        [TestCase(nameof(singleLyric), MovingTimeTagCaretMode.None, 0, "[3,end]")]
        [TestCase(nameof(singleLyric), MovingTimeTagCaretMode.OnlyStartTag, 0, "[3,start]")]
        [TestCase(nameof(singleLyric), MovingTimeTagCaretMode.OnlyEndTag, 0, "[3,end]")]
        [TestCase(nameof(singleLyricWithNoText), MovingTimeTagCaretMode.None, NOT_EXIST, NOT_EXIST_TAG)]
        [TestCase(nameof(twoLyricsWithText), MovingTimeTagCaretMode.None, 1, "[2,end]")]
        [TestCase(nameof(threeLyricsWithSpacing), MovingTimeTagCaretMode.None, 2, "[2,end]")]
        public void TestMoveToLast(string sourceName, MovingTimeTagCaretMode mode, int lyricIndex, string textTag)
        {
            var lyrics = GetLyricsByMethodName(sourceName);
            var caretPosition = createTimeTagIndexCaretPosition(lyrics, lyricIndex, textTag);

            // Check last position
            TestMoveToLast(lyrics, caretPosition, algorithms => algorithms.Mode = mode);
        }

        [TestCase(nameof(singleLyric), MovingTimeTagCaretMode.None, 0, "[0,start]")]
        [TestCase(nameof(singleLyric), MovingTimeTagCaretMode.OnlyStartTag, 0, "[0,start]")]
        [TestCase(nameof(singleLyric), MovingTimeTagCaretMode.OnlyEndTag, 0, "[0,end]")]
        [TestCase(nameof(singleLyricWithNoText), MovingTimeTagCaretMode.None, 0, NOT_EXIST_TAG)]
        public void TestMoveToTarget(string sourceName, MovingTimeTagCaretMode mode, int lyricIndex, string textTag)
        {
            var lyrics = GetLyricsByMethodName(sourceName);
            var lyric = lyrics[lyricIndex];
            var caretPosition = createTimeTagIndexCaretPosition(lyrics, lyricIndex, textTag);

            // Check move to target position.
            TestMoveToTarget(lyrics, lyric, caretPosition, algorithms => algorithms.Mode = mode);
        }

        protected override void AssertEqual(TimeTagIndexCaretPosition compare, TimeTagIndexCaretPosition actual)
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

        private static TimeTagIndexCaretPosition createTimeTagIndexCaretPosition(Lyric[] lyrics, int lyricIndex, string textIndexText)
        {
            if (lyricIndex == NOT_EXIST)
                return null;

            var lyric = lyrics.ElementAtOrDefault(lyricIndex);
            var textTag = TestCaseTagHelper.ParseTextIndex(textIndexText);
            return new TimeTagIndexCaretPosition(lyric, textTag);
        }

        #region source

        private static Lyric[] singleLyric => new[]
        {
            new Lyric
            {
                Text = "カラオケ"
            }
        };

        private static Lyric[] singleLyricWithNoText => new Lyric[]
        {
            new()
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
