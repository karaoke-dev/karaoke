// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Game.Graphics.Containers;
using osu.Game.Rulesets.Karaoke.Beatmaps.Metadatas;
using osu.Game.Rulesets.Karaoke.Edit.Singers.Components;
using osu.Game.Rulesets.Karaoke.Graphics.Containers;

namespace osu.Game.Rulesets.Karaoke.Edit.Singers
{
    public class SingerRearrangeableList : OrderRearrangeableListContainer<Singer>
    {
        private const float spacing = 5;

        protected override OsuRearrangeableListItem<Singer> CreateOsuDrawable(Singer item)
            => new SingerRearrangeableListItem(item);

        public SingerRearrangeableList()
        {
            ScrollContainer.Padding = new MarginPadding { Bottom = 64 + spacing };
            ScrollContainer.Add(new Container
            {
                Masking = true,
                CornerRadius = 5,
                RelativeSizeAxes = Axes.X,
                Height = 64,
                Anchor = Anchor.y2,
                Origin = Anchor.y0,
                Padding = new MarginPadding { Top = spacing, Left = 22 },
                Children = new Drawable[]
                {
                    new CreateNewLyricPlacementColumn
                    {
                        RelativeSizeAxes = Axes.Both,
                    }
                }
            });
        }
    }
}
