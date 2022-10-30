// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using System.Diagnostics;
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

        protected abstract void Validate(TCaretPosition input);

        protected abstract bool PositionMovable(TCaretPosition position);

        protected abstract TCaretPosition? MoveToPreviousLyric(TCaretPosition currentPosition);

        protected abstract TCaretPosition? MoveToNextLyric(TCaretPosition currentPosition);

        protected abstract TCaretPosition? MoveToFirstLyric();

        protected abstract TCaretPosition? MoveToLastLyric();

        protected abstract TCaretPosition? MoveToTargetLyric(Lyric lyric);

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

            Validate(tCaretPosition);

            var movedCaretPosition = MoveToPreviousLyric(tCaretPosition);
            if (movedCaretPosition == null)
                return movedCaretPosition;

            Validate(movedCaretPosition.Value);
            Debug.Assert(movedCaretPosition.Value.GenerateType == CaretGenerateType.Action);

            return movedCaretPosition;
        }

        public ICaretPosition? MoveToNextLyric(ICaretPosition currentPosition)
        {
            if (currentPosition is not TCaretPosition tCaretPosition)
                throw new InvalidCastException(nameof(currentPosition));

            Validate(tCaretPosition);

            var movedCaretPosition = MoveToNextLyric(tCaretPosition);
            if (movedCaretPosition == null)
                return movedCaretPosition;

            Validate(movedCaretPosition.Value);
            Debug.Assert(movedCaretPosition.Value.GenerateType == CaretGenerateType.Action);

            return movedCaretPosition;
        }

        ICaretPosition? ICaretPositionAlgorithm.MoveToFirstLyric()
        {
            var movedCaretPosition = MoveToFirstLyric();
            if (movedCaretPosition == null)
                return movedCaretPosition;

            Validate(movedCaretPosition.Value);
            Debug.Assert(movedCaretPosition.Value.GenerateType == CaretGenerateType.Action);

            return movedCaretPosition;
        }

        ICaretPosition? ICaretPositionAlgorithm.MoveToLastLyric()
        {
            var movedCaretPosition = MoveToLastLyric();
            if (movedCaretPosition == null)
                return movedCaretPosition;

            Validate(movedCaretPosition.Value);
            Debug.Assert(movedCaretPosition.Value.GenerateType == CaretGenerateType.Action);

            return movedCaretPosition;
        }

        ICaretPosition? ICaretPositionAlgorithm.MoveToTargetLyric(Lyric lyric)
        {
            var movedCaretPosition = MoveToTargetLyric(lyric);

            if (movedCaretPosition == null)
                return movedCaretPosition;

            Validate(movedCaretPosition.Value);
            Debug.Assert(movedCaretPosition.Value.GenerateType == CaretGenerateType.TargetLyric);

            return movedCaretPosition;
        }
    }
}
