// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Graphics.Textures;
using osu.Game.Skinning;

namespace osu.Game.Rulesets.Karaoke.Skinning
{
    public class LegacyNoteBackgroundHeadPiece : LegacyNotePiece
    {
        protected override Texture GetTexture(ISkinSource skin)
            => GetTextureFromLookup(skin, LegacyKaraokeSkinConfigurationLookups.NoteHeadImage, LegacyKaraokeSkinLayer.Background);
    }
}
