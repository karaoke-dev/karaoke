// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

#nullable disable

using System.IO;
using NUnit.Framework;
using osu.Framework.Audio;
using osu.Framework.Audio.Track;
using osu.Framework.Graphics.Textures;
using osu.Framework.IO.Stores;
using osu.Game.Database;
using osu.Game.IO;

namespace osu.Game.Rulesets.Karaoke.Tests.Resources
{
    public static class TestResources
    {
        public static DllResourceStore GetStore() => new(typeof(TestResources).Assembly);

        public static Stream OpenResource(string name) => GetStore().GetStream($"Resources/{name}");

        public static Stream OpenBeatmapResource(string name) => OpenResource($"Testing/Beatmaps/{name}.osu");

        public static Stream OpenSkinResource(string name) => OpenResource($"Testing/Skin/{name}.skin");

        public static Stream OpenLrcResource(string name) => OpenResource($"Testing/Lrc/{name}.lrc");

        public static string GetTestLrcForImport(string name)
        {
            string tempPath = Path.GetTempFileName() + ".lrc";

            using (var stream = OpenLrcResource(name))
            using (var newFile = File.Create(tempPath))
                stream.CopyTo(newFile);

            Assert.IsTrue(File.Exists(tempPath));
            return tempPath;
        }

        public static Stream OpenNicoKaraResource(string name) => OpenResource($"Testing/NicoKara/{name}.nkmproj");

        public static Stream OpenTrackResource(string name) => OpenResource($"Testing/Track/{name}.mp3");

        public static Track OpenTrackInfo(AudioManager audioManager, string name) => audioManager.GetTrackStore(GetStore()).Get($"Resources/Testing/Track/{name}.mp3");

        public static IStorageResourceProvider CreateSkinStorageResourceProvider(string skinName = "special-skin") => new TestStorageResourceProvider(skinName);

        private class TestStorageResourceProvider : IStorageResourceProvider
        {
            public TestStorageResourceProvider(string skinName)
            {
                Files = Resources = new NamespacedResourceStore<byte[]>(new DllResourceStore(GetType().Assembly), $"Resources/{skinName}");
            }

            public IResourceStore<TextureUpload> CreateTextureLoaderStore(IResourceStore<byte[]> underlyingStore) => null;

            public AudioManager AudioManager => null;
            public IResourceStore<byte[]> Files { get; }
            public IResourceStore<byte[]> Resources { get; }
            public RealmAccess RealmAccess => null;
        }
    }
}
