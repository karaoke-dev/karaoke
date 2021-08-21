// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Collections.Generic;
using System.Linq;
using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osu.Game.Graphics.Containers;
using osu.Game.Graphics.UserInterface;
using osu.Game.Overlays;
using osu.Game.Overlays.Settings;
using osu.Game.Rulesets.Karaoke.Screens.Config.Sections;

namespace osu.Game.Rulesets.Karaoke.Screens.Config
{
    public class KaraokeSettingsPanel : SettingsPanel
    {
        public new const float WIDTH = 300;

        public new SettingsSectionsContainer SectionsContainer => base.SectionsContainer;

        protected override IEnumerable<SettingsSection> CreateSections() => new SettingsSection[]
        {
            new ConfigSection(),
            new StyleSection(),
            new ScoringSection()
        };

        //protected override SettingsSectionsContainer CreateSettingsSections() => new KaraokeSettingsSectionsContainer();

        protected override Drawable CreateFooter() => new Container
        {
            Height = 130,
        };

        public KaraokeSettingsPanel()
            : base(false)
        {
        }

        // prevent click outside to hide the overlay
        protected override bool BlockPositionalInput => false;

        // prevent let main content darker.
        protected override bool DimMainContent => false;

        // prevent hide the overlay.
        public override void Hide() { }

        [BackgroundDependencyLoader]
        private void load(ConfigColourProvider colourProvider, Bindable<SettingsSection> selectedSection, Bindable<SettingsSubsection> selectedSubsection)
        {
            ContentContainer.Width = WIDTH;

            selectedSection.BindValueChanged(x =>
            {
                var background = ContentContainer.Children.OfType<Box>().FirstOrDefault();
                if (background == null)
                    return;

                var colour = colourProvider.GetBackground3Colour(x.NewValue);
                background.Delay(200).Then().FadeColour(colour, 500);
            });

            if (SectionsContainer.FixedHeader is SeekLimitedSearchTextBox searchTextBox)
            {
                searchTextBox.Current.ValueChanged += term =>
                {
                    // should clear selected sub-section if change search text.
                    selectedSubsection.Value = null;
                };
            }

            Show();
        }

        public class KaraokeSettingsSectionsContainer : SettingsSectionsContainer
        {
            private UserTrackingScrollContainer scrollContainer;
            private Box background;

            protected override UserTrackingScrollContainer CreateScrollContainer()
                => scrollContainer = base.CreateScrollContainer();

            [BackgroundDependencyLoader]
            private void load(ConfigColourProvider colourProvider, Bindable<SettingsSection> selectedSection, Bindable<SettingsSubsection> selectedSubsection)
            {
                // create hove background.
                scrollContainer.Add(background = new Box
                {
                    RelativeSizeAxes = Axes.X,
                    Depth = 1,
                });

                // change background color if section changed.
                selectedSection.BindValueChanged(x =>
                {
                    var colour = colourProvider.GetBackgroundColour(x.NewValue);
                    background.Delay(200).Then().FadeColour(colour, 500);
                });

                // move background to target sub-section if user hover to it.
                selectedSubsection.BindValueChanged(x =>
                {
                    var alpha = x.NewValue != null ? 0.6f : 0f;
                    background.FadeTo(alpha, 200);

                    if (x.NewValue == null)
                        return;

                    const int offset = 20;
                    var position = scrollContainer.GetChildPosInContent(x.NewValue);
                    background.MoveToY(position + offset, 50);
                    background.ResizeHeightTo(x.NewValue.DrawHeight, 100);
                });
            }

            public new void ScrollTo(Drawable section)
            {
                base.ScrollTo(section);

                // re-scroll to target place, not with weird spacing.
                scrollContainer.ScrollTo(scrollContainer.GetChildPosInContent(section) - (FixedHeader?.BoundingBox.Height ?? 0));
            }
        }
    }
}
