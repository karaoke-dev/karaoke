// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osu.Game.Graphics.Sprites;
using osu.Game.Overlays;
using osu.Game.Rulesets.Karaoke.Objects.Drawables;
using osu.Game.Skinning;

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
                    FillMode = FillMode.Fill,
                    Children = new Drawable[]
                    {
                        new LayoutPreviewArea
                        {
                            Name = "Preview size background",
                            RelativeSizeAxes = Axes.Both
                        },
                        new PreviewLyric
                        {
                            RelativeSizeAxes = Axes.Both,
                            Masking = true
                        },
                    }
                }
            };

            manager.PreviewPreviewRatio.BindValueChanged(e =>
            {
                // todo : adjust container's ratio
                var ratio = e.NewValue;
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
                    Child = new DrawableLyricLine(e.NewValue);
                }, true);
            }
        }

        internal class LayoutPreviewArea : CompositeDrawable
        {
            private OsuSpriteText widthRatioText;
            private OsuSpriteText heightRatioText;

            [BackgroundDependencyLoader]
            private void load(OverlayColourProvider colourProvider, LayoutManager manager)
            {
                InternalChildren = new Drawable[]
                {
                    new Box
                    {
                        Colour = colourProvider.Light1,
                    },
                    widthRatioText = new OsuSpriteText
                    {
                        Anchor = Anchor.BottomCentre,
                        Origin = Anchor.TopCentre
                    },
                    heightRatioText = new OsuSpriteText
                    {
                        Anchor = Anchor.CentreRight,
                        Origin = Anchor.CentreLeft,
                    }
                };

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
