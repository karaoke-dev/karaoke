// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osu.Game.Graphics.Containers;
using osu.Game.Overlays;
using osu.Game.Rulesets.Karaoke.Edit.Components;
using osuTK;

namespace osu.Game.Rulesets.Karaoke.Edit.Layout
{
    public class LayoutScreen : EditorSubScreen
    {
        private const float section_scale = 0.75f;

        [Cached]
        protected readonly OverlayColourProvider ColourProvider;

        [Cached]
        protected readonly LayoutManager LayoutManager;

        public LayoutScreen()
        {
            ColourProvider = new OverlayColourProvider(OverlayColourScheme.Green);
            AddInternal(LayoutManager = new LayoutManager());
        }

        [BackgroundDependencyLoader]
        private void load()
        {
            AddInternal(new Container
            {
                RelativeSizeAxes = Axes.Both,
                Padding = new MarginPadding(50),
                Child = new GridContainer
                {
                    RelativeSizeAxes = Axes.Both,
                    ColumnDimensions = new[]
                    {
                        new Dimension(GridSizeMode.Relative, 0.3f),
                        new Dimension()
                    },
                    Content = new[]
                    {
                        new Drawable[]
                        {
                            new Container
                            {
                                Name = "Layout adjustment area",
                                RelativeSizeAxes = Axes.Both,
                                Masking = true,
                                CornerRadius = 10,
                                Children = new Drawable[]
                                {
                                    new Box
                                    {
                                        Colour = ColourProvider.Background2,
                                        RelativeSizeAxes = Axes.Both,
                                    },
                                    new SectionsContainer<LayoutSection>
                                    {
                                        FixedHeader = new LayoutScreenHeader(),
                                        RelativeSizeAxes = Axes.Both,
                                        Scale = new Vector2(section_scale),
                                        Size = new Vector2(1 / section_scale),
                                        Children = new LayoutSection[]
                                        {
                                            new LayoutAlignmentSection(),
                                            new PreviewSection(),
                                        }
                                    }
                                }
                            },
                            new LayoutPreview
                            {
                                Name = "Layout preview area",
                                Anchor = Anchor.Centre,
                                Origin = Anchor.Centre,
                                Size = new Vector2(0.95f),
                                RelativeSizeAxes = Axes.Both
                            },
                        }
                    },
                }
            });
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
