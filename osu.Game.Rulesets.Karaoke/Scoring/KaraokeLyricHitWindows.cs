// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using osu.Game.Beatmaps;
using osu.Game.Rulesets.Scoring;

namespace osu.Game.Rulesets.Karaoke.Scoring;

public class KaraokeLyricHitWindows : KaraokeHitWindows
{
    private static readonly DifficultyRange perfect_window_range = new(40D, 20D, 10D);

    private double perfect;

    public override bool IsHitResultAllowed(HitResult result) =>
        result switch
        {
            HitResult.Perfect => true,
            // todo: add this in order not to throw error in some test cases.
            HitResult.Miss => true,
            _ => false,
        };

    public override void SetDifficulty(double difficulty)
    {
        perfect = IBeatmapDifficultyInfo.DifficultyRange(difficulty, perfect_window_range) ;
    }

    public override double WindowFor(HitResult result)
    {
        switch (result)
        {
            case HitResult.Perfect:
                return perfect;

            // todo: add this in order not to throw error in some test cases.
            case HitResult.Miss:
                return 1000;

            default:
                throw new ArgumentOutOfRangeException(nameof(result), result, null);
        }
    }
}
