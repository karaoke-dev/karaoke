// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using NUnit.Framework;
using osu.Game.Rulesets.Karaoke.Screens.Edit.Beatmaps.Lyrics.CaretPosition;
using osu.Game.Rulesets.Karaoke.Screens.Edit.Beatmaps.Lyrics.States;

namespace osu.Game.Rulesets.Karaoke.Tests.Screens.Edit.Beatmap.Lyrics.States;

public partial class LyricCaretStateSwitchModeTest : BaseLyricCaretStateTest
{
    #region Default state

    [Test]
    public void TestViewOnlyMode()
    {
        PrepareLyrics(new[] { "Lyric1", "Lyric2" });
        ChangeMode(TestCaretType.ViewOnly);

        AssertHoverCaretPosition(() => null);
        AssertCaretPosition(() => null);
        AssertDraggableCaretPosition(() => null);

        AssertCaretEnabled(false);
        AssertCaretDraggable(false);
    }

    [Test]
    public void TestCaretEnableMode()
    {
        PrepareLyrics(new[] { "Lyric1", "Lyric2" });
        ChangeMode(TestCaretType.CaretEnable);

        AssertHoverCaretPosition(() => null);
        AssertCaretPosition(() => new NavigateCaretPosition(GetLyric(0)));
        AssertDraggableCaretPosition(() => null);

        AssertCaretEnabled(true);
        AssertCaretDraggable(false);
    }

    [Test]
    public void TestCaretWithIndexMode()
    {
        PrepareLyrics(new[] { "Lyric1", "Lyric2" });
        ChangeMode(TestCaretType.CaretWithIndex);

        AssertHoverCaretPosition(() => null);
        AssertCaretPosition(() => new CuttingCaretPosition(GetLyric(0), 1));
        AssertDraggableCaretPosition(() => null);

        AssertCaretEnabled(true);
        AssertCaretDraggable(false);
    }

    [Test]
    public void TestCaretDraggableMode()
    {
        PrepareLyrics(new[] { "Lyric1", "Lyric2" });
        ChangeMode(TestCaretType.CaretDraggable);

        AssertHoverCaretPosition(() => null);
        AssertCaretPosition(() => new TypingCaretPosition(GetLyric(0), 0));
        AssertDraggableCaretPosition(() => null);

        AssertCaretEnabled(true);
        AssertCaretDraggable(true);
    }

    #endregion

    #region Default state with no lyric

    [Test]
    public void TestDefaultModeWithNoLyric([Values] TestCaretType type)
    {
        PrepareLyrics(Array.Empty<string>());
        ChangeMode(type);

        AssertHoverCaretPosition(() => null);
        AssertCaretPosition(() => null);
        AssertDraggableCaretPosition(() => null);
    }

    #endregion

    #region Switch mode

    [Test]
    public void TestSwitchModeNotCauseBroken([Values] TestCaretType currentMode, [Values] TestCaretType nextEditMode)
    {
        PrepareLyrics(new[] { "Lyric1", "Lyric2" });
        ChangeMode(currentMode);
        ChangeMode(nextEditMode);
    }

    [Test]
    public void TestSwitchTestFromRangeOfCaret()
    {
        PrepareLyrics(new[] { "Lyric1", "Lyric2" });
        ChangeMode(TestCaretType.CaretDraggable);
        PrepareCaretPosition(() => new TypingCaretPosition(GetLyric(0), 0));
        PrepareRangeCaretPosition(() =>
        {
            var startIndex = new TypingCaretPosition(GetLyric(0), 0);
            var endIndex = new TypingCaretPosition(GetLyric(0), 0);

            return new RangeCaretPosition(startIndex, endIndex);
        });

        ChangeMode(TestCaretType.ViewOnly);

        AssertHoverCaretPosition(() => null);
        AssertCaretPosition(() => null);
        AssertDraggableCaretPosition(() => null);
    }

    #endregion

    #region Test utility

    protected void AssertCaretEnabled(bool enabled)
    {
        AddAssert("Assert caret enabled", () => LyricCaretState.CaretEnabled == enabled);
    }

    protected void AssertCaretDraggable(bool caretDraggable)
    {
        AddAssert("Assert caret draggable", () => LyricCaretState.CaretDraggable == caretDraggable);
    }

    #endregion
}
