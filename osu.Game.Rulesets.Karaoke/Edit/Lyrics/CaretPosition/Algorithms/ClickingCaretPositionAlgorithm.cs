// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Game.Rulesets.Karaoke.Objects;

namespace osu.Game.Rulesets.Karaoke.Edit.Lyrics.CaretPosition.Algorithms
{
    public class ClickingCaretPositionAlgorithm : CaretPositionAlgorithm<NavigateCaretPosition>
    {
        public ClickingCaretPositionAlgorithm(Lyric[] lyrics)
            : base(lyrics)
        {
        }

        public override bool PositionMovable(NavigateCaretPosition position)
        {
            return true;
        }

        public override NavigateCaretPosition MoveUp(NavigateCaretPosition currentPosition)
        {
            return null;
        }

        public override NavigateCaretPosition MoveDown(NavigateCaretPosition currentPosition)
        {
            return null;
        }

        public override NavigateCaretPosition MoveLeft(NavigateCaretPosition currentPosition)
        {
            return null;
        }

        public override NavigateCaretPosition MoveRight(NavigateCaretPosition currentPosition)
        {
            return null;
        }

        public override NavigateCaretPosition MoveToFirst()
        {
            return null;
        }

        public override NavigateCaretPosition MoveToLast()
        {
            return null;
        }

        public override NavigateCaretPosition MoveToTarget(Lyric lyric) => new(lyric);
    }
}
