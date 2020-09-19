// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Collections.Generic;
using System.Linq;
using osu.Game.Beatmaps;
using osu.Game.Rulesets.Karaoke.Objects;

namespace osu.Game.Rulesets.Karaoke.Beatmaps
{
    public static class KaraokeBeatmapExtension
    {
        public static IDictionary<string, List<string>> GetTranslates(this IBeatmap beatmap) => (beatmap as KaraokeBeatmap)?.Translates;

        public static bool IsScorable(this IBeatmap beatmap) => beatmap?.HitObjects.OfType<Note>().Any(x => x.Display) ?? false;

        public static List<string> GetTranslate(this IBeatmap beatmap, string languageCode)
        {
            if (beatmap.GetTranslates().TryGetValue(languageCode, out var result))
                return result;

            return null;
        }

        public static string[] AvailableTranslates(this IBeatmap beatmap) => beatmap?.GetTranslates()?.Keys.ToArray() ?? new string[] { };

        public static bool AnyTranslate(this IBeatmap beatmap) => beatmap?.AvailableTranslates()?.Any() ?? false;

        public static float PitchToScale(this IBeatmap beatmap, float pitch)
        {
            return pitch / 20 - 7;
        }
    }
}
