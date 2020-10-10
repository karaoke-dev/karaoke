// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Game.Rulesets.Scoring;

namespace osu.Game.Rulesets.Karaoke.Judgements
{
    public class KaraokeLyricJudgement : KaraokeJudgement
    {
        public LyricTime Time { get; set; }

        public override HitResult MaxResult => HitResult.Perfect;

        protected override double HealthIncreaseFor(HitResult result) => 0;
    }

    public enum LyricTime
    {
        NotYet,

        Available,

        Exceed
    }
}
