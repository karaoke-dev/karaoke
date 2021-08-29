// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Game.Graphics.Containers;
using osu.Game.Rulesets.Karaoke.Beatmaps.Metadatas;
using osu.Game.Rulesets.Karaoke.Edit.Singers.Rows;
using osu.Game.Rulesets.Karaoke.Graphics.Containers;
using osuTK;

namespace osu.Game.Rulesets.Karaoke.Edit.Singers
{
    public class SingerRearrangeableList : OrderRearrangeableListContainer<Singer>
    {
        protected override Vector2 Spacing => new(0, 5);

        public SingerRearrangeableList()
        {
            Padding = new MarginPadding
            {
                Top = Spacing.Y,
                Bottom = Spacing.Y,
            };
        }

        protected override OsuRearrangeableListItem<Singer> CreateOsuDrawable(Singer item)
            => new SingerRearrangeableListItem(item);

        protected override Drawable CreateBottomDrawable()
        {
            return new Container
            {
                RelativeSizeAxes = Axes.X,
                Height = 64,
                Padding = new MarginPadding { Left = 22 },
                Child = new Container
                {
                    Masking = true,
                    CornerRadius = 5,
                    RelativeSizeAxes = Axes.Both,
                    Child = new CreateNewLyricPlacementColumn
                    {
                        RelativeSizeAxes = Axes.Both,
                    }
                }
            };
        }
    }
}
