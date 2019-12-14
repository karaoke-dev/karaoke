// Copyright (c) ppy Pty Ltd <contact@ppy.sh>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using System.IO;
using System.Reflection;

namespace osu.Game.Rulesets.Karaoke.Tests.Resources
{
    public static class TestResources
    {
        public static string ResourceAssembly = "osu.Game.Rulesets.Karaoke.Tests";

        public static Stream OpenBeatmapResource(string name) => OpenResource($"Testing.Beatmaps.{name}.osu");

        public static Stream OpenNicoKaraResource(string name) => OpenResource($"Testing.NicoKara.{name}.nkmproj");

        public static Stream OpenResource(string name)
        {
            var localPath = Path.GetDirectoryName(Uri.UnescapeDataString(new UriBuilder(Assembly.GetExecutingAssembly().CodeBase).Path));
            return Assembly.LoadFrom(Path.Combine(localPath, $"{ResourceAssembly}.dll")).GetManifestResourceStream($@"{ResourceAssembly}.Resources.{name}");
        }
    }
}
