// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Graphics;
using osu.Framework.Timing;
using osu.Game.Rulesets.Karaoke.Objects;
using osu.Game.Rulesets.Karaoke.Objects.Drawables;
using osu.Game.Rulesets.Karaoke.Scoring;
using osuTK;

namespace osu.Game.Rulesets.Karaoke.Screens.Config.Previews.Gameplay
{
    public class LyricPreview : SettingsSubsectionPreview
    {
        public LyricPreview()
        {
            Size = new Vector2(0.7f, 0.5f);

            Child = new DrawableLyric(createPreviewLyric())
            {
                Anchor = Anchor.Centre,
                Origin = Anchor.Centre,
                Clock = new StopClock(0),
            };
        }

        private Lyric createPreviewLyric()
            => new Lyric
            {
                Text = "karaoke",
                HitWindows = new KaraokeHitWindows(),
            };

        private class StopClock : IFrameBasedClock
        {
            public StopClock(double targetTime)
            {
                CurrentTime = targetTime;
            }

            public double ElapsedFrameTime => 0;

            public double FramesPerSecond => 0;

            public FrameTimeInfo TimeInfo => new FrameTimeInfo { Current = CurrentTime, Elapsed = ElapsedFrameTime };

            public double CurrentTime { get; private set; }

            public double Rate => 0;

            public bool IsRunning => false;

            public void ProcessFrame()
            {
            }
        }
    }
}
