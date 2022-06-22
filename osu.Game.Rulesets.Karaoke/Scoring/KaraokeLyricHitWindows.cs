// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

#nullable disable

using osu.Game.Rulesets.Scoring;

namespace osu.Game.Rulesets.Karaoke.Scoring
{
    public class KaraokeLyricHitWindows : KaraokeHitWindows
    {
        public const HitResult DEFAULT_HIT_RESULT = HitResult.Perfect;

        private static readonly DifficultyRange[] lyric_ranges =
        {
            new(DEFAULT_HIT_RESULT, 40, 20, 10),
        };

        public override bool IsHitResultAllowed(HitResult result) =>
            result switch
            {
                DEFAULT_HIT_RESULT => true,
                _ => false
            };

        protected override DifficultyRange[] GetRanges() => lyric_ranges;
    }
}
