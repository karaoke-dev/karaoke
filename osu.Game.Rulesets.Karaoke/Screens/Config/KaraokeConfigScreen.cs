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
        private readonly KaraokeSettingsPanel settingsPanel;
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
                    settingsPanel = new KaraokeSettingsPanel(),
                    header = new Header
                    {
                        Padding = new MarginPadding{ Left = SettingsPanel.WIDTH },
                    },
                }
            };

            selectedSection.BindValueChanged(e =>
            {
                var newSection = e.NewValue;
                background.Delay(200).Then().FadeColour(colourProvider.GetBackgroundColour(newSection), 500);

                // prevent trigger secoll by config section.
                if (settingsPanel.SectionsContainer.SelectedSection.Value != newSection)
                    settingsPanel.SectionsContainer.ScrollTo(newSection);
            });
        }

        [BackgroundDependencyLoader]
        private void load()
        {
            header.TabItems = settingsPanel.SectionsContainer.Children;
            settingsPanel.SectionsContainer.SelectedSection.ValueChanged += section =>
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
