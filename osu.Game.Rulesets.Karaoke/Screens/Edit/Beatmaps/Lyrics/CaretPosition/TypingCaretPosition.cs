// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Game.Rulesets.Karaoke.Objects;

namespace osu.Game.Rulesets.Karaoke.Screens.Edit.Beatmaps.Lyrics.CaretPosition;

public readonly struct TypingCaretPosition : ICharGapCaretPosition
{
    public TypingCaretPosition(Lyric lyric, int charGap)
    {
        Lyric = lyric;
        CharGap = charGap;
    }

    public Lyric Lyric { get; }

    public int CharGap { get; }
}
