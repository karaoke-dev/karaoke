// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

#nullable disable

using osu.Framework.Audio;
using osu.Framework.Graphics.Rendering;
using osu.Framework.Graphics.Rendering.Dummy;
using osu.Framework.Graphics.Textures;
using osu.Framework.IO.Stores;
using osu.Game.Database;
using osu.Game.IO;

namespace osu.Game.Rulesets.Karaoke.Skinning
{
    public class InternalSkinStorageResourceProvider : IStorageResourceProvider
    {
        public InternalSkinStorageResourceProvider(string skinName)
        {
            var store = new KaraokeRuleset().CreateResourceStore();
            Files = Resources = new NamespacedResourceStore<byte[]>(store, $"Skin/{skinName}");
        }

        public IRenderer Renderer => new DummyRenderer();

        public AudioManager AudioManager => null;
        public IResourceStore<byte[]> Files { get; }
        public IResourceStore<byte[]> Resources { get; }
        public RealmAccess RealmAccess => null;
        public IResourceStore<TextureUpload> CreateTextureLoaderStore(IResourceStore<byte[]> underlyingStore) => null;
    }
}
