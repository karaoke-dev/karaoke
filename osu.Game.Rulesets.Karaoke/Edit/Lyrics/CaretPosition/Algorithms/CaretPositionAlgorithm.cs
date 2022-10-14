// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using osu.Game.Rulesets.Karaoke.Objects;

namespace osu.Game.Rulesets.Karaoke.Edit.Lyrics.CaretPosition.Algorithms
{
    public abstract class CaretPositionAlgorithm<TCaretPosition> : ICaretPositionAlgorithm where TCaretPosition : struct, ICaretPosition
    {
        // Lyrics is not lock and can be accessible.
        protected readonly Lyric[] Lyrics;

        protected CaretPositionAlgorithm(Lyric[] lyrics)
        {
            Lyrics = lyrics;
        }

        public abstract bool PositionMovable(TCaretPosition position);

        public abstract TCaretPosition? MoveToPreviousLyric(TCaretPosition currentPosition);

        public abstract TCaretPosition? MoveToNextLyric(TCaretPosition currentPosition);

        public abstract TCaretPosition? MoveToFirstLyric();

        public abstract TCaretPosition? MoveToLastLyric();

        public abstract TCaretPosition? MoveToTargetLyric(Lyric lyric);

        public bool PositionMovable(ICaretPosition position)
        {
            if (position is not TCaretPosition tCaretPosition)
                throw new InvalidCastException(nameof(position));

            return PositionMovable(tCaretPosition);
        }

        public ICaretPosition? MoveToPreviousLyric(ICaretPosition currentPosition)
        {
            if (currentPosition is not TCaretPosition tCaretPosition)
                throw new InvalidCastException(nameof(currentPosition));

            return MoveToPreviousLyric(tCaretPosition);
        }

        public ICaretPosition? MoveToNextLyric(ICaretPosition currentPosition)
        {
            if (currentPosition is not TCaretPosition tCaretPosition)
                throw new InvalidCastException(nameof(currentPosition));

            return MoveToNextLyric(tCaretPosition);
        }

        ICaretPosition? ICaretPositionAlgorithm.MoveToFirstLyric()
            => MoveToFirstLyric();

        ICaretPosition? ICaretPositionAlgorithm.MoveToLastLyric()
            => MoveToLastLyric();

        ICaretPosition? ICaretPositionAlgorithm.MoveToTargetLyric(Lyric lyric)
            => MoveToTargetLyric(lyric);
    }
}
