// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System.IO;
using osu.Framework.Audio;
using osu.Framework.Audio.Track;
using osu.Framework.IO.Stores;

namespace osu.Game.Rulesets.Karaoke.Tests.Resources
{
    public static class TestResources
    {
        public static DllResourceStore GetStore() => new DllResourceStore(typeof(TestResources).Assembly);

        public static Stream OpenResource(string name) => GetStore().GetStream($"Resources/{name}");

        public static Stream OpenBeatmapResource(string name) => OpenResource($"Testing/Beatmaps/{name}.osu");

        public static Stream OpenSkinResource(string name) => OpenResource($"Testing/Skin/{name}.skin");

        public static Stream OpenLrcResource(string name) => OpenResource($"Testing/Lrc/{name}.lrc");

        public static Stream OpenNicoKaraResource(string name) => OpenResource($"Testing/NicoKara/{name}.nkmproj");

        public static Stream OpenTrackResource(string name) => OpenResource($"Testing/Track/{name}.mp3");

        public static Track OpenTrackInfo(AudioManager audioManager, string name) => audioManager.GetTrackStore(GetStore()).Get($"Resources/Testing/Track/{name}.mp3");
    }
}
