// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Linq;
using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Testing;
using osu.Game.Graphics.Sprites;
using osu.Game.Overlays.Settings;

namespace osu.Game.Rulesets.Karaoke.Screens.Settings
{
    public abstract class KaraokeSettingsSection : SettingsSection
    {
        private const int margin = 20;

        protected KaraokeSettingsSection()
        {
            Margin = new MarginPadding { Bottom = margin };
        }

        [BackgroundDependencyLoader]
        private void load(KaraokeSettingsColourProvider colourProvider)
        {
            var colour = colourProvider.GetContentColour(this);

            // set header box and text to target color.
            var headerBox = FlowContent.ChildrenOfType<Box>().FirstOrDefault(x => x.Name == "separator");
            var title = FlowContent.ChildrenOfType<OsuSpriteText>().FirstOrDefault(x => x.Text == Header);
            if (headerBox == null || title == null)
                return;

            headerBox.Colour = colour;
            title.Colour = colour;
        }
    }
}
