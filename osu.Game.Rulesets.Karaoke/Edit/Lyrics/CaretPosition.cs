// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Graphics.Sprites;
using osu.Game.Rulesets.Karaoke.Objects;

namespace osu.Game.Rulesets.Karaoke.Edit.Lyrics
{
    public readonly struct CaretPosition
    {
        public CaretPosition(Lyric lyric, TextIndex index)
        {
            Lyric = lyric;
            Index = index;
            TimeTag = null;
            Mode = CaretMode.Edit;
        }

        public CaretPosition(Lyric lyric, TimeTag timeTag)
        {
            Lyric = lyric;
            Index = default;
            TimeTag = timeTag;
            Mode = CaretMode.Recording;
        }

        public Lyric Lyric { get; }

        public TextIndex Index { get; }

        public TimeTag TimeTag { get; }

        public CaretMode Mode { get; }
    }

    public enum CaretMode
    {
        Edit,

        Recording
    }
}
