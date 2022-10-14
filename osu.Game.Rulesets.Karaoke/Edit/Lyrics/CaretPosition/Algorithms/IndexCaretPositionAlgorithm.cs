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

        public abstract TCaretPosition? MoveToPreviousIndex(TCaretPosition currentPosition);

        public abstract TCaretPosition? MoveToNextIndex(TCaretPosition currentPosition);

        public ICaretPosition? MoveToPreviousIndex(ICaretPosition currentPosition)
        {
            if (currentPosition is not TCaretPosition tCaretPosition)
                throw new InvalidCastException(nameof(currentPosition));

            return MoveToPreviousIndex(tCaretPosition);
        }

        public ICaretPosition? MoveToNextIndex(ICaretPosition currentPosition)
        {
            if (currentPosition is not TCaretPosition tCaretPosition)
                throw new InvalidCastException(nameof(currentPosition));

            return MoveToNextIndex(tCaretPosition);
        }
    }
}
