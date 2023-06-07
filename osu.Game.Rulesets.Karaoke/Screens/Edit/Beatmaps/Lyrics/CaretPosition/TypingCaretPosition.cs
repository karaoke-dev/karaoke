// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Game.Rulesets.Karaoke.Objects;

namespace osu.Game.Rulesets.Karaoke.Screens.Edit.Beatmaps.Lyrics.CaretPosition;

public readonly struct TypingCaretPosition : ICharGapCaretPosition, IRangeIndexCaretPosition
{
    public TypingCaretPosition(Lyric lyric, int charGap, int releaseCharGap, CaretGenerateType generateType = CaretGenerateType.Action)
    {
        Lyric = lyric;
        CharGap = charGap;
        ReleaseCharGap = releaseCharGap;
        GenerateType = generateType;
    }

    public Lyric Lyric { get; }

    public int CharGap { get; }

    public int ReleaseCharGap { get; }

    public CaretGenerateType GenerateType { get; }
}
