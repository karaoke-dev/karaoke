// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System.ComponentModel;
using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osu.Game.Graphics.Containers;
using osu.Game.Graphics.UserInterface;
using osu.Game.Overlays;
using osuTK;
using Container = osu.Framework.Graphics.Containers.Container;

namespace osu.Game.Rulesets.Karaoke.Screens.Skin.Style
{
    public class StyleScreen : KaraokeSkinEditorScreen
    {
        [Cached]
        protected readonly OverlayColourProvider ColourProvider;

        [Cached]
        protected readonly StyleManager StyleManager;

        private StyleSectionsContainer styleSections;
        private Container previewContainer;

        public StyleScreen()
            : base(KaraokeSkinEditorScreenMode.Style)
        {
            ColourProvider = new OverlayColourProvider(OverlayColourScheme.Pink);
            AddInternal(StyleManager = new StyleManager());
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
                }
            });

            styleSections.BindableStyle.BindValueChanged(e =>
            {
                previewContainer.Child = e.NewValue switch
                {
                    Style.Lyric => new LyricStylePreview
                    {
                        Name = "Lyric style preview area",
                        Anchor = Anchor.Centre,
                        Origin = Anchor.Centre,
                        Size = new Vector2(0.95f),
                        RelativeSizeAxes = Axes.Both
                    },
                    Style.Note => new NoteStylePreview
                    {
                        Name = "Note style preview area",
                        Anchor = Anchor.Centre,
                        Origin = Anchor.Centre,
                        Size = new Vector2(0.95f),
                        RelativeSizeAxes = Axes.Both
                    },
                    _ => previewContainer.Child
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
                    Children = e.NewValue switch
                    {
                        Style.Lyric => new StyleSection[]
                        {
                            new LyricColorSection(),
                            new LyricFontSection(),
                            new LyricShadowSection(),
                        },
                        Style.Note => new StyleSection[]
                        {
                            new NoteColorSection(),
                            new NoteFontSection(),
                        },
                        _ => Children
                    };
                }, true);
            }

            internal class StyleScreenHeader : OverlayHeader
            {
                public Bindable<Style> BindableStyle = new();

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
        [Description("Lyric")]
        Lyric,

        [Description("Note")]
        Note
    }
}
