// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using osu.Game.Rulesets.Karaoke.Objects;

namespace osu.Game.Rulesets.Karaoke.Screens.Edit.Beatmaps.Lyrics.CaretPosition.Algorithms;

/// <summary>
/// Algorithm for move the caret position indicate the position that type to the <see cref="Lyric.Text"/>.
/// </summary>
public class TypingCaretPositionAlgorithm : CharGapCaretPositionAlgorithm<TypingCaretPosition>, IApplicableToEndIndex
{
    public TypingCaretPositionAlgorithm(Lyric[] lyrics)
        : base(lyrics)
    {
    }

    protected override bool PositionMovable(TypingCaretPosition position)
    {
        return IndexInTextRange(position.CharGap, position.Lyric)
               && IndexInTextRange(position.ReleaseCharGap, position.Lyric);
    }

    protected override TypingCaretPosition CreateCaretPosition(Lyric lyric, int index, CaretGenerateType generateType = CaretGenerateType.Action) => new(lyric, index, index, generateType);

    protected override int GetMinIndex(string text) => 0;

    protected override int GetMaxIndex(string text) => text.Length;

    public IRangeIndexCaretPosition? AdjustEndIndex<TIndex>(IRangeIndexCaretPosition currentPosition, TIndex index) where TIndex : notnull
    {
        if (currentPosition is not TypingCaretPosition createRubyTagCaretPosition)
            throw new InvalidCastException(nameof(currentPosition));

        if (index is not int charGap)
            throw new InvalidCastException();

        var movedCaretPosition = new TypingCaretPosition(createRubyTagCaretPosition.Lyric, createRubyTagCaretPosition.CharGap, charGap, CaretGenerateType.TargetLyric);
        return PostValidate(movedCaretPosition, CaretGenerateType.TargetLyric);
    }
}
