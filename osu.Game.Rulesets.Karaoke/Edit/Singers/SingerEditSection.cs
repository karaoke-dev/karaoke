// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Game.Rulesets.Karaoke.Beatmaps.Metadatas;

namespace osu.Game.Rulesets.Karaoke.Edit.Singers
{
    public class SingerEditSection : CompositeDrawable
    {
        private SingerRearrangeableListContainer singerContainers;

        [BackgroundDependencyLoader]
        private void load()
        {
            InternalChild = new GridContainer
            {
                RelativeSizeAxes = Axes.Both,
                RowDimensions = new[]
                {
                    new Dimension(GridSizeMode.Absolute, 100),
                    new Dimension()
                },
                Content = new Drawable[][]
                {
                    new Drawable[]
                    {
                        new Container
                        {
                            Name = "Default",
                            RelativeSizeAxes = Axes.Both,
                        }
                    },
                    new Drawable[]
                    {
                        singerContainers = new SingerRearrangeableListContainer
                        {
                            Name = "List of singer",
                            RelativeSizeAxes = Axes.Both,
                        }
                    }
                }
            };

            singerContainers.Items.Add(new Singer(0));
            singerContainers.Items.Add(new Singer(1));
            singerContainers.Items.Add(new Singer(2));
        }
    }
}
