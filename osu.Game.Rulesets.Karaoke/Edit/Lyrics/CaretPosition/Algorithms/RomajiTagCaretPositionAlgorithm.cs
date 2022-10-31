// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Collections.Generic;
using osu.Game.Rulesets.Karaoke.Objects;

namespace osu.Game.Rulesets.Karaoke.Edit.Lyrics.CaretPosition.Algorithms
{
    public class RomajiTagCaretPositionAlgorithm : TextTagCaretPositionAlgorithm<RomajiTagCaretPosition, RomajiTag>
    {
        public RomajiTagCaretPositionAlgorithm(Lyric[] lyrics)
            : base(lyrics)
        {
        }

        protected override RomajiTag GetTextTagByCaret(RomajiTagCaretPosition position)
            => position.RomajiTag;

        protected override IList<RomajiTag> GetTextTagsByLyric(Lyric lyric)
            => lyric.RomajiTags;

        protected override RomajiTagCaretPosition GenerateCaretPosition(Lyric lyric, RomajiTag textTag, CaretGenerateType generateType)
            => new(lyric, textTag, generateType);
    }
}
