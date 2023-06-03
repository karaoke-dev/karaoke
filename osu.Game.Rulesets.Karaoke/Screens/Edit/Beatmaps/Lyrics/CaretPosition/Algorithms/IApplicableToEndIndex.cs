// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

namespace osu.Game.Rulesets.Karaoke.Screens.Edit.Beatmaps.Lyrics.CaretPosition.Algorithms;

public interface IApplicableToEndIndex
{
    IRangeIndexCaretPosition? AdjustEndIndex<TIndex>(IRangeIndexCaretPosition currentPosition, TIndex index) where TIndex : notnull;
}
