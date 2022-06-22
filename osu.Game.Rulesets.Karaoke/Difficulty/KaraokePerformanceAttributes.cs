// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

#nullable disable

using System.Collections.Generic;
using Newtonsoft.Json;
using osu.Game.Rulesets.Difficulty;

namespace osu.Game.Rulesets.Karaoke.Difficulty
{
    public class KaraokePerformanceAttributes : PerformanceAttributes
    {
        [JsonProperty("difficulty")]
        public double Difficulty { get; set; }

        [JsonProperty("accuracy")]
        public double Accuracy { get; set; }

        [JsonProperty("scaled_score")]
        public double ScaledScore { get; set; }

        public override IEnumerable<PerformanceDisplayAttribute> GetAttributesForDisplay()
        {
            foreach (var attribute in base.GetAttributesForDisplay())
                yield return attribute;

            yield return new PerformanceDisplayAttribute(nameof(Difficulty), "Difficulty", Difficulty);
            yield return new PerformanceDisplayAttribute(nameof(Accuracy), "Accuracy", Accuracy);
        }
    }
}
