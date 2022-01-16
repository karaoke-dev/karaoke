// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using osu.Game.Beatmaps;
using osu.Game.Rulesets.Karaoke.Beatmaps.Metadatas;
using osu.Game.Rulesets.Karaoke.Objects;

namespace osu.Game.Rulesets.Karaoke.Beatmaps
{
    public static class KaraokeBeatmapExtension
    {
        public static bool IsScorable(this IBeatmap beatmap) => beatmap?.HitObjects.OfType<Note>().Any(x => x.Display) ?? false;

        public static List<CultureInfo> AvailableTranslates(this IBeatmap beatmap) => (beatmap as KaraokeBeatmap)?.AvailableTranslates ?? new List<CultureInfo>();

        public static bool AnyTranslate(this IBeatmap beatmap) => beatmap?.AvailableTranslates()?.Any() ?? false;

        public static float PitchToScale(this IBeatmap beatmap, float pitch)
        {
            return pitch / 20 - 7;
        }

        public static List<Singer> GetSingers(this IBeatmap beatmap) => (beatmap as KaraokeBeatmap)?.Singers;
    }
}
