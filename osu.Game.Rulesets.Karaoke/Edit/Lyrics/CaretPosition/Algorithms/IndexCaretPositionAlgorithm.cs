// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
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
    }
}
