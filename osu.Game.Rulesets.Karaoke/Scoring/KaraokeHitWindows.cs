// Copyright (c) ppy Pty Ltd <contact@ppy.sh>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Game.Rulesets.Scoring;

namespace osu.Game.Rulesets.Karaoke.Scoring
{
    public class KaraokeHitWindows : HitWindows
    {
        private static readonly DifficultyRange[] karaoke_ranges =
        {
            new DifficultyRange(HitResult.Perfect, 80, 50, 20),
            new DifficultyRange(HitResult.Meh, 80, 50, 20),
            new DifficultyRange(HitResult.Miss, 2000, 1500, 1000),
        };

        public override bool IsHitResultAllowed(HitResult result)
        {
            // In karaoke ruleset, time range is not the first thing.
            switch (result)
            {
                // Karaoke note hit result
                case HitResult.Perfect:
                    return true;

                // Lyric hit result
                case HitResult.Meh:
                    return true;
            }

            return false;
        }

        protected override DifficultyRange[] GetRanges() => karaoke_ranges;
    }
}
