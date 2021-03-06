// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Game.Rulesets.Karaoke.Objects;
using osu.Game.Rulesets.Karaoke.Objects.Types;

namespace osu.Game.Rulesets.Karaoke.Edit.Lyrics.CaretPosition
{
    public class CreateTextTagCaretPosition : ITextTagCaretPosition
    {
        public CreateTextTagCaretPosition(Lyric lyric, ITextTag textTag)
        {
            Lyric = lyric;
            TextTag = textTag; // use text tag in order to let us know will create ruby or romaji.
        }

        public Lyric Lyric { get; }

        public ITextTag TextTag { get; }
    }
}
