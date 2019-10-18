// Copyright (c) ppy Pty Ltd <contact@ppy.sh>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Collections.Generic;
using osu.Game.Beatmaps;
using osu.Game.Replays;
using osu.Game.Rulesets.KaraokeRuleset.Objects;
using osu.Game.Rulesets.Replays;

namespace osu.Game.Rulesets.KaraokeRuleset.Replays
{
    public class KaraokeRulesetAutoGenerator : AutoGenerator
    {
        protected Replay Replay;
        protected List<ReplayFrame> Frames => Replay.Frames;

        public new Beatmap<KaraokeRulesetHitObject> Beatmap => (Beatmap<KaraokeRulesetHitObject>)base.Beatmap;

        public KaraokeRulesetAutoGenerator(IBeatmap beatmap)
            : base(beatmap)
        {
            Replay = new Replay();
        }

        public override Replay Generate()
        {
            Frames.Add(new KaraokeRulesetReplayFrame());

            foreach (KaraokeRulesetHitObject hitObject in Beatmap.HitObjects)
            {
                Frames.Add(new KaraokeRulesetReplayFrame
                {
                    Time = hitObject.StartTime,
                    Position = hitObject.Position,
                    // todo: add required inputs and extra frames.
                });
            }

            return Replay;
        }
    }
}
