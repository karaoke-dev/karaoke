// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Game.Replays;
using osu.Game.Rulesets.Karaoke.Beatmaps;
using osu.Game.Rulesets.Replays;
using System.Collections.Generic;

namespace osu.Game.Rulesets.Karaoke.Replays
{
    public class KaraokeAutoGeneratorBySinger : AutoGenerator
    {
        public KaraokeAutoGeneratorBySinger(KaraokeBeatmap beatmap)
           : base(beatmap)
        {
            
        }

        public override Replay Generate()
        {
            return new Replay
            {
                Frames = new List<ReplayFrame>()
            };
        }
    }
}
