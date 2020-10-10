// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Game.Rulesets.Scoring;

namespace osu.Game.Rulesets.Karaoke.Judgements
{
    public class KaraokeNoteJudgement : KaraokeJudgement
    {
        public bool Saitenable { get; set; }

        protected override double HealthIncreaseFor(HitResult result)
        {
            if (!Saitenable)
                return 0;

            switch (result)
            {
                case HitResult.Miss:
                    return -0.125;

                case HitResult.Meh:
                    return 0.005;

                case HitResult.Ok:
                    return 0.010;

                case HitResult.Good:
                    return 0.035;

                case HitResult.Great:
                    return 0.055;

                case HitResult.Perfect:
                    return 0.065;

                default:
                    return 0;
            }
        }
    }
}
