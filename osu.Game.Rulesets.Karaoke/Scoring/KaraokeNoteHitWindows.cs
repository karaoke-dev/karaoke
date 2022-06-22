// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

#nullable disable

using osu.Game.Rulesets.Scoring;

namespace osu.Game.Rulesets.Karaoke.Scoring
{
    public class KaraokeNoteHitWindows : KaraokeHitWindows
    {
        private static readonly DifficultyRange[] karaoke_ranges =
        {
            new(HitResult.Perfect, 80, 50, 20),
            new(HitResult.Meh, 80, 50, 20),
            new(HitResult.Miss, 2000, 1500, 1000),
        };

        public override bool IsHitResultAllowed(HitResult result) =>
            result switch
            {
                HitResult.Perfect => true,
                HitResult.Meh => true,
                _ => false
            };

        protected override DifficultyRange[] GetRanges() => karaoke_ranges;
    }
}
