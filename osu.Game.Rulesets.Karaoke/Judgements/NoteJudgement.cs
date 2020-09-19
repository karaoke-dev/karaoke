﻿// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Game.Rulesets.Scoring;

namespace osu.Game.Rulesets.Karaoke.Judgements
{
    public class NoteJudgement : KaraokeJudgement
    {
        protected override int NumericResultFor(HitResult result)
        {
            switch (result)
            {
                default:
                    return 0;

                case HitResult.Meh:
                    return 300;

                case HitResult.Ok:
                    return 100;

                case HitResult.Good:
                    return 200;

                case HitResult.Great:
                case HitResult.Perfect:
                    return 300;
            }
        }

        protected override double HealthIncreaseFor(HitResult result)
        {
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
