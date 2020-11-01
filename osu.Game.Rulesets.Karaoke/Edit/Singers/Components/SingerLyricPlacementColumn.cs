// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Cursor;
using osu.Framework.Graphics.Shapes;
using osu.Game.Rulesets.Karaoke.Beatmaps.Metadatas;
using osu.Game.Rulesets.Karaoke.Graphics.Cursor;
using osuTK.Graphics;

namespace osu.Game.Rulesets.Karaoke.Edit.Singers.Components
{
    public class SingerLyricPlacementColumn : LyricPlacementColumn
    {
        public SingerLyricPlacementColumn(Singer singer)
            : base(singer)
        {
        }

        // todo : implement singer info here
        protected override float SingerInfoSize => base.SingerInfoSize - 22;

        protected override Drawable CreateSingerInfo(Singer singer)
        {
            return new DrawableSingerInfo(singer)
            {
                RelativeSizeAxes = Axes.Both,
            };
        }

        internal class DrawableSingerInfo : CompositeDrawable, IHasCustomTooltip
        {
            private readonly Singer singer;

            public DrawableSingerInfo(Singer singer)
            {
                this.singer = singer;
                InternalChildren = new Drawable[]
                {
                    new Box
                    {
                        Name = "Background",
                        RelativeSizeAxes = Axes.Both,
                        Colour = singer.Color ?? new Color4(),
                        Alpha = singer.Color != null ? 1 : 0
                    },
                    new FillFlowContainer
                    {
                        Name = "Infos",
                        RelativeSizeAxes = Axes.Both,
                    }
                };
            }

            public object TooltipContent => singer;

            public ITooltip GetCustomTooltip() => new SingerToolTip();
        }
    }
}
