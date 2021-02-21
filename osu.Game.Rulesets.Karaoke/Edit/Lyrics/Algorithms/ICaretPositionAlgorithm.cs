// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

namespace osu.Game.Rulesets.Karaoke.Edit.Lyrics.Algorithms
{
    public interface ICaretPositionAlgorithm<T> where T : ICaretPosition
    {
        bool PositionMovable(T position);

        T MoveUp(T currentPosition);

        T MoveDown(T currentPosition);

        T MoveLeft(T currentPosition);

        T MoveRight(T currentPosition);

        T MoveToFirst();

        T MoveToLast();
    }
}
