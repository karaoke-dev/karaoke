// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

#nullable disable

using osu.Game.Rulesets.Karaoke.Objects;
using osu.Game.Rulesets.Karaoke.Objects.Types;

namespace osu.Game.Rulesets.Karaoke.Edit.Lyrics.CaretPosition
{
    public class EditTextTagCaretPosition : ITextTagCaretPosition
    {
        public EditTextTagCaretPosition(Lyric lyric, ITextTag textTag)
        {
            Lyric = lyric;
            TextTag = textTag;
        }

        public Lyric Lyric { get; }

        public ITextTag TextTag { get; }
    }
}
