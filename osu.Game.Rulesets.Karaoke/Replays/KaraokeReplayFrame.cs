// Copyright (c) ppy Pty Ltd <contact@ppy.sh>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Game.Rulesets.Replays;

namespace osu.Game.Rulesets.Karaoke.Replays
{
    public class KaraokeReplayFrame : ReplayFrame
    {
        /// <summary>
        /// Use for SaitenPlayfield
        /// Maybe format will be changed, but i have no idea now.
        /// </summary>
        public float Scale { get; set; }

        // To record this frame has sound.
        public bool Sound { get; set; }

        public KaraokeReplayFrame()
        {
        }

        public KaraokeReplayFrame(double time, float scale)
            : base(time)
        {
            Scale = scale;
            Sound = true;
        }
    }
}
