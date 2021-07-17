// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Collections.Generic;
using System.Linq;
using osu.Framework.Allocation;
using osu.Framework.Caching;
using osu.Game.Graphics;
using osu.Game.Rulesets.Karaoke.UI.Position;

namespace osu.Game.Rulesets.Karaoke.UI.Components
{
    public class RealTimeSaitenVisualization : VoiceVisualization<KeyValuePair<double, KaraokeSaitenAction>>
    {
        private readonly Cached addStateCache = new Cached();

        protected override float PathRadius => 2.5f;

        protected override float Offset => DrawSize.X;

        [Resolved]
        private INotePositionInfo notePositionInfo { get; set; }

        public RealTimeSaitenVisualization()
        {
            Masking = true;
        }

        protected override double GetTime(KeyValuePair<double, KaraokeSaitenAction> action) => action.Key;

        protected override float GetPosition(KeyValuePair<double, KaraokeSaitenAction> action) => notePositionInfo.Calculator.YPositionAt(action.Value);

        private bool createNew = true;

        private double minAvailableTime;

        public void AddAction(KaraokeSaitenAction action)
        {
            if (Time.Current <= minAvailableTime)
                return;

            minAvailableTime = Time.Current;

            if (createNew)
            {
                createNew = false;

                CreateNew(new KeyValuePair<double, KaraokeSaitenAction>(Time.Current, action));
            }
            else
            {
                Append(new KeyValuePair<double, KaraokeSaitenAction>(Time.Current, action));
            }

            // Trigger update last frame
            addStateCache.Invalidate();
        }

        public void Release()
        {
            if (Time.Current < minAvailableTime)
                return;

            minAvailableTime = Time.Current;

            createNew = true;
        }

        protected override void Update()
        {
            // If addStateCache is invalid, means last path should be re-calculate
            if (!addStateCache.IsValid && Paths.Any())
            {
                var updatePath = Paths.LastOrDefault();
                MarkAsInvalid(updatePath);
                addStateCache.Validate();
            }

            base.Update();
        }

        [BackgroundDependencyLoader]
        private void load(OsuColour colours)
        {
            Colour = colours.Yellow;
        }
    }
}
