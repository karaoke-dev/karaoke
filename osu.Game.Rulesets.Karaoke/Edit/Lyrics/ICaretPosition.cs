// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Graphics.Sprites;
using osu.Game.Rulesets.Karaoke.Objects;

namespace osu.Game.Rulesets.Karaoke.Edit.Lyrics
{
    public interface ICaretPosition
    {
        public Lyric Lyric { get; }
    }

    public readonly struct TextCaretPosition : ICaretPosition
    {
        public TextCaretPosition(Lyric lyric, int index)
        {
            Lyric = lyric;
            Index = index;
        }

        public Lyric Lyric { get; }

        public int Index { get; }
    }

    public readonly struct TimeTagIndexCaretPosition : ICaretPosition
    {
        public TimeTagIndexCaretPosition(Lyric lyric, TextIndex index)
        {
            Lyric = lyric;
            Index = index;
        }

        public Lyric Lyric { get; }

        public TextIndex Index { get; }
    }

    public readonly struct TimeTagCaretPosition : ICaretPosition
    {
        public TimeTagCaretPosition(Lyric lyric, TimeTag timeTag)
        {
            Lyric = lyric;
            TimeTag = timeTag;
        }

        public Lyric Lyric { get; }

        public TimeTag TimeTag { get; }
    }
}
