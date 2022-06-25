// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Game.Rulesets.Scoring;

namespace osu.Game.Rulesets.Karaoke.Judgements
{
    public class KaraokeNoteJudgement : KaraokeJudgement
    {
        public bool Scorable { get; set; }

        protected override double HealthIncreaseFor(HitResult result)
        {
            if (!Scorable)
                return 0;

            return result switch
            {
                HitResult.Miss => -0.125,
                HitResult.Meh => 0.005,
                HitResult.Ok => 0.010,
                HitResult.Good => 0.035,
                HitResult.Great => 0.055,
                HitResult.Perfect => 0.065,
                _ => 0
            };
        }
    }
}
