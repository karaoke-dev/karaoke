// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using osu.Framework.Graphics.Sprites;
using osu.Game.Rulesets.Karaoke.Edit.Lyrics.CaretPosition;
using osu.Game.Rulesets.Karaoke.Edit.Lyrics.CaretPosition.Algorithms;
using osu.Game.Rulesets.Karaoke.Objects;
using osu.Game.Rulesets.Karaoke.Tests.Helper;

namespace osu.Game.Rulesets.Karaoke.Tests.Editor.Lyrics.CaretPosition.Algorithms
{
    [TestFixture]
    public class TimeTagCaretPositionAlgorithmTest : BaseIndexCaretPositionAlgorithmTest<TimeTagCaretPositionAlgorithm, TimeTagCaretPosition>
    {
        private const int not_exist_tag = -1;

        #region Lyric

        [TestCase(nameof(singleLyric), MovingTimeTagCaretMode.None, 0, 0, true)]
        [TestCase(nameof(singleLyric), MovingTimeTagCaretMode.OnlyStartTag, 0, 0, true)]
        [TestCase(nameof(singleLyric), MovingTimeTagCaretMode.OnlyEndTag, 0, 0, false)]
        [TestCase(nameof(singleLyricWithoutTimeTag), MovingTimeTagCaretMode.None, 0, not_exist_tag, false)]
        [TestCase(nameof(singleLyricWithoutTimeTag), MovingTimeTagCaretMode.OnlyStartTag, 0, not_exist_tag, false)]
        [TestCase(nameof(singleLyricWithoutTimeTag), MovingTimeTagCaretMode.OnlyEndTag, 0, not_exist_tag, false)]
        [TestCase(nameof(singleLyricWithNoText), MovingTimeTagCaretMode.None, 0, not_exist_tag, false)]
        [TestCase(nameof(singleLyricWithNoText), MovingTimeTagCaretMode.OnlyStartTag, 0, not_exist_tag, false)]
        [TestCase(nameof(singleLyricWithNoText), MovingTimeTagCaretMode.OnlyEndTag, 0, not_exist_tag, false)]
        public void TestPositionMovable(string sourceName, MovingTimeTagCaretMode mode, int lyricIndex, int timeTagIndex, bool movable)
        {
            var lyrics = GetLyricsByMethodName(sourceName);
            var caret = createCaretPosition(lyrics, lyricIndex, timeTagIndex);

            // Check is movable
            TestPositionMovable(lyrics, caret, movable, algorithms => algorithms.Mode = mode);
        }

        [TestCase(nameof(singleLyric), MovingTimeTagCaretMode.None, 0, 0, null, null)]
        [TestCase(nameof(twoLyricsWithText), MovingTimeTagCaretMode.None, 1, 0, 0, 0)]
        [TestCase(nameof(twoLyricsWithText), MovingTimeTagCaretMode.None, 1, 3, 0, 3)]
        [TestCase(nameof(twoLyricsWithText), MovingTimeTagCaretMode.OnlyStartTag, 1, 3, 0, 3)]
        [TestCase(nameof(twoLyricsWithText), MovingTimeTagCaretMode.OnlyEndTag, 1, 3, 0, 4)]
        [TestCase(nameof(threeLyricsWithSpacing), MovingTimeTagCaretMode.None, 2, 0, 0, 0)]
        [TestCase(nameof(threeLyricsWithSpacing), MovingTimeTagCaretMode.None, 2, 3, 0, 3)]
        [TestCase(nameof(threeLyricsWithSpacing), MovingTimeTagCaretMode.OnlyStartTag, 2, 3, 0, 3)]
        [TestCase(nameof(threeLyricsWithSpacing), MovingTimeTagCaretMode.OnlyEndTag, 2, 3, 0, 4)]
        public void TestMoveToPreviousLyric(string sourceName, MovingTimeTagCaretMode mode, int lyricIndex, int index, int? expectedLyricIndex, int? expectedTimeTagIndex)
        {
            var lyrics = GetLyricsByMethodName(sourceName);
            var caret = createCaretPosition(lyrics, lyricIndex, index);
            var expected = createExpectedCaretPosition(lyrics, expectedLyricIndex, expectedTimeTagIndex);

            // Check is movable
            TestMoveToPreviousLyric(lyrics, caret, expected, algorithms => algorithms.Mode = mode);
        }

        [TestCase(nameof(singleLyric), MovingTimeTagCaretMode.None, 0, 0, null, null)]
        [TestCase(nameof(twoLyricsWithText), MovingTimeTagCaretMode.None, 0, 0, 1, 0)]
        [TestCase(nameof(twoLyricsWithText), MovingTimeTagCaretMode.None, 0, 2, 1, 2)]
        [TestCase(nameof(twoLyricsWithText), MovingTimeTagCaretMode.OnlyStartTag, 0, 2, 1, 2)]
        [TestCase(nameof(twoLyricsWithText), MovingTimeTagCaretMode.OnlyEndTag, 0, 2, 1, 3)]
        [TestCase(nameof(threeLyricsWithSpacing), MovingTimeTagCaretMode.None, 0, 0, 2, 0)]
        [TestCase(nameof(threeLyricsWithSpacing), MovingTimeTagCaretMode.None, 0, 2, 2, 2)]
        [TestCase(nameof(threeLyricsWithSpacing), MovingTimeTagCaretMode.OnlyStartTag, 0, 2, 2, 2)]
        [TestCase(nameof(threeLyricsWithSpacing), MovingTimeTagCaretMode.OnlyEndTag, 0, 2, 2, 3)]
        public void TestMoveToNextLyric(string sourceName, MovingTimeTagCaretMode mode, int lyricIndex, int index, int? expectedLyricIndex, int? expectedTimeTagIndex)
        {
            var lyrics = GetLyricsByMethodName(sourceName);
            var caret = createCaretPosition(lyrics, lyricIndex, index);
            var expected = createExpectedCaretPosition(lyrics, expectedLyricIndex, expectedTimeTagIndex);

            // Check is movable
            TestMoveToNextLyric(lyrics, caret, expected, algorithms => algorithms.Mode = mode);
        }

        [TestCase(nameof(singleLyric), MovingTimeTagCaretMode.None, 0, 0)]
        [TestCase(nameof(singleLyric), MovingTimeTagCaretMode.OnlyStartTag, 0, 0)]
        [TestCase(nameof(singleLyric), MovingTimeTagCaretMode.OnlyEndTag, 0, 4)]
        [TestCase(nameof(singleLyricWithoutTimeTag), MovingTimeTagCaretMode.None, null, null)]
        [TestCase(nameof(singleLyricWithoutTimeTag), MovingTimeTagCaretMode.OnlyStartTag, null, null)]
        [TestCase(nameof(singleLyricWithoutTimeTag), MovingTimeTagCaretMode.OnlyEndTag, null, null)]
        [TestCase(nameof(twoLyricsWithText), MovingTimeTagCaretMode.None, 0, 0)]
        [TestCase(nameof(twoLyricsWithText), MovingTimeTagCaretMode.OnlyStartTag, 0, 0)]
        [TestCase(nameof(twoLyricsWithText), MovingTimeTagCaretMode.OnlyEndTag, 0, 4)]
        [TestCase(nameof(threeLyricsWithSpacing), MovingTimeTagCaretMode.None, 0, 0)]
        [TestCase(nameof(threeLyricsWithSpacing), MovingTimeTagCaretMode.OnlyStartTag, 0, 0)]
        [TestCase(nameof(threeLyricsWithSpacing), MovingTimeTagCaretMode.OnlyEndTag, 0, 4)]
        public void TestMoveToFirstLyric(string sourceName, MovingTimeTagCaretMode mode, int? expectedLyricIndex, int? expectedTimeTagIndex)
        {
            var lyrics = GetLyricsByMethodName(sourceName);
            var expected = createExpectedCaretPosition(lyrics, expectedLyricIndex, expectedTimeTagIndex);

            // Check is movable
            TestMoveToFirstLyric(lyrics, expected, algorithms => algorithms.Mode = mode);
        }

        [TestCase(nameof(singleLyric), MovingTimeTagCaretMode.None, 0, 4)]
        [TestCase(nameof(singleLyric), MovingTimeTagCaretMode.OnlyStartTag, 0, 3)]
        [TestCase(nameof(singleLyric), MovingTimeTagCaretMode.OnlyEndTag, 0, 4)]
        [TestCase(nameof(singleLyricWithoutTimeTag), MovingTimeTagCaretMode.None, null, null)]
        [TestCase(nameof(singleLyricWithoutTimeTag), MovingTimeTagCaretMode.OnlyStartTag, null, null)]
        [TestCase(nameof(singleLyricWithoutTimeTag), MovingTimeTagCaretMode.OnlyEndTag, null, null)]
        [TestCase(nameof(twoLyricsWithText), MovingTimeTagCaretMode.None, 1, 3)]
        [TestCase(nameof(twoLyricsWithText), MovingTimeTagCaretMode.OnlyStartTag, 1, 2)]
        [TestCase(nameof(twoLyricsWithText), MovingTimeTagCaretMode.OnlyEndTag, 1, 3)]
        [TestCase(nameof(threeLyricsWithSpacing), MovingTimeTagCaretMode.None, 2, 3)]
        [TestCase(nameof(threeLyricsWithSpacing), MovingTimeTagCaretMode.OnlyStartTag, 2, 2)]
        [TestCase(nameof(threeLyricsWithSpacing), MovingTimeTagCaretMode.OnlyEndTag, 2, 3)]
        public void TestMoveToLastLyric(string sourceName, MovingTimeTagCaretMode mode, int? expectedLyricIndex, int? expectedTimeTagIndex)
        {
            var lyrics = GetLyricsByMethodName(sourceName);
            var expected = createExpectedCaretPosition(lyrics, expectedLyricIndex, expectedTimeTagIndex);

            // Check is movable
            TestMoveToLastLyric(lyrics, expected, algorithms => algorithms.Mode = mode);
        }

        [TestCase(nameof(singleLyric), MovingTimeTagCaretMode.None, 0, 0, 0)]
        [TestCase(nameof(singleLyric), MovingTimeTagCaretMode.OnlyStartTag, 0, 0, 0)]
        [TestCase(nameof(singleLyric), MovingTimeTagCaretMode.OnlyEndTag, 0, 0, 4)]
        [TestCase(nameof(singleLyricWithoutTimeTag), MovingTimeTagCaretMode.None, 0, null, null)] // should not hover to the lyric if contains no time-tag in the lyric.
        [TestCase(nameof(singleLyricWithoutTimeTag), MovingTimeTagCaretMode.OnlyStartTag, 0, null, null)]
        [TestCase(nameof(singleLyricWithoutTimeTag), MovingTimeTagCaretMode.OnlyEndTag, 0, null, null)]
        [TestCase(nameof(singleLyricWithNoText), MovingTimeTagCaretMode.None, 0, null, null)] // should not hover to the lyric if contains no text and no time-tag in the lyric
        public void TestMoveToTargetLyric(string sourceName, MovingTimeTagCaretMode mode, int lyricIndex, int? expectedLyricIndex, int? expectedTimeTagIndex)
        {
            var lyrics = GetLyricsByMethodName(sourceName);
            var lyric = lyrics[lyricIndex];
            var expected = createExpectedCaretPosition(lyrics, expectedLyricIndex, expectedTimeTagIndex);

            // Check move to target position.
            TestMoveToTargetLyric(lyrics, lyric, expected, algorithms => algorithms.Mode = mode);
        }

        #endregion

        #region Lyric index

        [TestCase(nameof(singleLyric), MovingTimeTagCaretMode.None, 0, 0, null, null)]
        [TestCase(nameof(singleLyric), MovingTimeTagCaretMode.None, 0, 4, 0, 3)]
        [TestCase(nameof(singleLyric), MovingTimeTagCaretMode.OnlyStartTag, 0, 4, 0, 3)]
        [TestCase(nameof(singleLyric), MovingTimeTagCaretMode.OnlyEndTag, 0, 4, null, null)]
        public void TestMoveToPreviousIndex(string sourceName, MovingTimeTagCaretMode mode, int lyricIndex, int index, int? expectedLyricIndex, int? expectedTimeTagIndex)
        {
            var lyrics = GetLyricsByMethodName(sourceName);
            var caret = createCaretPosition(lyrics, lyricIndex, index);
            var expected = createExpectedCaretPosition(lyrics, expectedLyricIndex, expectedTimeTagIndex);

            // Check is movable
            TestMoveToPreviousIndex(lyrics, caret, expected, algorithms => algorithms.Mode = mode);
        }

        [TestCase(nameof(singleLyric), MovingTimeTagCaretMode.None, 0, 4, null, null)]
        [TestCase(nameof(singleLyric), MovingTimeTagCaretMode.None, 0, 0, 0, 1)]
        [TestCase(nameof(singleLyric), MovingTimeTagCaretMode.OnlyStartTag, 0, 0, 0, 1)]
        [TestCase(nameof(singleLyric), MovingTimeTagCaretMode.OnlyEndTag, 0, 0, 0, 4)]
        public void TestMoveToNextIndex(string sourceName, MovingTimeTagCaretMode mode, int lyricIndex, int index, int? expectedLyricIndex, int? expectedTimeTagIndex)
        {
            var lyrics = GetLyricsByMethodName(sourceName);
            var caret = createCaretPosition(lyrics, lyricIndex, index);
            var expected = createExpectedCaretPosition(lyrics, expectedLyricIndex, expectedTimeTagIndex);

            // Check is movable
            TestMoveToNextIndex(lyrics, caret, expected, algorithms => algorithms.Mode = mode);
        }

        [TestCase(nameof(singleLyric), MovingTimeTagCaretMode.None, 0, 0, 0)]
        [TestCase(nameof(singleLyric), MovingTimeTagCaretMode.OnlyStartTag, 0, 0, 0)]
        [TestCase(nameof(singleLyric), MovingTimeTagCaretMode.OnlyEndTag, 0, 0, 4)]
        [TestCase(nameof(singleLyricWithNoText), MovingTimeTagCaretMode.None, 0, null, null)]
        [TestCase(nameof(singleLyricWithNoText), MovingTimeTagCaretMode.OnlyStartTag, 0, null, null)]
        [TestCase(nameof(singleLyricWithNoText), MovingTimeTagCaretMode.OnlyEndTag, 0, null, null)]
        public void TestMoveToFirstIndex(string sourceName, MovingTimeTagCaretMode mode, int lyricIndex, int? expectedLyricIndex, int? expectedTimeTagIndex)
        {
            var lyrics = GetLyricsByMethodName(sourceName);
            var lyric = lyrics[lyricIndex];
            var expected = createExpectedCaretPosition(lyrics, expectedLyricIndex, expectedTimeTagIndex);

            // Check is movable
            TestMoveToFirstIndex(lyrics, lyric, expected, algorithms => algorithms.Mode = mode);
        }

        [TestCase(nameof(singleLyric), MovingTimeTagCaretMode.None, 0, 0, 4)]
        [TestCase(nameof(singleLyric), MovingTimeTagCaretMode.OnlyStartTag, 0, 0, 3)]
        [TestCase(nameof(singleLyric), MovingTimeTagCaretMode.OnlyEndTag, 0, 0, 4)]
        [TestCase(nameof(singleLyricWithNoText), MovingTimeTagCaretMode.None, 0, null, null)]
        [TestCase(nameof(singleLyricWithNoText), MovingTimeTagCaretMode.OnlyStartTag, 0, null, null)]
        [TestCase(nameof(singleLyricWithNoText), MovingTimeTagCaretMode.OnlyEndTag, 0, null, null)]
        public void TestMoveToLastIndex(string sourceName, MovingTimeTagCaretMode mode, int lyricIndex, int? expectedLyricIndex, int? expectedTimeTagIndex)
        {
            var lyrics = GetLyricsByMethodName(sourceName);
            var lyric = lyrics[lyricIndex];
            var expected = createExpectedCaretPosition(lyrics, expectedLyricIndex, expectedTimeTagIndex);

            // Check is movable
            TestMoveToLastIndex(lyrics, lyric, expected, algorithms => algorithms.Mode = mode);
        }

        #endregion

        protected override void AssertEqual(TimeTagCaretPosition expected, TimeTagCaretPosition actual)
        {
            Assert.AreEqual(expected.Lyric, actual.Lyric);
            Assert.AreEqual(expected.TimeTag, actual.TimeTag);
        }

        private static TimeTagCaretPosition createCaretPosition(IEnumerable<Lyric> lyrics, int lyricIndex, int timeTagIndex)
        {
            var lyric = lyrics.ElementAtOrDefault(lyricIndex);
            var timeTag = timeTagIndex == not_exist_tag
                ? new TimeTag(new TextIndex(not_exist_tag))
                : lyric?.TimeTags.ElementAtOrDefault(timeTagIndex);

            if (lyric == null || timeTag == null)
                throw new ArgumentNullException();

            return new TimeTagCaretPosition(lyric, timeTag);
        }

        private static TimeTagCaretPosition? createExpectedCaretPosition(IEnumerable<Lyric> lyrics, int? lyricIndex, int? timeTagIndex)
        {
            if (lyricIndex == null || timeTagIndex == null)
                return null;

            return createCaretPosition(lyrics, lyricIndex.Value, timeTagIndex.Value);
        }

        #region source

        private static Lyric[] singleLyric => new[]
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

        private static Lyric[] singleLyricWithoutTimeTag => new[]
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

        private static Lyric[] threeLyricsWithSpacing => new[]
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
