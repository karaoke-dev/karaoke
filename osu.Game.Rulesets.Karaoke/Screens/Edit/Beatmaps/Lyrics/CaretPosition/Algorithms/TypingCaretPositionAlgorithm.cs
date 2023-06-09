// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Game.Rulesets.Karaoke.Objects;

namespace osu.Game.Rulesets.Karaoke.Screens.Edit.Beatmaps.Lyrics.CaretPosition.Algorithms;

/// <summary>
/// Algorithm for move the caret position indicate the position that type to the <see cref="Lyric.Text"/>.
/// </summary>
public class TypingCaretPositionAlgorithm : CharGapCaretPositionAlgorithm<TypingCaretPosition>
{
    public TypingCaretPositionAlgorithm(Lyric[] lyrics)
        : base(lyrics)
    {
    }

    protected override TypingCaretPosition CreateCaretPosition(Lyric lyric, int index, CaretGenerateType generateType = CaretGenerateType.Action) => new(lyric, index, generateType);

    protected override int GetMinIndex(string text) => 0;

    protected override int GetMaxIndex(string text) => text.Length;
}
