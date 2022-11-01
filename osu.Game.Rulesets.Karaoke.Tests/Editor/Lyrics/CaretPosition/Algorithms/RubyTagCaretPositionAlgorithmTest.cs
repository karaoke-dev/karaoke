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
    public class RubyTagCaretPositionAlgorithmTest : BaseIndexCaretPositionAlgorithmTest<RubyTagCaretPositionAlgorithm, RubyTagCaretPosition>
    {
        #region Lyric

        [TestCase(nameof(singleLyric), 0, 0, true)]
        public void TestPositionMovable(string sourceName, int lyricIndex, int rubyIndex, bool movable)
        {
            var lyrics = GetLyricsByMethodName(sourceName);
            var caret = createCaretPosition(lyrics, lyricIndex, rubyIndex);

            // Check is movable, will always be true in this algorithm.
            TestPositionMovable(lyrics, caret, movable);
        }

        [TestCase(nameof(singleLyric), 0, 0, null, null)]
        [TestCase(nameof(singleLyricWithNoRuby), 0, 0, null, null)]
        [TestCase(nameof(twoLyricsWithText), 1, 0, 0, 0)]
        [TestCase(nameof(threeLyricsWithSpacing), 2, 0, 1, null)]
        public void TestMoveToPreviousLyric(string sourceName, int lyricIndex, int rubyIndex, int? expectedLyricIndex, int? expectedRubyIndex)
        {
            var lyrics = GetLyricsByMethodName(sourceName);
            var caret = createCaretPosition(lyrics, lyricIndex, rubyIndex);
            var expected = createExpectedCaretPosition(lyrics, expectedLyricIndex, expectedRubyIndex);

            // Check is movable
            TestMoveToPreviousLyric(lyrics, caret, expected);
        }

        [TestCase(nameof(singleLyric), 0, 0, null, null)]
        [TestCase(nameof(singleLyricWithNoRuby), 0, 0, null, null)]
        [TestCase(nameof(twoLyricsWithText), 0, 0, 1, 0)]
        [TestCase(nameof(threeLyricsWithSpacing), 0, 0, 1, null)]
        public void TestMoveToNextLyric(string sourceName, int lyricIndex, int rubyIndex, int? expectedLyricIndex, int? expectedRubyIndex)
        {
            var lyrics = GetLyricsByMethodName(sourceName);
            var caret = createCaretPosition(lyrics, lyricIndex, rubyIndex);
            var expected = createExpectedCaretPosition(lyrics, expectedLyricIndex, expectedRubyIndex);

            // Check is movable
            TestMoveToNextLyric(lyrics, caret, expected);
        }

        [TestCase(nameof(singleLyric), 0, 0)]
        [TestCase(nameof(singleLyricWithNoRuby), 0, null)]
        [TestCase(nameof(twoLyricsWithText), 0, 0)]
        [TestCase(nameof(threeLyricsWithSpacing), 0, 0)]
        public void TestMoveToFirstLyric(string sourceName, int? expectedLyricIndex, int? expectedRubyIndex)
        {
            var lyrics = GetLyricsByMethodName(sourceName);
            var expected = createExpectedCaretPosition(lyrics, expectedLyricIndex, expectedRubyIndex);

            // Check first position
            TestMoveToFirstLyric(lyrics, expected);
        }

        [TestCase(nameof(singleLyric), 0, 0)]
        [TestCase(nameof(singleLyricWithNoRuby), 0, null)]
        [TestCase(nameof(twoLyricsWithText), 1, 0)]
        [TestCase(nameof(threeLyricsWithSpacing), 2, 0)]
        public void TestMoveToLastLyric(string sourceName, int? expectedLyricIndex, int? expectedRubyIndex)
        {
            var lyrics = GetLyricsByMethodName(sourceName);
            var expected = createExpectedCaretPosition(lyrics, expectedLyricIndex, expectedRubyIndex);

            // Check last position
            TestMoveToLastLyric(lyrics, expected);
        }

        [TestCase(nameof(singleLyric), 0, 0)]
        [TestCase(nameof(singleLyricWithNoRuby), 0, null)]
        public void TestMoveToTargetLyric(string sourceName, int expectedLyricIndex, int? rubyIndex)
        {
            var lyrics = GetLyricsByMethodName(sourceName);
            var lyric = lyrics[expectedLyricIndex];
            var expected = createExpectedCaretPosition(lyrics, expectedLyricIndex, rubyIndex);

            // Check move to target position.
            TestMoveToTargetLyric(lyrics, lyric, expected);
        }

        #endregion

        #region Lyric index

        [TestCase(nameof(singleLyric), 0, 0, null, null)] // should always not movable.
        [TestCase(nameof(singleLyric), 0, 3, null, null)]
        public void TestMoveToPreviousIndex(string sourceName, int lyricIndex, int index, int? expectedLyricIndex, int? expectedRubyIndex)
        {
            var lyrics = GetLyricsByMethodName(sourceName);
            var caret = createCaretPosition(lyrics, lyricIndex, index);
            var expected = createExpectedCaretPosition(lyrics, expectedLyricIndex, expectedRubyIndex);

            // Check is movable
            TestMoveToPreviousIndex(lyrics, caret, expected);
        }

        [TestCase(nameof(singleLyric), 0, 3, null, null)] // should always not movable.
        [TestCase(nameof(singleLyric), 0, 0, null, null)]
        public void TestMoveToNextIndex(string sourceName, int lyricIndex, int index, int? expectedLyricIndex, int? expectedRubyIndex)
        {
            var lyrics = GetLyricsByMethodName(sourceName);
            var caret = createCaretPosition(lyrics, lyricIndex, index);
            var expected = createExpectedCaretPosition(lyrics, expectedLyricIndex, expectedRubyIndex);

            // Check is movable
            TestMoveToNextIndex(lyrics, caret, expected);
        }

        [TestCase(nameof(singleLyric), 0, null, null)] // should always not movable.
        public void TestMoveToFirstIndex(string sourceName, int lyricIndex, int? expectedLyricIndex, int? expectedRubyIndex)
        {
            var lyrics = GetLyricsByMethodName(sourceName);
            var lyric = lyrics[lyricIndex];
            var expected = createExpectedCaretPosition(lyrics, expectedLyricIndex, expectedRubyIndex);

            // Check is movable
            TestMoveToFirstIndex(lyrics, lyric, expected);
        }

        [TestCase(nameof(singleLyric), 0, null, null)] // should always not movable.
        public void TestMoveToLastIndex(string sourceName, int lyricIndex, int? expectedLyricIndex, int? expectedRubyIndex)
        {
            var lyrics = GetLyricsByMethodName(sourceName);
            var lyric = lyrics[lyricIndex];
            var expected = createExpectedCaretPosition(lyrics, expectedLyricIndex, expectedRubyIndex);

            // Check is movable
            TestMoveToLastIndex(lyrics, lyric, expected);
        }

        #endregion

        protected override void AssertEqual(RubyTagCaretPosition expected, RubyTagCaretPosition actual)
        {
            Assert.AreEqual(expected.Lyric, actual.Lyric);
            Assert.AreEqual(expected.RubyTag, actual.RubyTag);
        }

        private static RubyTagCaretPosition createCaretPosition(IEnumerable<Lyric> lyrics, int lyricIndex, int? rubyTagIndex)
        {
            var lyric = lyrics.ElementAtOrDefault(lyricIndex);
            var rubyTag = rubyTagIndex != null ? lyric?.RubyTags.ElementAtOrDefault(rubyTagIndex.Value) : null;

            if (lyric == null)
                throw new ArgumentNullException();

            return new RubyTagCaretPosition(lyric, rubyTag);
        }

        private static RubyTagCaretPosition? createExpectedCaretPosition(IEnumerable<Lyric> lyrics, int? lyricIndex, int? rubyTagIndex)
        {
            if (lyricIndex == null)
                return null;

            return createCaretPosition(lyrics, lyricIndex.Value, rubyTagIndex);
        }

        #region source

        private static Lyric[] singleLyric => new[]
        {
            new Lyric
            {
                Text = "カラオケ",
                RubyTags = TestCaseTagHelper.ParseRubyTags(new[]
                {
                    "[0,1]:か",
                    "[1,2]:ら",
                    "[2,3]:お",
                    "[3,4]:け"
                }),
            }
        };

        private static Lyric[] singleLyricWithNoRuby => new[]
        {
            new Lyric()
        };

        private static Lyric[] twoLyricsWithText => new[]
        {
            new Lyric
            {
                Text = "カラオケ",
                RubyTags = TestCaseTagHelper.ParseRubyTags(new[]
                {
                    "[0,1]:か",
                    "[1,2]:ら",
                    "[2,3]:お",
                    "[3,4]:け"
                }),
            },
            new Lyric
            {
                Text = "大好き",
                RubyTags = TestCaseTagHelper.ParseRubyTags(new[]
                {
                    "[0,1]:たい",
                    "[1,2]:す",
                }),
            }
        };

        private static Lyric[] threeLyricsWithSpacing => new[]
        {
            new Lyric
            {
                Text = "カラオケ",
                RubyTags = TestCaseTagHelper.ParseRubyTags(new[]
                {
                    "[0,1]:か",
                    "[1,2]:ら",
                    "[2,3]:お",
                    "[3,4]:け"
                }),
            },
            new Lyric(),
            new Lyric
            {
                Text = "大好き",
                RubyTags = TestCaseTagHelper.ParseRubyTags(new[]
                {
                    "[0,1]:たい",
                    "[1,2]:す",
                }),
            }
        };

        #endregion
    }
}
