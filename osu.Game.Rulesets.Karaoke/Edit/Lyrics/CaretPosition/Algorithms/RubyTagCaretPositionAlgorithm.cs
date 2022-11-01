// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Collections.Generic;
using osu.Game.Rulesets.Karaoke.Objects;

namespace osu.Game.Rulesets.Karaoke.Edit.Lyrics.CaretPosition.Algorithms
{
    public class RubyTagCaretPositionAlgorithm : TextTagCaretPositionAlgorithm<RubyTagCaretPosition, RubyTag>
    {
        public RubyTagCaretPositionAlgorithm(Lyric[] lyrics)
            : base(lyrics)
        {
        }

        protected override RubyTag GetTextTagByCaret(RubyTagCaretPosition position)
            => position.RubyTag;

        protected override IList<RubyTag> GetTextTagsByLyric(Lyric lyric)
            => lyric.RubyTags;

        protected override RubyTagCaretPosition GenerateCaretPosition(Lyric lyric, RubyTag textTag, CaretGenerateType generateType)
            => new(lyric, textTag, generateType);
    }
}
