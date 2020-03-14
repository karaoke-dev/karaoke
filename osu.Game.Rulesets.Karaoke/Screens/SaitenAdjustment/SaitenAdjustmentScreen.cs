// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Game.Rulesets.Karaoke.Screens.SaitenAdjustment.UI;
using osu.Game.Screens;

namespace osu.Game.Rulesets.Karaoke.Screens.SaitenAdjustment
{
    public class SaitenAdjustmentScreen : OsuScreen
    {
        public SaitenAdjustmentScreen()
        {
            AddInternal(new Container
            {
                RelativeSizeAxes = Axes.Both,
                Children = new[]
                {
                    new SaitenAdjustmantmentVisualization()
                }
            });
        }
    }
}
