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

        public abstract TCaretPosition? MoveUp(TCaretPosition currentPosition);

        public abstract TCaretPosition? MoveDown(TCaretPosition currentPosition);

        public abstract TCaretPosition? MoveToFirst();

        public abstract TCaretPosition? MoveToLast();

        public abstract TCaretPosition? MoveToTarget(Lyric lyric);

        public bool PositionMovable(ICaretPosition position)
        {
            if (position is not TCaretPosition tCaretPosition)
                throw new InvalidCastException(nameof(position));

            return PositionMovable(tCaretPosition);
        }

        public ICaretPosition? MoveUp(ICaretPosition currentPosition)
        {
            if (currentPosition is not TCaretPosition tCaretPosition)
                throw new InvalidCastException(nameof(currentPosition));

            return MoveUp(tCaretPosition);
        }

        public ICaretPosition? MoveDown(ICaretPosition currentPosition)
        {
            if (currentPosition is not TCaretPosition tCaretPosition)
                throw new InvalidCastException(nameof(currentPosition));

            return MoveDown(tCaretPosition);
        }

        ICaretPosition? ICaretPositionAlgorithm.MoveToFirst()
            => MoveToFirst();

        ICaretPosition? ICaretPositionAlgorithm.MoveToLast()
            => MoveToLast();

        ICaretPosition? ICaretPositionAlgorithm.MoveToTarget(Lyric lyric)
            => MoveToTarget(lyric);
    }
}
