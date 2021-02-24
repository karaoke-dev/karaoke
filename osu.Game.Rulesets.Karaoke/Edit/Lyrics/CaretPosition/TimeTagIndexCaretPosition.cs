// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Graphics.Sprites;
using osu.Game.Rulesets.Karaoke.Objects;

namespace osu.Game.Rulesets.Karaoke.Edit.Lyrics.CaretPosition
{
    public class TimeTagIndexCaretPosition : ICaretPosition
    {
        public TimeTagIndexCaretPosition(Lyric lyric, TextIndex index)
        {
            Lyric = lyric;
            Index = index;
        }

        public Lyric Lyric { get; }

        public TextIndex Index { get; }
    }
}
