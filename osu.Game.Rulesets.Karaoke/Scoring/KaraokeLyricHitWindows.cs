// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Game.Rulesets.Scoring;

namespace osu.Game.Rulesets.Karaoke.Scoring;

public class KaraokeLyricHitWindows : KaraokeHitWindows
{
    private static readonly DifficultyRange[] lyric_ranges =
    {
        new(HitResult.Perfect, 40, 20, 10),
    };

    public override bool IsHitResultAllowed(HitResult result) =>
        result switch
        {
            HitResult.Perfect => true,
            _ => false,
        };

    protected override DifficultyRange[] GetRanges() => lyric_ranges;
}
