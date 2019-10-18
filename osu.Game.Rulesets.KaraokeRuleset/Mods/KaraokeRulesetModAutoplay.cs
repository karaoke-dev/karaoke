// Copyright (c) ppy Pty Ltd <contact@ppy.sh>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Game.Beatmaps;
using osu.Game.Rulesets.Mods;
using osu.Game.Rulesets.KaraokeRuleset.Objects;
using osu.Game.Rulesets.KaraokeRuleset.Replays;
using osu.Game.Scoring;
using osu.Game.Users;

namespace osu.Game.Rulesets.KaraokeRuleset.Mods
{
    public class KaraokeRulesetModAutoplay : ModAutoplay<KaraokeRulesetHitObject>
    {
        public override Score CreateReplayScore(IBeatmap beatmap) => new Score
        {
            ScoreInfo = new ScoreInfo
            {
                User = new User { Username = "sample" },
            },
            Replay = new KaraokeRulesetAutoGenerator(beatmap).Generate(),
        };
    }
}
