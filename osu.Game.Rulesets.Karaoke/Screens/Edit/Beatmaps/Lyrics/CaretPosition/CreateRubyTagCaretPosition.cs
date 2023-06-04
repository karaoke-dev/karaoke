// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Game.Rulesets.Karaoke.Objects;

namespace osu.Game.Rulesets.Karaoke.Screens.Edit.Beatmaps.Lyrics.CaretPosition;

public readonly struct CreateRubyTagCaretPosition : ICharIndexCaretPosition, IRangeIndexCaretPosition
{
    public CreateRubyTagCaretPosition(Lyric lyric, int charIndex, int releaseCharIndex, CaretGenerateType generateType = CaretGenerateType.Action)
    {
        Lyric = lyric;
        CharIndex = charIndex;
        ReleaseCharIndex = releaseCharIndex;
        GenerateType = generateType;
    }

    public Lyric Lyric { get; }

    public int CharIndex { get; }

    public int ReleaseCharIndex { get; }

    public CaretGenerateType GenerateType { get; }
}
