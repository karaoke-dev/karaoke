// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Graphics.Textures;
using osu.Framework.IO.Stores;
using SharpFNT;

namespace osu.Game.Rulesets.Karaoke.IO.Stores
{
    public class FntGlyphStore : GlyphStore
    {
        public BitmapFont BitmapFont => Font;

        public FntGlyphStore(ResourceStore<byte[]> store, string assetName = null, IResourceStore<TextureUpload> textureLoader = null)
            : base(store, assetName, textureLoader)
        {
        }
    }
}
