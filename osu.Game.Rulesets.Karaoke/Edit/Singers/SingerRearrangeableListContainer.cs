// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osu.Game.Graphics.Containers;
using osu.Game.Rulesets.Karaoke.Beatmaps.Metadatas;
using osu.Game.Overlays;

namespace osu.Game.Rulesets.Karaoke.Edit.Singers
{
    public class SingerRearrangeableListContainer : OsuRearrangeableListContainer<Singer>
    {
        protected override OsuRearrangeableListItem<Singer> CreateOsuDrawable(Singer item)
            => new SingerRearrangeableListItem(item);

        public class SingerRearrangeableListItem : OsuRearrangeableListItem<Singer>
        {
            private Box background;

            public SingerRearrangeableListItem(Singer item)
                : base(item)
            {
            }

            protected override Drawable CreateContent()
            {
                return new Container
                {
                    Masking = true,
                    CornerRadius = 5,
                    AutoSizeAxes = Axes.Y,
                    RelativeSizeAxes = Axes.X,
                    Children = new Drawable[]
                    {
                        background = new Box
                        {
                            RelativeSizeAxes = Axes.Both,
                            Alpha = 0.3f
                        },
                    }
                };
            }

            [BackgroundDependencyLoader]
            private void load(OverlayColourProvider privider)
            {
                background.Colour = privider.Content1;
            }
        }
    }
}
