// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

#nullable disable

using System;
using System.Linq;
using osu.Framework.Allocation;
using osu.Framework.Extensions.IEnumerableExtensions;
using osu.Game.Graphics;
using osu.Game.Replays;
using osu.Game.Rulesets.Karaoke.Replays;
using osu.Game.Rulesets.Karaoke.UI.Position;

namespace osu.Game.Rulesets.Karaoke.UI.Components
{
    public class ReplayScoringVisualization : VoiceVisualization<KaraokeReplayFrame>
    {
        protected override float PathRadius => 1.5f;

        [Resolved]
        private INotePositionInfo notePositionInfo { get; set; }

        public ReplayScoringVisualization(Replay replay)
        {
            var frames = replay?.Frames.OfType<KaraokeReplayFrame>();
            frames?.ForEach(Add);
        }

        protected override double GetTime(KaraokeReplayFrame frame) => frame.Time;

        protected override float GetPosition(KaraokeReplayFrame frame) => notePositionInfo.Calculator.YPositionAt(frame);

        private bool createNew = true;

        private double minAvailableTime;

        public void Add(KaraokeReplayFrame frame)
        {
            // Start time should be largest and cannot be removed.
            double startTime = frame.Time;
            if (startTime <= minAvailableTime)
                throw new ArgumentOutOfRangeException(nameof(startTime));

            minAvailableTime = startTime;

            if (!frame.Sound)
            {
                // Next replay frame will create new path
                createNew = true;
                return;
            }

            if (createNew)
            {
                createNew = false;

                CreateNew(frame);
            }
            else
            {
                Append(frame);
            }
        }

        [BackgroundDependencyLoader]
        private void load(OsuColour colours)
        {
            Colour = colours.GrayF;
        }
    }
}
