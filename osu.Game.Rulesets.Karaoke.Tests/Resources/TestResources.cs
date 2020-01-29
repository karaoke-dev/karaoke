// Copyright (c) ppy Pty Ltd <contact@ppy.sh>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using System.IO;
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
    }
}
