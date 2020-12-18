// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Graphics.Sprites;
using osu.Game.Rulesets.Karaoke.Objects;

namespace osu.Game.Rulesets.Karaoke.Edit.Lyrics
{
    public struct CursorPosition
    {
        public CursorPosition(Lyric lyric, TimeTagIndex timeTagIndex)
        {
            Lyric = lyric;
            Index = timeTagIndex;
        }

        public Lyric Lyric { get; }

        public TimeTagIndex Index { get; }
    }
}
