// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Game.Rulesets.Karaoke.Objects;

namespace osu.Game.Rulesets.Karaoke.Edit.Lyrics.CaretPosition.Algorithms
{
    public class CuttingCaretPositionAlgorithm : TextCaretPositionAlgorithm<CuttingCaretPosition>
    {
        public CuttingCaretPositionAlgorithm(Lyric[] lyrics)
            : base(lyrics)
        {
        }

        protected override CuttingCaretPosition CreateCaretPosition(Lyric lyric, int index, CaretGenerateType generateType = CaretGenerateType.Action) => new(lyric, index, generateType);

        protected override int GetMinIndex(string text) => 1;

        protected override int GetMaxIndex(string text) => text.Length - 1;
    }
}
