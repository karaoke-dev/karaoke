// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Linq;
using osu.Framework.Caching;
using osu.Game.Rulesets.Karaoke.Replays;

namespace osu.Game.Rulesets.Karaoke.UI.Components
{
    public class RealTimeSaitenVisualization : SaitenVisualization
    {
        private readonly Cached addStateCache = new Cached();

        public RealTimeSaitenVisualization()
        {
            addStateCache.Validate();
        }

        public void AddAction(KaraokeSaitenAction action)
        {
            if (Time.Current <= MaxAvailableTime)
                return;

            Add(new KaraokeReplayFrame(Time.Current, action.Scale));

            // Trigger update last frame
            addStateCache.Invalidate();
        }

        public void Release()
        {
            if (Time.Current < MaxAvailableTime)
                return;

            Add(new KaraokeReplayFrame(Time.Current + 1));
        }

        protected override void Update()
        {
            // If addStateCache is invalid, means last path should be re-calculate
            if (!addStateCache.IsValid)
            {
                var updatePath = Paths.LastOrDefault();
                MarkAsInvalid(updatePath);
                addStateCache.Validate();
            }

            base.Update();
        }
    }
}
