// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Game.Rulesets.Karaoke.Objects;

namespace osu.Game.Rulesets.Karaoke.Edit.Lyrics.CaretPosition
{
    public struct RomajiTagCaretPosition : ITextTagCaretPosition
    {
        public RomajiTagCaretPosition(Lyric lyric, RomajiTag? romajiTag, CaretGenerateType generateType = CaretGenerateType.Action)
        {
            Lyric = lyric;
            RomajiTag = romajiTag;
            GenerateType = generateType;
        }

        public Lyric Lyric { get; }

        public RomajiTag? RomajiTag { get; }

        public CaretGenerateType GenerateType { get; }
    }
}
