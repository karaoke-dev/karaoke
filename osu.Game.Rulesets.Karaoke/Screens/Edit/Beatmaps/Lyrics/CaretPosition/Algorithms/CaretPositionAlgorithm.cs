// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using System.Diagnostics;
using osu.Game.Rulesets.Karaoke.Objects;

namespace osu.Game.Rulesets.Karaoke.Screens.Edit.Beatmaps.Lyrics.CaretPosition.Algorithms;

public abstract class CaretPositionAlgorithm<TCaretPosition> : ICaretPositionAlgorithm where TCaretPosition : struct, ICaretPosition
{
    // Lyrics is not lock and can be accessible.
    protected readonly Lyric[] Lyrics;

    protected CaretPositionAlgorithm(Lyric[] lyrics)
    {
        Lyrics = lyrics;
    }

    protected abstract void Validate(TCaretPosition input);

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

        Validate(tCaretPosition);

        var movedCaretPosition = MoveToPreviousLyric(tCaretPosition);
        return PostValidate(movedCaretPosition, CaretGenerateType.Action);
    }

    public ICaretPosition? MoveToNextLyric(ICaretPosition currentPosition)
    {
        if (currentPosition is not TCaretPosition tCaretPosition)
            throw new InvalidCastException(nameof(currentPosition));

        Validate(tCaretPosition);

        var movedCaretPosition = MoveToNextLyric(tCaretPosition);
        return PostValidate(movedCaretPosition, CaretGenerateType.Action);
    }

    ICaretPosition? ICaretPositionAlgorithm.MoveToFirstLyric()
    {
        var movedCaretPosition = MoveToFirstLyric();
        return PostValidate(movedCaretPosition, CaretGenerateType.Action);
    }

    ICaretPosition? ICaretPositionAlgorithm.MoveToLastLyric()
    {
        var movedCaretPosition = MoveToLastLyric();
        return PostValidate(movedCaretPosition, CaretGenerateType.Action);
    }

    ICaretPosition? ICaretPositionAlgorithm.MoveToTargetLyric(Lyric lyric)
    {
        var movedCaretPosition = MoveToTargetLyric(lyric);
        return PostValidate(movedCaretPosition, CaretGenerateType.TargetLyric);
    }

    protected TCaretPosition? PostValidate(TCaretPosition? movedCaretPosition, CaretGenerateType generateType)
    {
        if (movedCaretPosition == null)
            return null;

        if (!PositionMovable(movedCaretPosition.Value))
            return null;

        Validate(movedCaretPosition.Value);
        Debug.Assert(movedCaretPosition.Value.GenerateType == generateType);

        return movedCaretPosition;
    }
}
