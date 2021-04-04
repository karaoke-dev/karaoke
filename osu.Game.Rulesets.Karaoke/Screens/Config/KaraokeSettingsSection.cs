// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Linq;
using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Game.Overlays.Settings;

namespace osu.Game.Rulesets.Karaoke.Screens.Config
{
    public abstract class KaraokeSettingsSection : SettingsSection
    {
        private const int margin = 20;

        public KaraokeSettingsSection()
        {
            Margin = new MarginPadding { Bottom = margin };
        }

        [BackgroundDependencyLoader]
        private void load(ConfigColourProvider colourProvider)
        {
            var colour = colourProvider.GetContentColour(this);

            // set header box and text to target color.
            var headerBox = InternalChildren.FirstOrDefault();
            var title = (InternalChildren.LastOrDefault() as Container).Children?.FirstOrDefault();
            headerBox.Colour = colour;
            title.Colour = colour;
        }
    }
}
