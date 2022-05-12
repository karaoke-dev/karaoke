// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using System.Collections.Generic;
using System.Linq;
using osu.Game.Beatmaps;
using osu.Game.Extensions;
using osu.Game.Rulesets.Difficulty;
using osu.Game.Rulesets.Difficulty.Preprocessing;
using osu.Game.Rulesets.Difficulty.Skills;
using osu.Game.Rulesets.Karaoke.Beatmaps;
using osu.Game.Rulesets.Karaoke.Difficulty.Preprocessing;
using osu.Game.Rulesets.Karaoke.Difficulty.Skills;
using osu.Game.Rulesets.Karaoke.Mods;
using osu.Game.Rulesets.Karaoke.Objects;
using osu.Game.Rulesets.Mods;

namespace osu.Game.Rulesets.Karaoke.Difficulty
{
    public class KaraokeDifficultyCalculator : DifficultyCalculator
    {
        private const double star_scaling_factor = 0.018;

        private readonly bool isForCurrentRuleset;
        private readonly double originalOverallDifficulty;

        public KaraokeDifficultyCalculator(IRulesetInfo ruleset, IWorkingBeatmap beatmap)
            : base(ruleset, beatmap)
        {
            isForCurrentRuleset = beatmap.BeatmapInfo.Ruleset.MatchesOnlineID(ruleset);
            originalOverallDifficulty = beatmap.BeatmapInfo.Difficulty.OverallDifficulty;
        }

        protected override DifficultyAttributes CreateDifficultyAttributes(IBeatmap beatmap, Mod[] mods, Skill[] skills, double clockRate)
        {
            if (beatmap.HitObjects.Count == 0)
                return new KaraokeDifficultyAttributes { Mods = mods };

            return new KaraokeDifficultyAttributes
            {
                StarRating = skills[0].DifficultyValue() * star_scaling_factor,
                Mods = mods,
                // Todo: This int cast is temporary to achieve 1:1 results with osu!stable, and should be removed in the future
                GreatHitWindow = (int)Math.Ceiling(getHitWindow300(mods) / clockRate),
                MaxCombo = beatmap.HitObjects.Sum(h => h is Note ? 2 : 1),
            };
        }

        protected override IEnumerable<DifficultyHitObject> CreateDifficultyHitObjects(IBeatmap beatmap, double clockRate)
        {
            var sortedObjects = beatmap.HitObjects.OfType<Note>().ToArray();

            // todo : might have a sort.
            // LegacySortHelper<HitObject>.Sort(sortedObjects, Comparer<HitObject>.Create((a, b) => (int)Math.Round(a.StartTime) - (int)Math.Round(b.StartTime)));

            for (int i = 1; i < sortedObjects.Length; i++)
                yield return new KaraokeDifficultyHitObject(sortedObjects[i], sortedObjects[i - 1], clockRate);
        }

        // Sorting is done in CreateDifficultyHitObjects, since the full list of hitobjects is required.
        protected override IEnumerable<DifficultyHitObject> SortObjects(IEnumerable<DifficultyHitObject> input) => input;

        protected override Skill[] CreateSkills(IBeatmap beatmap, Mod[] mods, double clockRate) => new Skill[]
        {
            new Strain(mods, ((KaraokeBeatmap)beatmap).TotalColumns)
        };

        protected override Mod[] DifficultyAdjustmentMods =>
            new Mod[]
            {
                new KaraokeModDisableNote(),
                new KaraokeModHiddenNote(),
            };

        private int getHitWindow300(Mod[] mods)
        {
            if (isForCurrentRuleset)
            {
                double od = Math.Min(10.0, Math.Max(0, 10.0 - originalOverallDifficulty));
                return applyModAdjustments(34 + 3 * od, mods);
            }

            if (Math.Round(originalOverallDifficulty) > 4)
                return applyModAdjustments(34, mods);

            return applyModAdjustments(47, mods);

            static int applyModAdjustments(double value, Mod[] mods)
            {
                if (mods.Any(m => m is KaraokeModDisableNote))
                    value /= 1.4;
                else if (mods.Any(m => m is KaraokeModHiddenNote))
                    value *= 1.4;

                return (int)value;
            }
        }
    }
}
