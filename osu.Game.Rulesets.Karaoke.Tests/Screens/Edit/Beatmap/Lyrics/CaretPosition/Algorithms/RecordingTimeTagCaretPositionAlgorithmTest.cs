// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using osu.Framework.Graphics.Sprites;
using osu.Game.Rulesets.Karaoke.Objects;
using osu.Game.Rulesets.Karaoke.Screens.Edit.Beatmaps.Lyrics.CaretPosition;
using osu.Game.Rulesets.Karaoke.Screens.Edit.Beatmaps.Lyrics.CaretPosition.Algorithms;
using osu.Game.Rulesets.Karaoke.Tests.Helper;

namespace osu.Game.Rulesets.Karaoke.Tests.Screens.Edit.Beatmap.Lyrics.CaretPosition.Algorithms;

[TestFixture]
public class RecordingTimeTagCaretPositionAlgorithmTest : BaseIndexCaretPositionAlgorithmTest<RecordingTimeTagCaretPositionAlgorithm, RecordingTimeTagCaretPosition>
{
    private const int not_exist_tag = -1;

    #region Lyric

    [TestCase(nameof(singleLyric), RecordingTimeTagCaretMoveMode.None, 0, 0, true)]
    [TestCase(nameof(singleLyric), RecordingTimeTagCaretMoveMode.OnlyStartTag, 0, 0, true)]
    [TestCase(nameof(singleLyric), RecordingTimeTagCaretMoveMode.OnlyEndTag, 0, 0, false)]
    [TestCase(nameof(singleLyricWithoutTimeTag), RecordingTimeTagCaretMoveMode.None, 0, not_exist_tag, false)]
    [TestCase(nameof(singleLyricWithoutTimeTag), RecordingTimeTagCaretMoveMode.OnlyStartTag, 0, not_exist_tag, false)]
    [TestCase(nameof(singleLyricWithoutTimeTag), RecordingTimeTagCaretMoveMode.OnlyEndTag, 0, not_exist_tag, false)]
    [TestCase(nameof(singleLyricWithNoText), RecordingTimeTagCaretMoveMode.None, 0, not_exist_tag, false)]
    [TestCase(nameof(singleLyricWithNoText), RecordingTimeTagCaretMoveMode.OnlyStartTag, 0, not_exist_tag, false)]
    [TestCase(nameof(singleLyricWithNoText), RecordingTimeTagCaretMoveMode.OnlyEndTag, 0, not_exist_tag, false)]
    public void TestPositionMovable(string sourceName, RecordingTimeTagCaretMoveMode mode, int lyricIndex, int timeTagIndex, bool movable)
    {
        var lyrics = GetLyricsByMethodName(sourceName);
        var caret = createCaretPosition(lyrics, lyricIndex, timeTagIndex);

        // Check is movable
        TestPositionMovable(lyrics, caret, movable, algorithms => algorithms.Mode = mode);
    }

    [TestCase(nameof(singleLyric), RecordingTimeTagCaretMoveMode.None, 0, 0, null, null)]
    [TestCase(nameof(twoLyricsWithText), RecordingTimeTagCaretMoveMode.None, 1, 0, 0, 0)]
    [TestCase(nameof(twoLyricsWithText), RecordingTimeTagCaretMoveMode.None, 1, 3, 0, 0)] // should move to first start index.
    [TestCase(nameof(twoLyricsWithText), RecordingTimeTagCaretMoveMode.OnlyStartTag, 1, 3, 0, 0)] // should move to first start index.
    [TestCase(nameof(twoLyricsWithText), RecordingTimeTagCaretMoveMode.OnlyEndTag, 1, 3, 0, 4)] // should move to first end index.
    [TestCase(nameof(threeLyricsWithSpacing), RecordingTimeTagCaretMoveMode.None, 2, 0, 0, 0)]
    [TestCase(nameof(threeLyricsWithSpacing), RecordingTimeTagCaretMoveMode.None, 2, 3, 0, 0)]
    [TestCase(nameof(threeLyricsWithSpacing), RecordingTimeTagCaretMoveMode.OnlyStartTag, 2, 3, 0, 0)]
    [TestCase(nameof(threeLyricsWithSpacing), RecordingTimeTagCaretMoveMode.OnlyEndTag, 2, 3, 0, 4)]
    public void TestMoveToPreviousLyric(string sourceName, RecordingTimeTagCaretMoveMode mode, int lyricIndex, int index, int? expectedLyricIndex, int? expectedTimeTagIndex)
    {
        var lyrics = GetLyricsByMethodName(sourceName);
        var caret = createCaretPosition(lyrics, lyricIndex, index);
        var expected = createExpectedCaretPosition(lyrics, expectedLyricIndex, expectedTimeTagIndex);

        // Check is movable
        TestMoveToPreviousLyric(lyrics, caret, expected, algorithms => algorithms.Mode = mode);
    }

    [TestCase(nameof(singleLyric), RecordingTimeTagCaretMoveMode.None, 0, 0, null, null)]
    [TestCase(nameof(twoLyricsWithText), RecordingTimeTagCaretMoveMode.None, 0, 0, 1, 0)]
    [TestCase(nameof(twoLyricsWithText), RecordingTimeTagCaretMoveMode.None, 0, 2, 1, 0)] // should move to first start index.
    [TestCase(nameof(twoLyricsWithText), RecordingTimeTagCaretMoveMode.OnlyStartTag, 0, 2, 1, 0)] // should move to first start index.
    [TestCase(nameof(twoLyricsWithText), RecordingTimeTagCaretMoveMode.OnlyEndTag, 0, 2, 1, 3)] // should move to first end index.
    [TestCase(nameof(threeLyricsWithSpacing), RecordingTimeTagCaretMoveMode.None, 0, 0, 2, 0)]
    [TestCase(nameof(threeLyricsWithSpacing), RecordingTimeTagCaretMoveMode.None, 0, 2, 2, 0)]
    [TestCase(nameof(threeLyricsWithSpacing), RecordingTimeTagCaretMoveMode.OnlyStartTag, 0, 2, 2, 0)]
    [TestCase(nameof(threeLyricsWithSpacing), RecordingTimeTagCaretMoveMode.OnlyEndTag, 0, 2, 2, 3)]
    public void TestMoveToNextLyric(string sourceName, RecordingTimeTagCaretMoveMode mode, int lyricIndex, int index, int? expectedLyricIndex, int? expectedTimeTagIndex)
    {
        var lyrics = GetLyricsByMethodName(sourceName);
        var caret = createCaretPosition(lyrics, lyricIndex, index);
        var expected = createExpectedCaretPosition(lyrics, expectedLyricIndex, expectedTimeTagIndex);

        // Check is movable
        TestMoveToNextLyric(lyrics, caret, expected, algorithms => algorithms.Mode = mode);
    }

    [TestCase(nameof(singleLyric), RecordingTimeTagCaretMoveMode.None, 0, 0)]
    [TestCase(nameof(singleLyric), RecordingTimeTagCaretMoveMode.OnlyStartTag, 0, 0)]
    [TestCase(nameof(singleLyric), RecordingTimeTagCaretMoveMode.OnlyEndTag, 0, 4)]
    [TestCase(nameof(singleLyricWithoutTimeTag), RecordingTimeTagCaretMoveMode.None, null, null)]
    [TestCase(nameof(singleLyricWithoutTimeTag), RecordingTimeTagCaretMoveMode.OnlyStartTag, null, null)]
    [TestCase(nameof(singleLyricWithoutTimeTag), RecordingTimeTagCaretMoveMode.OnlyEndTag, null, null)]
    [TestCase(nameof(twoLyricsWithText), RecordingTimeTagCaretMoveMode.None, 0, 0)]
    [TestCase(nameof(twoLyricsWithText), RecordingTimeTagCaretMoveMode.OnlyStartTag, 0, 0)]
    [TestCase(nameof(twoLyricsWithText), RecordingTimeTagCaretMoveMode.OnlyEndTag, 0, 4)]
    [TestCase(nameof(threeLyricsWithSpacing), RecordingTimeTagCaretMoveMode.None, 0, 0)]
    [TestCase(nameof(threeLyricsWithSpacing), RecordingTimeTagCaretMoveMode.OnlyStartTag, 0, 0)]
    [TestCase(nameof(threeLyricsWithSpacing), RecordingTimeTagCaretMoveMode.OnlyEndTag, 0, 4)]
    public void TestMoveToFirstLyric(string sourceName, RecordingTimeTagCaretMoveMode mode, int? expectedLyricIndex, int? expectedTimeTagIndex)
    {
        var lyrics = GetLyricsByMethodName(sourceName);
        var expected = createExpectedCaretPosition(lyrics, expectedLyricIndex, expectedTimeTagIndex);

        // Check is movable
        TestMoveToFirstLyric(lyrics, expected, algorithms => algorithms.Mode = mode);
    }

    [TestCase(nameof(singleLyric), RecordingTimeTagCaretMoveMode.None, 0, 4)]
    [TestCase(nameof(singleLyric), RecordingTimeTagCaretMoveMode.OnlyStartTag, 0, 3)]
    [TestCase(nameof(singleLyric), RecordingTimeTagCaretMoveMode.OnlyEndTag, 0, 4)]
    [TestCase(nameof(singleLyricWithoutTimeTag), RecordingTimeTagCaretMoveMode.None, null, null)]
    [TestCase(nameof(singleLyricWithoutTimeTag), RecordingTimeTagCaretMoveMode.OnlyStartTag, null, null)]
    [TestCase(nameof(singleLyricWithoutTimeTag), RecordingTimeTagCaretMoveMode.OnlyEndTag, null, null)]
    [TestCase(nameof(twoLyricsWithText), RecordingTimeTagCaretMoveMode.None, 1, 3)]
    [TestCase(nameof(twoLyricsWithText), RecordingTimeTagCaretMoveMode.OnlyStartTag, 1, 2)]
    [TestCase(nameof(twoLyricsWithText), RecordingTimeTagCaretMoveMode.OnlyEndTag, 1, 3)]
    [TestCase(nameof(threeLyricsWithSpacing), RecordingTimeTagCaretMoveMode.None, 2, 3)]
    [TestCase(nameof(threeLyricsWithSpacing), RecordingTimeTagCaretMoveMode.OnlyStartTag, 2, 2)]
    [TestCase(nameof(threeLyricsWithSpacing), RecordingTimeTagCaretMoveMode.OnlyEndTag, 2, 3)]
    public void TestMoveToLastLyric(string sourceName, RecordingTimeTagCaretMoveMode mode, int? expectedLyricIndex, int? expectedTimeTagIndex)
    {
        var lyrics = GetLyricsByMethodName(sourceName);
        var expected = createExpectedCaretPosition(lyrics, expectedLyricIndex, expectedTimeTagIndex);

        // Check is movable
        TestMoveToLastLyric(lyrics, expected, algorithms => algorithms.Mode = mode);
    }

    [TestCase(nameof(singleLyric), RecordingTimeTagCaretMoveMode.None, 0, 0, 0)]
    [TestCase(nameof(singleLyric), RecordingTimeTagCaretMoveMode.OnlyStartTag, 0, 0, 0)]
    [TestCase(nameof(singleLyric), RecordingTimeTagCaretMoveMode.OnlyEndTag, 0, 0, 4)]
    [TestCase(nameof(singleLyricWithoutTimeTag), RecordingTimeTagCaretMoveMode.None, 0, null, null)] // should not hover to the lyric if contains no time-tag in the lyric.
    [TestCase(nameof(singleLyricWithoutTimeTag), RecordingTimeTagCaretMoveMode.OnlyStartTag, 0, null, null)]
    [TestCase(nameof(singleLyricWithoutTimeTag), RecordingTimeTagCaretMoveMode.OnlyEndTag, 0, null, null)]
    [TestCase(nameof(singleLyricWithNoText), RecordingTimeTagCaretMoveMode.None, 0, null, null)] // should not hover to the lyric if contains no text and no time-tag in the lyric
    public void TestMoveToTargetLyric(string sourceName, RecordingTimeTagCaretMoveMode mode, int lyricIndex, int? expectedLyricIndex, int? expectedTimeTagIndex)
    {
        var lyrics = GetLyricsByMethodName(sourceName);
        var lyric = lyrics[lyricIndex];
        var expected = createExpectedCaretPosition(lyrics, expectedLyricIndex, expectedTimeTagIndex);

        // Check move to target position.
        TestMoveToTargetLyric(lyrics, lyric, expected, algorithms => algorithms.Mode = mode);
    }

    #endregion

    #region Lyric index

    [TestCase(nameof(singleLyric), RecordingTimeTagCaretMoveMode.None, 0, 0, null, null)]
    [TestCase(nameof(singleLyric), RecordingTimeTagCaretMoveMode.None, 0, 4, 0, 3)]
    [TestCase(nameof(singleLyric), RecordingTimeTagCaretMoveMode.OnlyStartTag, 0, 4, 0, 3)]
    [TestCase(nameof(singleLyric), RecordingTimeTagCaretMoveMode.OnlyEndTag, 0, 4, null, null)]
    public void TestMoveToPreviousIndex(string sourceName, RecordingTimeTagCaretMoveMode mode, int lyricIndex, int index, int? expectedLyricIndex, int? expectedTimeTagIndex)
    {
        var lyrics = GetLyricsByMethodName(sourceName);
        var caret = createCaretPosition(lyrics, lyricIndex, index);
        var expected = createExpectedCaretPosition(lyrics, expectedLyricIndex, expectedTimeTagIndex);

        // Check is movable
        TestMoveToPreviousIndex(lyrics, caret, expected, algorithms => algorithms.Mode = mode);
    }

    [TestCase(nameof(singleLyric), RecordingTimeTagCaretMoveMode.None, 0, 4, null, null)]
    [TestCase(nameof(singleLyric), RecordingTimeTagCaretMoveMode.None, 0, 0, 0, 1)]
    [TestCase(nameof(singleLyric), RecordingTimeTagCaretMoveMode.OnlyStartTag, 0, 0, 0, 1)]
    [TestCase(nameof(singleLyric), RecordingTimeTagCaretMoveMode.OnlyEndTag, 0, 0, 0, 4)]
    public void TestMoveToNextIndex(string sourceName, RecordingTimeTagCaretMoveMode mode, int lyricIndex, int index, int? expectedLyricIndex, int? expectedTimeTagIndex)
    {
        var lyrics = GetLyricsByMethodName(sourceName);
        var caret = createCaretPosition(lyrics, lyricIndex, index);
        var expected = createExpectedCaretPosition(lyrics, expectedLyricIndex, expectedTimeTagIndex);

        // Check is movable
        TestMoveToNextIndex(lyrics, caret, expected, algorithms => algorithms.Mode = mode);
    }

    [TestCase(nameof(singleLyric), RecordingTimeTagCaretMoveMode.None, 0, 0, 0)]
    [TestCase(nameof(singleLyric), RecordingTimeTagCaretMoveMode.OnlyStartTag, 0, 0, 0)]
    [TestCase(nameof(singleLyric), RecordingTimeTagCaretMoveMode.OnlyEndTag, 0, 0, 4)]
    [TestCase(nameof(singleLyricWithNoText), RecordingTimeTagCaretMoveMode.None, 0, null, null)]
    [TestCase(nameof(singleLyricWithNoText), RecordingTimeTagCaretMoveMode.OnlyStartTag, 0, null, null)]
    [TestCase(nameof(singleLyricWithNoText), RecordingTimeTagCaretMoveMode.OnlyEndTag, 0, null, null)]
    public void TestMoveToFirstIndex(string sourceName, RecordingTimeTagCaretMoveMode mode, int lyricIndex, int? expectedLyricIndex, int? expectedTimeTagIndex)
    {
        var lyrics = GetLyricsByMethodName(sourceName);
        var lyric = lyrics[lyricIndex];
        var expected = createExpectedCaretPosition(lyrics, expectedLyricIndex, expectedTimeTagIndex);

        // Check is movable
        TestMoveToFirstIndex(lyrics, lyric, expected, algorithms => algorithms.Mode = mode);
    }

    [TestCase(nameof(singleLyric), RecordingTimeTagCaretMoveMode.None, 0, 0, 4)]
    [TestCase(nameof(singleLyric), RecordingTimeTagCaretMoveMode.OnlyStartTag, 0, 0, 3)]
    [TestCase(nameof(singleLyric), RecordingTimeTagCaretMoveMode.OnlyEndTag, 0, 0, 4)]
    [TestCase(nameof(singleLyricWithNoText), RecordingTimeTagCaretMoveMode.None, 0, null, null)]
    [TestCase(nameof(singleLyricWithNoText), RecordingTimeTagCaretMoveMode.OnlyStartTag, 0, null, null)]
    [TestCase(nameof(singleLyricWithNoText), RecordingTimeTagCaretMoveMode.OnlyEndTag, 0, null, null)]
    public void TestMoveToLastIndex(string sourceName, RecordingTimeTagCaretMoveMode mode, int lyricIndex, int? expectedLyricIndex, int? expectedTimeTagIndex)
    {
        var lyrics = GetLyricsByMethodName(sourceName);
        var lyric = lyrics[lyricIndex];
        var expected = createExpectedCaretPosition(lyrics, expectedLyricIndex, expectedTimeTagIndex);

        // Check is movable
        TestMoveToLastIndex(lyrics, lyric, expected, algorithms => algorithms.Mode = mode);
    }

    [TestCase(nameof(singleLyric), RecordingTimeTagCaretMoveMode.None, 0, 0, 0, 0)]
    [TestCase(nameof(singleLyric), RecordingTimeTagCaretMoveMode.None, 0, 4, 0, 4)]
    public void TestMoveToTargetLyric(string sourceName, RecordingTimeTagCaretMoveMode mode, int lyricIndex, int timeTagIndex, int? expectedLyricIndex, int? expectedTimeTagIndex)
    {
        var lyrics = GetLyricsByMethodName(sourceName);
        var lyric = lyrics[lyricIndex];
        var timeTag = lyric.TimeTags.ElementAt(timeTagIndex);
        var expected = createExpectedCaretPosition(lyrics, expectedLyricIndex, expectedTimeTagIndex);

        // Check move to target position.
        TestMoveToTargetLyric(lyrics, lyric, timeTag, expected, algorithms => algorithms.Mode = mode);
    }

    #endregion

    private static RecordingTimeTagCaretPosition createCaretPosition(IEnumerable<Lyric> lyrics, int lyricIndex, int timeTagIndex)
    {
        var lyric = lyrics.ElementAtOrDefault(lyricIndex);
        var timeTag = timeTagIndex == not_exist_tag
            ? new TimeTag(new TextIndex(not_exist_tag))
            : lyric?.TimeTags.ElementAtOrDefault(timeTagIndex);

        if (lyric == null || timeTag == null)
            throw new ArgumentNullException();

        return new RecordingTimeTagCaretPosition(lyric, timeTag);
    }

    private static RecordingTimeTagCaretPosition? createExpectedCaretPosition(IEnumerable<Lyric> lyrics, int? lyricIndex, int? timeTagIndex)
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
                "[3,end]:5000",
            }),
        },
    };

    private static Lyric[] singleLyricWithoutTimeTag => new[]
    {
        new Lyric
        {
            Text = "カラオケ",
        },
    };

    private static Lyric[] singleLyricWithNoText => new[]
    {
        new Lyric(),
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
                "[3,end]:5000",
            }),
        },
        new Lyric
        {
            Text = "大好き",
            TimeTags = TestCaseTagHelper.ParseTimeTags(new[]
            {
                "[0,start]:1000",
                "[1,start]:2000",
                "[2,start]:3000",
                "[2,end]:5000",
            }),
        },
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
                "[3,end]:5000",
            }),
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
                "[2,end]:5000",
            }),
        },
    };

    #endregion
}
