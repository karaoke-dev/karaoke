// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Extensions.Color4Extensions;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osu.Game.Beatmaps;
using osu.Game.Graphics;
using osu.Game.Graphics.Containers;
using osu.Game.Graphics.Sprites;
using osuTK;
using osuTK.Graphics;

namespace osu.Game.Rulesets.Karaoke.Statistics
{
    public class BeatmapMetadataGraph : Container
    {
        private const float spacing = 10;
        private const float transition_duration = 250;

        public BeatmapMetadataGraph(IBeatmap beatmap)
        {
            Masking = true;
            CornerRadius = 5;

            var beatmapInfo = beatmap.BeatmapInfo;
            InternalChildren = new Drawable[]
            {
                new Box
                {
                    Name = "Background",
                    RelativeSizeAxes = Axes.Both,
                    Colour = Color4.Black.Opacity(0.5f),
                },
                new OsuScrollContainer
                {
                    RelativeSizeAxes = Axes.Both,
                    ScrollbarVisible = false,
                    Padding = new MarginPadding { Left = spacing / 2, Top = spacing / 2 },
                    Child = new FillFlowContainer
                    {
                        RelativeSizeAxes = Axes.X,
                        AutoSizeAxes = Axes.Y,
                        LayoutDuration = transition_duration,
                        LayoutEasing = Easing.OutQuad,
                        Spacing = new Vector2(spacing),
                        Children = new[]
                        {
                            new MetadataSection("Description")
                            {
                                Text = beatmapInfo?.Version
                            },
                            new MetadataSection("Source")
                            {
                                Text = beatmapInfo?.Metadata?.Source
                            },
                            new MetadataSection("Tags")
                            {
                                Text = beatmapInfo?.Metadata?.Tags
                            },
                        },
                    },
                },
            };
        }

        private class MetadataSection : Container
        {
            private readonly FillFlowContainer textContainer;
            private TextFlowContainer textFlow;

            public MetadataSection(string title)
            {
                RelativeSizeAxes = Axes.X;
                AutoSizeAxes = Axes.Y;

                InternalChild = textContainer = new FillFlowContainer
                {
                    RelativeSizeAxes = Axes.X,
                    AutoSizeAxes = Axes.Y,
                    Spacing = new Vector2(spacing / 2),
                    Children = new Drawable[]
                    {
                        new Container
                        {
                            RelativeSizeAxes = Axes.X,
                            AutoSizeAxes = Axes.Y,
                            Child = new OsuSpriteText
                            {
                                Text = title,
                                Font = OsuFont.GetFont(weight: FontWeight.Bold, size: 14),
                            },
                        },
                    },
                };
            }

            public string Text
            {
                set
                {
                    if (string.IsNullOrEmpty(value))
                    {
                        this.FadeOut(transition_duration);
                        return;
                    }

                    this.FadeIn(transition_duration);

                    setTextAsync(value);
                }
            }

            private void setTextAsync(string text)
            {
                textFlow?.Expire();
                textContainer.Add(textFlow = new OsuTextFlowContainer(s => s.Font = s.Font.With(size: 14))
                {
                    RelativeSizeAxes = Axes.X,
                    AutoSizeAxes = Axes.Y,
                    Colour = Color4.White.Opacity(0.75f),
                    Text = text
                });
            }
        }
    }
}
