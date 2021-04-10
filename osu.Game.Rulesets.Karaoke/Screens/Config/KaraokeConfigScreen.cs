// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Extensions.Color4Extensions;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Input.Events;
using osu.Framework.Platform;
using osu.Game.Graphics.Containers;
using osu.Game.Overlays.Settings;
using osu.Game.Rulesets.Karaoke.Extensions;
using osu.Game.Rulesets.Karaoke.Fonts;
using osu.Game.Screens;

namespace osu.Game.Rulesets.Karaoke.Screens.Config
{
    public class KaraokeConfigScreen : OsuScreen
    {
        [Cached]
        private ConfigColourProvider colourProvider = new ConfigColourProvider();

        [Cached]
        private Bindable<SettingsSection> selectedSection = new Bindable<SettingsSection>();

        [Cached]
        private Bindable<SettingsSubsection> selectedSubsection = new Bindable<SettingsSubsection>();

        private readonly KaraokeConfigWaveContainer waves;
        private readonly Box background;
        private readonly KaraokeSettingsPanel settingsPanel;
        private readonly Header header;
        private readonly Container previewArea;

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
                        Padding = new MarginPadding { Left = SettingsPanel.WIDTH },
                    },
                    previewArea = new Container
                    {
                        RelativeSizeAxes = Axes.Both,
                        Padding = new MarginPadding { Top = Header.HEIGHT, Left = SettingsPanel.WIDTH }
                    }
                }
            };

            selectedSection.BindValueChanged(e =>
            {
                var newSection = e.NewValue;
                background.Delay(200).Then().FadeColour(colourProvider.GetBackgroundColour(newSection), 500);

                // prevent trigger scroll by config section.
                if (settingsPanel.SectionsContainer.SelectedSection.Value != newSection)
                    settingsPanel.SectionsContainer.ScrollTo(newSection);
            });

            selectedSubsection.BindValueChanged(e =>
            {
                if (e.NewValue is KaraokeSettingsSubsection settingsSubsection)
                {
                    previewArea.Child = settingsSubsection.CreatePreview();
                }
            });
        }

        [BackgroundDependencyLoader]
        private void load(GameHost host)
        {
            // todo : not really sure how to clean-up cached manager
            if (host.Dependencies.Get<FontManager>() == null)
            {
                // because not possible to remove cache from host, so only inject once.
                var manager = new FontManager(host.Storage);
                AddInternal(manager);
                host.Dependencies.Cache(manager);
            }

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

        protected override bool OnScroll(ScrollEvent e)
        {
            // Prevent scroll event cause volume control appear.
            return true;
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
