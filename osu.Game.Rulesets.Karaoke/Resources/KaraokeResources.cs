// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System.IO;
using osu.Framework.IO.Stores;
using osu.Game.Beatmaps;
using osu.Game.Beatmaps.Formats;
using osu.Game.IO;

namespace osu.Game.Rulesets.Karaoke.Resources
{
    public static class KaraokeResources
    {
        public static DllResourceStore GetStore() => new DllResourceStore(typeof(KaraokeResources).Assembly);

        public static Stream OpenResource(string name) => GetStore().GetStream($"Resources/{name}");

        public static Stream OpenBeatmapResource(string name) => OpenResource($"Beatmaps/{name}.osu");

        public static Beatmap OpenBeatmap(string name)
        {
            using (var resStream = OpenBeatmapResource(name))
            using (var stream = new LineBufferedReader(resStream))
            {
                var decoder = Decoder.GetDecoder<Beatmap>(stream);
                return decoder.Decode(stream);
            }
        }
    }
}
