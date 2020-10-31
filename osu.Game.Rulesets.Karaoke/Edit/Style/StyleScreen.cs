// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osu.Game.Graphics.Containers;
using osu.Game.Graphics.UserInterface;
using osu.Game.Overlays;
using osu.Game.Screens.Edit;
using osuTK;

namespace osu.Game.Rulesets.Karaoke.Edit.Style
{
    public class StyleScreen : EditorScreen
    {
        [Cached]
        protected readonly OverlayColourProvider ColourProvider;

        [Cached]
        protected readonly StyleManager StyleManager;

        private StyleSectionsContainer styleSections;
        private Container previewContainer;

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
                                styleSections = new StyleSectionsContainer
                                {
                                    RelativeSizeAxes = Axes.Both,
                                }
                            }
                        },
                        previewContainer = new Container
                        {
                            RelativeSizeAxes = Axes.Both,
                        }
                    }
                },
            };

            styleSections.BindableStyle.BindValueChanged(e =>
            {
                switch (e.NewValue)
                {
                    case Style.Lyric:
                        previewContainer.Child = new LyricStylePreview
                        {
                            Name = "Lyric style preview area",
                            Anchor = Anchor.Centre,
                            Origin = Anchor.Centre,
                            Size = new Vector2(0.95f),
                            RelativeSizeAxes = Axes.Both
                        };
                        break;

                    case Style.Note:
                        previewContainer.Child = new NoteStylePreview
                        {
                            Name = "Note style preview area",
                            Anchor = Anchor.Centre,
                            Origin = Anchor.Centre,
                            Size = new Vector2(0.95f),
                            RelativeSizeAxes = Axes.Both
                        };
                        break;
                };
            }, true);
        }

        internal class StyleSectionsContainer : SectionsContainer<StyleSection>
        {
            private readonly StyleScreenHeader header;

            private const float section_scale = 0.75f;

            public IBindable<Style> BindableStyle => header.BindableStyle;

            public StyleSectionsContainer()
            {
                FixedHeader = header = new StyleScreenHeader();

                Scale = new Vector2(section_scale);
                Size = new Vector2(1 / section_scale);

                header.BindableStyle.BindValueChanged(e =>
                {
                    switch (e.NewValue)
                    {
                        case Style.Lyric:
                            Children = new StyleSection[]
                            {
                                    new LyricColorSection(),
                                    new LyricFontSection(),
                                    new LyricShadowSection(),
                            };
                            break;
                        case Style.Note:
                            Children = new StyleSection[]
                            {
                                    new NoteColorSection(),
                                    new NoteFontSection(),
                            };
                            break;
                    }
                }, true);
            }

            internal class StyleScreenHeader : OverlayHeader
            {
                public Bindable<Style> BindableStyle = new Bindable<Style>();

                protected override OverlayTitle CreateTitle() => new LayoutScreenTitle();

                protected override Drawable CreateTitleContent()
                {
                    return new Container
                    {
                        RelativeSizeAxes = Axes.X,
                        Padding = new MarginPadding { Left = 100 },
                        Height = 40,
                        Child = new OsuEnumDropdown<Style>
                        {
                            Anchor = Anchor.TopRight,
                            Origin = Anchor.TopRight,
                            RelativeSizeAxes = Axes.X,
                            Current = BindableStyle,
                        }
                    };
                }

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

    public enum Style
    {
        [System.ComponentModel.Description("Lyric")]
        Lyric,

        [System.ComponentModel.Description("Note")]
        Note
    }
}
