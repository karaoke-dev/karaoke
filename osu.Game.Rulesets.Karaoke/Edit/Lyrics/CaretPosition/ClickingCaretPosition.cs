// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

#nullable disable

using osu.Game.Rulesets.Karaoke.Objects;

namespace osu.Game.Rulesets.Karaoke.Edit.Lyrics.CaretPosition
{
    public class ClickingCaretPosition : ICaretPosition
    {
        public ClickingCaretPosition(Lyric lyric)
        {
            Lyric = lyric;
        }

        public Lyric Lyric { get; }
    }
}
