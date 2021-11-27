// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Collections.Generic;
using System.Linq;
using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Input.Events;
using osu.Game.Graphics.Containers;
using osu.Game.Graphics.UserInterface;
using osu.Game.Input.Bindings;
using osu.Game.Overlays;
using osu.Game.Overlays.Settings;
using osu.Game.Rulesets.Karaoke.Extensions;
using osu.Game.Rulesets.Karaoke.Screens.Settings.Sections;

namespace osu.Game.Rulesets.Karaoke.Screens.Settings
{
    public class KaraokeSettingsPanel : SettingsPanel
    {
        public new const float WIDTH = 300;

        private Box hoverBackground;

        protected override IEnumerable<SettingsSection> CreateSections() => new SettingsSection[]
        {
            new ConfigSection(),
            new StyleSection(),
            new ScoringSection()
        };

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

        // prevent handle back key event every time, should call onPressed() only once.
        protected override bool BlockNonPositionalInput => false;

        // on press should return false to prevent handle the back key action.
        public override bool OnPressed(KeyBindingPressEvent<GlobalAction> e)
            => false;

        // prevent let main content darker.
        protected override bool DimMainContent => false;

        // prevent hide the overlay.
        public override void Hide() { }

        public void ScrollToSection(SettingsSection settingsSection)
        {
            // prevent trigger scroll by config section.
            if (SectionsContainer.SelectedSection.Value == settingsSection)
                return;

            // instead of base scroll to method, using customized method to prevent weird spacing.
            // SectionsContainer.ScrollTo(settingsSection);
            var scrollContainer = SectionsContainer.GetInternalChildren()?.OfType<UserTrackingScrollContainer>().FirstOrDefault();
            scrollContainer?.ScrollTo(scrollContainer.GetChildPosInContent(settingsSection) - (SectionsContainer.FixedHeader?.BoundingBox.Height ?? 0));
        }

        public IReadOnlyList<SettingsSection> Sections => SectionsContainer.Children;

        [BackgroundDependencyLoader]
        private void load(KaraokeSettingsColourProvider colourProvider, Bindable<SettingsSection> selectedSection, Bindable<SettingsSubsection> selectedSubsection)
        {
            initialSelectionContainer();
            initialContentContainer();
            initialSearchTextBox();
            initialBackground();

            Show();

            void initialSelectionContainer() =>
                SectionsContainer.SelectedSection.ValueChanged += section =>
                {
                    selectedSection.Value = section.NewValue;
                };

            void initialContentContainer()
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
            }

            void initialSearchTextBox()
            {
                if (SectionsContainer.FixedHeader is SeekLimitedSearchTextBox searchTextBox)
                {
                    searchTextBox.Current.ValueChanged += term =>
                    {
                        // should clear selected sub-section if change search text.
                        selectedSubsection.Value = null;
                    };
                }
            }

            void initialBackground()
            {
                var scrollContainer = SectionsContainer.GetInternalChildren()?.OfType<UserTrackingScrollContainer>().FirstOrDefault();
                if (scrollContainer == null)
                    return;

                // create hove background.
                scrollContainer.Add(hoverBackground = new Box
                {
                    RelativeSizeAxes = Axes.X,
                    Depth = 1,
                });

                // change background color if section changed.
                selectedSection.BindValueChanged(x =>
                {
                    var colour = colourProvider.GetBackgroundColour(x.NewValue);
                    hoverBackground.Delay(200).Then().FadeColour(colour, 500);
                });

                // move background to target sub-section if user hover to it.
                selectedSubsection.BindValueChanged(x =>
                {
                    var alpha = x.NewValue != null ? 0.6f : 0f;
                    hoverBackground.FadeTo(alpha, 200);

                    if (x.NewValue == null)
                        return;

                    const int offset = 8;
                    var position = scrollContainer.GetChildPosInContent(x.NewValue);
                    hoverBackground.MoveToY(position + offset, 50);
                    hoverBackground.ResizeHeightTo(x.NewValue.DrawHeight, 100);
                });
            }
        }
    }
}
