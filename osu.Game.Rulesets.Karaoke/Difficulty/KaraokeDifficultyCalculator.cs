// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Collections.Generic;
using System.Linq;
using osu.Game.Beatmaps;
using osu.Game.Rulesets.Difficulty;
using osu.Game.Rulesets.Difficulty.Preprocessing;
using osu.Game.Rulesets.Difficulty.Skills;
using osu.Game.Rulesets.Karaoke.Difficulty.Preprocessing;
using osu.Game.Rulesets.Karaoke.Difficulty.Skills;
using osu.Game.Rulesets.Karaoke.Mods;
using osu.Game.Rulesets.Karaoke.Objects;
using osu.Game.Rulesets.Karaoke.Scoring;
using osu.Game.Rulesets.Mods;
using osu.Game.Rulesets.Scoring;

namespace osu.Game.Rulesets.Karaoke.Difficulty
{
    public class KaraokeDifficultyCalculator : DifficultyCalculator
    {
        private const double star_scaling_factor = 0.05;

        public KaraokeDifficultyCalculator(Ruleset ruleset, WorkingBeatmap beatmap)
            : base(ruleset, beatmap)
        {
        }

        protected override DifficultyAttributes CreateDifficultyAttributes(IBeatmap beatmap, Mod[] mods, Skill[] skills, double clockRate)
        {
            if (beatmap.HitObjects.Count == 0)
                return new KaraokeDifficultyAttributes { Mods = mods, Skills = skills };

            HitWindows hitWindows = new KaraokeHitWindows();
            hitWindows.SetDifficulty(beatmap.BeatmapInfo.BaseDifficulty.OverallDifficulty);

            return new KaraokeDifficultyAttributes
            {
                StarRating = skills[0].DifficultyValue() * star_scaling_factor,
                Mods = mods,
                // Todo: This int cast is temporary to achieve 1:1 results with osu!stable, and should be removed in the future
                GreatHitWindow = (int)hitWindows.WindowFor(HitResult.Great) / clockRate,
                Skills = skills
            };
        }

        protected override IEnumerable<DifficultyHitObject> CreateDifficultyHitObjects(IBeatmap beatmap, double clockRate)
        {
            // Only karaoke note can be apply in difficulty calculation
            var notes = beatmap.HitObjects.OfType<Note>().ToList();

            for (int i = 1; i < notes.Count; i++)
                yield return new KaraokeDifficultyHitObject(notes[i], notes[i - 1], clockRate);
        }

        protected override Skill[] CreateSkills(IBeatmap beatmap, Mod[] mods, double clockRate)
        {
            // Only karaoke note can be apply in difficulty calculation
            var notes = beatmap.HitObjects.OfType<Note>().ToList();
            if (!notes.Any())
                return new Skill[] { };

            // TODO : need to get real value in the future
            var maxNoteColumn = notes.Max(x => x.Tone);
            var minNoteColumn = notes.Min(x => x.Tone);

            int columnCount = maxNoteColumn.Scale - minNoteColumn.Scale + 1;

            var skills = new List<Skill> { new Overall(columnCount, minNoteColumn.Scale, mods) };

            for (int i = 0; i < columnCount; i++)
                skills.Add(new Individual(i, columnCount, minNoteColumn.Scale, mods));

            return skills.ToArray();
        }

        protected override Mod[] DifficultyAdjustmentMods =>
            new Mod[]
            {
                new KaraokeModPractice(),
            };
    }
}
