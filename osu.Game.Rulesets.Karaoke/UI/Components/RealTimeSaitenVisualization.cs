// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Game.Rulesets.Karaoke.Replays;

namespace osu.Game.Rulesets.Karaoke.UI.Components
{
    public class RealTimeSaitenVisualization : SaitenVisualization
    {
        public RealTimeSaitenVisualization(NotePlayfield playfield)
            : base(playfield)
        {
        }

        public void AddAction(KaraokeSaitenAction action)
        {
            if (Time.Current <= MaxAvailableTime)
                return;

            Add(new KaraokeReplayFrame
            {
                Time = Time.Current,
                Scale = action.Scale,
                Sound = true
            });
        }

        public void Release()
        {
            if (Time.Current < MaxAvailableTime)
                return;

            Add(new KaraokeReplayFrame
            {
                Time = Time.Current + 1,
                Sound = false
            });
        }
    }
}
