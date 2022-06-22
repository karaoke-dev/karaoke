// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

#nullable disable

using System.Collections.Generic;
using System.Linq;
using osu.Game.Rulesets.Karaoke.Replays;
using osu.Game.Rulesets.Replays;
using osu.Game.Rulesets.UI;
using osu.Game.Scoring;
using osuTK;

namespace osu.Game.Rulesets.Karaoke.UI
{
    public class KaraokeReplayRecorder : ReplayRecorder<KaraokeScoringAction>
    {
        public KaraokeReplayRecorder(Score score)
            : base(score)
        {
        }

        protected override ReplayFrame HandleFrame(Vector2 mousePosition, List<KaraokeScoringAction> actions, ReplayFrame previousFrame)
        {
            if (actions.Any())
                return new KaraokeReplayFrame(Time.Current, actions.FirstOrDefault().Scale);

            return new KaraokeReplayFrame(Time.Current);
        }
    }
}
