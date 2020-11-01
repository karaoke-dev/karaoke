// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osu.Game.Overlays;
using osu.Game.Rulesets.Edit;
using osu.Game.Rulesets.Karaoke.Beatmaps.Metadatas;
using osu.Game.Screens.Edit.Components.Timelines.Summary.Parts;
using osu.Game.Screens.Edit.Compose.Components;
using osu.Game.Screens.Edit.Compose.Components.Timeline;

namespace osu.Game.Rulesets.Karaoke.Edit.Singers.Components
{
    public abstract class LyricPlacementColumn : CompositeDrawable
    {
        private Box background;
        private readonly Singer singer;

        protected LyricPlacementColumn(Singer singer)
        {
            this.singer = singer;
        }

        [BackgroundDependencyLoader]
        private void load(OverlayColourProvider colourProvider)
        {
            InternalChildren = new Drawable[]
            {
                background = new Box
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
                            CreateSingerInfo(singer).With(x =>
                            {
                                x.RelativeSizeAxes = Axes.Both;
                            }),
                            new Box
                            {
                                RelativeSizeAxes = Axes.Both,
                                Colour = colourProvider.Dark1,
                            },
                            new SingerLyricBlueprintContainer
                            {
                                RelativeSizeAxes = Axes.Both,
                            }
                        }
                    }
                }
            };
        }

        protected abstract float SingerInfoSize { get; }

        protected abstract Drawable CreateSingerInfo(Singer singer) ;

        protected class SingerLyricBlueprintContainer : BlueprintContainer
        {
            [Resolved(CanBeNull = true)]
            private Timeline timeline { get; set; }

            protected class TimelineSelectionBlueprintContainer : Container<SelectionBlueprint>
            {
                protected override Container<SelectionBlueprint> Content { get; }

                public TimelineSelectionBlueprintContainer()
                {
                    AddInternal(new SingerLyricPart<SelectionBlueprint>(Content = new Container<SelectionBlueprint> { RelativeSizeAxes = Axes.Both }) { RelativeSizeAxes = Axes.Both });
                }
            }

            protected class SingerLyricPart<T> : TimelinePart<T> where T : Drawable
            {
                public SingerLyricPart(Container<T> content = null)
                    : base(content)
                {
                }
            }
        }
    }
}
