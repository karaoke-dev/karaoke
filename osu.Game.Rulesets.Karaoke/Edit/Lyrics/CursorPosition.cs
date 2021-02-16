// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Graphics.Sprites;
using osu.Game.Rulesets.Karaoke.Objects;

namespace osu.Game.Rulesets.Karaoke.Edit.Lyrics
{
    public readonly struct CursorPosition
    {
        public CursorPosition(Lyric lyric, TextIndex index)
        {
            Lyric = lyric;
            Index = index;
            TimeTag = null;
            Mode = CursorMode.Edit;
        }

        public CursorPosition(Lyric lyric, TimeTag timeTag)
        {
            Lyric = lyric;
            Index = default;
            TimeTag = timeTag;
            Mode = CursorMode.Recording;
        }

        public Lyric Lyric { get; }

        public TextIndex Index { get; }

        public TimeTag TimeTag { get; }

        public CursorMode Mode { get; }
    }

    public enum CursorMode
    {
        Edit,

        Recording
    }
}
