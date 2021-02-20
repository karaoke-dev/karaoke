// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

namespace osu.Game.Rulesets.Karaoke.Edit.Lyrics.Algorithms
{
    public interface ICursorPositionAlgorithm
    {
        bool PositionMovable(CursorPosition position);

        CursorPosition? MoveUp(CursorPosition currentPosition);

        CursorPosition? MoveDown(CursorPosition currentPosition);

        CursorPosition? MoveLeft(CursorPosition currentPosition);

        CursorPosition? MoveRight(CursorPosition currentPosition);

        CursorPosition? MoveToFirst();

        CursorPosition? MoveToLast();
    }
}
