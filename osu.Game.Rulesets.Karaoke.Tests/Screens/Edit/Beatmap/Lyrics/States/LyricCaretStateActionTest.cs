// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using System.Collections.Generic;
using NUnit.Framework;
using osu.Game.Rulesets.Karaoke.Screens.Edit.Beatmaps.Lyrics.CaretPosition;
using osu.Game.Rulesets.Karaoke.Screens.Edit.Beatmaps.Lyrics.States;

namespace osu.Game.Rulesets.Karaoke.Tests.Screens.Edit.Beatmap.Lyrics.States;

public partial class LyricCaretStateActionTest : BaseLyricCaretStateTest
{
    #region Action with lyric

    [Test]
    public void TestGetCaretPositionByActionWithViewOnlyMode()
    {
        PrepareLyrics(new[] { "Lyric1" });
        ChangeMode(TestCaretType.ViewOnly);

        // All caret position should be null.
        AssertGetCaretPositionByAction(MovingCaretAction.PreviousLyric, () => null);
        AssertGetCaretPositionByAction(MovingCaretAction.NextLyric, () => null);
        AssertGetCaretPositionByAction(MovingCaretAction.FirstLyric, () => null);
        AssertGetCaretPositionByAction(MovingCaretAction.LastLyric, () => null);
        AssertGetCaretPositionByAction(MovingCaretAction.PreviousIndex, () => null);
        AssertGetCaretPositionByAction(MovingCaretAction.NextIndex, () => null);
        AssertGetCaretPositionByAction(MovingCaretAction.FirstIndex, () => null);
        AssertGetCaretPositionByAction(MovingCaretAction.LastIndex, () => null);
    }

    [Test]
    public void TestGetCaretPositionByActionWithCaretEnableMode()
    {
        PrepareLyrics(new[] { "Lyric1", "Lyric2" });
        ChangeMode(TestCaretType.CaretEnable);

        // previous lyric.
        PrepareCaretPosition(() => new NavigateCaretPosition(GetLyric(0)));
        AssertGetCaretPositionByAction(MovingCaretAction.PreviousLyric, () => null);
        PrepareCaretPosition(() => new NavigateCaretPosition(GetLyric(1)));
        AssertGetCaretPositionByAction(MovingCaretAction.PreviousLyric, () => new NavigateCaretPosition(GetLyric(0)));

        // next lyric.
        PrepareCaretPosition(() => new NavigateCaretPosition(GetLyric(0)));
        AssertGetCaretPositionByAction(MovingCaretAction.NextLyric, () => new NavigateCaretPosition(GetLyric(1)));
        PrepareCaretPosition(() => new NavigateCaretPosition(GetLyric(1)));
        AssertGetCaretPositionByAction(MovingCaretAction.NextLyric, () => null);

        // first lyric.
        PrepareCaretPosition(() => new NavigateCaretPosition(GetLyric(0)));
        AssertGetCaretPositionByAction(MovingCaretAction.FirstLyric, () => new NavigateCaretPosition(GetLyric(0)));
        PrepareCaretPosition(() => new NavigateCaretPosition(GetLyric(1)));
        AssertGetCaretPositionByAction(MovingCaretAction.FirstLyric, () => new NavigateCaretPosition(GetLyric(0)));

        // last lyric.
        PrepareCaretPosition(() => new NavigateCaretPosition(GetLyric(0)));
        AssertGetCaretPositionByAction(MovingCaretAction.LastLyric, () => new NavigateCaretPosition(GetLyric(1)));
        PrepareCaretPosition(() => new NavigateCaretPosition(GetLyric(1)));
        AssertGetCaretPositionByAction(MovingCaretAction.LastLyric, () => new NavigateCaretPosition(GetLyric(1)));

        // should not have value in the previous index.
        PrepareCaretPosition(() => new NavigateCaretPosition(GetLyric(0)));
        AssertGetCaretPositionByAction(MovingCaretAction.PreviousIndex, () => null);

        // should not have value in the next index.
        PrepareCaretPosition(() => new NavigateCaretPosition(GetLyric(0)));
        AssertGetCaretPositionByAction(MovingCaretAction.NextIndex, () => null);

        // should not have value in the first index.
        PrepareCaretPosition(() => new NavigateCaretPosition(GetLyric(0)));
        AssertGetCaretPositionByAction(MovingCaretAction.FirstIndex, () => null);

        // should not have value in the last index.
        PrepareCaretPosition(() => new NavigateCaretPosition(GetLyric(0)));
        AssertGetCaretPositionByAction(MovingCaretAction.LastIndex, () => null);
    }

    [Test]
    public void TestGetCaretPositionByActionWithCaretWithIndexMode()
    {
        PrepareLyrics(new[] { "Lyric1", "Lyric2" });
        ChangeMode(TestCaretType.CaretWithIndex);

        // previous lyric.
        PrepareCaretPosition(() => new CuttingCaretPosition(GetLyric(0), 1));
        AssertGetCaretPositionByAction(MovingCaretAction.PreviousLyric, () => null);
        PrepareCaretPosition(() => new CuttingCaretPosition(GetLyric(1), 1));
        AssertGetCaretPositionByAction(MovingCaretAction.PreviousLyric, () => new CuttingCaretPosition(GetLyric(0), 1));

        // next lyric.
        PrepareCaretPosition(() => new CuttingCaretPosition(GetLyric(0), 1));
        AssertGetCaretPositionByAction(MovingCaretAction.NextLyric, () => new CuttingCaretPosition(GetLyric(1), 1));
        PrepareCaretPosition(() => new CuttingCaretPosition(GetLyric(1), 1));
        AssertGetCaretPositionByAction(MovingCaretAction.NextLyric, () => null);

        // first lyric.
        PrepareCaretPosition(() => new CuttingCaretPosition(GetLyric(0), 1));
        AssertGetCaretPositionByAction(MovingCaretAction.FirstLyric, () => new CuttingCaretPosition(GetLyric(0), 1));
        PrepareCaretPosition(() => new CuttingCaretPosition(GetLyric(1), 1));
        AssertGetCaretPositionByAction(MovingCaretAction.FirstLyric, () => new CuttingCaretPosition(GetLyric(0), 1));

        // last lyric.
        PrepareCaretPosition(() => new CuttingCaretPosition(GetLyric(0), 1));
        AssertGetCaretPositionByAction(MovingCaretAction.LastLyric, () => new CuttingCaretPosition(GetLyric(1), 5));
        PrepareCaretPosition(() => new CuttingCaretPosition(GetLyric(1), 1));
        AssertGetCaretPositionByAction(MovingCaretAction.LastLyric, () => new CuttingCaretPosition(GetLyric(1), 5));

        // previous index.
        PrepareCaretPosition(() => new CuttingCaretPosition(GetLyric(0), 1));
        AssertGetCaretPositionByAction(MovingCaretAction.PreviousIndex, () => null);
        PrepareCaretPosition(() => new CuttingCaretPosition(GetLyric(0), 5));
        AssertGetCaretPositionByAction(MovingCaretAction.PreviousIndex, () => new CuttingCaretPosition(GetLyric(0), 4));
        PrepareCaretPosition(() => new CuttingCaretPosition(GetLyric(1), 1)); // should change to the previous lyric
        AssertGetCaretPositionByAction(MovingCaretAction.PreviousIndex, () => new CuttingCaretPosition(GetLyric(0), 5));

        // next index.
        PrepareCaretPosition(() => new CuttingCaretPosition(GetLyric(1), 1));
        AssertGetCaretPositionByAction(MovingCaretAction.NextIndex, () => new CuttingCaretPosition(GetLyric(1), 2));
        PrepareCaretPosition(() => new CuttingCaretPosition(GetLyric(1), 5));
        AssertGetCaretPositionByAction(MovingCaretAction.NextIndex, () => null);
        PrepareCaretPosition(() => new CuttingCaretPosition(GetLyric(0), 5)); // should change to the next lyric
        AssertGetCaretPositionByAction(MovingCaretAction.NextIndex, () => new CuttingCaretPosition(GetLyric(1), 1));

        // first index.
        PrepareCaretPosition(() => new CuttingCaretPosition(GetLyric(0), 1));
        AssertGetCaretPositionByAction(MovingCaretAction.FirstIndex, () => null);
        PrepareCaretPosition(() => new CuttingCaretPosition(GetLyric(0), 5));
        AssertGetCaretPositionByAction(MovingCaretAction.FirstIndex, () => new CuttingCaretPosition(GetLyric(0), 1));
        PrepareCaretPosition(() => new CuttingCaretPosition(GetLyric(1), 1)); // should change to the previous lyric
        AssertGetCaretPositionByAction(MovingCaretAction.FirstIndex, () => new CuttingCaretPosition(GetLyric(0), 1));

        // last index.
        PrepareCaretPosition(() => new CuttingCaretPosition(GetLyric(1), 1));
        AssertGetCaretPositionByAction(MovingCaretAction.LastIndex, () => new CuttingCaretPosition(GetLyric(1), 5));
        PrepareCaretPosition(() => new CuttingCaretPosition(GetLyric(1), 5));
        AssertGetCaretPositionByAction(MovingCaretAction.LastIndex, () => null);
        PrepareCaretPosition(() => new CuttingCaretPosition(GetLyric(0), 5)); // should change to the next lyric
        AssertGetCaretPositionByAction(MovingCaretAction.LastIndex, () => new CuttingCaretPosition(GetLyric(1), 5));
    }

    [Test]
    public void TestGetCaretPositionByActionWithCaretDraggableMode()
    {
        PrepareLyrics(new[] { "Lyric1", "Lyric2" });
        ChangeMode(TestCaretType.CaretDraggable);

        // previous lyric.
        PrepareCaretPosition(() => new TypingCaretPosition(GetLyric(0), 0));
        AssertGetCaretPositionByAction(MovingCaretAction.PreviousLyric, () => null);
        PrepareCaretPosition(() => new TypingCaretPosition(GetLyric(1), 0));
        AssertGetCaretPositionByAction(MovingCaretAction.PreviousLyric, () => new TypingCaretPosition(GetLyric(0), 0));

        // next lyric.
        PrepareCaretPosition(() => new TypingCaretPosition(GetLyric(0), 0));
        AssertGetCaretPositionByAction(MovingCaretAction.NextLyric, () => new TypingCaretPosition(GetLyric(1), 0));
        PrepareCaretPosition(() => new TypingCaretPosition(GetLyric(1), 0));
        AssertGetCaretPositionByAction(MovingCaretAction.NextLyric, () => null);

        // first lyric.
        PrepareCaretPosition(() => new TypingCaretPosition(GetLyric(0), 0));
        AssertGetCaretPositionByAction(MovingCaretAction.FirstLyric, () => new TypingCaretPosition(GetLyric(0), 0));
        PrepareCaretPosition(() => new TypingCaretPosition(GetLyric(1), 0));
        AssertGetCaretPositionByAction(MovingCaretAction.FirstLyric, () => new TypingCaretPosition(GetLyric(0), 0));

        // last lyric.
        PrepareCaretPosition(() => new TypingCaretPosition(GetLyric(0), 0));
        AssertGetCaretPositionByAction(MovingCaretAction.LastLyric, () => new TypingCaretPosition(GetLyric(1), 6));
        PrepareCaretPosition(() => new TypingCaretPosition(GetLyric(1), 0));
        AssertGetCaretPositionByAction(MovingCaretAction.LastLyric, () => new TypingCaretPosition(GetLyric(1), 6));

        // previous index.
        PrepareCaretPosition(() => new TypingCaretPosition(GetLyric(0), 0));
        AssertGetCaretPositionByAction(MovingCaretAction.PreviousIndex, () => null);
        PrepareCaretPosition(() => new TypingCaretPosition(GetLyric(0), 6));
        AssertGetCaretPositionByAction(MovingCaretAction.PreviousIndex, () => new TypingCaretPosition(GetLyric(0), 5));
        PrepareCaretPosition(() => new TypingCaretPosition(GetLyric(1), 0)); // should change to the previous lyric
        AssertGetCaretPositionByAction(MovingCaretAction.PreviousIndex, () => new TypingCaretPosition(GetLyric(0), 6));

        // next index.
        PrepareCaretPosition(() => new TypingCaretPosition(GetLyric(1), 0));
        AssertGetCaretPositionByAction(MovingCaretAction.NextIndex, () => new TypingCaretPosition(GetLyric(1), 1));
        PrepareCaretPosition(() => new TypingCaretPosition(GetLyric(1), 6));
        AssertGetCaretPositionByAction(MovingCaretAction.NextIndex, () => null);
        PrepareCaretPosition(() => new TypingCaretPosition(GetLyric(0), 6)); // should change to the next lyric
        AssertGetCaretPositionByAction(MovingCaretAction.NextIndex, () => new TypingCaretPosition(GetLyric(1), 0));

        // first index.
        PrepareCaretPosition(() => new TypingCaretPosition(GetLyric(0), 0));
        AssertGetCaretPositionByAction(MovingCaretAction.FirstIndex, () => null);
        PrepareCaretPosition(() => new TypingCaretPosition(GetLyric(0), 6));
        AssertGetCaretPositionByAction(MovingCaretAction.FirstIndex, () => new TypingCaretPosition(GetLyric(0), 0));
        PrepareCaretPosition(() => new TypingCaretPosition(GetLyric(1), 0)); // should change to the previous lyric
        AssertGetCaretPositionByAction(MovingCaretAction.FirstIndex, () => new TypingCaretPosition(GetLyric(0), 0));

        // last index.
        PrepareCaretPosition(() => new TypingCaretPosition(GetLyric(1), 0));
        AssertGetCaretPositionByAction(MovingCaretAction.LastIndex, () => new TypingCaretPosition(GetLyric(1), 6));
        PrepareCaretPosition(() => new TypingCaretPosition(GetLyric(1), 6));
        AssertGetCaretPositionByAction(MovingCaretAction.LastIndex, () => null);
        PrepareCaretPosition(() => new TypingCaretPosition(GetLyric(0), 6)); // should change to the next lyric
        AssertGetCaretPositionByAction(MovingCaretAction.LastIndex, () => new TypingCaretPosition(GetLyric(1), 6));
    }

    [Test]
    public void TestGetCaretPositionByActionWithCaretDraggableModeWithDragRange()
    {
        PrepareLyrics(new[] { "Lyric1", "Lyric2", "Lyric3" });
        ChangeMode(TestCaretType.CaretDraggable);

        // Testing the case without selecting the whole lyric.
        PrepareRangeCaretPosition(() =>
        {
            var startIndex = new TypingCaretPosition(GetLyric(1), 1);
            var endIndex = new TypingCaretPosition(GetLyric(1), 5);

            return new RangeCaretPosition(startIndex, endIndex, RangeCaretDraggingState.EndDrag);
        });
        AssertGetCaretPositionByAction(MovingCaretAction.PreviousLyric, () => new TypingCaretPosition(GetLyric(0), 1));
        AssertGetCaretPositionByAction(MovingCaretAction.NextLyric, () => new TypingCaretPosition(GetLyric(2), 1));
        AssertGetCaretPositionByAction(MovingCaretAction.FirstLyric, () => new TypingCaretPosition(GetLyric(0), 0));
        AssertGetCaretPositionByAction(MovingCaretAction.LastLyric, () => new TypingCaretPosition(GetLyric(2), 6));
        AssertGetCaretPositionByAction(MovingCaretAction.PreviousIndex, () => new TypingCaretPosition(GetLyric(1), 1));
        AssertGetCaretPositionByAction(MovingCaretAction.NextIndex, () => new TypingCaretPosition(GetLyric(1), 5));
        AssertGetCaretPositionByAction(MovingCaretAction.FirstIndex, () => new TypingCaretPosition(GetLyric(1), 0));
        AssertGetCaretPositionByAction(MovingCaretAction.LastIndex, () => new TypingCaretPosition(GetLyric(1), 6));

        // Testing the case with select the whole lyric.
        PrepareRangeCaretPosition(() =>
        {
            var startIndex = new TypingCaretPosition(GetLyric(1), 0);
            var endIndex = new TypingCaretPosition(GetLyric(1), 6);

            return new RangeCaretPosition(startIndex, endIndex, RangeCaretDraggingState.EndDrag);
        });
        AssertGetCaretPositionByAction(MovingCaretAction.PreviousLyric, () => new TypingCaretPosition(GetLyric(0), 0));
        AssertGetCaretPositionByAction(MovingCaretAction.NextLyric, () => new TypingCaretPosition(GetLyric(2), 0));
        AssertGetCaretPositionByAction(MovingCaretAction.FirstLyric, () => new TypingCaretPosition(GetLyric(0), 0));
        AssertGetCaretPositionByAction(MovingCaretAction.LastLyric, () => new TypingCaretPosition(GetLyric(2), 6));
        AssertGetCaretPositionByAction(MovingCaretAction.PreviousIndex, () => new TypingCaretPosition(GetLyric(1), 0));
        AssertGetCaretPositionByAction(MovingCaretAction.NextIndex, () => new TypingCaretPosition(GetLyric(1), 6));
        AssertGetCaretPositionByAction(MovingCaretAction.FirstIndex, () => new TypingCaretPosition(GetLyric(1), 0));
        AssertGetCaretPositionByAction(MovingCaretAction.LastIndex, () => new TypingCaretPosition(GetLyric(1), 6));

        // Now, test the case with single lyric.
        PrepareLyrics(new[] { "Lyric1" });

        // Testing the case without selecting the whole lyric.
        PrepareRangeCaretPosition(() =>
        {
            var startIndex = new TypingCaretPosition(GetLyric(0), 1);
            var endIndex = new TypingCaretPosition(GetLyric(0), 5);

            return new RangeCaretPosition(startIndex, endIndex, RangeCaretDraggingState.EndDrag);
        });
        AssertGetCaretPositionByAction(MovingCaretAction.PreviousLyric, () => null);
        AssertGetCaretPositionByAction(MovingCaretAction.NextLyric, () => null);
        AssertGetCaretPositionByAction(MovingCaretAction.FirstLyric, () => new TypingCaretPosition(GetLyric(0), 0));
        AssertGetCaretPositionByAction(MovingCaretAction.LastLyric, () => new TypingCaretPosition(GetLyric(0), 6));
        AssertGetCaretPositionByAction(MovingCaretAction.PreviousIndex, () => new TypingCaretPosition(GetLyric(0), 1));
        AssertGetCaretPositionByAction(MovingCaretAction.NextIndex, () => new TypingCaretPosition(GetLyric(0), 5));
        AssertGetCaretPositionByAction(MovingCaretAction.FirstIndex, () => new TypingCaretPosition(GetLyric(0), 0));
        AssertGetCaretPositionByAction(MovingCaretAction.LastIndex, () => new TypingCaretPosition(GetLyric(0), 6));

        // Testing the case with select the whole lyric.
        PrepareRangeCaretPosition(() =>
        {
            var startIndex = new TypingCaretPosition(GetLyric(0), 0);
            var endIndex = new TypingCaretPosition(GetLyric(0), 6);

            return new RangeCaretPosition(startIndex, endIndex, RangeCaretDraggingState.EndDrag);
        });
        AssertGetCaretPositionByAction(MovingCaretAction.PreviousLyric, () => null);
        AssertGetCaretPositionByAction(MovingCaretAction.NextLyric, () => null);
        AssertGetCaretPositionByAction(MovingCaretAction.FirstLyric, () => new TypingCaretPosition(GetLyric(0), 0));
        AssertGetCaretPositionByAction(MovingCaretAction.LastLyric, () => new TypingCaretPosition(GetLyric(0), 6));
        AssertGetCaretPositionByAction(MovingCaretAction.PreviousIndex, () => new TypingCaretPosition(GetLyric(0), 0));
        AssertGetCaretPositionByAction(MovingCaretAction.NextIndex, () => new TypingCaretPosition(GetLyric(0), 6));
        AssertGetCaretPositionByAction(MovingCaretAction.FirstIndex, () => new TypingCaretPosition(GetLyric(0), 0));
        AssertGetCaretPositionByAction(MovingCaretAction.LastIndex, () => new TypingCaretPosition(GetLyric(0), 6));
    }

    #endregion

    #region Action with not lyric

    [Test]
    public void TestGetCaretPositionByActionWithNoLyric([Values] TestCaretType type, [Values] MovingCaretAction action)
    {
        PrepareLyrics(Array.Empty<string>());
        ChangeMode(type);

        // make sure that action should not cause the exception and should not change the caret position.
        AssertGetCaretPositionByAction(action, () => null);
    }

    #endregion

    #region Test utility

    protected void AssertGetCaretPositionByAction(MovingCaretAction action, Func<ICaretPosition?> getPosition)
    {
        AddAssert("Assert caret position by action", () =>
        {
            var expectedCaret = getPosition();
            var actualCaret = LyricCaretState.GetCaretPositionByAction(action);

            return EqualityComparer<ICaretPosition?>.Default.Equals(expectedCaret, actualCaret);
        });
    }

    #endregion
}
