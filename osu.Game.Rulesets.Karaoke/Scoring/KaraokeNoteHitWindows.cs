// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using osu.Game.Beatmaps;
using osu.Game.Rulesets.Scoring;

namespace osu.Game.Rulesets.Karaoke.Scoring;

public class KaraokeNoteHitWindows : KaraokeHitWindows
{
    private static readonly DifficultyRange perfect_window_range = new(80D, 50D, 20D);
    private static readonly DifficultyRange meh_window_range = new(80D, 50D, 20D);
    private static readonly DifficultyRange miss_window_range = new(2000D, 1500D, 1000D);

    private double perfect;
    private double meh;
    private double miss;

    public override bool IsHitResultAllowed(HitResult result) =>
        result switch
        {
            HitResult.Perfect => true,
            HitResult.Meh => true,
            HitResult.Miss => true,
            _ => false,
        };

    public override void SetDifficulty(double difficulty)
    {
        perfect = IBeatmapDifficultyInfo.DifficultyRange(difficulty, perfect_window_range) ;
        meh = IBeatmapDifficultyInfo.DifficultyRange(difficulty, meh_window_range);
        miss = IBeatmapDifficultyInfo.DifficultyRange(difficulty, miss_window_range);
    }

    public override double WindowFor(HitResult result)
    {
        switch (result)
        {
            case HitResult.Perfect:
                return perfect;

            case HitResult.Meh:
                return meh;

            case HitResult.Miss:
                return miss;

            default:
                throw new ArgumentOutOfRangeException(nameof(result), result, null);
        }
    }
}
