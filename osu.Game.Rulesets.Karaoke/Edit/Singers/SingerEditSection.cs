// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

#nullable disable

using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Game.Rulesets.Karaoke.Edit.ChangeHandlers.Singers;
using osu.Game.Rulesets.Karaoke.Edit.Singers.Rows;

namespace osu.Game.Rulesets.Karaoke.Edit.Singers
{
    public class SingerEditSection : CompositeDrawable
    {
        private SingerRearrangeableList singerContainers;

        [BackgroundDependencyLoader]
        private void load(ISingersChangeHandler singersChangeHandler)
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

            singerContainers.Items.BindTo(singersChangeHandler.Singers);
            singerContainers.OnOrderChanged += singersChangeHandler.ChangeOrder;
        }
    }
}
