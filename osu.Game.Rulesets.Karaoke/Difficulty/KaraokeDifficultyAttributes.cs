// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Collections.Generic;
using Newtonsoft.Json;
using osu.Game.Beatmaps;
using osu.Game.Rulesets.Difficulty;

namespace osu.Game.Rulesets.Karaoke.Difficulty;

public class KaraokeDifficultyAttributes : DifficultyAttributes
{
    /// <summary>
    /// The hit window for a GREAT hit inclusive of rate-adjusting mods (DT/HT/etc).
    /// </summary>
    /// <remarks>
    /// Rate-adjusting mods do not affect the hit window at all in osu-stable.
    /// </remarks>
    [JsonProperty("great_hit_window")]
    public double GreatHitWindow { get; set; }

    /// <summary>
    /// The score multiplier applied via score-reducing mods.
    /// </summary>
    [JsonProperty("score_multiplier")]
    public double ScoreMultiplier { get; set; }

    public override IEnumerable<(int attributeId, object value)> ToDatabaseAttributes()
    {
        foreach (var v in base.ToDatabaseAttributes())
            yield return v;

        yield return (ATTRIB_ID_DIFFICULTY, StarRating);
    }

    public override void FromDatabaseAttributes(IReadOnlyDictionary<int, double> values, IBeatmapOnlineInfo onlineInfo)
    {
        base.FromDatabaseAttributes(values, onlineInfo);

        StarRating = values[ATTRIB_ID_DIFFICULTY];
    }
}
