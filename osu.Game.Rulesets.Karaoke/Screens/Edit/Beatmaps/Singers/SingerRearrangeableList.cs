// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Game.Graphics.Containers;
using osu.Game.Rulesets.Karaoke.Beatmaps.Metadatas.Types;
using osu.Game.Rulesets.Karaoke.Graphics.Containers;
using osu.Game.Rulesets.Karaoke.Screens.Edit.Beatmaps.Singers.Rows;
using osuTK;

namespace osu.Game.Rulesets.Karaoke.Screens.Edit.Beatmaps.Singers
{
    public class SingerRearrangeableList : OrderRearrangeableListContainer<ISinger>
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

        protected override OsuRearrangeableListItem<ISinger> CreateOsuDrawable(ISinger item)
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
