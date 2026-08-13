// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Game.Rulesets.Karaoke.Mods;
using osu.Game.Rulesets.Scoring;

namespace osu.Game.Rulesets.Karaoke.Scoring;

public class KaraokeScoreMultiplierCalculator : ScoreMultiplierCalculator
{
    public KaraokeScoreMultiplierCalculator(ScoreMultiplierContext context)
        : base(context)
    {
        Single<KaraokeModNoFail>(hasMultiplier: 0.5);
        Single<KaraokeModHiddenNote>(hasMultiplier: 1.06);
        Single<KaraokeModPractice>(hasMultiplier: 0);
        Single<KaraokeModDisableNote>(hasMultiplier: 0);
    }
}
