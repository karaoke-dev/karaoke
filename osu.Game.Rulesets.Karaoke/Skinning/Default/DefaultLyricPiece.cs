// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

#nullable disable

using osu.Framework.Graphics.Sprites;
using osu.Game.Rulesets.Karaoke.Graphics.Sprites;
using osu.Game.Rulesets.Karaoke.Objects;

namespace osu.Game.Rulesets.Karaoke.Skinning.Default
{
    /// <summary>
    /// This component is focusing on display ruby and romaji.
    /// </summary>
    public class DefaultLyricPiece : DrawableKaraokeSpriteText<LyricSpriteText>
    {
        private const int whole_chunk_index = -1;

        public DefaultLyricPiece(Lyric hitObject, int chunkIndex = whole_chunk_index)
            : base(hitObject, chunkIndex)
        {
        }
    }
}
