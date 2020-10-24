// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osu.Game.Graphics.Containers;
using osu.Game.Overlays;
using osu.Game.Screens.Edit;
using osuTK;

namespace osu.Game.Rulesets.Karaoke.Edit.Style
{
    public class StyleScreen : EditorScreen
    {
        private const float section_scale = 0.75f;

        [Cached]
        protected readonly OverlayColourProvider ColourProvider;

        [Cached]
        protected readonly StyleManager StyleManager;

        public StyleScreen()
          : base(EditorScreenMode.SongSetup)
        {
            ColourProvider = new OverlayColourProvider(OverlayColourScheme.Pink);
            Content.Add(StyleManager = new StyleManager());
        }

        [BackgroundDependencyLoader]
        private void load()
        {
            Padding = new MarginPadding(50);
            Child = new GridContainer
            {
                RelativeSizeAxes = Axes.Both,
                ColumnDimensions = new[]
                {
                    new Dimension(GridSizeMode.Relative, 0.3f),
                    new Dimension(GridSizeMode.Distributed)
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
                                new SectionsContainer<StyleSection>
                                {
                                    FixedHeader = new StyleScreenHeader(),
                                    RelativeSizeAxes = Axes.Both,
                                    Scale = new Vector2(section_scale),
                                    Size = new Vector2(1 / section_scale),
                                    Children = new StyleSection[]
                                    {
                                        new ColorSection(),
                                        new FontSection(),
                                        new ShadowSection(),
                                    }
                                }
                            }
                        },
                        new StylePreview
                        {
                            Name = "Layout preview area",
                            Anchor = Anchor.Centre,
                            Origin = Anchor.Centre,
                            Size = new Vector2(0.95f),
                            RelativeSizeAxes = Axes.Both
                        },
                    }
                },
            };
        }

        internal class StyleScreenHeader : OverlayHeader
        {
            protected override OverlayTitle CreateTitle() => new LayoutScreenTitle();

            private class LayoutScreenTitle : OverlayTitle
            {
                public LayoutScreenTitle()
                {
                    Title = "style";
                    Description = "create style of your beatmap";
                    IconTexture = "Icons/Hexacons/social";
                }
            }
        }
    }
}
