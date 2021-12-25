// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using Newtonsoft.Json;
using osu.Game.Rulesets.Difficulty;

namespace osu.Game.Rulesets.Karaoke.Difficulty
{
    public class KaraokePerformanceAttributes : PerformanceAttributes
    {
        [JsonProperty("accuracy")]
        public double Accuracy { get; set; }

        [JsonProperty("scaled_score")]
        public double ScaledScore { get; set; }
    }
}
