// Copyright (c) ppy Pty Ltd <contact@ppy.sh>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Collections.Generic;
using osu.Game.Rulesets.Replays;
using osuTK;

namespace osu.Game.Rulesets.Karaoke.Replays
{
    public class KaraokeReplayFrame : ReplayFrame
    {
        public List<KaraokeAction> Actions = new List<KaraokeAction>();
        public Vector2 Position;

        public KaraokeReplayFrame(KaraokeAction? button = null)
        {
            if (button.HasValue)
                Actions.Add(button.Value);
        }
    }
}
