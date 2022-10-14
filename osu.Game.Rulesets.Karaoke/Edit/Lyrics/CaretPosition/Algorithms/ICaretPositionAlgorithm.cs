// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Game.Rulesets.Karaoke.Objects;

namespace osu.Game.Rulesets.Karaoke.Edit.Lyrics.CaretPosition.Algorithms
{
    public interface ICaretPositionAlgorithm
    {
        public bool PositionMovable(ICaretPosition position);

        public ICaretPosition? MoveToPreviousLyric(ICaretPosition currentPosition);

        public ICaretPosition? MoveToNextLyric(ICaretPosition currentPosition);

        public ICaretPosition? MoveToFirstLyric();

        public ICaretPosition? MoveToLastLyric();

        public ICaretPosition? MoveToTargetLyric(Lyric lyric);
    }
}
