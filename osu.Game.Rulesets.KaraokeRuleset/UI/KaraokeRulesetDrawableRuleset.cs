// Copyright (c) ppy Pty Ltd <contact@ppy.sh>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Collections.Generic;
using osu.Framework.Allocation;
using osu.Framework.Input;
using osu.Game.Beatmaps;
using osu.Game.Input.Handlers;
using osu.Game.Replays;
using osu.Game.Rulesets.Mods;
using osu.Game.Rulesets.Objects.Drawables;
using osu.Game.Rulesets.KaraokeRuleset.Objects;
using osu.Game.Rulesets.KaraokeRuleset.Objects.Drawables;
using osu.Game.Rulesets.KaraokeRuleset.Replays;
using osu.Game.Rulesets.KaraokeRuleset.Scoring;
using osu.Game.Rulesets.Scoring;
using osu.Game.Rulesets.UI;

namespace osu.Game.Rulesets.KaraokeRuleset.UI
{
    [Cached]
    public class DrawableKaraokeRulesetRuleset : DrawableRuleset<KaraokeRulesetHitObject>
    {
        public DrawableKaraokeRulesetRuleset(KaraokeRulesetRuleset ruleset, IWorkingBeatmap beatmap, IReadOnlyList<Mod> mods)
            : base(ruleset, beatmap, mods)
        {
        }

        public override ScoreProcessor CreateScoreProcessor() => new KaraokeRulesetScoreProcessor(this);

        protected override Playfield CreatePlayfield() => new KaraokeRulesetPlayfield();

        protected override ReplayInputHandler CreateReplayInputHandler(Replay replay) => new KaraokeRulesetFramedReplayInputHandler(replay);

        public override DrawableHitObject<KaraokeRulesetHitObject> CreateDrawableRepresentation(KaraokeRulesetHitObject h) => new DrawableKaraokeRulesetHitObject(h);

        protected override PassThroughInputManager CreateInputManager() => new KaraokeRulesetInputManager(Ruleset?.RulesetInfo);
    }
}
