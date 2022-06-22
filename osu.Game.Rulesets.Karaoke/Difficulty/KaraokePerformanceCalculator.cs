// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

#nullable disable

using System;
using System.Collections.Generic;
using System.Linq;
using osu.Game.Rulesets.Difficulty;
using osu.Game.Rulesets.Mods;
using osu.Game.Rulesets.Scoring;
using osu.Game.Scoring;

namespace osu.Game.Rulesets.Karaoke.Difficulty
{
    public class KaraokePerformanceCalculator : PerformanceCalculator
    {
        // Score after being scaled by non-difficulty-increasing mods
        private double scaledScore;

        private int countPerfect;
        private int countGreat;
        private int countGood;
        private int countOk;
        private int countMeh;
        private int countMiss;

        public KaraokePerformanceCalculator()
            : base(new KaraokeRuleset())
        {
        }

        protected override PerformanceAttributes CreatePerformanceAttributes(ScoreInfo score, DifficultyAttributes attributes)
        {
            var karaokeAttributes = (KaraokeDifficultyAttributes)attributes;

            scaledScore = score.TotalScore;
            countPerfect = score.Statistics.GetValueOrDefault(HitResult.Perfect);
            countGreat = score.Statistics.GetValueOrDefault(HitResult.Great);
            countGood = score.Statistics.GetValueOrDefault(HitResult.Good);
            countOk = score.Statistics.GetValueOrDefault(HitResult.Ok);
            countMeh = score.Statistics.GetValueOrDefault(HitResult.Meh);
            countMiss = score.Statistics.GetValueOrDefault(HitResult.Miss);

            if (karaokeAttributes.ScoreMultiplier > 0)
            {
                // Scale score up, so it's comparable to other keymods
                scaledScore *= 1.0 / karaokeAttributes.ScoreMultiplier;
            }

            // Arbitrary initial value for scaling pp in order to standardize distributions across game modes.
            // The specific number has no intrinsic meaning and can be adjusted as needed.
            double multiplier = 0.8;

            if (score.Mods.Any(m => m is ModNoFail))
                multiplier *= 0.9;
            if (score.Mods.Any(m => m is ModEasy))
                multiplier *= 0.5;

            double difficultyValue = computeDifficultyValue(karaokeAttributes);
            double accValue = computeAccuracyValue(difficultyValue, karaokeAttributes);
            double totalValue =
                Math.Pow(
                    Math.Pow(difficultyValue, 1.1) +
                    Math.Pow(accValue, 1.1), 1.0 / 1.1
                ) * multiplier;

            return new KaraokePerformanceAttributes
            {
                Difficulty = difficultyValue,
                Accuracy = accValue,
                ScaledScore = scaledScore,
                Total = totalValue
            };
        }

        private double computeDifficultyValue(KaraokeDifficultyAttributes attributes)
        {
            double difficultyValue = Math.Pow(5 * Math.Max(1, attributes.StarRating / 0.2) - 4.0, 2.2) / 135.0;

            difficultyValue *= 1.0 + 0.1 * Math.Min(1.0, totalHits / 1500.0);

            if (scaledScore <= 500000)
                difficultyValue = 0;
            else if (scaledScore <= 600000)
                difficultyValue *= (scaledScore - 500000) / 100000 * 0.3;
            else if (scaledScore <= 700000)
                difficultyValue *= 0.3 + (scaledScore - 600000) / 100000 * 0.25;
            else if (scaledScore <= 800000)
                difficultyValue *= 0.55 + (scaledScore - 700000) / 100000 * 0.20;
            else if (scaledScore <= 900000)
                difficultyValue *= 0.75 + (scaledScore - 800000) / 100000 * 0.15;
            else
                difficultyValue *= 0.90 + (scaledScore - 900000) / 100000 * 0.1;

            return difficultyValue;
        }

        private double computeAccuracyValue(double difficultyValue, KaraokeDifficultyAttributes attributes)
        {
            if (attributes.GreatHitWindow <= 0)
                return 0;

            // Lots of arbitrary values from testing.
            // Considering to use derivation from perfect accuracy in a probabilistic manner - assume normal distribution
            double accuracyValue = Math.Max(0.0, 0.2 - (attributes.GreatHitWindow - 34) * 0.006667)
                                   * difficultyValue
                                   * Math.Pow(Math.Max(0.0, scaledScore - 960000) / 40000, 1.1);

            return accuracyValue;
        }

        private double totalHits => countPerfect + countOk + countGreat + countGood + countMeh + countMiss;
    }
}
