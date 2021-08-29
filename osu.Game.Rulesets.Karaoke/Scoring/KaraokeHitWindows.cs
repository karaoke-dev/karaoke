// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Game.Rulesets.Scoring;

namespace osu.Game.Rulesets.Karaoke.Scoring
{
    public class KaraokeHitWindows : HitWindows
    {
        private static readonly DifficultyRange[] karaoke_ranges =
        {
            new(HitResult.Perfect, 80, 50, 20),
            new(HitResult.Meh, 80, 50, 20),
            new(HitResult.Miss, 2000, 1500, 1000),
        };

        public override bool IsHitResultAllowed(HitResult result)
        {
            // In karaoke ruleset, time range is not the first thing.
            return result switch
            {
                // Karaoke note hit result
                HitResult.Perfect => true,
                // Lyric hit result
                HitResult.Meh => true,
                _ => false
            };
        }

        protected override DifficultyRange[] GetRanges() => karaoke_ranges;
    }
}
