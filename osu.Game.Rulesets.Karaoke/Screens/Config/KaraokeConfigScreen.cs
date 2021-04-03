// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Extensions.Color4Extensions;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Shapes;
using osu.Game.Graphics.Containers;
using osu.Game.Overlays.Settings;
using osu.Game.Screens;

namespace osu.Game.Rulesets.Karaoke.Screens.Config
{
    public class KaraokeConfigScreen : OsuScreen
    {
        [Cached]
        private ConfigColourProvider colourProvider = new ConfigColourProvider();

        [Cached]
        private Bindable<SettingsSection> selectedSection = new Bindable<SettingsSection>();

        private readonly KaraokeConfigWaveContainer waves;
        private readonly Box background;
        private readonly KaraokeSettingsOverlay settingsOverlay;
        private readonly Header header;

        public KaraokeConfigScreen()
        {
            var backgroundColour = Color4Extensions.FromHex(@"3e3a44");

            InternalChild = waves = new KaraokeConfigWaveContainer
            {
                RelativeSizeAxes = Axes.Both,
                Children = new Drawable[]
                {
                    background = new Box
                    {
                        RelativeSizeAxes = Axes.Both,
                        Colour = backgroundColour,
                    },
                    settingsOverlay = new KaraokeSettingsOverlay(),
                    header = new Header(),
                }
            };

            selectedSection.ValueChanged += term =>
            {
                if (settingsOverlay.SectionsContainer.SelectedSection.Value == term.NewValue)
                    return;

                // update scroll position
                settingsOverlay.SectionsContainer.ScrollTo(term.NewValue);
                // change background color
            };
        }

        [BackgroundDependencyLoader]
        private void load()
        {
            header.TabItems = settingsOverlay.SectionsContainer.Children;
            settingsOverlay.SectionsContainer.SelectedSection.ValueChanged += section =>
            {
                selectedSection.Value = section.NewValue;
            };
        }

        protected override void LoadComplete()
        {
            base.LoadComplete();
            waves.Show();
        }

        private class KaraokeConfigWaveContainer : WaveContainer
        {
            protected override bool StartHidden => true;

            public KaraokeConfigWaveContainer()
            {
                FirstWaveColour = Color4Extensions.FromHex(@"654d8c");
                SecondWaveColour = Color4Extensions.FromHex(@"554075");
                ThirdWaveColour = Color4Extensions.FromHex(@"44325e");
                FourthWaveColour = Color4Extensions.FromHex(@"392850");
            }
        }
    }
}
