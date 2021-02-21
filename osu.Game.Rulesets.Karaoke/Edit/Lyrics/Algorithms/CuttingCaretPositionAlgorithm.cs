// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Game.Rulesets.Karaoke.Objects;
using osu.Game.Rulesets.Karaoke.Utils;

namespace osu.Game.Rulesets.Karaoke.Edit.Lyrics.Algorithms
{
    public class CuttingCaretPositionAlgorithm : TypingCaretPositionAlgorithm
    {
        public CuttingCaretPositionAlgorithm(Lyric[] lyrics)
            : base(lyrics)
        {
        }

        public override bool PositionMovable(CaretPosition position)
        {
            if (!base.PositionMovable(position))
                return false;

            if (TextIndexUtils.ToStringIndex(position.Index) == 0)
                return false;

            return true;
        }
    }
}
