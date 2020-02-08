// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Linq;
using osu.Game.Rulesets.Difficulty.Preprocessing;
using osu.Game.Rulesets.Difficulty.Skills;
using osu.Game.Rulesets.Karaoke.Difficulty.Preprocessing;

namespace osu.Game.Rulesets.Karaoke.Difficulty.Skills
{
    public class Individual : Skill
    {
        protected override double SkillMultiplier => 1;
        protected override double StrainDecayBase => 0.125;

        private readonly double[] holdEndTimes;

        private readonly int column;
        private readonly int minColumn;

        public Individual(int column, int columnCount, int minColumn)
        {
            this.column = column;
            this.minColumn = minColumn;

            holdEndTimes = new double[columnCount];
        }

        protected override double StrainValueOf(DifficultyHitObject current)
        {
            var karaokeCurrent = (KaraokeDifficultyHitObject)current;
            var endTime = karaokeCurrent.BaseObject.EndTime;

            try
            {
                if (karaokeCurrent.BaseObject.Tone.Scale != column)
                    return 0;

                // We give a slight bonus if something is held meanwhile
                return holdEndTimes.Any(t => t > endTime) ? 2.5 : 2;
            }
            finally
            {
                holdEndTimes[karaokeCurrent.BaseObject.Tone.Scale - minColumn] = endTime;
            }
        }
    }
}
