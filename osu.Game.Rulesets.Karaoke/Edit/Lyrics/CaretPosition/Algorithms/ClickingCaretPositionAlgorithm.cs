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

        public override ClickingCaretPosition? MoveUp(ClickingCaretPosition currentPosition)
        {
            return null;
        }

        public override ClickingCaretPosition? MoveDown(ClickingCaretPosition currentPosition)
        {
            return null;
        }

        public override ClickingCaretPosition? MoveLeft(ClickingCaretPosition currentPosition)
        {
            return null;
        }

        public override ClickingCaretPosition? MoveRight(ClickingCaretPosition currentPosition)
        {
            return null;
        }

        public override ClickingCaretPosition? MoveToFirst()
        {
            return null;
        }

        public override ClickingCaretPosition? MoveToLast()
        {
            return null;
        }

        public override ClickingCaretPosition MoveToTarget(Lyric lyric) => new(lyric, CaretGenerateType.TargetLyric);
    }
}
