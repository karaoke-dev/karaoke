// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Allocation;
using osu.Framework.Extensions.Color4Extensions;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Shapes;
using osu.Game.Graphics;
using osu.Game.Overlays;
using osuTK.Graphics;

namespace osu.Game.Rulesets.Karaoke.UI.HUD
{
    public class PopupOverlay : WaveOverlayContainer
    {
        protected override bool BlockNonPositionalInput => false;

        protected override bool DimMainContent => false;

        public PopupOverlay()
        {
            AddInternal(new Box
            {
                Name = "Background",
                RelativeSizeAxes = Axes.Both,
                Colour = Color4.Black,
                Alpha = 0.4f,
                Depth = 1,
            });
        }

        [BackgroundDependencyLoader]
        private void load(OsuColour colours)
        {
            Waves.FirstWaveColour = Color4Extensions.FromHex(@"19b0e2").Opacity(50);
            Waves.SecondWaveColour = Color4Extensions.FromHex(@"2280a2").Opacity(50);
            Waves.ThirdWaveColour = Color4Extensions.FromHex(@"005774").Opacity(50);
            Waves.FourthWaveColour = Color4Extensions.FromHex(@"003a4e").Opacity(50);
        }
    }
}
