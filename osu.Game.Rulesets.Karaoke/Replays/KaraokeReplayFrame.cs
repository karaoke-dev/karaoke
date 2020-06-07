// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Game.Rulesets.Replays;

namespace osu.Game.Rulesets.Karaoke.Replays
{
    public class KaraokeReplayFrame : ReplayFrame
    {
        /// <summary>
        /// Use for Saiten playfield
        /// Maybe format will be changed, but i have no idea now.
        /// </summary>
        public float Scale { get; set; }

        // To record this frame has sound.
        public bool Sound { get; set; }

        public KaraokeReplayFrame(double time)
             : base(time)
        {
        }

        public KaraokeReplayFrame(double time, float scale)
            : base(time)
        {
            Scale = scale;
            Sound = true;
        }

        public override string ToString() => $"{Time}, {Scale}";
    }
}
