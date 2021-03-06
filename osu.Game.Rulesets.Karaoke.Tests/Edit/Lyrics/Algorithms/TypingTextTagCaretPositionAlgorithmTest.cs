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
    public class TypingTextTagCaretPositionAlgorithmTest : BaseCaretPositionAlgorithmTest<TypingTextTagCaretPositionAlgorithm, TypingTextTagCaretPosition>
    {
        [TestCase(nameof(singleLyric), 0, 0, 0, true)]
        [TestCase(nameof(singleLyric), 0, 0, 1, true)]
        [TestCase(nameof(singleLyric), 0, 0, 2, false)]
        [TestCase(nameof(singleLyric), 0, 0, -1, false)]
        public void TestPositionMovable(string sourceName, int lyricIndex, int textTagIndex, int typingCaretIndex, bool movable)
        {
            var lyrics = GetLyricsByMethodName(sourceName);
            var caretPosition = CreateTypingTextTagCaretPosition(lyrics, lyricIndex, textTagIndex, typingCaretIndex);

            // Check is movable
            TestPositionMovable(lyrics, caretPosition, movable);
        }

        [TestCase(nameof(singleLyric), 0, 0, 0)]
        [TestCase(nameof(singleLyric), 0, 0, 1)]
        [TestCase(nameof(singleLyric), 0, 0, 2)]
        [TestCase(nameof(singleLyric), 0, 0, -1)]
        public void TestMoveUp(string sourceName, int lyricIndex, int textTagIndex, int typingCaretIndex)
        {
            var lyrics = GetLyricsByMethodName(sourceName);
            var caretPosition = CreateTypingTextTagCaretPosition(lyrics, lyricIndex, textTagIndex, typingCaretIndex);

            // In this algorithm cannot move-up
            TestMoveUp(lyrics, caretPosition, null);
        }

        [TestCase(nameof(singleLyric), 0, 0, 0)]
        [TestCase(nameof(singleLyric), 0, 0, 1)]
        [TestCase(nameof(singleLyric), 0, 0, 2)]
        [TestCase(nameof(singleLyric), 0, 0, -1)]
        public void TestMoveDown(string sourceName, int lyricIndex, int textTagIndex, int typingCaretIndex)
        {
            var lyrics = GetLyricsByMethodName(sourceName);
            var caretPosition = CreateTypingTextTagCaretPosition(lyrics, lyricIndex, textTagIndex, typingCaretIndex);

            // In this algorithm cannot move-down
            TestMoveDown(lyrics, caretPosition, null);
        }

        [TestCase(nameof(singleLyric), 0, 3, 1, 0)]
        [TestCase(nameof(singleLyric), 0, 3, 0, 0)]
        public void TestMoveLeft(string sourceName, int lyricIndex, int textTagIndex, int typingCaretIndex, int newTypingCaretIndex)
        {
            var lyrics = GetLyricsByMethodName(sourceName);
            var caretPosition = CreateTypingTextTagCaretPosition(lyrics, lyricIndex, textTagIndex, typingCaretIndex);
            var newCaretPosition = typingCaretIndex != newTypingCaretIndex
                ? CreateTypingTextTagCaretPosition(lyrics, lyricIndex, textTagIndex, newTypingCaretIndex)
                : null;

            // In this algorithm cannot move-down
            TestMoveLeft(lyrics, caretPosition, newCaretPosition);
        }

        [TestCase(nameof(singleLyric), 0, 0, 0, 1)]
        [TestCase(nameof(singleLyric), 0, 0, 1, 1)]
        public void TestMoveRight(string sourceName, int lyricIndex, int textTagIndex, int typingCaretIndex, int newTypingCaretIndex)
        {
            var lyrics = GetLyricsByMethodName(sourceName);
            var caretPosition = CreateTypingTextTagCaretPosition(lyrics, lyricIndex, textTagIndex, typingCaretIndex);
            var newCaretPosition = typingCaretIndex != newTypingCaretIndex
                ? CreateTypingTextTagCaretPosition(lyrics, lyricIndex, textTagIndex, newTypingCaretIndex)
                : null;

            // In this algorithm cannot move-down
            TestMoveRight(lyrics, caretPosition, newCaretPosition);
        }

        [TestCase(nameof(singleLyric))]
        public void TestMoveToFirst(string sourceName)
        {
            var lyrics = GetLyricsByMethodName(sourceName);

            // In this algorithm cannot move to first
            TestMoveToFirst(lyrics, null);
        }

        [TestCase(nameof(singleLyric))]
        public void TestMoveToLast(string sourceName)
        {
            var lyrics = GetLyricsByMethodName(sourceName);

            // In this algorithm cannot move to last
            TestMoveToLast(lyrics, null);
        }

        protected override void AssertEqual(TypingTextTagCaretPosition compare, TypingTextTagCaretPosition actual)
        {
            if (compare == null)
            {
                Assert.IsNull(actual);
            }
            else
            {
                Assert.AreEqual(compare.Lyric, actual.Lyric);
                Assert.AreEqual(compare.TextTag, actual.TextTag);
                Assert.AreEqual(compare.TypingCaretIndex, actual.TypingCaretIndex);
            }
        }

        protected TypingTextTagCaretPosition CreateTypingTextTagCaretPosition(Lyric[] lyrics, int lyricIndex, int textTagIndex, int typingCaretIndex)
        {
            if (lyricIndex == NOT_EXIST)
                return null;

            var lyric = lyrics.ElementAtOrDefault(lyricIndex);

            // todo : need to able to switch between ruby or romaji.
            var textTag = lyric?.RubyTags.ElementAtOrDefault(textTagIndex);
            return new TypingTextTagCaretPosition(lyric, textTag, typingCaretIndex);
        }

        #region source

        private Lyric[] singleLyric => new[]
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

        #endregion
    }
}
