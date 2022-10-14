// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Game.Rulesets.Karaoke.Objects;

namespace osu.Game.Rulesets.Karaoke.Edit.Lyrics.CaretPosition.Algorithms
{
    public interface ICaretPositionAlgorithm
    {
        public bool PositionMovable(ICaretPosition position);

        public ICaretPosition? MoveUp(ICaretPosition currentPosition);

        public ICaretPosition? MoveDown(ICaretPosition currentPosition);

        public ICaretPosition? MoveToFirst();

        public ICaretPosition? MoveToLast();

        public ICaretPosition? MoveToTarget(Lyric lyric);
    }
}
