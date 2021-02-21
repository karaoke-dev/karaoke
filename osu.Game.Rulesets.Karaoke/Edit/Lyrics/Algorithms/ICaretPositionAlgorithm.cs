// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

namespace osu.Game.Rulesets.Karaoke.Edit.Lyrics.Algorithms
{
    public interface ICaretPositionAlgorithm
    {
        bool PositionMovable(CaretPosition position);

        CaretPosition? MoveUp(CaretPosition currentPosition);

        CaretPosition? MoveDown(CaretPosition currentPosition);

        CaretPosition? MoveLeft(CaretPosition currentPosition);

        CaretPosition? MoveRight(CaretPosition currentPosition);

        CaretPosition? MoveToFirst();

        CaretPosition? MoveToLast();
    }
}
