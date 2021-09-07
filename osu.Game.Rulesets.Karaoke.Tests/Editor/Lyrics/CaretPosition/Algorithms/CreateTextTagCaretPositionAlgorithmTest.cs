// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Linq;
using NUnit.Framework;
using osu.Game.Rulesets.Karaoke.Edit.Lyrics.CaretPosition;
using osu.Game.Rulesets.Karaoke.Edit.Lyrics.CaretPosition.Algorithms;
using osu.Game.Rulesets.Karaoke.Objects;
using osu.Game.Rulesets.Karaoke.Tests.Helper;

namespace osu.Game.Rulesets.Karaoke.Tests.Editor.Lyrics.CaretPosition.Algorithms
{
    [TestFixture]
    public class CreateTextTagCaretPositionAlgorithmTest : BaseCaretPositionAlgorithmTest<CreateTextTagCaretPositionAlgorithm, CreateTextTagCaretPosition>
    {
        [TestCase(nameof(singleLyric), 0, 0, 0, true)] // it's movable but start and end index at same value means will not create this ruby/romaji tag.
        [TestCase(nameof(singleLyric), 0, 0, 2, true)]
        [TestCase(nameof(singleLyric), 0, 2, 0, true)] // it's movable but not ok while creating new ruby/romaji tag.
        [TestCase(nameof(singleLyric), 0, -1, 0, false)]
        [TestCase(nameof(singleLyric), 0, 4, 5, false)]
        [TestCase(nameof(singleLyricWithNoText), 0, 0, 0, false)]
        public void TestPositionMovable(string sourceName, int lyricIndex, int startIndex, int endIndex, bool movable)
        {
            var lyrics = GetLyricsByMethodName(sourceName);
            var caretPosition = createCreateTextTagCaretPosition(lyrics, lyricIndex, startIndex, endIndex);

            // Check is movable
            TestPositionMovable(lyrics, caretPosition, movable);
        }

        [TestCase(nameof(singleLyric), 0, 0, 2)]
        [TestCase(nameof(singleLyricWithNoText), 0, 0, 0)]
        public void TestMoveUp(string sourceName, int lyricIndex, int startIndex, int endIndex)
        {
            var lyrics = GetLyricsByMethodName(sourceName);
            var caretPosition = createCreateTextTagCaretPosition(lyrics, lyricIndex, startIndex, endIndex);

            // Anyway it's not movable.
            TestMoveUp(lyrics, caretPosition, null);
        }

        [TestCase(nameof(singleLyric), 0, 0, 2)]
        [TestCase(nameof(singleLyricWithNoText), 0, 0, 0)]
        public void TestMoveDown(string sourceName, int lyricIndex, int startIndex, int endIndex)
        {
            var lyrics = GetLyricsByMethodName(sourceName);
            var caretPosition = createCreateTextTagCaretPosition(lyrics, lyricIndex, startIndex, endIndex);

            // Anyway it's not movable.
            TestMoveDown(lyrics, caretPosition, null);
        }

        [TestCase(nameof(singleLyric), 0, 0, 2)]
        [TestCase(nameof(singleLyricWithNoText), 0, 0, 0)]
        public void TestMoveLeft(string sourceName, int lyricIndex, int startIndex, int endIndex)
        {
            var lyrics = GetLyricsByMethodName(sourceName);
            var caretPosition = createCreateTextTagCaretPosition(lyrics, lyricIndex, startIndex, endIndex);

            // Anyway it's not movable.
            TestMoveLeft(lyrics, caretPosition, null);
        }

        [TestCase(nameof(singleLyric), 0, 0, 2)]
        [TestCase(nameof(singleLyricWithNoText), 0, 0, 0)]
        public void TestMoveRight(string sourceName, int lyricIndex, int startIndex, int endIndex)
        {
            var lyrics = GetLyricsByMethodName(sourceName);
            var caretPosition = createCreateTextTagCaretPosition(lyrics, lyricIndex, startIndex, endIndex);

            // Anyway it's not movable.
            TestMoveRight(lyrics, caretPosition, null);
        }

        [TestCase(nameof(singleLyric))]
        [TestCase(nameof(singleLyricWithNoText))]
        public void TestMoveToFirst(string sourceName)
        {
            var lyrics = GetLyricsByMethodName(sourceName);

            // Anyway it's not movable.
            TestMoveToFirst(lyrics, null);
        }

        [TestCase(nameof(singleLyric))]
        [TestCase(nameof(singleLyricWithNoText))]
        public void TestMoveToLast(string sourceName)
        {
            var lyrics = GetLyricsByMethodName(sourceName);

            // Anyway it's not movable.
            TestMoveToLast(lyrics, null);
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

        protected override void AssertEqual(CreateTextTagCaretPosition compare, CreateTextTagCaretPosition actual)
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

        private static CreateTextTagCaretPosition createCreateTextTagCaretPosition(Lyric[] lyrics, int lyricIndex, int startIndex, int endIndex)
        {
            if (lyricIndex == NOT_EXIST)
                return null;

            var lyric = lyrics.ElementAtOrDefault(lyricIndex);

            // todo : need to able to switch between ruby or romaji.
            var textTag = new RubyTag
            {
                StartIndex = startIndex,
                EndIndex = endIndex,
                Text = "ルビ"
            };
            return new CreateTextTagCaretPosition(lyric, textTag);
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

        #endregion
    }
}
