// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Game.Rulesets.Karaoke.Objects;

namespace osu.Game.Rulesets.Karaoke.Edit.Lyrics.CaretPosition.Algorithms
{
    public class ClickingCaretPositionAlgorithm : CaretPositionAlgorithm<ClickingCaretPosition>
    {
        public ClickingCaretPositionAlgorithm(Lyric[] lyrics)
            : base(lyrics)
        {
        }

        public override bool PositionMovable(ClickingCaretPosition position)
        {
            return true;
        }

        public override ClickingCaretPosition? MoveToPreviousLyric(ClickingCaretPosition currentPosition)
        {
            return null;
        }

        public override ClickingCaretPosition? MoveToNextLyric(ClickingCaretPosition currentPosition)
        {
            return null;
        }

        public override ClickingCaretPosition? MoveToFirstLyric()
        {
            return null;
        }

        public override ClickingCaretPosition? MoveToLastLyric()
        {
            return null;
        }

        public override ClickingCaretPosition? MoveToTargetLyric(Lyric lyric) => new(lyric, CaretGenerateType.TargetLyric);
    }
}
