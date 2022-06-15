// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using osu.Game.Beatmaps;
using osu.Game.Rulesets.Karaoke.Beatmaps.Metadatas;

namespace osu.Game.Rulesets.Karaoke.Beatmaps
{
    public static class KaraokeBeatmapExtension
    {
        public static bool IsScorable(this IBeatmap beatmap)
        {
            if (beatmap is not KaraokeBeatmap karaokeBeatmap)
                throw new InvalidCastException();

            return karaokeBeatmap.Scorable;
        }

        public static List<CultureInfo> AvailableTranslates(this IBeatmap beatmap) => (beatmap as KaraokeBeatmap)?.AvailableTranslates ?? new List<CultureInfo>();

        public static bool AnyTranslate(this IBeatmap beatmap) => beatmap?.AvailableTranslates()?.Any() ?? false;

        public static float PitchToScale(this IBeatmap beatmap, float pitch)
        {
            return pitch / 20 - 7;
        }

        public static List<Singer> GetSingers(this IBeatmap beatmap) => (beatmap as KaraokeBeatmap)?.Singers;
    }
}
