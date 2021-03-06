// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Linq;
using NUnit.Framework;
using osu.Game.Rulesets.Karaoke.Edit.Lyrics;
using osu.Game.Rulesets.Karaoke.Edit.Lyrics.Algorithms;
using osu.Game.Rulesets.Karaoke.Edit.Lyrics.CaretPosition;
using osu.Game.Rulesets.Karaoke.Objects;
using osu.Game.Rulesets.Karaoke.Tests.Helper;

namespace osu.Game.Rulesets.Karaoke.Tests.Edit.Lyrics.Algorithms
{
    [TestFixture]
    public class TimeTagCaretPositionAlgorithmTest : BaseCaretPositionAlgorithmTest<TimeTagCaretPositionAlgorithm, TimeTagCaretPosition>
    {
        protected const int NOT_EXIST_TAG = -1;

        [TestCase(nameof(singleLyric), RecordingMovingCaretMode.None, 0, 0, true)]
        [TestCase(nameof(singleLyric), RecordingMovingCaretMode.OnlyStartTag, 0, 0, true)]
        [TestCase(nameof(singleLyric), RecordingMovingCaretMode.OnlyEndTag, 0, 0, false)]
        [TestCase(nameof(singleLyricWithoutTimeTag), RecordingMovingCaretMode.None, 0, NOT_EXIST_TAG, false)]
        [TestCase(nameof(singleLyricWithoutTimeTag), RecordingMovingCaretMode.OnlyStartTag, 0, NOT_EXIST_TAG, false)]
        [TestCase(nameof(singleLyricWithoutTimeTag), RecordingMovingCaretMode.OnlyEndTag, 0, NOT_EXIST_TAG, false)]
        [TestCase(nameof(singleLyricWithNoText), RecordingMovingCaretMode.None, 0, NOT_EXIST_TAG, false)]
        [TestCase(nameof(singleLyricWithNoText), RecordingMovingCaretMode.OnlyStartTag, 0, NOT_EXIST_TAG, false)]
        [TestCase(nameof(singleLyricWithNoText), RecordingMovingCaretMode.OnlyEndTag, 0, NOT_EXIST_TAG, false)]
        public void TestPositionMovable(string sourceName, RecordingMovingCaretMode mode, int lyricIndex, int timeTagIndex, bool movable)
        {
            var lyrics = GetLyricsByMethodName(sourceName);
            var caretPosition = CreateTimeTagCaretPosition(lyrics, lyricIndex, timeTagIndex);

            // Check is movable
            TestPositionMovable(lyrics, caretPosition, movable, algorithms => algorithms.Mode = mode);
        }

        [TestCase(nameof(singleLyric), RecordingMovingCaretMode.None, 0, 0, NOT_EXIST, NOT_EXIST_TAG)]
        [TestCase(nameof(twoLyricsWithText), RecordingMovingCaretMode.None, 1, 0, 0, 0)]
        [TestCase(nameof(twoLyricsWithText), RecordingMovingCaretMode.None, 1, 3, 0, 3)]
        [TestCase(nameof(twoLyricsWithText), RecordingMovingCaretMode.OnlyStartTag, 1, 3, 0, 3)]
        [TestCase(nameof(twoLyricsWithText), RecordingMovingCaretMode.OnlyEndTag, 1, 3, 0, 4)] // todo : Not really sure the behavior in here
        [TestCase(nameof(threeLyricsWithSpacing), RecordingMovingCaretMode.None, 2, 0, 0, 0)]
        [TestCase(nameof(threeLyricsWithSpacing), RecordingMovingCaretMode.None, 2, 3, 0, 3)]
        [TestCase(nameof(threeLyricsWithSpacing), RecordingMovingCaretMode.OnlyStartTag, 2, 3, 0, 3)]
        [TestCase(nameof(threeLyricsWithSpacing), RecordingMovingCaretMode.OnlyEndTag, 2, 3, 0, 4)] // todo : Not really sure the behavior in here
        public void TestMoveUp(string sourceName, RecordingMovingCaretMode mode, int lyricIndex, int index, int newLyricIndex, int newIndex)
        {
            var lyrics = GetLyricsByMethodName(sourceName);
            var caretPosition = CreateTimeTagCaretPosition(lyrics, lyricIndex, index);
            var newCaretPosition = CreateTimeTagCaretPosition(lyrics, newLyricIndex, newIndex);

            // Check is movable
            TestMoveUp(lyrics, caretPosition, newCaretPosition, algorithms => algorithms.Mode = mode);
        }

        [TestCase(nameof(singleLyric), RecordingMovingCaretMode.None, 0, 0, NOT_EXIST, NOT_EXIST_TAG)]
        [TestCase(nameof(twoLyricsWithText), RecordingMovingCaretMode.None, 0, 0, 1, 0)]
        [TestCase(nameof(twoLyricsWithText), RecordingMovingCaretMode.None, 0, 2, 1, 2)]
        [TestCase(nameof(twoLyricsWithText), RecordingMovingCaretMode.OnlyStartTag, 0, 2, 1, 2)]
        [TestCase(nameof(twoLyricsWithText), RecordingMovingCaretMode.OnlyEndTag, 0, 2, 1, 3)] // todo : Not really sure the behavior in here
        [TestCase(nameof(threeLyricsWithSpacing), RecordingMovingCaretMode.None, 0, 0, 2, 0)]
        [TestCase(nameof(threeLyricsWithSpacing), RecordingMovingCaretMode.None, 0, 2, 2, 2)]
        [TestCase(nameof(threeLyricsWithSpacing), RecordingMovingCaretMode.OnlyStartTag, 0, 2, 2, 2)]
        [TestCase(nameof(threeLyricsWithSpacing), RecordingMovingCaretMode.OnlyEndTag, 0, 2, 2, 3)] // todo : Not really sure the behavior in here
        public void TestMoveDown(string sourceName, RecordingMovingCaretMode mode, int lyricIndex, int index, int newLyricIndex, int newIndex)
        {
            var lyrics = GetLyricsByMethodName(sourceName);
            var caretPosition = CreateTimeTagCaretPosition(lyrics, lyricIndex, index);
            var newCaretPosition = CreateTimeTagCaretPosition(lyrics, newLyricIndex, newIndex);

            // Check is movable
            TestMoveDown(lyrics, caretPosition, newCaretPosition, algorithms => algorithms.Mode = mode);
        }

        [TestCase(nameof(singleLyric), RecordingMovingCaretMode.None, 0, 0, NOT_EXIST, NOT_EXIST_TAG)]
        [TestCase(nameof(twoLyricsWithText), RecordingMovingCaretMode.None, 1, 0, 0, 4)]
        [TestCase(nameof(twoLyricsWithText), RecordingMovingCaretMode.OnlyStartTag, 1, 0, 0, 3)]
        [TestCase(nameof(twoLyricsWithText), RecordingMovingCaretMode.OnlyEndTag, 1, 0, 0, 4)]
        [TestCase(nameof(threeLyricsWithSpacing), RecordingMovingCaretMode.None, 2, 0, 0, 4)]
        [TestCase(nameof(threeLyricsWithSpacing), RecordingMovingCaretMode.OnlyStartTag, 2, 0, 0, 3)]
        [TestCase(nameof(threeLyricsWithSpacing), RecordingMovingCaretMode.OnlyEndTag, 2, 0, 0, 4)]
        public void TestMoveLeft(string sourceName, RecordingMovingCaretMode mode, int lyricIndex, int index, int newLyricIndex, int newIndex)
        {
            var lyrics = GetLyricsByMethodName(sourceName);
            var caretPosition = CreateTimeTagCaretPosition(lyrics, lyricIndex, index);
            var newCaretPosition = CreateTimeTagCaretPosition(lyrics, newLyricIndex, newIndex);

            // Check is movable
            TestMoveLeft(lyrics, caretPosition, newCaretPosition, algorithms => algorithms.Mode = mode);
        }

        [TestCase(nameof(singleLyric), RecordingMovingCaretMode.None, 0, 4, NOT_EXIST, NOT_EXIST_TAG)]
        [TestCase(nameof(twoLyricsWithText), RecordingMovingCaretMode.None, 0, 4, 1, 0)]
        [TestCase(nameof(twoLyricsWithText), RecordingMovingCaretMode.OnlyStartTag, 0, 4, 1, 0)]
        [TestCase(nameof(twoLyricsWithText), RecordingMovingCaretMode.OnlyEndTag, 0, 4, 1, 3)]
        [TestCase(nameof(threeLyricsWithSpacing), RecordingMovingCaretMode.None, 0, 4, 2, 0)]
        [TestCase(nameof(threeLyricsWithSpacing), RecordingMovingCaretMode.OnlyStartTag, 0, 4, 2, 0)]
        [TestCase(nameof(threeLyricsWithSpacing), RecordingMovingCaretMode.OnlyEndTag, 0, 4, 2, 3)]
        public void TestMoveRight(string sourceName, RecordingMovingCaretMode mode, int lyricIndex, int index, int newLyricIndex, int newIndex)
        {
            var lyrics = GetLyricsByMethodName(sourceName);
            var caretPosition = CreateTimeTagCaretPosition(lyrics, lyricIndex, index);
            var newCaretPosition = CreateTimeTagCaretPosition(lyrics, newLyricIndex, newIndex);

            // Check is movable
            TestMoveRight(lyrics, caretPosition, newCaretPosition, algorithms => algorithms.Mode = mode);
        }

        [TestCase(nameof(singleLyric), RecordingMovingCaretMode.None, 0, 0)]
        [TestCase(nameof(singleLyric), RecordingMovingCaretMode.OnlyStartTag, 0, 0)]
        [TestCase(nameof(singleLyric), RecordingMovingCaretMode.OnlyEndTag, 0, 4)]
        [TestCase(nameof(singleLyricWithoutTimeTag), RecordingMovingCaretMode.None, NOT_EXIST, NOT_EXIST_TAG)]
        [TestCase(nameof(singleLyricWithoutTimeTag), RecordingMovingCaretMode.OnlyStartTag, NOT_EXIST, NOT_EXIST_TAG)]
        [TestCase(nameof(singleLyricWithoutTimeTag), RecordingMovingCaretMode.OnlyEndTag, NOT_EXIST, NOT_EXIST_TAG)]
        [TestCase(nameof(twoLyricsWithText), RecordingMovingCaretMode.None, 0, 0)]
        [TestCase(nameof(twoLyricsWithText), RecordingMovingCaretMode.OnlyStartTag, 0, 0)]
        [TestCase(nameof(twoLyricsWithText), RecordingMovingCaretMode.OnlyEndTag, 0, 4)]
        [TestCase(nameof(threeLyricsWithSpacing), RecordingMovingCaretMode.None, 0, 0)]
        [TestCase(nameof(threeLyricsWithSpacing), RecordingMovingCaretMode.OnlyStartTag, 0, 0)]
        [TestCase(nameof(threeLyricsWithSpacing), RecordingMovingCaretMode.OnlyEndTag, 0, 4)]
        public void TestMoveToFirst(string sourceName, RecordingMovingCaretMode mode, int lyricIndex, int index)
        {
            var lyrics = GetLyricsByMethodName(sourceName);
            var caretPosition = CreateTimeTagCaretPosition(lyrics, lyricIndex, index);

            // Check is movable
            TestMoveToFirst(lyrics, caretPosition, algorithms => algorithms.Mode = mode);
        }

        [TestCase(nameof(singleLyric), RecordingMovingCaretMode.None, 0, 4)]
        [TestCase(nameof(singleLyric), RecordingMovingCaretMode.OnlyStartTag, 0, 3)]
        [TestCase(nameof(singleLyric), RecordingMovingCaretMode.OnlyEndTag, 0, 4)]
        [TestCase(nameof(singleLyricWithoutTimeTag), RecordingMovingCaretMode.None, NOT_EXIST, NOT_EXIST_TAG)]
        [TestCase(nameof(singleLyricWithoutTimeTag), RecordingMovingCaretMode.OnlyStartTag, NOT_EXIST, NOT_EXIST_TAG)]
        [TestCase(nameof(singleLyricWithoutTimeTag), RecordingMovingCaretMode.OnlyEndTag, NOT_EXIST, NOT_EXIST_TAG)]
        [TestCase(nameof(twoLyricsWithText), RecordingMovingCaretMode.None, 1, 3)]
        [TestCase(nameof(twoLyricsWithText), RecordingMovingCaretMode.OnlyStartTag, 1, 2)]
        [TestCase(nameof(twoLyricsWithText), RecordingMovingCaretMode.OnlyEndTag, 1, 3)]
        [TestCase(nameof(threeLyricsWithSpacing), RecordingMovingCaretMode.None, 2, 3)]
        [TestCase(nameof(threeLyricsWithSpacing), RecordingMovingCaretMode.OnlyStartTag, 2, 2)]
        [TestCase(nameof(threeLyricsWithSpacing), RecordingMovingCaretMode.OnlyEndTag, 2, 3)]
        public void TestMoveToLast(string sourceName, RecordingMovingCaretMode mode, int lyricIndex, int index)
        {
            var lyrics = GetLyricsByMethodName(sourceName);
            var caretPosition = CreateTimeTagCaretPosition(lyrics, lyricIndex, index);

            // Check is movable
            TestMoveToLast(lyrics, caretPosition, algorithms => algorithms.Mode = mode);
        }

        protected override void AssertEqual(TimeTagCaretPosition compare, TimeTagCaretPosition actual)
        {
            if (compare == null)
            {
                Assert.IsNull(actual);
            }
            else
            {
                Assert.AreEqual(compare.Lyric, actual.Lyric);
                Assert.AreEqual(compare.TimeTag, actual.TimeTag);
            }
        }

        protected TimeTagCaretPosition CreateTimeTagCaretPosition(Lyric[] lyrics, int lyricIndex, int timeTagIndex)
        {
            if (lyricIndex == NOT_EXIST)
                return null;

            var lyric = lyrics.ElementAtOrDefault(lyricIndex);
            var timeTag = lyric?.TimeTags?.ElementAtOrDefault(timeTagIndex);
            return new TimeTagCaretPosition(lyric, timeTag);
        }

        #region source

        private Lyric[] singleLyric => new[]
        {
            new Lyric
            {
                Text = "カラオケ",
                TimeTags = TestCaseTagHelper.ParseTimeTags(new[]
                {
                    "[0,start]:1000",
                    "[1,start]:2000",
                    "[2,start]:3000",
                    "[3,start]:4000",
                    "[3,end]:5000"
                })
            }
        };

        private Lyric[] singleLyricWithoutTimeTag => new[]
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
                Text = "カラオケ",
                TimeTags = TestCaseTagHelper.ParseTimeTags(new[]
                {
                    "[0,start]:1000",
                    "[1,start]:2000",
                    "[2,start]:3000",
                    "[3,start]:4000",
                    "[3,end]:5000"
                })
            },
            new Lyric
            {
                Text = "大好き",
                TimeTags = TestCaseTagHelper.ParseTimeTags(new[]
                {
                    "[0,start]:1000",
                    "[1,start]:2000",
                    "[2,start]:3000",
                    "[2,end]:5000"
                })
            }
        };

        private Lyric[] threeLyricsWithSpacing => new[]
        {
            new Lyric
            {
                Text = "カラオケ",
                TimeTags = TestCaseTagHelper.ParseTimeTags(new[]
                {
                    "[0,start]:1000",
                    "[1,start]:2000",
                    "[2,start]:3000",
                    "[3,start]:4000",
                    "[3,end]:5000"
                })
            },
            new Lyric(),
            new Lyric
            {
                Text = "大好き",
                TimeTags = TestCaseTagHelper.ParseTimeTags(new[]
                {
                    "[0,start]:1000",
                    "[1,start]:2000",
                    "[2,start]:3000",
                    "[2,end]:5000"
                })
            }
        };

        #endregion
    }
}
