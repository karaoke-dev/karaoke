// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using NUnit.Framework;
using osu.Game.Rulesets.Karaoke.Objects;
using osu.Game.Rulesets.Karaoke.Screens.Edit.Beatmaps.Lyrics.CaretPosition;
using osu.Game.Rulesets.Karaoke.Screens.Edit.Beatmaps.Lyrics.States;

namespace osu.Game.Rulesets.Karaoke.Tests.Screens.Edit.Beatmap.Lyrics.States;

public partial class LyricCaretStateMoveCaretTest : BaseLyricCaretStateTest
{
    #region Hover lyric

    [Test]
    public void TestMoveHoverCaretToTargetPosition()
    {
        PrepareLyrics(new[] { "Lyric1", "Lyric2" });

        // view only.
        ChangeMode(TestCaretType.ViewOnly);
        MoveHoverCaretToTargetPosition(() => GetLyric(1));

        AssertHoverCaretPosition(() => null);
        AssertCaretPosition(() => null);
        AssertDraggableCaretPosition(() => null);

        // caret enable.
        ChangeMode(TestCaretType.CaretEnable);
        MoveHoverCaretToTargetPosition(() => GetLyric(1));

        AssertHoverCaretPosition(() => new NavigateCaretPosition(GetLyric(1)));
        AssertCaretPosition(() => new NavigateCaretPosition(GetLyric(0)));
        AssertDraggableCaretPosition(() => null);

        // caret with index.
        ChangeMode(TestCaretType.CaretWithIndex);
        MoveHoverCaretToTargetPosition(() => GetLyric(1));

        AssertHoverCaretPosition(() => new CuttingCaretPosition(GetLyric(1), 1));
        AssertCaretPosition(() => new CuttingCaretPosition(GetLyric(0), 1));
        AssertDraggableCaretPosition(() => null);

        // caret with index (2).
        ChangeMode(TestCaretType.CaretWithIndex);
        MoveHoverCaretToTargetPosition(() => GetLyric(1), () => 2);

        AssertHoverCaretPosition(() => new CuttingCaretPosition(GetLyric(1), 2));
        AssertCaretPosition(() => new CuttingCaretPosition(GetLyric(0), 1));
        AssertDraggableCaretPosition(() => null);

        // caret draggable.
        ChangeMode(TestCaretType.CaretDraggable);
        MoveHoverCaretToTargetPosition(() => GetLyric(1));

        AssertHoverCaretPosition(() => new TypingCaretPosition(GetLyric(1), 0));
        AssertCaretPosition(() => new TypingCaretPosition(GetLyric(0), 0));
        AssertDraggableCaretPosition(() => null);

        // caret draggable (2).
        ChangeMode(TestCaretType.CaretDraggable);
        MoveHoverCaretToTargetPosition(() => GetLyric(1), () => 1);

        AssertHoverCaretPosition(() => new TypingCaretPosition(GetLyric(1), 1));
        AssertCaretPosition(() => new TypingCaretPosition(GetLyric(0), 0));
        AssertDraggableCaretPosition(() => null);
    }

    [Test]
    public void TestConfirmHoverCaretPosition()
    {
        PrepareLyrics(new[] { "Lyric1", "Lyric2" });

        // view only.
        ChangeMode(TestCaretType.ViewOnly);
        ConfirmHoverCaretPosition();

        AssertHoverCaretPosition(() => null);
        AssertCaretPosition(() => null);
        AssertDraggableCaretPosition(() => null);

        // caret enable.
        ChangeMode(TestCaretType.CaretEnable);
        PrepareHoverCaretPosition(() => new NavigateCaretPosition(GetLyric(1)));
        ConfirmHoverCaretPosition();

        AssertHoverCaretPosition(() => null);
        AssertCaretPosition(() => new NavigateCaretPosition(GetLyric(1)));
        AssertDraggableCaretPosition(() => null);

        // caret with index.
        ChangeMode(TestCaretType.CaretWithIndex);
        PrepareHoverCaretPosition(() => new CuttingCaretPosition(GetLyric(1), 1));
        ConfirmHoverCaretPosition();

        AssertHoverCaretPosition(() => null);
        AssertCaretPosition(() => new CuttingCaretPosition(GetLyric(1), 1));
        AssertDraggableCaretPosition(() => null);

        // caret draggable.
        ChangeMode(TestCaretType.CaretDraggable);
        PrepareHoverCaretPosition(() => new TypingCaretPosition(GetLyric(1), 0));
        ConfirmHoverCaretPosition();

        AssertHoverCaretPosition(() => null);
        AssertCaretPosition(() => new TypingCaretPosition(GetLyric(1), 0));
        AssertDraggableCaretPosition(() => null);
    }

    [Test]
    public void TestClearHoverCaretPosition()
    {
        PrepareLyrics(new[] { "Lyric1", "Lyric2" });

        // view only.
        ChangeMode(TestCaretType.ViewOnly);
        ClearHoverCaretPosition();

        AssertHoverCaretPosition(() => null);
        AssertCaretPosition(() => null);
        AssertDraggableCaretPosition(() => null);

        // caret enable.
        ChangeMode(TestCaretType.CaretEnable);
        PrepareHoverCaretPosition(() => new NavigateCaretPosition(GetLyric(1)));
        ClearHoverCaretPosition();

        AssertHoverCaretPosition(() => null);
        AssertCaretPosition(() => new NavigateCaretPosition(GetLyric(0)));
        AssertDraggableCaretPosition(() => null);

        // caret with index.
        ChangeMode(TestCaretType.CaretWithIndex);
        PrepareHoverCaretPosition(() => new CuttingCaretPosition(GetLyric(1), 1));
        ClearHoverCaretPosition();

        AssertHoverCaretPosition(() => null);
        AssertCaretPosition(() => new CuttingCaretPosition(GetLyric(0), 1));
        AssertDraggableCaretPosition(() => null);

        // caret draggable.
        ChangeMode(TestCaretType.CaretDraggable);
        PrepareHoverCaretPosition(() => new TypingCaretPosition(GetLyric(1), 0));
        ClearHoverCaretPosition();

        AssertHoverCaretPosition(() => null);
        AssertCaretPosition(() => new TypingCaretPosition(GetLyric(0), 0));
        AssertDraggableCaretPosition(() => null);
    }

    [Test]
    public void TestMoveCaretToTargetPosition()
    {
        PrepareLyrics(new[] { "Lyric1", "Lyric2" });

        // view only.
        ChangeMode(TestCaretType.ViewOnly);
        MoveCaretToTargetPosition(() => GetLyric(1));

        AssertHoverCaretPosition(() => null);
        AssertCaretPosition(() => null);
        AssertDraggableCaretPosition(() => null);

        // caret enable.
        ChangeMode(TestCaretType.CaretEnable);
        MoveCaretToTargetPosition(() => GetLyric(1));

        AssertHoverCaretPosition(() => null);
        AssertCaretPosition(() => new NavigateCaretPosition(GetLyric(1)));
        AssertDraggableCaretPosition(() => null);

        // caret with index.
        ChangeMode(TestCaretType.CaretWithIndex);
        MoveCaretToTargetPosition(() => GetLyric(1));

        AssertHoverCaretPosition(() => null);
        AssertCaretPosition(() => new CuttingCaretPosition(GetLyric(1), 1));
        AssertDraggableCaretPosition(() => null);

        // caret with index (2).
        ChangeMode(TestCaretType.CaretWithIndex);
        MoveCaretToTargetPosition(() => GetLyric(1), () => 2);

        AssertHoverCaretPosition(() => null);
        AssertCaretPosition(() => new CuttingCaretPosition(GetLyric(1), 2));
        AssertDraggableCaretPosition(() => null);

        // caret draggable.
        ChangeMode(TestCaretType.CaretDraggable);
        MoveCaretToTargetPosition(() => GetLyric(1));

        AssertHoverCaretPosition(() => null);
        AssertCaretPosition(() => new TypingCaretPosition(GetLyric(1), 0));
        AssertDraggableCaretPosition(() => null);

        // caret draggable (2).
        ChangeMode(TestCaretType.CaretDraggable);
        MoveCaretToTargetPosition(() => GetLyric(1), () => 1);

        AssertHoverCaretPosition(() => null);
        AssertCaretPosition(() => new TypingCaretPosition(GetLyric(1), 1));
        AssertDraggableCaretPosition(() => null);
    }

    [Test]
    public void TestMoveDraggingCaretIndex()
    {
        PrepareLyrics(new[] { "Lyric1", "Lyric2" });
        ChangeMode(TestCaretType.CaretDraggable);
        MoveCaretToTargetPosition(() => GetLyric(1), () => 1);

        // start dragging.
        StartDragging();
        AssertHoverCaretPosition(() => null);
        AssertCaretPosition(() => new TypingCaretPosition(GetLyric(1), 1));
        AssertDraggableCaretPosition(() =>
        {
            var startPosition = new TypingCaretPosition(GetLyric(1), 1);
            var endPosition = new TypingCaretPosition(GetLyric(1), 1);

            return new RangeCaretPosition(startPosition, endPosition, RangeCaretDraggingState.StartDrag);
        });

        // move dragging index.
        MoveDraggingCaretIndex(() => 2);
        AssertHoverCaretPosition(() => null);
        AssertCaretPosition(() => new TypingCaretPosition(GetLyric(1), 1));
        AssertDraggableCaretPosition(() =>
        {
            var startPosition = new TypingCaretPosition(GetLyric(1), 1);
            var endPosition = new TypingCaretPosition(GetLyric(1), 2);

            return new RangeCaretPosition(startPosition, endPosition, RangeCaretDraggingState.Dragging);
        });

        // end dragging.
        EndDragging();
        AssertHoverCaretPosition(() => null);
        AssertCaretPosition(() => new TypingCaretPosition(GetLyric(1), 1));
        AssertDraggableCaretPosition(() =>
        {
            var startPosition = new TypingCaretPosition(GetLyric(1), 1);
            var endPosition = new TypingCaretPosition(GetLyric(1), 2);

            return new RangeCaretPosition(startPosition, endPosition, RangeCaretDraggingState.EndDrag);
        });
    }

    #endregion

    #region Test utility

    protected void MoveHoverCaretToTargetPosition(Func<Lyric> lyric)
    {
        AddStep("Moving hover caret by action", () =>
        {
            LyricCaretState.MoveHoverCaretToTargetPosition(lyric());
        });
    }

    protected void MoveHoverCaretToTargetPosition<TIndex>(Func<Lyric> lyric, Func<TIndex> index)
        where TIndex : notnull
    {
        AddStep("Moving hover caret by caret position", () =>
        {
            LyricCaretState.MoveHoverCaretToTargetPosition(lyric(), index());
        });
    }

    protected void ConfirmHoverCaretPosition()
    {
        AddStep("Moving hover caret by action", () =>
        {
            LyricCaretState.ConfirmHoverCaretPosition();
        });
    }

    protected void ClearHoverCaretPosition()
    {
        AddStep("Moving hover caret by action", () =>
        {
            LyricCaretState.ClearHoverCaretPosition();
        });
    }

    protected void MoveCaretToTargetPosition(Func<Lyric> lyric)
    {
        AddStep("Moving caret by action", () =>
        {
            LyricCaretState.MoveCaretToTargetPosition(lyric());
        });
    }

    protected void MoveCaretToTargetPosition<TIndex>(Func<Lyric> lyric, Func<TIndex> index)
        where TIndex : notnull
    {
        AddStep("Moving caret by action", () =>
        {
            LyricCaretState.MoveCaretToTargetPosition(lyric(), index());
        });
    }

    protected void StartDragging()
    {
        AddStep("Start dragging", () =>
        {
            LyricCaretState.StartDragging();
        });
    }

    protected void MoveDraggingCaretIndex<TIndex>(Func<TIndex> index)
        where TIndex : notnull
    {
        AddStep("Moving caret by caret position", () =>
        {
            LyricCaretState.MoveDraggingCaretIndex(index());
        });
    }

    protected void EndDragging()
    {
        AddStep("End dragging", () =>
        {
            LyricCaretState.EndDragging();
        });
    }

    #endregion
}
