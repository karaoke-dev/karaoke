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
using osu.Game.Rulesets.Karaoke.Screens.Settings.Previews;
using osu.Game.Rulesets.Karaoke.Skinning.Fonts;
using osu.Game.Rulesets.UI;
using osu.Game.Screens;

namespace osu.Game.Rulesets.Karaoke.Screens.Settings
{
    public class KaraokeSettings : OsuScreen
    {
        [Cached]
        private KaraokeSettingsColourProvider colourProvider = new();

        [Cached]
        private Bindable<SettingsSection> selectedSection = new();

        [Cached]
        private Bindable<SettingsSubsection> selectedSubsection = new();

        private readonly KaraokeConfigWaveContainer waves;
        private readonly Box background;
        private readonly KaraokeSettingsPanel settingsPanel;
        private readonly Header header;
        private readonly Container previewArea;

        public KaraokeSettings()
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
                        Padding = new MarginPadding { Left = KaraokeSettingsPanel.WIDTH },
                    },
                    previewArea = new Container
                    {
                        RelativeSizeAxes = Axes.Both,
                        Padding = new MarginPadding { Top = Header.HEIGHT, Left = KaraokeSettingsPanel.WIDTH }
                    }
                }
            };

            // wait for a period until all children loaded.
            // todo : should have a better way to do this.
            Scheduler.AddDelayed(() =>
            {
                header.TabItems = settingsPanel.Sections;
                header.SelectedSection = selectedSection;
            }, 2000);

            selectedSection.BindValueChanged(e =>
            {
                var newSection = e.NewValue;
                background.Delay(200).Then().FadeColour(colourProvider.GetBackgroundColour(newSection), 500);

                settingsPanel.ScrollToSection(newSection);
            });

            selectedSubsection.BindValueChanged(e =>
            {
                var preview = e.NewValue is KaraokeSettingsSubsection settingsSubsection
                    ? settingsSubsection.CreatePreview()
                    : new DefaultPreview();

                previewArea.Child = new DelayedLoadWrapper(preview)
                {
                    RelativeSizeAxes = Axes.Both
                };
            }, true);
        }

        protected override IReadOnlyDependencyContainer CreateChildDependencies(IReadOnlyDependencyContainer parent)
        {
            var baseDependencies = new DependencyContainer(base.CreateChildDependencies(parent));

            return new OsuScreenDependencies(false, new DrawableRulesetDependencies(baseDependencies.GetRuleset(), baseDependencies));
        }

        [BackgroundDependencyLoader]
        private void load(GameHost host)
        {
            // todo : not really sure how to clean-up cached manager
            if (host.Dependencies.Get<FontManager>() != null)
                return;

            // because not possible to remove cache from host, so only inject once.
            var manager = new FontManager();
            AddInternal(manager);
            host.Dependencies.Cache(manager);
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
