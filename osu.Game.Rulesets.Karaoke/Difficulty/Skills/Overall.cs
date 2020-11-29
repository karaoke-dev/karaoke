// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Game.Rulesets.Difficulty.Preprocessing;
using osu.Game.Rulesets.Difficulty.Skills;
using osu.Game.Rulesets.Karaoke.Difficulty.Preprocessing;

namespace osu.Game.Rulesets.Karaoke.Difficulty.Skills
{
    public class Overall : Skill
    {
        protected override double SkillMultiplier => 1;
        protected override double StrainDecayBase => 0.3;

        private readonly double[] holdEndTimes;

        private readonly int columnCount;
        private readonly int minColumn;

        public Overall(int columnCount, int minColumn)
        {
            this.columnCount = columnCount;
            this.minColumn = minColumn;

            holdEndTimes = new double[columnCount];
        }

        protected override double StrainValueOf(DifficultyHitObject current)
        {
            var karaokeCurrent = (KaraokeDifficultyHitObject)current;
            var endTime = karaokeCurrent.BaseObject.EndTime;

            double holdFactor = 1.0; // Factor in case something else is held
            double holdAddition = 0; // in addition to the current note in case it's a hold and has to be released awkwardly

            for (int i = 0; i < columnCount; i++)
            {
                // If there is at least one other overlapping end or note, then we get an addition, buuuuuut...
                if (current.BaseObject.StartTime < holdEndTimes[i] && endTime > holdEndTimes[i])
                    holdAddition = 1.0;

                // ... this addition only is valid if there is _no_ other note with the same ending.
                // Releasing multiple notes at the same time is just as easy as releasing one
                if (endTime == holdEndTimes[i])
                    holdAddition = 0;

                // We give a slight bonus if something is held meanwhile
                if (holdEndTimes[i] > endTime)
                    holdFactor = 1.25;
            }

            holdEndTimes[karaokeCurrent.BaseObject.Tone.Scale - minColumn] = endTime;

            return (1 + holdAddition) * holdFactor;
        }
    }
}
