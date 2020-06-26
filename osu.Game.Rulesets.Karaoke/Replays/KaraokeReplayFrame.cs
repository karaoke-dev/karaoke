// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Game.Beatmaps;
using osu.Game.Replays.Legacy;
using osu.Game.Rulesets.Replays;
using osu.Game.Rulesets.Replays.Types;

namespace osu.Game.Rulesets.Karaoke.Replays
{
    public class KaraokeReplayFrame : ReplayFrame, IConvertibleReplayFrame
    {
        /// <summary>
        /// Use for Saiten playfield
        /// Maybe format will be changed, but i have no idea now.
        /// </summary>
        public float Scale { get; private set; }

        // To record this frame has sound.
        public bool Sound { get; private set; }

        public KaraokeReplayFrame()
        {
        }

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

        public void FromLegacy(LegacyReplayFrame currentFrame, IBeatmap beatmap, ReplayFrame lastFrame = null)
        {
            Sound = currentFrame.MouseY.HasValue;
            Scale = currentFrame.MouseY.GetValueOrDefault();
        }

        public LegacyReplayFrame ToLegacy(IBeatmap beatmap)
        {
            var mouseYPosition = Sound ? Scale : default(float?);
            return new LegacyReplayFrame(Time, null, mouseYPosition, ReplayButtonState.None);
        }
    }
}
