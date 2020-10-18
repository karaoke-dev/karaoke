// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osu.Game.Graphics.Containers;
using osu.Game.Overlays;
using osu.Game.Screens.Edit;

namespace osu.Game.Rulesets.Karaoke.Edit.Layout
{
    public class LayoutScreen : EditorScreen
    {
        [Cached]
        protected readonly OverlayColourProvider ColourProvider;

        public LayoutScreen()
           : base(EditorScreenMode.SongSetup)
        {
            ColourProvider = new OverlayColourProvider(OverlayColourScheme.Pink);
        }

        [BackgroundDependencyLoader]
        private void load()
        {
            Padding = new MarginPadding(50);
            Child = new GridContainer
            {
                RelativeSizeAxes = Axes.Both,
                RowDimensions = new[]
                {
                    new Dimension(GridSizeMode.Absolute, 500),
                    new Dimension(GridSizeMode.Distributed)
                },
                Content = new []
                {
                    new []
                    {
                        new SectionsContainer<LayoutSection>
                        {
                            FixedHeader = new LayoutScreenHeader(),
                            RelativeSizeAxes = Axes.Both,
                            Children = new LayoutSection[]
                            {
                                new PositionSection(),
                                new IntervalSection(),
                                new RubyRomajiSection(),
                            }
                        }
                    },
                    new Drawable[]
                    {
                        new Box(),
                    }
                },
            };
        }

        internal class LayoutScreenHeader : OverlayHeader
        {
            protected override OverlayTitle CreateTitle() => new LayoutScreenTitle();

            private class LayoutScreenTitle : OverlayTitle
            {
                public LayoutScreenTitle()
                {
                    Title = "layout";
                    Description = "create layout of your beatmap";
                    IconTexture = "Icons/Hexacons/social";
                }
            }
        }
    }
}
