// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using osu.Game.Rulesets.Karaoke.Screens.Edit.Beatmaps.Lyrics.CaretPosition;

namespace osu.Game.Rulesets.Karaoke.Screens.Edit.Beatmaps.Lyrics.States;

public class RangeCaretPosition : RangeCaretPosition<IIndexCaretPosition>
{
    public RangeCaretPosition(IIndexCaretPosition start, IIndexCaretPosition end)
        : base(start, end)
    {
    }

    public RangeCaretPosition<TIndexCaretPosition> GetRangeCaretPositionWithType<TIndexCaretPosition>()
        where TIndexCaretPosition : struct, IIndexCaretPosition
    {
        if (Start is not TIndexCaretPosition start || End is not TIndexCaretPosition end)
            throw new InvalidCastException();

        return new RangeCaretPosition<TIndexCaretPosition>(start, end);
    }
}

public class RangeCaretPosition<TIndexCaretPosition> where TIndexCaretPosition : IIndexCaretPosition
{
    public RangeCaretPosition(TIndexCaretPosition start, TIndexCaretPosition end)
    {
        Start = start;
        End = end;
    }

    public TIndexCaretPosition Start { get; }

    public TIndexCaretPosition End { get; }

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
}
