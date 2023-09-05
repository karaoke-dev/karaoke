// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using System.Collections.Generic;
using osu.Game.Rulesets.Karaoke.Screens.Edit.Beatmaps.Lyrics.CaretPosition;

namespace osu.Game.Rulesets.Karaoke.Screens.Edit.Beatmaps.Lyrics.States;

public class RangeCaretPosition : RangeCaretPosition<IIndexCaretPosition>
{
    public RangeCaretPosition(IIndexCaretPosition start, IIndexCaretPosition end, RangeCaretDraggingState draggingState)
        : base(start, end, draggingState)
    {
    }

    public RangeCaretPosition<TIndexCaretPosition> GetRangeCaretPositionWithType<TIndexCaretPosition>()
        where TIndexCaretPosition : struct, IIndexCaretPosition
    {
        if (Start is not TIndexCaretPosition start || End is not TIndexCaretPosition end)
            throw new InvalidCastException();

        return new RangeCaretPosition<TIndexCaretPosition>(start, end, DraggingState);
    }
}

public class RangeCaretPosition<TIndexCaretPosition> : IEquatable<RangeCaretPosition<TIndexCaretPosition>> where TIndexCaretPosition : IIndexCaretPosition
{
    public RangeCaretPosition(TIndexCaretPosition start, TIndexCaretPosition end, RangeCaretDraggingState draggingState)
    {
        Start = start;
        End = end;
        DraggingState = draggingState;
    }

    public TIndexCaretPosition Start { get; }

    public TIndexCaretPosition End { get; }

    public RangeCaretDraggingState DraggingState { get; }

    /// <summary>
    /// Get the range caret position with ordered.
    /// </summary>
    /// <returns></returns>
    public Tuple<TIndexCaretPosition, TIndexCaretPosition> GetRangeCaretPosition()
    {
        return Start < End
            ? new Tuple<TIndexCaretPosition, TIndexCaretPosition>(Start, End)
            : new Tuple<TIndexCaretPosition, TIndexCaretPosition>(End, Start);
    }

    public bool Equals(RangeCaretPosition<TIndexCaretPosition>? other)
    {
        if (ReferenceEquals(null, other))
            return false;

        if (ReferenceEquals(this, other))
            return true;

        return EqualityComparer<TIndexCaretPosition>.Default.Equals(Start, other.Start)
               && EqualityComparer<TIndexCaretPosition>.Default.Equals(End, other.End)
               && DraggingState == other.DraggingState;
    }

    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(null, obj))
            return false;

        if (ReferenceEquals(this, obj))
            return true;

        return obj.GetType() == GetType() && Equals((RangeCaretPosition<TIndexCaretPosition>)obj);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Start, End);
    }
}

public enum RangeCaretDraggingState
{
    StartDrag,

    Dragging,

    EndDrag,
}
