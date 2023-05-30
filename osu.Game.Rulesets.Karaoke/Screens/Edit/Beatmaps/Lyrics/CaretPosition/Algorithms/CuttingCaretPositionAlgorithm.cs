// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Game.Rulesets.Karaoke.Objects;

namespace osu.Game.Rulesets.Karaoke.Screens.Edit.Beatmaps.Lyrics.CaretPosition.Algorithms;

/// <summary>
/// Algorithm for move the caret position indicate the position that cut the <see cref="Lyric.Text"/>.
/// </summary>
public class CuttingCaretPositionAlgorithm : CharGapCaretPositionAlgorithm<CuttingCaretPosition>
{
    public CuttingCaretPositionAlgorithm(Lyric[] lyrics)
        : base(lyrics)
    {
    }

    protected override CuttingCaretPosition CreateCaretPosition(Lyric lyric, int index, CaretGenerateType generateType = CaretGenerateType.Action) => new(lyric, index, generateType);

    protected override int GetMinIndex(string text) => 1;

    protected override int GetMaxIndex(string text) => text.Length - 1;
}
