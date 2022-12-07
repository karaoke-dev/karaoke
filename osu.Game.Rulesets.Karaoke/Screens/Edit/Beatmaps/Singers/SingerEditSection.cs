// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

#nullable disable

using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Game.Rulesets.Karaoke.Edit.ChangeHandlers.Beatmaps;
using osu.Game.Rulesets.Karaoke.Screens.Edit.Beatmaps.Singers.Rows;

namespace osu.Game.Rulesets.Karaoke.Screens.Edit.Beatmaps.Singers
{
    public partial class SingerEditSection : CompositeDrawable
    {
        private SingerRearrangeableList singerContainers;

        [BackgroundDependencyLoader]
        private void load(IBeatmapSingersChangeHandler beatmapSingersChangeHandler)
        {
            InternalChild = new GridContainer
            {
                RelativeSizeAxes = Axes.Both,
                RowDimensions = new[]
                {
                    new Dimension(GridSizeMode.Absolute, 100),
                    new Dimension()
                },
                Content = new[]
                {
                    new Drawable[]
                    {
                        new DefaultLyricPlacementColumn
                        {
                            Name = "Default",
                            RelativeSizeAxes = Axes.Both,
                        }
                    },
                    new Drawable[]
                    {
                        singerContainers = new SingerRearrangeableList
                        {
                            Name = "List of singer",
                            RelativeSizeAxes = Axes.Both,
                            DisplayBottomDrawable = true,
                        }
                    }
                }
            };

            singerContainers.Items.BindTo(beatmapSingersChangeHandler.Singers);
            singerContainers.OnOrderChanged += beatmapSingersChangeHandler.ChangeOrder;
        }
    }
}
