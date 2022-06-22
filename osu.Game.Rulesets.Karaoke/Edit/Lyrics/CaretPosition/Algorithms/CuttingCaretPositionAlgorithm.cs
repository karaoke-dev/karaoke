// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

#nullable disable

using osu.Game.Rulesets.Karaoke.Objects;

namespace osu.Game.Rulesets.Karaoke.Edit.Lyrics.CaretPosition.Algorithms
{
    public class CuttingCaretPositionAlgorithm : TypingCaretPositionAlgorithm
    {
        public CuttingCaretPositionAlgorithm(Lyric[] lyrics)
            : base(lyrics)
        {
        }

        protected override int GetMinIndex(string text) => 1;

        protected override int GetMaxIndex(string text) => (text?.Length ?? 0) - 1;
    }
}
