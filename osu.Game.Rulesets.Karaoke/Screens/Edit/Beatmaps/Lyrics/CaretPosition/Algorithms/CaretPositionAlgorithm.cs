// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using System.Diagnostics;
using osu.Game.Rulesets.Karaoke.Objects;

namespace osu.Game.Rulesets.Karaoke.Screens.Edit.Beatmaps.Lyrics.CaretPosition.Algorithms;

/// <summary>
/// Base class for move the <see cref="ICaretPosition"/> to the previous or next position.
/// </summary>
/// <typeparam name="TCaretPosition"></typeparam>
public abstract class CaretPositionAlgorithm<TCaretPosition> : ICaretPositionAlgorithm where TCaretPosition : struct, ICaretPosition
{
    // Lyrics is not lock and can be accessible.
    protected readonly Lyric[] Lyrics;

    protected CaretPositionAlgorithm(Lyric[] lyrics)
    {
        Lyrics = lyrics;
    }

    protected abstract bool PositionMovable(TCaretPosition position);

    protected abstract TCaretPosition? MoveToPreviousLyric(TCaretPosition currentPosition);

    protected abstract TCaretPosition? MoveToNextLyric(TCaretPosition currentPosition);

    protected abstract TCaretPosition? MoveToFirstLyric();

    protected abstract TCaretPosition? MoveToLastLyric();

    protected abstract TCaretPosition? MoveToTargetLyric(Lyric lyric);

    public ICaretPosition? MoveToPreviousLyric(ICaretPosition currentPosition)
    {
        if (currentPosition is not TCaretPosition tCaretPosition)
            throw new InvalidCastException(nameof(currentPosition));

        PreValidate(tCaretPosition);

        var movedCaretPosition = MoveToPreviousLyric(tCaretPosition);
        return PostValidate(movedCaretPosition);
    }

    public ICaretPosition? MoveToNextLyric(ICaretPosition currentPosition)
    {
        if (currentPosition is not TCaretPosition tCaretPosition)
            throw new InvalidCastException(nameof(currentPosition));

        PreValidate(tCaretPosition);

        var movedCaretPosition = MoveToNextLyric(tCaretPosition);
        return PostValidate(movedCaretPosition);
    }

    ICaretPosition? ICaretPositionAlgorithm.MoveToFirstLyric()
    {
        var movedCaretPosition = MoveToFirstLyric();
        return PostValidate(movedCaretPosition);
    }

    ICaretPosition? ICaretPositionAlgorithm.MoveToLastLyric()
    {
        var movedCaretPosition = MoveToLastLyric();
        return PostValidate(movedCaretPosition);
    }

    ICaretPosition? ICaretPositionAlgorithm.MoveToTargetLyric(Lyric lyric)
    {
        var movedCaretPosition = MoveToTargetLyric(lyric);
        return PostValidate(movedCaretPosition);
    }

    protected virtual void PreValidate(TCaretPosition input)
    {
        Debug.Assert(PositionMovable(input));
    }

    protected TCaretPosition? PostValidate(TCaretPosition? movedCaretPosition)
    {
        if (movedCaretPosition == null)
            return null;

        if (!PositionMovable(movedCaretPosition.Value))
            return null;

        PreValidate(movedCaretPosition.Value);

        return movedCaretPosition;
    }
}
