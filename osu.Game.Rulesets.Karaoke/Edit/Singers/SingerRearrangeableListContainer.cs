// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Cursor;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Input.Events;
using osu.Game.Graphics;
using osu.Game.Graphics.Containers;
using osu.Game.Rulesets.Karaoke.Beatmaps.Metadatas;
using osu.Game.Rulesets.Karaoke.Graphics.Cursor;
using osuTK.Graphics;

namespace osu.Game.Rulesets.Karaoke.Edit.Singers
{
    public class SingerRearrangeableListContainer : OsuRearrangeableListContainer<Singer>
    {
        protected override OsuRearrangeableListItem<Singer> CreateOsuDrawable(Singer item)
            => new SingerRearrangeableListItem(item);

        public class SingerRearrangeableListItem : OsuRearrangeableListItem<Singer>
        {
            private Box dragAlert;

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
                    RelativeSizeAxes = Axes.X,
                    Height = 120,
                    Margin = new MarginPadding { Top = 5 },
                    Children = new Drawable[]
                    {
                        dragAlert = new Box
                        {
                            RelativeSizeAxes = Axes.Both,
                            Alpha = 0
                        },
                        new RealSingerContent(Model)
                        {
                            RelativeSizeAxes = Axes.Both,
                        }
                    }
                };
            }

            [BackgroundDependencyLoader]
            private void load(OsuColour colours)
            {
                dragAlert.Colour = colours.YellowDarker;
            }

            protected override bool OnDragStart(DragStartEvent e)
            {
                if (!base.OnDragStart(e))
                    return false;

                dragAlert.Show();
                return true;
            }

            protected override void OnDragEnd(DragEndEvent e)
            {
                dragAlert.Hide();
                base.OnDragEnd(e);
            }

            public class RealSingerContent : SingerContent
            {
                public RealSingerContent(Singer singer)
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
                    private Singer singer;
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
    }
}
