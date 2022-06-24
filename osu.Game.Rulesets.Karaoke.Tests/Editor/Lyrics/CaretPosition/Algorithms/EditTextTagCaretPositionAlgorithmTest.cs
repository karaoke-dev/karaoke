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
    public class EditTextTagCaretPositionAlgorithmTest : BaseCaretPositionAlgorithmTest<EditTextTagCaretPositionAlgorithm, EditTextTagCaretPosition>
    {
        [TestCase(nameof(singleLyric), 0, 0, true)]
        [TestCase(nameof(singleLyric), 0, 3, true)]
        public void TestPositionMovable(string sourceName, int lyricIndex, int textTagIndex, bool movable)
        {
            var lyrics = GetLyricsByMethodName(sourceName);
            var caret = createEditTextTagCaretPosition(lyrics, lyricIndex, textTagIndex);

            // Check is movable
            TestPositionMovable(lyrics, caret, movable);
        }

        [TestCase(nameof(singleLyric), 0, 0, null, null)]
        [TestCase(nameof(twoLyricsWithText), 1, 0, 0, 0)]
        [TestCase(nameof(twoLyricsWithText), 1, 1, 0, 1)]
        [TestCase(nameof(threeLyricsWithSpacing), 2, 0, 0, 0)]
        [TestCase(nameof(threeLyricsWithSpacing), 2, 1, 0, 1)]
        public void TestMoveUp(string sourceName, int lyricIndex, int index, int? expectedLyricIndex, int? expectedIndex)
        {
            var lyrics = GetLyricsByMethodName(sourceName);
            var caret = createEditTextTagCaretPosition(lyrics, lyricIndex, index);
            var expected = createExpectedEditTextTagCaretPosition(lyrics, expectedLyricIndex, expectedIndex);

            // Check is movable
            TestMoveUp(lyrics, caret, expected);
        }

        [TestCase(nameof(singleLyric), 0, 0, null, null)]
        [TestCase(nameof(twoLyricsWithText), 0, 0, 1, 0)]
        [TestCase(nameof(twoLyricsWithText), 0, 3, 1, 1)]
        [TestCase(nameof(threeLyricsWithSpacing), 0, 0, 2, 0)]
        [TestCase(nameof(threeLyricsWithSpacing), 0, 3, 2, 1)]
        public void TestMoveDown(string sourceName, int lyricIndex, int index, int? expectedLyricIndex, int? expectedIndex)
        {
            var lyrics = GetLyricsByMethodName(sourceName);
            var caret = createEditTextTagCaretPosition(lyrics, lyricIndex, index);
            var expected = createExpectedEditTextTagCaretPosition(lyrics, expectedLyricIndex, expectedIndex);

            // Check is movable
            TestMoveDown(lyrics, caret, expected);
        }

        [TestCase(nameof(singleLyric), 0, 0, null, null)]
        [TestCase(nameof(twoLyricsWithText), 1, 0, 0, 3)]
        [TestCase(nameof(threeLyricsWithSpacing), 2, 0, 0, 3)]
        public void TestMoveLeft(string sourceName, int lyricIndex, int index, int? expectedLyricIndex, int? expectedIndex)
        {
            var lyrics = GetLyricsByMethodName(sourceName);
            var caret = createEditTextTagCaretPosition(lyrics, lyricIndex, index);
            var expected = createExpectedEditTextTagCaretPosition(lyrics, expectedLyricIndex, expectedIndex);

            // Check is movable
            TestMoveLeft(lyrics, caret, expected);
        }

        [TestCase(nameof(singleLyric), 0, 3, null, null)]
        [TestCase(nameof(twoLyricsWithText), 0, 3, 1, 0)]
        [TestCase(nameof(threeLyricsWithSpacing), 0, 3, 2, 0)]
        public void TestMoveRight(string sourceName, int lyricIndex, int index, int? expectedLyricIndex, int? expectedIndex)
        {
            var lyrics = GetLyricsByMethodName(sourceName);
            var caret = createEditTextTagCaretPosition(lyrics, lyricIndex, index);
            var expected = createExpectedEditTextTagCaretPosition(lyrics, expectedLyricIndex, expectedIndex);

            // Check is movable
            TestMoveRight(lyrics, caret, expected);
        }

        [Ignore("Not that important for now.")]
        [TestCase(nameof(singleLyric), 0, 0)]
        [TestCase(nameof(singleLyricWithNoText), null, null)]
        [TestCase(nameof(twoLyricsWithText), 0, 0)]
        [TestCase(nameof(threeLyricsWithSpacing), 0, 0)]
        public void TestMoveToFirst(string sourceName, int? expectedLyricIndex, int? expectedIndex)
        {
            var lyrics = GetLyricsByMethodName(sourceName);
            var expected = createExpectedEditTextTagCaretPosition(lyrics, expectedLyricIndex, expectedIndex);

            // Check is movable
            TestMoveToFirst(lyrics, expected);
        }

        [Ignore("Not that important for now.")]
        [TestCase(nameof(singleLyric), 0, 0)]
        [TestCase(nameof(singleLyricWithNoText), null, null)]
        [TestCase(nameof(twoLyricsWithText), 0, 0)]
        [TestCase(nameof(threeLyricsWithSpacing), 0, 0)]
        public void TestMoveToLast(string sourceName, int? expectedLyricIndex, int? expectedIndex)
        {
            var lyrics = GetLyricsByMethodName(sourceName);
            var expected = createExpectedEditTextTagCaretPosition(lyrics, expectedLyricIndex, expectedIndex);

            // Check is movable
            TestMoveToLast(lyrics, expected);
        }

        [TestCase(nameof(singleLyric), 0)]
        [TestCase(nameof(singleLyricWithNoText), 0)]
        public void TestMoveToTarget(string sourceName, int expectedLyricIndex)
        {
            var lyrics = GetLyricsByMethodName(sourceName);
            var lyric = lyrics[expectedLyricIndex];

            // lazy to implement this algorithm because this algorithm haven't being used.
            TestMoveToTarget(lyrics, lyric, null);
        }

        protected override void AssertEqual(EditTextTagCaretPosition expected, EditTextTagCaretPosition actual)
        {
            Assert.AreEqual(expected.Lyric, actual.Lyric);
            Assert.AreEqual(expected.TextTag, actual.TextTag);
        }

        private static EditTextTagCaretPosition createEditTextTagCaretPosition(IEnumerable<Lyric> lyrics, int lyricIndex, int timeTagIndex)
        {
            var lyric = lyrics.ElementAtOrDefault(lyricIndex);

            // todo : need to able to switch between ruby or romaji.
            var ruby = lyric?.RubyTags.ElementAtOrDefault(timeTagIndex);

            if (lyric == null || ruby == null)
                throw new ArgumentNullException();

            return new EditTextTagCaretPosition(lyric, ruby);
        }

        private static EditTextTagCaretPosition? createExpectedEditTextTagCaretPosition(IEnumerable<Lyric> lyrics, int? lyricIndex, int? timeTagIndex)
        {
            if (lyricIndex == null || timeTagIndex == null)
                return null;

            return createEditTextTagCaretPosition(lyrics, lyricIndex.Value, timeTagIndex.Value);
        }

        #region source

        private static Lyric[] singleLyric => new[]
        {
            new Lyric
            {
                RubyTags = TestCaseTagHelper.ParseRubyTags(new[]
                {
                    "[0,1]:か",
                    "[1,2]:ら",
                    "[2,3]:お",
                    "[3,4]:け"
                }),
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
                RubyTags = TestCaseTagHelper.ParseRubyTags(new[]
                {
                    "[0,1]:か",
                    "[1,2]:ら",
                    "[2,3]:お",
                    "[3,4]:け"
                }),
                Text = "カラオケ"
            },
            new Lyric
            {
                RubyTags = TestCaseTagHelper.ParseRubyTags(new[]
                {
                    "[0,1]:だい",
                    "[1,2]:す",
                }),
                Text = "大好き"
            }
        };

        private static Lyric[] threeLyricsWithSpacing => new[]
        {
            new Lyric
            {
                RubyTags = TestCaseTagHelper.ParseRubyTags(new[]
                {
                    "[0,1]:か",
                    "[1,2]:ら",
                    "[2,3]:お",
                    "[3,4]:け"
                }),
                Text = "カラオケ"
            },
            new Lyric(),
            new Lyric
            {
                RubyTags = TestCaseTagHelper.ParseRubyTags(new[]
                {
                    "[0,1]:だい",
                    "[1,2]:す",
                }),
                Text = "大好き"
            }
        };

        #endregion
    }
}
