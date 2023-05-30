// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Game.Rulesets.Karaoke.Objects;

namespace osu.Game.Rulesets.Karaoke.Screens.Edit.Beatmaps.Lyrics.CaretPosition;

public readonly struct TimeTagIndexCaretPosition : ICharGapCaretPosition
{
    public TimeTagIndexCaretPosition(Lyric lyric, int charIndex, CaretGenerateType generateType = CaretGenerateType.Action)
    {
        Lyric = lyric;
        GapIndex = charIndex;
        GenerateType = generateType;
    }

    public Lyric Lyric { get; }

    public int GapIndex { get; }

    public CaretGenerateType GenerateType { get; }
}
