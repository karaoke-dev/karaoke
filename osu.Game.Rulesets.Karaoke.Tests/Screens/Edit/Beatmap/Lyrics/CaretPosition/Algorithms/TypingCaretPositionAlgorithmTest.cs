// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using osu.Game.Rulesets.Karaoke.Objects;
using osu.Game.Rulesets.Karaoke.Screens.Edit.Beatmaps.Lyrics.CaretPosition;
using osu.Game.Rulesets.Karaoke.Screens.Edit.Beatmaps.Lyrics.CaretPosition.Algorithms;

namespace osu.Game.Rulesets.Karaoke.Tests.Screens.Edit.Beatmap.Lyrics.CaretPosition.Algorithms;

[TestFixture]
public class TypingCaretPositionAlgorithmTest : BaseIndexCaretPositionAlgorithmTest<TypingCaretPositionAlgorithm, TypingCaretPosition>
{
    #region Lyric

    [TestCase(nameof(singleLyric), 0, 0, true)]
    [TestCase(nameof(singleLyric), 0, 4, true)]
    [TestCase(nameof(singleLyric), 0, 5, false)]
    [TestCase(nameof(singleLyric), 0, -1, false)]
    [TestCase(nameof(singleLyricWithNoText), 0, 0, true)] // it's still movable even text is empty or null.
    [TestCase(nameof(singleLyricWithNoText), 0, 1, false)]
    public void TestPositionMovable(string sourceName, int lyricIndex, int index, bool movable)
    {
        var lyrics = GetLyricsByMethodName(sourceName);
        var caret = createCaretPosition(lyrics, lyricIndex, index);

        // Check is movable
        TestPositionMovable(lyrics, caret, movable);
    }

    [TestCase(nameof(singleLyric), 0, 0, null, null)] // cannot move up if at top index.
    [TestCase(nameof(singleLyricWithNoText), 0, 0, null, null)]
    [TestCase(nameof(twoLyricsWithText), 1, 0, 0, 0)]
    [TestCase(nameof(threeLyricsWithSpacing), 2, 0, 1, 0)]
    [TestCase(nameof(threeLyricsWithSpacing), 2, 3, 1, 0)]
    public void TestMoveToPreviousLyric(string sourceName, int lyricIndex, int index, int? expectedLyricIndex, int? expectedIndex)
    {
        var lyrics = GetLyricsByMethodName(sourceName);
        var caret = createCaretPosition(lyrics, lyricIndex, index);
        var expected = createExpectedCaretPosition(lyrics, expectedLyricIndex, expectedIndex);

        // Check is movable
        TestMoveToPreviousLyric(lyrics, caret, expected);
    }

    [TestCase(nameof(singleLyric), 0, 0, null, null)] // cannot move down if at bottom index.
    [TestCase(nameof(singleLyricWithNoText), 0, 0, null, null)]
    [TestCase(nameof(twoLyricsWithText), 0, 0, 1, 0)]
    [TestCase(nameof(threeLyricsWithSpacing), 0, 0, 1, 0)]
    [TestCase(nameof(threeLyricsWithSpacing), 0, 4, 1, 0)]
    public void TestMoveToNextLyric(string sourceName, int lyricIndex, int index, int? expectedLyricIndex, int? expectedIndex)
    {
        var lyrics = GetLyricsByMethodName(sourceName);
        var caret = createCaretPosition(lyrics, lyricIndex, index);
        var expected = createExpectedCaretPosition(lyrics, expectedLyricIndex, expectedIndex);

        // Check is movable
        TestMoveToNextLyric(lyrics, caret, expected);
    }

    [TestCase(nameof(singleLyric), 0, 0)]
    [TestCase(nameof(singleLyricWithNoText), 0, 0)]
    [TestCase(nameof(twoLyricsWithText), 0, 0)]
    [TestCase(nameof(threeLyricsWithSpacing), 0, 0)]
    public void TestMoveToFirstLyric(string sourceName, int? expectedLyricIndex, int? expectedIndex)
    {
        var lyrics = GetLyricsByMethodName(sourceName);
        var expected = createExpectedCaretPosition(lyrics, expectedLyricIndex, expectedIndex);

        // Check first position
        TestMoveToFirstLyric(lyrics, expected);
    }

    [TestCase(nameof(singleLyric), 0, 4)]
    [TestCase(nameof(singleLyricWithNoText), 0, 0)]
    [TestCase(nameof(twoLyricsWithText), 1, 3)]
    [TestCase(nameof(threeLyricsWithSpacing), 2, 3)]
    public void TestMoveToLastLyric(string sourceName, int? expectedLyricIndex, int? expectedIndex)
    {
        var lyrics = GetLyricsByMethodName(sourceName);
        var expected = createExpectedCaretPosition(lyrics, expectedLyricIndex, expectedIndex);

        // Check last position
        TestMoveToLastLyric(lyrics, expected);
    }

    [TestCase(nameof(singleLyric), 0, 0)]
    [TestCase(nameof(singleLyricWithNoText), 0, 0)]
    public void TestMoveToTargetLyric(string sourceName, int lyricIndex, int? expectedIndex)
    {
        var lyrics = GetLyricsByMethodName(sourceName);
        var lyric = lyrics[lyricIndex];
        var expected = createExpectedCaretPosition(lyrics, lyricIndex, expectedIndex);

        // Check move to target position.
        TestMoveToTargetLyric(lyrics, lyric, expected);
    }

    #endregion

    #region Lyric index

    [TestCase(nameof(singleLyric), 0, 0, null, null)]
    [TestCase(nameof(singleLyric), 0, 1, 0, 0)]
    [TestCase(nameof(singleLyricWithNoText), 0, 0, null, null)]
    public void TestMoveToPreviousIndex(string sourceName, int lyricIndex, int index, int? expectedLyricIndex, int? expectedIndex)
    {
        var lyrics = GetLyricsByMethodName(sourceName);
        var caret = createCaretPosition(lyrics, lyricIndex, index);
        var expected = createExpectedCaretPosition(lyrics, expectedLyricIndex, expectedIndex);

        // Check is movable
        TestMoveToPreviousIndex(lyrics, caret, expected);
    }

    [TestCase(nameof(singleLyric), 0, 4, null, null)]
    [TestCase(nameof(singleLyric), 0, 3, 0, 4)]
    [TestCase(nameof(singleLyricWithNoText), 0, 0, null, null)]
    public void TestMoveToNextIndex(string sourceName, int lyricIndex, int index, int? expectedLyricIndex, int? expectedIndex)
    {
        var lyrics = GetLyricsByMethodName(sourceName);
        var caret = createCaretPosition(lyrics, lyricIndex, index);
        var expected = createExpectedCaretPosition(lyrics, expectedLyricIndex, expectedIndex);

        // Check is movable
        TestMoveToNextIndex(lyrics, caret, expected);
    }

    [TestCase(nameof(singleLyric), 0, 0, 0)]
    [TestCase(nameof(singleLyricWithNoText), 0, 0, 0)]
    public void TestMoveToFirstIndex(string sourceName, int lyricIndex, int? expectedLyricIndex, int? expectedIndex)
    {
        var lyrics = GetLyricsByMethodName(sourceName);
        var lyric = lyrics[lyricIndex];
        var expected = createExpectedCaretPosition(lyrics, expectedLyricIndex, expectedIndex);

        // Check is movable
        TestMoveToFirstIndex(lyrics, lyric, expected);
    }

    [TestCase(nameof(singleLyric), 0, 0, 4)]
    [TestCase(nameof(singleLyricWithNoText), 0, 0, 0)]
    public void TestMoveToLastIndex(string sourceName, int lyricIndex, int? expectedLyricIndex, int? expectedIndex)
    {
        var lyrics = GetLyricsByMethodName(sourceName);
        var lyric = lyrics[lyricIndex];
        var expected = createExpectedCaretPosition(lyrics, expectedLyricIndex, expectedIndex);

        // Check is movable
        TestMoveToLastIndex(lyrics, lyric, expected);
    }

    [TestCase(nameof(singleLyric), 0, 0, 0)]
    [TestCase(nameof(singleLyric), 0, 4, 4)]
    [TestCase(nameof(singleLyric), 0, -1, null)] // will check the invalid case.
    [TestCase(nameof(singleLyric), 0, 5, null)]
    public void TestMoveToTargetLyric(string sourceName, int lyricIndex, int textIndex, int? expectedTextIndex)
    {
        var lyrics = GetLyricsByMethodName(sourceName);
        var lyric = lyrics[lyricIndex];
        var expected = createExpectedCaretPosition(lyrics, lyricIndex, expectedTextIndex);

        // Check move to target position.
        TestMoveToTargetLyric(lyrics, lyric, textIndex, expected);
    }

    [TestCase(nameof(singleLyric), 0, 0, 4, 4)]
    [TestCase(nameof(singleLyric), 0, 4, 0, 0)] // should be OK if the release char index is smaller than char index.
    [TestCase(nameof(singleLyric), 0, 0, 0, 0)] // should be OK if the release char index is same as char index.
    [TestCase(nameof(singleLyric), 0, 4, 4, 4)]
    [TestCase(nameof(singleLyric), 0, 0, -1, null)] // should be invalid if the release char index is out of range.
    [TestCase(nameof(singleLyric), 0, 0, 5, null)]
    [TestCase(nameof(singleLyric), 0, -1, 0, null)] // should be invalid if the start char index is out of range.
    [TestCase(nameof(singleLyric), 0, -1, 4, null)]
    public void TestAdjustEndIndex(string sourceName, int lyricIndex, int charGap, int releaseCharGap, int? expectedReleaseCharGap)
    {
        var lyrics = GetLyricsByMethodName(sourceName);

        var caretPosition = createCaretPosition(lyrics, lyricIndex, charGap);
        var expected = expectedReleaseCharGap == null ? null : createExpectedCaretPosition(lyrics, lyricIndex, charGap, expectedReleaseCharGap);

        // Check move to target position.
        TestAdjustEndIndex(lyrics, caretPosition, releaseCharGap, expected);
    }

    #endregion

    protected override void AssertEqual(TypingCaretPosition expected, TypingCaretPosition actual)
    {
        Assert.AreEqual(expected.Lyric, actual.Lyric);
        Assert.AreEqual(expected.CharGap, actual.CharGap);
    }

    private static TypingCaretPosition createCaretPosition(IEnumerable<Lyric> lyrics, int lyricIndex, int index, int? releaseIndex = null)
    {
        var lyric = lyrics.ElementAtOrDefault(lyricIndex);
        if (lyric == null)
            throw new ArgumentNullException();

        return new TypingCaretPosition(lyric, index, releaseIndex ?? index);
    }

    private static TypingCaretPosition? createExpectedCaretPosition(IEnumerable<Lyric> lyrics, int? lyricIndex, int? index, int? releaseIndex = null)
    {
        if (lyricIndex == null || index == null)
            return null;

        return createCaretPosition(lyrics, lyricIndex.Value, index.Value, releaseIndex);
    }

    #region source

    private static Lyric[] singleLyric => new[]
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
            Text = "カラオケ"
        },
        new Lyric
        {
            Text = "大好き"
        }
    };

    private static Lyric[] threeLyricsWithSpacing => new[]
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
