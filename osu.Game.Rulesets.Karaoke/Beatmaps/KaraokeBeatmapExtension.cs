// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using osu.Game.Beatmaps;

namespace osu.Game.Rulesets.Karaoke.Beatmaps;

public static class KaraokeBeatmapExtension
{
    public static bool IsScorable(this IBeatmap beatmap)
    {
        // we should throw invalidate exception here but it will cause test case failed.
        // because beatmap in the working beatmap in test case not always be karaoke beatmap class.
        return beatmap is KaraokeBeatmap karaokeBeatmap && karaokeBeatmap.Scorable;
    }

    public static IList<CultureInfo> AvailableTranslates(this IBeatmap beatmap) => (beatmap as KaraokeBeatmap)?.AvailableTranslationLanguages ?? new List<CultureInfo>();

    public static bool AnyTranslate(this IBeatmap beatmap) => beatmap.AvailableTranslates().Any();

    public static float PitchToScale(this IBeatmap beatmap, float pitch)
    {
        return pitch / 20 - 7;
    }
}
