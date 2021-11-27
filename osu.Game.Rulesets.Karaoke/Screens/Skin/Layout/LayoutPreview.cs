// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Globalization;
using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osu.Game.Graphics;
using osu.Game.Graphics.Sprites;
using osu.Game.Overlays;
using osu.Game.Rulesets.Karaoke.Objects;
using osu.Game.Rulesets.Karaoke.Objects.Drawables;
using osuTK;

namespace osu.Game.Rulesets.Karaoke.Screens.Skin.Layout
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
                            Child = new LyricPreviewArea
                            {
                                RelativeSizeAxes = Axes.Both,
                            },
                        },
                    }
                }
            };

            manager.PreviewScreenRatio.BindValueChanged(e =>
            {
                // todo : adjust container's ratio
                var ratio = e.NewValue;
                if (ratio.IsValid())
                    previewContainer.FillAspectRatio = ratio.Width / ratio.Height;
            }, true);
        }

        internal class LyricPreviewArea : Container
        {
            [BackgroundDependencyLoader]
            private void load(LayoutManager manager)
            {
                manager.PreviewLyric.BindValueChanged(e =>
                {
                    if (e.NewValue != null)
                        Child = new PreviewDrawableLyric(e.NewValue);
                }, true);

                manager.PreviewSingers.BindValueChanged(v =>
                {
                    if (Child is PreviewDrawableLyric lyric)
                        lyric.HitObject.Singers = v.NewValue;
                }, true);

                manager.EditLayout.BindValueChanged(v =>
                {
                    if (Child is PreviewDrawableLyric lyric)
                        lyric.HitObject.LayoutIndex = v.NewValue.ID;
                }, true);
            }

            public class PreviewDrawableLyric : DrawableLyric
            {
                public PreviewDrawableLyric(Lyric hitObject)
                    : base(hitObject)
                {
                    // todo: if wants to let this shit display language, should make a new config for that.
                    // but that's not important for now.
                    // DisplayTranslateLanguage = new CultureInfo("en-US");
                }
            }
        }

        internal class LayoutPreviewArea : Container
        {
            private readonly OsuSpriteText widthRatioText;
            private readonly OsuSpriteText heightRatioText;

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
                        Font = OsuFont.GetFont(size: 24),
                        Text = "Width"
                    },
                    heightRatioText = new OsuSpriteText
                    {
                        Anchor = Anchor.CentreRight,
                        Origin = Anchor.CentreLeft,
                        X = 10,
                        Font = OsuFont.GetFont(size: 24),
                        Text = "Height"
                    }
                };
            }

            [BackgroundDependencyLoader]
            private void load(OverlayColourProvider colourProvider, LayoutManager manager)
            {
                background.Colour = colourProvider.Light1;
                content.BorderColour = colourProvider.Dark1;
                manager.PreviewScreenRatio.BindValueChanged(v =>
                {
                    var newRation = v.NewValue;
                    if (!newRation.IsValid())
                        return;

                    widthRatioText.Text = newRation.Width.ToString(CultureInfo.InvariantCulture);
                    heightRatioText.Text = newRation.Height.ToString(CultureInfo.InvariantCulture);
                }, true);
            }
        }
    }
}
