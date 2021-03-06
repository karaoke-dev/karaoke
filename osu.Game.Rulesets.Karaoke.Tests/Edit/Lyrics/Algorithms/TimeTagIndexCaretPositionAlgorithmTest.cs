// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Linq;
using NUnit.Framework;
using osu.Game.Rulesets.Karaoke.Edit.Lyrics.Algorithms;
using osu.Game.Rulesets.Karaoke.Edit.Lyrics.CaretPosition;
using osu.Game.Rulesets.Karaoke.Objects;
using osu.Game.Rulesets.Karaoke.Tests.Helper;

namespace osu.Game.Rulesets.Karaoke.Tests.Edit.Lyrics.Algorithms
{
    [TestFixture]
    public class TimeTagIndexCaretPositionAlgorithmTest : BaseCaretPositionAlgorithmTest<TimeTagIndexCaretPositionAlgorithm, TimeTagIndexCaretPosition>
    {
        protected const string NOT_EXIST_TAG = null;

        [TestCase(nameof(singleLyric), 0, "[0,start]", true)]
        [TestCase(nameof(singleLyric), 0, "[3,end]", true)]
        [TestCase(nameof(singleLyric), 0, "[4,start]", false)]
        [TestCase(nameof(singleLyric), 0, "[-1,end]", false)]
        [TestCase(nameof(singleLyricWithNoText), 0, "[0,start]", false)] // it's not movable if no text
        [TestCase(nameof(singleLyricWithNoText), 0, "[0,end]", false)]
        public void TestPositionMovable(string sourceName, int lyricIndex, string timeTag, bool movable)
        {
            var lyrics = GetLyricsByMethodName(sourceName);
            var caretPosition = CreateTimeTagIndexCaretPosition(lyrics, lyricIndex, timeTag);

            // Check is movable
            TestPositionMovable(lyrics, caretPosition, movable);
        }

        [TestCase(nameof(singleLyric), 0, "[0,start]", NOT_EXIST, NOT_EXIST_TAG)] // cannot move up if at top index.
        [TestCase(nameof(twoLyricsWithText), 1, "[0,start]", 0, "[0,start]")]
        [TestCase(nameof(threeLyricsWithSpacing), 2, "[0,start]", 0, "[0,start]")]
        [TestCase(nameof(threeLyricsWithSpacing), 2, "[2,start]", 0, "[2,start]")]
        public void TestMoveUp(string sourceName, int lyricIndex, string textTag, int newLyricIndex, string newTextTag)
        {
            var lyrics = GetLyricsByMethodName(sourceName);
            var caretPosition = CreateTimeTagIndexCaretPosition(lyrics, lyricIndex, textTag);
            var newCaretPosition = CreateTimeTagIndexCaretPosition(lyrics, newLyricIndex, newTextTag);

            // Check is movable
            TestMoveUp(lyrics, caretPosition, newCaretPosition);
        }

        [TestCase(nameof(singleLyric), 0, "[0,start]", NOT_EXIST, NOT_EXIST_TAG)] // cannot move down if at bottom index.
        [TestCase(nameof(twoLyricsWithText), 0, "[0,start]", 1, "[0,start]")]
        [TestCase(nameof(threeLyricsWithSpacing), 0, "[0,start]", 2, "[0,start]")]
        [TestCase(nameof(threeLyricsWithSpacing), 0, "[3,start]", 2, "[2,start]")]
        [TestCase(nameof(threeLyricsWithSpacing), 0, "[3,end]", 2, "[2,end]")]
        public void TestMoveDown(string sourceName, int lyricIndex, string textTag, int newLyricIndex, string newTextTag)
        {
            var lyrics = GetLyricsByMethodName(sourceName);
            var caretPosition = CreateTimeTagIndexCaretPosition(lyrics, lyricIndex, textTag);
            var newCaretPosition = CreateTimeTagIndexCaretPosition(lyrics, newLyricIndex, newTextTag);

            // Check is movable
            TestMoveDown(lyrics, caretPosition, newCaretPosition);
        }

        [TestCase(nameof(singleLyric), 0, "[0,start]", NOT_EXIST, NOT_EXIST_TAG)]
        [TestCase(nameof(twoLyricsWithText), 1, "[0,start]", 0, "[3,end]")]
        [TestCase(nameof(threeLyricsWithSpacing), 2, "[0,start]", 0, "[3,end]")]
        [TestCase(nameof(threeLyricsWithSpacing), 2, "[2,end]", 2, "[2,start]")]
        public void TestMoveLeft(string sourceName, int lyricIndex, string textTag, int newLyricIndex, string newTextTag)
        {
            var lyrics = GetLyricsByMethodName(sourceName);
            var caretPosition = CreateTimeTagIndexCaretPosition(lyrics, lyricIndex, textTag);
            var newCaretPosition = CreateTimeTagIndexCaretPosition(lyrics, newLyricIndex, newTextTag);

            // Check is movable
            TestMoveLeft(lyrics, caretPosition, newCaretPosition);
        }

        [TestCase(nameof(singleLyric), 0, "[3,end]", NOT_EXIST, NOT_EXIST_TAG)]
        [TestCase(nameof(twoLyricsWithText), 0, "[3,end]", 1, "[0,start]")]
        [TestCase(nameof(threeLyricsWithSpacing), 0, "[3,end]", 2, "[0,start]")]
        [TestCase(nameof(threeLyricsWithSpacing), 0, "[2,end]", 0, "[3,start]")]
        public void TestMoveRight(string sourceName, int lyricIndex, string textTag, int newLyricIndex, string newTextTag)
        {
            var lyrics = GetLyricsByMethodName(sourceName);
            var caretPosition = CreateTimeTagIndexCaretPosition(lyrics, lyricIndex, textTag);
            var newCaretPosition = CreateTimeTagIndexCaretPosition(lyrics, newLyricIndex, newTextTag);

            // Check is movable
            TestMoveRight(lyrics, caretPosition, newCaretPosition);
        }

        [TestCase(nameof(singleLyric), 0, "[0,start]")]
        [TestCase(nameof(singleLyricWithNoText), NOT_EXIST, NOT_EXIST_TAG)]
        [TestCase(nameof(twoLyricsWithText), 0, "[0,start]")]
        [TestCase(nameof(threeLyricsWithSpacing), 0, "[0,start]")]
        public void TestMoveToFirst(string sourceName, int lyricIndex, string textTag)
        {
            var lyrics = GetLyricsByMethodName(sourceName);
            var caretPosition = CreateTimeTagIndexCaretPosition(lyrics, lyricIndex, textTag);

            // Check first position
            TestMoveToFirst(lyrics, caretPosition);
        }

        [TestCase(nameof(singleLyric), 0, "[3,end]")]
        [TestCase(nameof(singleLyricWithNoText), NOT_EXIST, NOT_EXIST_TAG)]
        [TestCase(nameof(twoLyricsWithText), 1, "[2,end]")]
        [TestCase(nameof(threeLyricsWithSpacing), 2, "[2,end]")]
        public void TestMoveToLast(string sourceName, int lyricIndex, string textTag)
        {
            var lyrics = GetLyricsByMethodName(sourceName);
            var caretPosition = CreateTimeTagIndexCaretPosition(lyrics, lyricIndex, textTag);

            // Check last position
            TestMoveToLast(lyrics, caretPosition);
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

        protected TimeTagIndexCaretPosition CreateTimeTagIndexCaretPosition(Lyric[] lyrics, int lyricIndex, string textIndexText)
        {
            if (lyricIndex == NOT_EXIST)
                return null;

            var lyric = lyrics.ElementAtOrDefault(lyricIndex);
            var textTag = TestCaseTagHelper.ParseTextIndex(textIndexText);
            return new TimeTagIndexCaretPosition(lyric, textTag);
        }

        #region source

        private Lyric[] singleLyric => new[]
        {
            new Lyric
            {
                Text = "カラオケ"
            }
        };

        private Lyric[] singleLyricWithNoText => new Lyric[]
        {
            new()
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
