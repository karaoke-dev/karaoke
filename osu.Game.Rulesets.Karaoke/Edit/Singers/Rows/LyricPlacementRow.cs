// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using JetBrains.Annotations;
using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osu.Game.Overlays;
using osu.Game.Rulesets.Karaoke.Beatmaps.Metadatas;

namespace osu.Game.Rulesets.Karaoke.Edit.Singers.Rows
{
    public abstract class LyricPlacementColumn : CompositeDrawable
    {
        protected const int INFO_SIZE = 178;

        private readonly Singer singer;

        protected LyricPlacementColumn(Singer singer)
        {
            this.singer = singer;
        }

        [BackgroundDependencyLoader(true)]
        private void load(OverlayColourProvider colourProvider, [CanBeNull] KaraokeHitObjectComposer composer)
        {
            InternalChildren = new Drawable[]
            {
                new Box
                {
                    RelativeSizeAxes = Axes.Both,
                    Alpha = 0.3f,
                    Colour = colourProvider.Background1,
                },
                new GridContainer
                {
                    RelativeSizeAxes = Axes.Both,
                    ColumnDimensions = new[]
                    {
                        new Dimension(GridSizeMode.Absolute, SingerInfoSize),
                        new Dimension(GridSizeMode.Absolute, 5),
                        new Dimension(),
                    },
                    Content = new[]
                    {
                        new[]
                        {
                            CreateSingerInfo(singer).With(x => { x.RelativeSizeAxes = Axes.Both; }),
                            new Box
                            {
                                Name = "Separator",
                                RelativeSizeAxes = Axes.Both,
                                Colour = colourProvider.Dark1,
                            },
                            CreateTimeLinePart(singer).With(x => { x.RelativeSizeAxes = Axes.Both; }),
                        }
                    }
                }
            };
        }

        protected virtual float SingerInfoSize => INFO_SIZE;

        protected abstract Drawable CreateSingerInfo(Singer singer);

        protected abstract Drawable CreateTimeLinePart(Singer singer);
    }
}
