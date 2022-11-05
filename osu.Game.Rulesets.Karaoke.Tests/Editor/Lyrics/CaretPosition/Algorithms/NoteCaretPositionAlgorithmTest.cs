// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using osu.Game.Rulesets.Karaoke.Edit.Lyrics.CaretPosition;
using osu.Game.Rulesets.Karaoke.Edit.Lyrics.CaretPosition.Algorithms;
using osu.Game.Rulesets.Karaoke.Objects;

namespace osu.Game.Rulesets.Karaoke.Tests.Editor.Lyrics.CaretPosition.Algorithms
{
    public class NoteCaretPositionAlgorithmTest : BaseIndexCaretPositionAlgorithmTest<NoteCaretPositionAlgorithm, NoteCaretPosition>
    {
        #region Lyric

        [TestCase(nameof(singleLyric), 0, 0, true)]
        public void TestPositionMovable(string sourceName, int lyricIndex, int noteIndex, bool movable)
        {
            var lyrics = GetLyricsByMethodName(sourceName);
            var caret = createCaretPosition(lyrics, lyricIndex, noteIndex);

            // Check is movable, will always be true in this algorithm.
            TestPositionMovable(lyrics, caret, movable);
        }

        [TestCase(nameof(singleLyric), 0, 0, null, null)]
        [TestCase(nameof(singleLyricWithNoNote), 0, 0, null, null)]
        [TestCase(nameof(twoLyricsWithText), 1, 0, 0, null)]
        [TestCase(nameof(threeLyricsWithSpacing), 2, 0, 1, null)]
        public void TestMoveToPreviousLyric(string sourceName, int lyricIndex, int noteIndex, int? expectedLyricIndex, int? expectedNoteIndex)
        {
            var lyrics = GetLyricsByMethodName(sourceName);
            var caret = createCaretPosition(lyrics, lyricIndex, noteIndex);
            var expected = createExpectedCaretPosition(lyrics, expectedLyricIndex, expectedNoteIndex);

            // Check is movable
            TestMoveToPreviousLyric(lyrics, caret, expected);
        }

        [TestCase(nameof(singleLyric), 0, 0, null, null)]
        [TestCase(nameof(singleLyricWithNoNote), 0, 0, null, null)]
        [TestCase(nameof(twoLyricsWithText), 0, 0, 1, null)]
        [TestCase(nameof(threeLyricsWithSpacing), 0, 0, 1, null)]
        public void TestMoveToNextLyric(string sourceName, int lyricIndex, int noteIndex, int? expectedLyricIndex, int? expectedNoteIndex)
        {
            var lyrics = GetLyricsByMethodName(sourceName);
            var caret = createCaretPosition(lyrics, lyricIndex, noteIndex);
            var expected = createExpectedCaretPosition(lyrics, expectedLyricIndex, expectedNoteIndex);

            // Check is movable
            TestMoveToNextLyric(lyrics, caret, expected);
        }

        [TestCase(nameof(singleLyric), 0, null)]
        [TestCase(nameof(singleLyricWithNoNote), 0, null)]
        [TestCase(nameof(twoLyricsWithText), 0, null)]
        [TestCase(nameof(threeLyricsWithSpacing), 0, null)]
        public void TestMoveToFirstLyric(string sourceName, int? expectedLyricIndex, int? expectedNoteIndex)
        {
            var lyrics = GetLyricsByMethodName(sourceName);
            var expected = createExpectedCaretPosition(lyrics, expectedLyricIndex, expectedNoteIndex);

            // Check first position
            TestMoveToFirstLyric(lyrics, expected);
        }

        [TestCase(nameof(singleLyric), 0, null)]
        [TestCase(nameof(singleLyricWithNoNote), 0, null)]
        [TestCase(nameof(twoLyricsWithText), 1, null)]
        [TestCase(nameof(threeLyricsWithSpacing), 2, null)]
        public void TestMoveToLastLyric(string sourceName, int? expectedLyricIndex, int? expectedNoteIndex)
        {
            var lyrics = GetLyricsByMethodName(sourceName);
            var expected = createExpectedCaretPosition(lyrics, expectedLyricIndex, expectedNoteIndex);

            // Check last position
            TestMoveToLastLyric(lyrics, expected);
        }

        [TestCase(nameof(singleLyric), 0, null)]
        [TestCase(nameof(singleLyricWithNoNote), 0, null)]
        public void TestMoveToTargetLyric(string sourceName, int expectedLyricIndex, int? noteIndex)
        {
            var lyrics = GetLyricsByMethodName(sourceName);
            var lyric = lyrics[expectedLyricIndex];
            var expected = createExpectedCaretPosition(lyrics, expectedLyricIndex, noteIndex);

            // Check move to target position.
            TestMoveToTargetLyric(lyrics, lyric, expected);
        }

        #endregion

        #region Lyric index

        [TestCase(nameof(singleLyric), 0, 0, null, null)] // should always not movable.
        [TestCase(nameof(singleLyric), 0, 3, null, null)]
        public void TestMoveToPreviousIndex(string sourceName, int lyricIndex, int index, int? expectedLyricIndex, int? expectedNoteIndex)
        {
            var lyrics = GetLyricsByMethodName(sourceName);
            var caret = createCaretPosition(lyrics, lyricIndex, index);
            var expected = createExpectedCaretPosition(lyrics, expectedLyricIndex, expectedNoteIndex);

            // Check is movable
            TestMoveToPreviousIndex(lyrics, caret, expected);
        }

        [TestCase(nameof(singleLyric), 0, 3, null, null)] // should always not movable.
        [TestCase(nameof(singleLyric), 0, 0, null, null)]
        public void TestMoveToNextIndex(string sourceName, int lyricIndex, int index, int? expectedLyricIndex, int? expectedNoteIndex)
        {
            var lyrics = GetLyricsByMethodName(sourceName);
            var caret = createCaretPosition(lyrics, lyricIndex, index);
            var expected = createExpectedCaretPosition(lyrics, expectedLyricIndex, expectedNoteIndex);

            // Check is movable
            TestMoveToNextIndex(lyrics, caret, expected);
        }

        [TestCase(nameof(singleLyric), 0, null, null)] // should always not movable.
        public void TestMoveToFirstIndex(string sourceName, int lyricIndex, int? expectedLyricIndex, int? expectedNoteIndex)
        {
            var lyrics = GetLyricsByMethodName(sourceName);
            var lyric = lyrics[lyricIndex];
            var expected = createExpectedCaretPosition(lyrics, expectedLyricIndex, expectedNoteIndex);

            // Check is movable
            TestMoveToFirstIndex(lyrics, lyric, expected);
        }

        [TestCase(nameof(singleLyric), 0, null, null)] // should always not movable.
        public void TestMoveToLastIndex(string sourceName, int lyricIndex, int? expectedLyricIndex, int? expectedNoteIndex)
        {
            var lyrics = GetLyricsByMethodName(sourceName);
            var lyric = lyrics[lyricIndex];
            var expected = createExpectedCaretPosition(lyrics, expectedLyricIndex, expectedNoteIndex);

            // Check is movable
            TestMoveToLastIndex(lyrics, lyric, expected);
        }

        [TestCase(nameof(singleLyric), 0, null, null)]
        public void TestMoveToTarget(string sourceName, int expectedLyricIndex, int? expectedNoteIndex, int? noteIndex)
        {
            var lyrics = GetLyricsByMethodName(sourceName);
            var lyric = lyrics[expectedLyricIndex];
            var note = noteIndex != null ? new Note { ReferenceLyric = lyric } : null;
            var expected = createExpectedCaretPosition(lyrics, expectedLyricIndex, noteIndex);

            // Check move to target position.
            TestMoveToTargetLyric(lyrics, lyric, note, expected);
        }

        #endregion

        protected override void AssertEqual(NoteCaretPosition expected, NoteCaretPosition actual)
        {
            Assert.AreEqual(expected.Lyric, actual.Lyric);
            Assert.AreEqual(expected.Note, actual.Note);
        }

        private static NoteCaretPosition createCaretPosition(IEnumerable<Lyric> lyrics, int lyricIndex, int? noteIndex)
        {
            var lyric = lyrics.ElementAtOrDefault(lyricIndex);
            var note = noteIndex != null ? new Note { ReferenceLyric = lyric } : null;

            if (lyric == null)
                throw new ArgumentNullException();

            return new NoteCaretPosition(lyric, note);
        }

        private static NoteCaretPosition? createExpectedCaretPosition(IEnumerable<Lyric> lyrics, int? lyricIndex, int? noteIndex)
        {
            if (lyricIndex == null)
                return null;

            return createCaretPosition(lyrics, lyricIndex.Value, noteIndex);
        }

        #region source

        private static Lyric[] singleLyric => new[]
        {
            new Lyric
            {
                Text = "カラオケ",
            }
        };

        private static Lyric[] singleLyricWithNoNote => new[]
        {
            new Lyric()
        };

        private static Lyric[] twoLyricsWithText => new[]
        {
            new Lyric
            {
                Text = "カラオケ",
            },
            new Lyric
            {
                Text = "大好き",
            }
        };

        private static Lyric[] threeLyricsWithSpacing => new[]
        {
            new Lyric
            {
                Text = "カラオケ",
            },
            new Lyric(),
            new Lyric
            {
                Text = "大好き",
            }
        };

        #endregion
    }
}
