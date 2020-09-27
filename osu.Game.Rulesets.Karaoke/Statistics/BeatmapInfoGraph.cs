// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osu.Game.Beatmaps;
using osu.Game.Graphics;
using osu.Game.Graphics.Sprites;
using osu.Game.Rulesets.Karaoke.Beatmaps;
using System;

namespace osu.Game.Rulesets.Karaoke.Statistics
{
    public class BeatmapInfoGraph : CompositeDrawable
    {
        private readonly Box background;

        public BeatmapInfoGraph(IBeatmap beatmap)
        {
            if (!(beatmap is KaraokeBeatmap karaokeBeatmap))
                throw new Exception();

            // todo : apply some property here.
            InternalChild = new Container
            {
                Children = new Drawable[]
                {
                    background = new Box
                    {
                        Name = "Background",
                        Anchor = Anchor.Centre,
                        Origin = Anchor.Centre,
                        RelativeSizeAxes = Axes.Both,
                    },
                    new FillFlowContainer
                    {
                        RelativeSizeAxes = Axes.Both,
                        Children = new Drawable[]
                        {
                            new OsuSpriteText
                            {
                                Text = karaokeBeatmap?.Metadata?.Title,
                            },
                            new OsuSpriteText
                            {
                                Text = karaokeBeatmap?.Metadata?.Author?.Title,
                            },
                            new OsuSpriteText
                            {
                                // todo : mapper
                                Text = karaokeBeatmap.Metadata?.Author?.Title,
                            }

                            // todo : singers(hover should see full singer info)

                            // todo : tags
                        }
                    }
                }
            };
        }

        [BackgroundDependencyLoader]
        private void load(OsuColour colours)
        {
            background.Colour = colours.ContextMenuGray;
        }
    }
}
