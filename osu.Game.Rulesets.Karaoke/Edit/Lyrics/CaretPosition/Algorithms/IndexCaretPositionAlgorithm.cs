// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using System.Diagnostics;
using osu.Game.Rulesets.Karaoke.Objects;

namespace osu.Game.Rulesets.Karaoke.Edit.Lyrics.CaretPosition.Algorithms
{
    public abstract class IndexCaretPositionAlgorithm<TCaretPosition> : CaretPositionAlgorithm<TCaretPosition>, IIndexCaretPositionAlgorithm
        where TCaretPosition : struct, IIndexCaretPosition
    {
        protected IndexCaretPositionAlgorithm(Lyric[] lyrics)
            : base(lyrics)
        {
        }

        protected abstract TCaretPosition? MoveToPreviousIndex(TCaretPosition currentPosition);

        protected abstract TCaretPosition? MoveToNextIndex(TCaretPosition currentPosition);

        protected abstract TCaretPosition? MoveToFirstIndex(Lyric lyric);

        protected abstract TCaretPosition? MoveToLastIndex(Lyric lyric);

        protected abstract TCaretPosition? MoveToTargetLyric<TIndex>(Lyric lyric, TIndex? index);

        public IIndexCaretPosition? MoveToPreviousIndex(IIndexCaretPosition currentPosition)
        {
            if (currentPosition is not TCaretPosition tCaretPosition)
                throw new InvalidCastException(nameof(currentPosition));

            Validate(tCaretPosition);

            var movedCaretPosition = MoveToPreviousIndex(tCaretPosition);
            if (movedCaretPosition != null)
                Validate(movedCaretPosition.Value);

            return movedCaretPosition;
        }

        public IIndexCaretPosition? MoveToNextIndex(IIndexCaretPosition currentPosition)
        {
            if (currentPosition is not TCaretPosition tCaretPosition)
                throw new InvalidCastException(nameof(currentPosition));

            Validate(tCaretPosition);

            var movedCaretPosition = MoveToNextIndex(tCaretPosition);
            if (movedCaretPosition != null)
                Validate(movedCaretPosition.Value);

            return movedCaretPosition;
        }

        IIndexCaretPosition? IIndexCaretPositionAlgorithm.MoveToFirstIndex(Lyric lyric)
        {
            var movedCaretPosition = MoveToFirstIndex(lyric);
            if (movedCaretPosition != null)
                Validate(movedCaretPosition.Value);

            return movedCaretPosition;
        }

        IIndexCaretPosition? IIndexCaretPositionAlgorithm.MoveToLastIndex(Lyric lyric)
        {
            var movedCaretPosition = MoveToLastIndex(lyric);
            if (movedCaretPosition != null)
                Validate(movedCaretPosition.Value);

            return movedCaretPosition;
        }

        IIndexCaretPosition? IIndexCaretPositionAlgorithm.MoveToTargetLyric<TIndex>(Lyric lyric, TIndex? index) where TIndex : default
        {
            var movedCaretPosition = MoveToTargetLyric(lyric, index);

            if (movedCaretPosition == null)
                return movedCaretPosition;

            if (!PositionMovable(movedCaretPosition.Value))
                return null;

            Validate(movedCaretPosition.Value);
            Debug.Assert(movedCaretPosition.Value.GenerateType == CaretGenerateType.TargetLyric);

            return movedCaretPosition;
        }
    }
}
