// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Game.Rulesets.Karaoke.Beatmaps.Metadatas;
using System.Linq;

namespace osu.Game.Rulesets.Karaoke.Edit.Singers
{
    public class SingerEditSection : CompositeDrawable
    {
        private SingerRearrangeableListContainer singerContainers;

        [BackgroundDependencyLoader]
        private void load(SingerManager singerManager)
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
                        new SingerContent(new Singer(-1){ Name = "Default" })
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

            singerManager.Singers.BindCollectionChanged((a, b) =>
            {
                var newSingers = singerManager.Singers.ToList();
                singerContainers.Items.Clear();
                singerContainers.Items.AddRange(newSingers);
            }, true);
        }
    }
}
