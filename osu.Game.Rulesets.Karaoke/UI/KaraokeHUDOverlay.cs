// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;

namespace osu.Game.Rulesets.Karaoke.UI
{
    public class KaraokeHUDOverlay : Container
    {
        public readonly ControlLayer controlLayer;
        public KaraokeHUDOverlay()
        {
            RelativeSizeAxes = Axes.Both;

            Children = new Drawable[]
            {
                controlLayer = CreateControlLayer(),
            };
        }

        protected virtual ControlLayer CreateControlLayer() => new ControlLayer();
    }
}
