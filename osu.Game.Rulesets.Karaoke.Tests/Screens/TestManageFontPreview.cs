// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Graphics;
using osu.Framework.Timing;
using osu.Game.Overlays;
using osu.Game.Rulesets.Karaoke.Screens.Config.Previews.Graphics;
using osu.Game.Tests.Visual;

namespace osu.Game.Rulesets.Karaoke.Tests.Screens
{
    public class TestManageFontPreview : OsuTestScene
    {
        private readonly NowPlayingOverlay np;

        public TestManageFontPreview()
        {
            Clock = new FramedClock();
            Clock.ProcessFrame();

            AddRange(new Drawable[]
            {
                new ManageFontPreview
                {
                    Anchor = Anchor.Centre,
                    Origin = Anchor.Centre,
                },
                np = new NowPlayingOverlay
                {
                    Origin = Anchor.TopRight,
                    Anchor = Anchor.TopRight,
                }
            });
        }

        protected override void LoadComplete()
        {
            base.LoadComplete();
            np.ToggleVisibility();
        }
    }
}
