// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Graphics.Sprites;
using osu.Game.Rulesets.Karaoke.Objects;

namespace osu.Game.Rulesets.Karaoke.Edit.Lyrics.CaretPosition
{
    public struct TimeTagIndexCaretPosition : ICaretPosition
    {
        public TimeTagIndexCaretPosition(Lyric lyric, TextIndex index, CaretGenerateType generateType = CaretGenerateType.Action)
        {
            Lyric = lyric;
            Index = index;
            GenerateType = generateType;
        }

        public Lyric Lyric { get; }

        public TextIndex Index { get; }

        public CaretGenerateType GenerateType { get; }
    }
}
