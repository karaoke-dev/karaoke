// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Linq;
using NUnit.Framework;
using osu.Game.Rulesets.Karaoke.Edit.Lyrics.CaretPosition;
using osu.Game.Rulesets.Karaoke.Edit.Lyrics.CaretPosition.Algorithms;
using osu.Game.Rulesets.Karaoke.Objects;
using osu.Game.Rulesets.Karaoke.Tests.Helper;

namespace osu.Game.Rulesets.Karaoke.Tests.Edit.Lyrics.CaretPosition.Algorithms
{
    [TestFixture]
    public class EditTextTagCaretPositionAlgorithmTest : BaseCaretPositionAlgorithmTest<EditTextTagCaretPositionAlgorithm, EditTextTagCaretPosition>
    {
        protected const int NOT_EXIST_TAG = -1;

        [TestCase(nameof(singleLyric), 0, 0, true)]
        [TestCase(nameof(singleLyric), 0, 3, true)]
        public void TestPositionMovable(string sourceName, int lyricIndex, int textTagIndex, bool movable)
        {
            var lyrics = GetLyricsByMethodName(sourceName);
            var caretPosition = createEditTextTagCaretPosition(lyrics, lyricIndex, textTagIndex);

            // Check is movable
            TestPositionMovable(lyrics, caretPosition, movable);
        }

        [TestCase(nameof(singleLyric), 0, 0, NOT_EXIST, NOT_EXIST_TAG)]
        [TestCase(nameof(twoLyricsWithText), 1, 0, 0, 0)]
        [TestCase(nameof(twoLyricsWithText), 1, 1, 0, 1)]
        [TestCase(nameof(threeLyricsWithSpacing), 2, 0, 0, 0)]
        [TestCase(nameof(threeLyricsWithSpacing), 2, 1, 0, 1)]
        public void TestMoveUp(string sourceName, int lyricIndex, int index, int newLyricIndex, int newIndex)
        {
            var lyrics = GetLyricsByMethodName(sourceName);
            var caretPosition = createEditTextTagCaretPosition(lyrics, lyricIndex, index);
            var newCaretPosition = createEditTextTagCaretPosition(lyrics, newLyricIndex, newIndex);

            // Check is movable
            TestMoveUp(lyrics, caretPosition, newCaretPosition);
        }

        [TestCase(nameof(singleLyric), 0, 0, NOT_EXIST, NOT_EXIST_TAG)]
        [TestCase(nameof(twoLyricsWithText), 0, 0, 1, 0)]
        [TestCase(nameof(twoLyricsWithText), 0, 3, 1, 1)]
        [TestCase(nameof(threeLyricsWithSpacing), 0, 0, 2, 0)]
        [TestCase(nameof(threeLyricsWithSpacing), 0, 3, 2, 1)]
        public void TestMoveDown(string sourceName, int lyricIndex, int index, int newLyricIndex, int newIndex)
        {
            var lyrics = GetLyricsByMethodName(sourceName);
            var caretPosition = createEditTextTagCaretPosition(lyrics, lyricIndex, index);
            var newCaretPosition = createEditTextTagCaretPosition(lyrics, newLyricIndex, newIndex);

            // Check is movable
            TestMoveDown(lyrics, caretPosition, newCaretPosition);
        }

        [TestCase(nameof(singleLyric), 0, 0, NOT_EXIST, NOT_EXIST_TAG)]
        [TestCase(nameof(twoLyricsWithText), 1, 0, 0, 3)]
        [TestCase(nameof(threeLyricsWithSpacing), 2, 0, 0, 3)]
        public void TestMoveLeft(string sourceName, int lyricIndex, int index, int newLyricIndex, int newIndex)
        {
            var lyrics = GetLyricsByMethodName(sourceName);
            var caretPosition = createEditTextTagCaretPosition(lyrics, lyricIndex, index);
            var newCaretPosition = createEditTextTagCaretPosition(lyrics, newLyricIndex, newIndex);

            // Check is movable
            TestMoveLeft(lyrics, caretPosition, newCaretPosition);
        }

        [TestCase(nameof(singleLyric), 0, 3, NOT_EXIST, NOT_EXIST_TAG)]
        [TestCase(nameof(twoLyricsWithText), 0, 3, 1, 0)]
        [TestCase(nameof(threeLyricsWithSpacing), 0, 3, 2, 0)]
        public void TestMoveRight(string sourceName, int lyricIndex, int index, int newLyricIndex, int newIndex)
        {
            var lyrics = GetLyricsByMethodName(sourceName);
            var caretPosition = createEditTextTagCaretPosition(lyrics, lyricIndex, index);
            var newCaretPosition = createEditTextTagCaretPosition(lyrics, newLyricIndex, newIndex);

            // Check is movable
            TestMoveRight(lyrics, caretPosition, newCaretPosition);
        }

        [Ignore("Not that important for now.")]
        [TestCase(nameof(singleLyric), 0, 0)]
        [TestCase(nameof(singleLyricWithNoText), NOT_EXIST, NOT_EXIST_TAG)]
        [TestCase(nameof(twoLyricsWithText), 0, 0)]
        [TestCase(nameof(threeLyricsWithSpacing), 0, 0)]
        public void TestMoveToFirst(string sourceName, int lyricIndex, int index)
        {
            var lyrics = GetLyricsByMethodName(sourceName);
            var caretPosition = createEditTextTagCaretPosition(lyrics, lyricIndex, index);

            // Check is movable
            TestMoveToFirst(lyrics, caretPosition);
        }

        [Ignore("Not that important for now.")]
        [TestCase(nameof(singleLyric), 0, 0)]
        [TestCase(nameof(singleLyricWithNoText), NOT_EXIST, NOT_EXIST_TAG)]
        [TestCase(nameof(twoLyricsWithText), 0, 0)]
        [TestCase(nameof(threeLyricsWithSpacing), 0, 0)]
        public void TestMoveToLast(string sourceName, int lyricIndex, int index)
        {
            var lyrics = GetLyricsByMethodName(sourceName);
            var caretPosition = createEditTextTagCaretPosition(lyrics, lyricIndex, index);

            // Check is movable
            TestMoveToLast(lyrics, caretPosition);
        }

        [TestCase(nameof(singleLyric), 0)]
        [TestCase(nameof(singleLyricWithNoText), 0)]
        public void TestMoveToTarget(string sourceName, int lyricIndex)
        {
            var lyrics = GetLyricsByMethodName(sourceName);
            var lyric = lyrics[lyricIndex];

            // lazy to implement this algorithm because this algorithm haven't being used.
            TestMoveToTarget(lyrics, lyric, null);
        }

        protected override void AssertEqual(EditTextTagCaretPosition compare, EditTextTagCaretPosition actual)
        {
            if (compare == null)
            {
                Assert.IsNull(actual);
            }
            else
            {
                Assert.AreEqual(compare.Lyric, actual.Lyric);
                Assert.AreEqual(compare.TextTag, actual.TextTag);
            }
        }

        private static EditTextTagCaretPosition createEditTextTagCaretPosition(Lyric[] lyrics, int lyricIndex, int timeTagIndex)
        {
            if (lyricIndex == NOT_EXIST)
                return null;

            var lyric = lyrics.ElementAtOrDefault(lyricIndex);

            // todo : need to able to switch between ruby or romaji.
            var ruby = lyric?.RubyTags.ElementAtOrDefault(timeTagIndex);
            return new EditTextTagCaretPosition(lyric, ruby);
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
