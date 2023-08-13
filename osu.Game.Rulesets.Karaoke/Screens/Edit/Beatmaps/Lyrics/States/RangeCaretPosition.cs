// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using osu.Game.Rulesets.Karaoke.Screens.Edit.Beatmaps.Lyrics.CaretPosition;

namespace osu.Game.Rulesets.Karaoke.Screens.Edit.Beatmaps.Lyrics.States;

public struct RangeCaretPosition
{
    public RangeCaretPosition(IIndexCaretPosition start, IIndexCaretPosition end)
    {
        Start = start;
        End = end;
    }

    public IIndexCaretPosition Start { get; set; }

    public IIndexCaretPosition End { get; set; }

    /// <summary>
    /// Get the range caret position with ordered.
    /// </summary>
    /// <returns></returns>
    public Tuple<IIndexCaretPosition, IIndexCaretPosition> GetRangeCaretPosition()
    {
        return Start < End
            ? new Tuple<IIndexCaretPosition, IIndexCaretPosition>(Start, End)
            : new Tuple<IIndexCaretPosition, IIndexCaretPosition>(End, Start);
    }
}
