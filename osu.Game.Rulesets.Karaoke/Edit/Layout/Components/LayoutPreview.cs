// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osu.Game.Overlays;
using osu.Game.Rulesets.Karaoke.Graphics.UserInterfaceV2;
using osu.Game.Rulesets.Karaoke.Objects;
using osu.Game.Rulesets.Karaoke.Objects.Drawables;
using osu.Game.Skinning;
using osuTK;

namespace osu.Game.Rulesets.Karaoke.Edit.Layout.Components
{
    public class LayoutPreview : Container
    {
        private const float section_scale = 0.75f;

        [BackgroundDependencyLoader]
        private void load(OverlayColourProvider colourProvider)
        {
            Masking = true;
            CornerRadius = 15;
            FillMode = FillMode.Fit;
            FillAspectRatio = 1.25f;

            Children = new Drawable[]
            {
                new Box
                {
                    RelativeSizeAxes = Axes.Both,
                    Colour = colourProvider.Background1,
                },
                new GridContainer
                {
                    RelativeSizeAxes = Axes.Both,
                    RowDimensions = new []
                    {
                        new Dimension(GridSizeMode.Distributed),
                        new Dimension(GridSizeMode.Absolute, 100),
                    },
                    Content = new[]
                    {
                        new Drawable[]
                        {
                            new LayoutPreviewArea
                            {
                                Name = "Lyric preview area",
                            }
                        },
                        new Drawable[]
                        {
                            new GridContainer
                            {
                                Name = "Controls",
                                RelativeSizeAxes = Axes.Both,
                                Anchor = Anchor.Centre,
                                Origin = Anchor.Centre,
                                Scale = new Vector2(section_scale),
                                Size = new Vector2(1 / section_scale),
                                ColumnDimensions = new []
                                {
                                    new Dimension(GridSizeMode.Relative, 0.05f),
                                    new Dimension(GridSizeMode.Relative, 0.3f),
                                    new Dimension(GridSizeMode.Relative, 0.3f),
                                    new Dimension(GridSizeMode.Relative, 0.3f),
                                },
                                Content = new []
                                {
                                    new Drawable[]
                                    {
                                        null,
                                        new LabelledDropdown<PreviewRatop>
                                        {
                                            Label = "Ratio",
                                            Description = "Adjust to see different preview ratio."
                                        },
                                        new LabelledDropdown<PreviewSample>
                                        {
                                            Label = "Lyric",
                                            Description = "Select different lyric to check layout is valid."
                                        },
                                        new LabelledDropdown<int>
                                        {
                                            Label = "Style",
                                            Description = "Select different style to check layout is valid."
                                        },
                                    }
                                }
                            }
                        }
                    }
                }
            };
        }

        internal class LayoutPreviewArea : Container
        {
            private SkinProvidingContainer layoutArea;
            private DrawableLyricLine drawableLyricLine;

            [BackgroundDependencyLoader]
            private void load(OverlayColourProvider colourProvider)
            {
                Children = new Drawable[]
                {
                    new Box
                    {
                        Name = "Setting background",
                        RelativeSizeAxes = Axes.Both,
                        Colour = colourProvider.Light4
                    },
                    layoutArea = new SkinProvidingContainer(new LayoutEditorSkin())
                    {
                        RelativeSizeAxes = Axes.Both,
                    }
                };
            }

            public void InitialLyricLine(LyricLine lyricLine)
                => layoutArea.Child = drawableLyricLine = new DrawableLyricLine(lyricLine);
        }

        internal enum PreviewRatop
        {
            
        }

        internal enum PreviewSample
        {
            SampeSmall,

            SampleMedium,

            SampleLarge
        }
    }
}
