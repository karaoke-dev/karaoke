// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using osu.Game.Rulesets.Karaoke.Edit.Lyrics.CaretPosition;
using osu.Game.Rulesets.Karaoke.Edit.Lyrics.CaretPosition.Algorithms;
using osu.Game.Rulesets.Karaoke.Objects;
using osu.Game.Rulesets.Karaoke.Tests.Helper;

namespace osu.Game.Rulesets.Karaoke.Tests.Editor.Lyrics.CaretPosition.Algorithms
{
    [TestFixture]
    public class TimeTagIndexCaretPositionAlgorithmTest : BaseCaretPositionAlgorithmTest<TimeTagIndexCaretPositionAlgorithm, TimeTagIndexCaretPosition>
    {
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
            var caret = createTimeTagIndexCaretPosition(lyrics, lyricIndex, timeTag);

            // Check is movable
            TestPositionMovable(lyrics, caret, movable, algorithms => algorithms.Mode = mode);
        }

        [TestCase(nameof(singleLyric), MovingTimeTagCaretMode.None, 0, "[0,start]", null, null)] // cannot move up if at top index.
        [TestCase(nameof(twoLyricsWithText), MovingTimeTagCaretMode.None, 1, "[0,start]", 0, "[0,start]")]
        [TestCase(nameof(twoLyricsWithText), MovingTimeTagCaretMode.OnlyStartTag, 1, "[0,start]", 0, "[0,start]")]
        [TestCase(nameof(twoLyricsWithText), MovingTimeTagCaretMode.OnlyEndTag, 1, "[0,start]", 0, "[0,end]")]
        [TestCase(nameof(threeLyricsWithSpacing), MovingTimeTagCaretMode.None, 2, "[0,start]", 0, "[0,start]")]
        [TestCase(nameof(threeLyricsWithSpacing), MovingTimeTagCaretMode.OnlyStartTag, 2, "[0,start]", 0, "[0,start]")]
        [TestCase(nameof(threeLyricsWithSpacing), MovingTimeTagCaretMode.OnlyEndTag, 2, "[0,start]", 0, "[0,end]")]
        [TestCase(nameof(threeLyricsWithSpacing), MovingTimeTagCaretMode.None, 2, "[2,end]", 0, "[2,end]")]
        [TestCase(nameof(threeLyricsWithSpacing), MovingTimeTagCaretMode.OnlyStartTag, 2, "[2,end]", 0, "[2,start]")]
        [TestCase(nameof(threeLyricsWithSpacing), MovingTimeTagCaretMode.OnlyEndTag, 2, "[2,end]", 0, "[2,end]")]
        public void TestMoveUp(string sourceName, MovingTimeTagCaretMode mode, int lyricIndex, string textTag, int? newLyricIndex, string? newTextTag)
        {
            var lyrics = GetLyricsByMethodName(sourceName);
            var caret = createTimeTagIndexCaretPosition(lyrics, lyricIndex, textTag);
            var expected = createExpectedTimeTagIndexCaretPosition(lyrics, newLyricIndex, newTextTag);

            // Check is movable
            TestMoveUp(lyrics, caret, expected, algorithms => algorithms.Mode = mode);
        }

        [TestCase(nameof(singleLyric), MovingTimeTagCaretMode.None, 0, "[0,start]", null, null)] // cannot move down if at bottom index.
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
        public void TestMoveDown(string sourceName, MovingTimeTagCaretMode mode, int lyricIndex, string textTag, int? newLyricIndex, string? newTextTag)
        {
            var lyrics = GetLyricsByMethodName(sourceName);
            var caret = createTimeTagIndexCaretPosition(lyrics, lyricIndex, textTag);
            var expected = createExpectedTimeTagIndexCaretPosition(lyrics, newLyricIndex, newTextTag);

            // Check is movable
            TestMoveDown(lyrics, caret, expected, algorithms => algorithms.Mode = mode);
        }

        [TestCase(nameof(singleLyric), MovingTimeTagCaretMode.None, 0, "[0,start]", null, null)]
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
        public void TestMoveLeft(string sourceName, MovingTimeTagCaretMode mode, int lyricIndex, string textTag, int? newLyricIndex, string? newTextTag)
        {
            var lyrics = GetLyricsByMethodName(sourceName);
            var caret = createTimeTagIndexCaretPosition(lyrics, lyricIndex, textTag);
            var expected = createExpectedTimeTagIndexCaretPosition(lyrics, newLyricIndex, newTextTag);

            // Check is movable
            TestMoveLeft(lyrics, caret, expected, algorithms => algorithms.Mode = mode);
        }

        [TestCase(nameof(singleLyric), MovingTimeTagCaretMode.None, 0, "[3,end]", null, null)]
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
        public void TestMoveRight(string sourceName, MovingTimeTagCaretMode mode, int lyricIndex, string textTag, int? newLyricIndex, string? newTextTag)
        {
            var lyrics = GetLyricsByMethodName(sourceName);
            var caret = createTimeTagIndexCaretPosition(lyrics, lyricIndex, textTag);
            var expected = createExpectedTimeTagIndexCaretPosition(lyrics, newLyricIndex, newTextTag);

            // Check is movable
            TestMoveRight(lyrics, caret, expected, algorithms => algorithms.Mode = mode);
        }

        [TestCase(nameof(singleLyric), MovingTimeTagCaretMode.None, 0, "[0,start]")]
        [TestCase(nameof(singleLyric), MovingTimeTagCaretMode.OnlyStartTag, 0, "[0,start]")]
        [TestCase(nameof(singleLyric), MovingTimeTagCaretMode.OnlyEndTag, 0, "[0,end]")]
        [TestCase(nameof(singleLyricWithNoText), MovingTimeTagCaretMode.None, null, null)]
        [TestCase(nameof(twoLyricsWithText), MovingTimeTagCaretMode.None, 0, "[0,start]")]
        [TestCase(nameof(threeLyricsWithSpacing), MovingTimeTagCaretMode.None, 0, "[0,start]")]
        public void TestMoveToFirst(string sourceName, MovingTimeTagCaretMode mode, int? lyricIndex, string? textTag)
        {
            var lyrics = GetLyricsByMethodName(sourceName);
            var expected = createExpectedTimeTagIndexCaretPosition(lyrics, lyricIndex, textTag);

            // Check first position
            TestMoveToFirst(lyrics, expected, algorithms => algorithms.Mode = mode);
        }

        [TestCase(nameof(singleLyric), MovingTimeTagCaretMode.None, 0, "[3,end]")]
        [TestCase(nameof(singleLyric), MovingTimeTagCaretMode.OnlyStartTag, 0, "[3,start]")]
        [TestCase(nameof(singleLyric), MovingTimeTagCaretMode.OnlyEndTag, 0, "[3,end]")]
        [TestCase(nameof(singleLyricWithNoText), MovingTimeTagCaretMode.None, null, null)]
        [TestCase(nameof(twoLyricsWithText), MovingTimeTagCaretMode.None, 1, "[2,end]")]
        [TestCase(nameof(threeLyricsWithSpacing), MovingTimeTagCaretMode.None, 2, "[2,end]")]
        public void TestMoveToLast(string sourceName, MovingTimeTagCaretMode mode, int? lyricIndex, string? textTag)
        {
            var lyrics = GetLyricsByMethodName(sourceName);
            var expected = createExpectedTimeTagIndexCaretPosition(lyrics, lyricIndex, textTag);

            // Check last position
            TestMoveToLast(lyrics, expected, algorithms => algorithms.Mode = mode);
        }

        [TestCase(nameof(singleLyric), MovingTimeTagCaretMode.None, 0, "[0,start]")]
        [TestCase(nameof(singleLyric), MovingTimeTagCaretMode.OnlyStartTag, 0, "[0,start]")]
        [TestCase(nameof(singleLyric), MovingTimeTagCaretMode.OnlyEndTag, 0, "[0,end]")]
        [TestCase(nameof(singleLyricWithNoText), MovingTimeTagCaretMode.None, 0, "[0,start]")]
        public void TestMoveToTarget(string sourceName, MovingTimeTagCaretMode mode, int lyricIndex, string? textTag)
        {
            var lyrics = GetLyricsByMethodName(sourceName);
            var lyric = lyrics[lyricIndex];
            var expected = createExpectedTimeTagIndexCaretPosition(lyrics, lyricIndex, textTag);

            // Check move to target position.
            TestMoveToTarget(lyrics, lyric, expected, algorithms => algorithms.Mode = mode);
        }

        protected override void AssertEqual(TimeTagIndexCaretPosition expected, TimeTagIndexCaretPosition actual)
        {
            Assert.AreEqual(expected.Lyric, actual.Lyric);
            Assert.AreEqual(expected.Index, actual.Index);
        }

        private static TimeTagIndexCaretPosition createTimeTagIndexCaretPosition(IEnumerable<Lyric> lyrics, int lyricIndex, string textIndexText)
        {
            var lyric = lyrics.ElementAtOrDefault(lyricIndex);
            if (lyric == null)
                throw new ArgumentNullException();

            var textTag = TestCaseTagHelper.ParseTextIndex(textIndexText);
            return new TimeTagIndexCaretPosition(lyric, textTag);
        }

        private static TimeTagIndexCaretPosition? createExpectedTimeTagIndexCaretPosition(IEnumerable<Lyric> lyrics, int? lyricIndex, string? textIndexText)
        {
            if (lyricIndex == null || textIndexText == null)
                return null;

            return createTimeTagIndexCaretPosition(lyrics, lyricIndex.Value, textIndexText);
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
