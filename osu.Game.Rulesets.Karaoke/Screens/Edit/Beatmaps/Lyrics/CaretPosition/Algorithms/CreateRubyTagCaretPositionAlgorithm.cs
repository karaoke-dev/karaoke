// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using osu.Game.Rulesets.Karaoke.Objects;

namespace osu.Game.Rulesets.Karaoke.Screens.Edit.Beatmaps.Lyrics.CaretPosition.Algorithms;

public class CreateRubyTagCaretPositionAlgorithm : CharIndexCaretPositionAlgorithm<CreateRubyTagCaretPosition>, IApplicableToEndIndex
{
    public CreateRubyTagCaretPositionAlgorithm(Lyric[] lyrics)
        : base(lyrics)
    {
    }

    protected override bool PositionMovable(CreateRubyTagCaretPosition position)
    {
        return IndexInTextRange(position.CharIndex, position.Lyric)
               && IndexInTextRange(position.ReleaseCharIndex, position.Lyric);
    }

    protected override CreateRubyTagCaretPosition CreateCaretPosition(Lyric lyric, int index, CaretGenerateType generateType = CaretGenerateType.Action) => new(lyric, index, index, generateType);

    public IRangeIndexCaretPosition? AdjustEndIndex<TIndex>(IRangeIndexCaretPosition currentPosition, TIndex index) where TIndex : notnull
    {
        if (currentPosition is not CreateRubyTagCaretPosition createRubyTagCaretPosition)
            throw new InvalidCastException(nameof(currentPosition));

        if (index is not int endIndex)
            throw new InvalidCastException();

        var movedCaretPosition = new CreateRubyTagCaretPosition(createRubyTagCaretPosition.Lyric, createRubyTagCaretPosition.CharIndex, endIndex, CaretGenerateType.TargetLyric);
        return PostValidate(movedCaretPosition, CaretGenerateType.TargetLyric);
    }
}
