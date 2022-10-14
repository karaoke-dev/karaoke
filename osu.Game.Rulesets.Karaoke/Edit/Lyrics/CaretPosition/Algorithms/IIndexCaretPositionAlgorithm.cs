// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

namespace osu.Game.Rulesets.Karaoke.Edit.Lyrics.CaretPosition.Algorithms
{
    public interface IIndexCaretPositionAlgorithm : ICaretPositionAlgorithm
    {
        public ICaretPosition? MoveToPreviousIndex(ICaretPosition currentPosition);

        public ICaretPosition? MoveToNextIndex(ICaretPosition currentPosition);
    }
}
