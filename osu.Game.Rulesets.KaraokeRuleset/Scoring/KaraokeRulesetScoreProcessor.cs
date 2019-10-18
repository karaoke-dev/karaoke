// Copyright (c) ppy Pty Ltd <contact@ppy.sh>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Game.Rulesets.KaraokeRuleset.Objects;
using osu.Game.Rulesets.Scoring;
using osu.Game.Rulesets.UI;

namespace osu.Game.Rulesets.KaraokeRuleset.Scoring
{
    public class KaraokeRulesetScoreProcessor : ScoreProcessor<KaraokeRulesetHitObject>
    {
        public KaraokeRulesetScoreProcessor(DrawableRuleset<KaraokeRulesetHitObject> ruleset)
            : base(ruleset)
        {
        }

        protected override void Reset(bool storeResults)
        {
            base.Reset(storeResults);

            Health.Value = 1;
            Accuracy.Value = 1;
        }
    }
}