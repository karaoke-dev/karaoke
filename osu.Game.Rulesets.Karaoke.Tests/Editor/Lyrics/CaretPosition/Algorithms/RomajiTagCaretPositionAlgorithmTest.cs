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
    public class RomajiTagCaretPositionAlgorithmTest : BaseIndexCaretPositionAlgorithmTest<RomajiTagCaretPositionAlgorithm, RomajiTagCaretPosition>
    {
        #region Lyric

        [TestCase(nameof(singleLyric), 0, 0, true)]
        public void TestPositionMovable(string sourceName, int lyricIndex, int romajiIndex, bool movable)
        {
            var lyrics = GetLyricsByMethodName(sourceName);
            var caret = createCaretPosition(lyrics, lyricIndex, romajiIndex);

            // Check is movable, will always be true in this algorithm.
            TestPositionMovable(lyrics, caret, movable);
        }

        [TestCase(nameof(singleLyric), 0, 0, null, null)]
        [TestCase(nameof(singleLyricWithNoRomaji), 0, 0, null, null)]
        [TestCase(nameof(twoLyricsWithText), 1, 0, 0, 0)]
        [TestCase(nameof(threeLyricsWithSpacing), 2, 0, 1, null)]
        public void TestMoveToPreviousLyric(string sourceName, int lyricIndex, int romajiIndex, int? expectedLyricIndex, int? expectedRomajiIndex)
        {
            var lyrics = GetLyricsByMethodName(sourceName);
            var caret = createCaretPosition(lyrics, lyricIndex, romajiIndex);
            var expected = createExpectedCaretPosition(lyrics, expectedLyricIndex, expectedRomajiIndex);

            // Check is movable
            TestMoveToPreviousLyric(lyrics, caret, expected);
        }

        [TestCase(nameof(singleLyric), 0, 0, null, null)]
        [TestCase(nameof(singleLyricWithNoRomaji), 0, 0, null, null)]
        [TestCase(nameof(twoLyricsWithText), 0, 0, 1, 0)]
        [TestCase(nameof(threeLyricsWithSpacing), 0, 0, 1, null)]
        public void TestMoveToNextLyric(string sourceName, int lyricIndex, int romajiIndex, int? expectedLyricIndex, int? expectedRomajiIndex)
        {
            var lyrics = GetLyricsByMethodName(sourceName);
            var caret = createCaretPosition(lyrics, lyricIndex, romajiIndex);
            var expected = createExpectedCaretPosition(lyrics, expectedLyricIndex, expectedRomajiIndex);

            // Check is movable
            TestMoveToNextLyric(lyrics, caret, expected);
        }

        [TestCase(nameof(singleLyric), 0, 0)]
        [TestCase(nameof(singleLyricWithNoRomaji), 0, null)]
        [TestCase(nameof(twoLyricsWithText), 0, 0)]
        [TestCase(nameof(threeLyricsWithSpacing), 0, 0)]
        public void TestMoveToFirstLyric(string sourceName, int? expectedLyricIndex, int? expectedRomajiIndex)
        {
            var lyrics = GetLyricsByMethodName(sourceName);
            var expected = createExpectedCaretPosition(lyrics, expectedLyricIndex, expectedRomajiIndex);

            // Check first position
            TestMoveToFirstLyric(lyrics, expected);
        }

        [TestCase(nameof(singleLyric), 0, 0)]
        [TestCase(nameof(singleLyricWithNoRomaji), 0, null)]
        [TestCase(nameof(twoLyricsWithText), 1, 0)]
        [TestCase(nameof(threeLyricsWithSpacing), 2, 0)]
        public void TestMoveToLastLyric(string sourceName, int? expectedLyricIndex, int? expectedRomajiIndex)
        {
            var lyrics = GetLyricsByMethodName(sourceName);
            var expected = createExpectedCaretPosition(lyrics, expectedLyricIndex, expectedRomajiIndex);

            // Check last position
            TestMoveToLastLyric(lyrics, expected);
        }

        [TestCase(nameof(singleLyric), 0, 0)]
        [TestCase(nameof(singleLyricWithNoRomaji), 0, null)]
        public void TestMoveToTargetLyric(string sourceName, int lyricIndex, int? expectedRomajiIndex)
        {
            var lyrics = GetLyricsByMethodName(sourceName);
            var lyric = lyrics[lyricIndex];
            var expected = createExpectedCaretPosition(lyrics, lyricIndex, expectedRomajiIndex);

            // Check move to target position.
            TestMoveToTargetLyric(lyrics, lyric, expected);
        }

        #endregion

        #region Lyric index

        [TestCase(nameof(singleLyric), 0, 0, null, null)] // should always not movable.
        [TestCase(nameof(singleLyric), 0, 3, null, null)]
        public void TestMoveToPreviousIndex(string sourceName, int lyricIndex, int index, int? expectedLyricIndex, int? expectedRomajiIndex)
        {
            var lyrics = GetLyricsByMethodName(sourceName);
            var caret = createCaretPosition(lyrics, lyricIndex, index);
            var expected = createExpectedCaretPosition(lyrics, expectedLyricIndex, expectedRomajiIndex);

            // Check is movable
            TestMoveToPreviousIndex(lyrics, caret, expected);
        }

        [TestCase(nameof(singleLyric), 0, 3, null, null)] // should always not movable.
        [TestCase(nameof(singleLyric), 0, 0, null, null)]
        public void TestMoveToNextIndex(string sourceName, int lyricIndex, int index, int? expectedLyricIndex, int? expectedRomajiIndex)
        {
            var lyrics = GetLyricsByMethodName(sourceName);
            var caret = createCaretPosition(lyrics, lyricIndex, index);
            var expected = createExpectedCaretPosition(lyrics, expectedLyricIndex, expectedRomajiIndex);

            // Check is movable
            TestMoveToNextIndex(lyrics, caret, expected);
        }

        [TestCase(nameof(singleLyric), 0, null, null)] // should always not movable.
        public void TestMoveToFirstIndex(string sourceName, int lyricIndex, int? expectedLyricIndex, int? expectedRomajiIndex)
        {
            var lyrics = GetLyricsByMethodName(sourceName);
            var lyric = lyrics[lyricIndex];
            var expected = createExpectedCaretPosition(lyrics, expectedLyricIndex, expectedRomajiIndex);

            // Check is movable
            TestMoveToFirstIndex(lyrics, lyric, expected);
        }

        [TestCase(nameof(singleLyric), 0, null, null)] // should always not movable.
        public void TestMoveToLastIndex(string sourceName, int lyricIndex, int? expectedLyricIndex, int? expectedRomajiIndex)
        {
            var lyrics = GetLyricsByMethodName(sourceName);
            var lyric = lyrics[lyricIndex];
            var expected = createExpectedCaretPosition(lyrics, expectedLyricIndex, expectedRomajiIndex);

            // Check is movable
            TestMoveToLastIndex(lyrics, lyric, expected);
        }

        [TestCase(nameof(singleLyric), 0, 0, 0)]
        [TestCase(nameof(singleLyric), 0, 3, 3)]
        [TestCase(nameof(singleLyricWithNoRomaji), 0, -1, null)] // will check the invalid case.
        [TestCase(nameof(singleLyricWithNoRomaji), 0, 4, null)]
        public void TestMoveToTargetLyric(string sourceName, int lyricIndex, int romajiIndex, int? expectedRomajiIndex)
        {
            var lyrics = GetLyricsByMethodName(sourceName);
            var lyric = lyrics[lyricIndex];
            var romaji = lyric.RomajiTags.ElementAtOrDefault(romajiIndex);
            var expected = createExpectedCaretPosition(lyrics, lyricIndex, expectedRomajiIndex);

            // Check move to target position.
            TestMoveToTargetLyric(lyrics, lyric, romaji, expected);
        }

        #endregion

        protected override void AssertEqual(RomajiTagCaretPosition expected, RomajiTagCaretPosition actual)
        {
            Assert.AreEqual(expected.Lyric, actual.Lyric);
            Assert.AreEqual(expected.RomajiTag, actual.RomajiTag);
        }

        private static RomajiTagCaretPosition createCaretPosition(IEnumerable<Lyric> lyrics, int lyricIndex, int? romajiTagIndex)
        {
            var lyric = lyrics.ElementAtOrDefault(lyricIndex);
            var romajiTag = romajiTagIndex != null ? lyric?.RomajiTags.ElementAtOrDefault(romajiTagIndex.Value) : null;

            if (lyric == null)
                throw new ArgumentNullException();

            return new RomajiTagCaretPosition(lyric, romajiTag);
        }

        private static RomajiTagCaretPosition? createExpectedCaretPosition(IEnumerable<Lyric> lyrics, int? lyricIndex, int? romajiTagIndex)
        {
            if (lyricIndex == null)
                return null;

            return createCaretPosition(lyrics, lyricIndex.Value, romajiTagIndex);
        }

        #region source

        private static Lyric[] singleLyric => new[]
        {
            new Lyric
            {
                Text = "カラオケ",
                RomajiTags = TestCaseTagHelper.ParseRomajiTags(new[]
                {
                    "[0,1]:ka",
                    "[1,2]:ra",
                    "[2,3]:o",
                    "[3,4]:ke"
                }),
            }
        };

        private static Lyric[] singleLyricWithNoRomaji => new[]
        {
            new Lyric()
        };

        private static Lyric[] twoLyricsWithText => new[]
        {
            new Lyric
            {
                Text = "カラオケ",
                RomajiTags = TestCaseTagHelper.ParseRomajiTags(new[]
                {
                    "[0,1]:ka",
                    "[1,2]:ra",
                    "[2,3]:o",
                    "[3,4]:ke"
                }),
            },
            new Lyric
            {
                Text = "大好き",
                RomajiTags = TestCaseTagHelper.ParseRomajiTags(new[]
                {
                    "[0,1]:tai",
                    "[1,2]:su",
                }),
            }
        };

        private static Lyric[] threeLyricsWithSpacing => new[]
        {
            new Lyric
            {
                Text = "カラオケ",
                RomajiTags = TestCaseTagHelper.ParseRomajiTags(new[]
                {
                    "[0,1]:ka",
                    "[1,2]:ra",
                    "[2,3]:o",
                    "[3,4]:ke"
                }),
            },
            new Lyric(),
            new Lyric
            {
                Text = "大好き",
                RomajiTags = TestCaseTagHelper.ParseRomajiTags(new[]
                {
                    "[0,1]:tai",
                    "[1,2]:su",
                }),
            }
        };

        #endregion
    }
}
