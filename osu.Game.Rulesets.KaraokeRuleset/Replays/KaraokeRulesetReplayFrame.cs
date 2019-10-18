// Copyright (c) ppy Pty Ltd <contact@ppy.sh>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Collections.Generic;
using osu.Game.Rulesets.Replays;
using osuTK;

namespace osu.Game.Rulesets.KaraokeRuleset.Replays
{
    public class KaraokeRulesetReplayFrame : ReplayFrame
    {
        public List<KaraokeRulesetAction> Actions = new List<KaraokeRulesetAction>();
        public Vector2 Position;

        public KaraokeRulesetReplayFrame(KaraokeRulesetAction? button = null)
        {
            if (button.HasValue)
                Actions.Add(button.Value);
        }
    }
}
