// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Game.Rulesets.Karaoke.Objects;

namespace osu.Game.Rulesets.Karaoke.Edit.Lyrics.CaretPosition.Algorithms
{
    public interface IIndexCaretPositionAlgorithm : ICaretPositionAlgorithm
    {
        IIndexCaretPosition? MoveToPreviousIndex(IIndexCaretPosition currentPosition);

        IIndexCaretPosition? MoveToNextIndex(IIndexCaretPosition currentPosition);

        IIndexCaretPosition? MoveToFirstIndex(Lyric lyric);

        IIndexCaretPosition? MoveToLastIndex(Lyric lyric);
    }
}
