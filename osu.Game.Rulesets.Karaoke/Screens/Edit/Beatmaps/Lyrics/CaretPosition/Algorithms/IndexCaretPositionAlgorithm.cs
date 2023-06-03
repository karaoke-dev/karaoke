// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using osu.Game.Rulesets.Karaoke.Objects;

namespace osu.Game.Rulesets.Karaoke.Screens.Edit.Beatmaps.Lyrics.CaretPosition.Algorithms;

/// <summary>
/// Base class for move the <see cref="ICaretPosition"/> to the previous or next position with target index.
/// </summary>
/// <typeparam name="TCaretPosition"></typeparam>
/// <typeparam name="TCaretIndex"></typeparam>
public abstract class IndexCaretPositionAlgorithm<TCaretPosition, TCaretIndex> : CaretPositionAlgorithm<TCaretPosition>, IIndexCaretPositionAlgorithm
    where TCaretPosition : struct, IIndexCaretPosition
    where TCaretIndex : notnull
{
    protected IndexCaretPositionAlgorithm(Lyric[] lyrics)
        : base(lyrics)
    {
    }

    protected abstract TCaretPosition? MoveToPreviousIndex(TCaretPosition currentPosition);

    protected abstract TCaretPosition? MoveToNextIndex(TCaretPosition currentPosition);

    protected abstract TCaretPosition? MoveToFirstIndex(Lyric lyric);

    protected abstract TCaretPosition? MoveToLastIndex(Lyric lyric);

    protected abstract TCaretPosition CreateCaretPosition(Lyric lyric, TCaretIndex index, CaretGenerateType generateType = CaretGenerateType.Action);

    public IIndexCaretPosition? MoveToPreviousIndex(IIndexCaretPosition currentPosition)
    {
        if (currentPosition is not TCaretPosition tCaretPosition)
            throw new InvalidCastException(nameof(currentPosition));

        Validate(tCaretPosition);

        var movedCaretPosition = MoveToPreviousIndex(tCaretPosition);
        return PostValidate(movedCaretPosition, CaretGenerateType.Action);
    }

    public IIndexCaretPosition? MoveToNextIndex(IIndexCaretPosition currentPosition)
    {
        if (currentPosition is not TCaretPosition tCaretPosition)
            throw new InvalidCastException(nameof(currentPosition));

        Validate(tCaretPosition);

        var movedCaretPosition = MoveToNextIndex(tCaretPosition);
        return PostValidate(movedCaretPosition, CaretGenerateType.Action);
    }

    IIndexCaretPosition? IIndexCaretPositionAlgorithm.MoveToFirstIndex(Lyric lyric)
    {
        var movedCaretPosition = MoveToFirstIndex(lyric);
        return PostValidate(movedCaretPosition, CaretGenerateType.Action);
    }

    IIndexCaretPosition? IIndexCaretPositionAlgorithm.MoveToLastIndex(Lyric lyric)
    {
        var movedCaretPosition = MoveToLastIndex(lyric);
        return PostValidate(movedCaretPosition, CaretGenerateType.Action);
    }

    IIndexCaretPosition? IIndexCaretPositionAlgorithm.MoveToTargetLyric<TIndex>(Lyric lyric, TIndex index)
    {
        if (index is not TCaretIndex caretIndex)
            throw new InvalidCastException();

        var movedCaretPosition = CreateCaretPosition(lyric, caretIndex, CaretGenerateType.TargetLyric);
        return PostValidate(movedCaretPosition, CaretGenerateType.TargetLyric);
    }
}
