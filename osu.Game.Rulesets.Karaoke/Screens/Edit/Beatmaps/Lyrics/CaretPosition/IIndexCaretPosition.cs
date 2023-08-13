// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;

namespace osu.Game.Rulesets.Karaoke.Screens.Edit.Beatmaps.Lyrics.CaretPosition;

public interface IIndexCaretPosition : ICaretPosition, IComparable<IIndexCaretPosition>
{
    public static bool operator >(IIndexCaretPosition caretPosition1, IIndexCaretPosition caretPosition2)
        => caretPosition1.CompareTo(caretPosition2) > 0;

    public static bool operator >=(IIndexCaretPosition caretPosition1, IIndexCaretPosition caretPosition2)
        => caretPosition1.CompareTo(caretPosition2) >= 0;

    public static bool operator <(IIndexCaretPosition caretPosition1, IIndexCaretPosition caretPosition2)
        => caretPosition1.CompareTo(caretPosition2) < 0;

    public static bool operator <=(IIndexCaretPosition caretPosition1, IIndexCaretPosition caretPosition2)
        => caretPosition1.CompareTo(caretPosition2) <= 0;
}
