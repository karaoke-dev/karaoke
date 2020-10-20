// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osu.Game.Graphics.Sprites;
using osu.Game.Overlays;
using osu.Game.Rulesets.Karaoke.Objects.Drawables;
using osuTK;

namespace osu.Game.Rulesets.Karaoke.Edit.Layout
{
    public class LayoutPreview : Container
    {
        private Container previewContainer;

        [BackgroundDependencyLoader]
        private void load(OverlayColourProvider colourProvider, LayoutManager manager)
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
                previewContainer = new Container
                {
                    Anchor = Anchor.Centre,
                    Origin = Anchor.Centre,
                    Size = new Vector2(0.8f),
                    FillMode = FillMode.Fit,
                    RelativeSizeAxes = Axes.Both,
                    Children = new Drawable[]
                    {
                        new LayoutPreviewArea
                        {
                            Name = "Preview size background",
                            RelativeSizeAxes = Axes.Both,
                            Child = new PreviewLyric
                            {
                                RelativeSizeAxes = Axes.Both,
                            },
                        },
                    }
                }
            };

            manager.PreviewPreviewRatio.BindValueChanged(e =>
            {
                // todo : adjust container's ratio
                var ratio = e.NewValue;
                if (ratio > 0)
                    previewContainer.FillAspectRatio = ratio;
            }, true);
        }

        internal class PreviewLyric : Container
        {
            [BackgroundDependencyLoader]
            private void load(LayoutManager manager)
            {
                manager.PreviewLyricLine.BindValueChanged(e =>
                {
                    if(e.NewValue != null)
                        Child = new DrawableLyricLine(e.NewValue);
                }, true);
            }
        }

        internal class LayoutPreviewArea : Container
        {
            private OsuSpriteText widthRatioText;
            private OsuSpriteText heightRatioText;

            private readonly Box background;
            private readonly Container content;

            protected override Container<Drawable> Content => content;

            public LayoutPreviewArea()
            {
                InternalChildren = new Drawable[]
                {
                    background = new Box
                    {
                        RelativeSizeAxes = Axes.Both,
                    },
                    content = new Container
                    {
                        RelativeSizeAxes = Axes.Both,
                        BorderThickness = 20,
                        Padding = new MarginPadding(20),
                        Masking = true
                    },
                    widthRatioText = new OsuSpriteText
                    {
                        Anchor = Anchor.BottomCentre,
                        Origin = Anchor.TopCentre,
                        Y = 10,
                        Text = "Width"
                    },
                    heightRatioText = new OsuSpriteText
                    {
                        Anchor = Anchor.CentreRight,
                        Origin = Anchor.CentreLeft,
                        X = 10,
                        Text = "Height"
                    }
                };
            }

            [BackgroundDependencyLoader]
            private void load(OverlayColourProvider colourProvider, LayoutManager manager)
            {
                background.Colour = colourProvider.Light1;
                content.BorderColour = colourProvider.Dark1;
                manager.PreviewPreviewRatio.BindValueChanged(v =>
                {
                    var newRation = v.NewValue;
                    // todo : apply ratio
                    widthRatioText.Text = newRation.ToString();
                    heightRatioText.Text = newRation.ToString();
                });
            }
        }
    }
}
