// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Cursor;
using osu.Framework.Graphics.Shapes;
using osu.Game.Graphics;
using osu.Game.Graphics.Sprites;
using osu.Game.Rulesets.Karaoke.Beatmaps.Metadatas;
using osu.Game.Rulesets.Karaoke.Graphics.Cursor;
using osu.Game.Rulesets.Karaoke.Graphics.Sprites;
using osuTK;
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
        protected override float SingerInfoSize => 178;

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
                        Alpha = singer.Color != null ? 0.3f : 0
                    },
                    new GridContainer
                    {
                        Name = "Infos",
                        RelativeSizeAxes = Axes.Both,
                        Margin = new MarginPadding(10),
                        ColumnDimensions = new[]
                        {
                            new Dimension(GridSizeMode.AutoSize, 48),
                            new Dimension(),
                        },
                        Content = new[]
                        {
                            new Drawable[]
                            {
                                new DrawableSingerAvatar
                                {
                                    Name = "Avatar",
                                    Size = new Vector2(48)
                                },
                                new FillFlowContainer
                                {
                                    Name = "Singer name",
                                    RelativeSizeAxes = Axes.X,
                                    Direction = FillDirection.Vertical,
                                    Spacing = new Vector2(1),
                                    Padding = new MarginPadding { Left = 5 },
                                    Children = new[]
                                    {
                                        new OsuSpriteText
                                        {
                                            Name = "Singer name",
                                            Text = singer.Name,
                                            Font = OsuFont.GetFont(weight: FontWeight.Bold, size: 20),
                                        },
                                        new OsuSpriteText
                                        {
                                            Name = "Romaji name",
                                            Text = singer.RomajiName,
                                            Font = OsuFont.GetFont(weight: FontWeight.Bold, size: 9),
                                        },
                                        new OsuSpriteText
                                        {
                                            Name = "English name",
                                            Text = singer.EnglishName != null ? $"({singer.EnglishName})" : "",
                                            Font = OsuFont.GetFont(weight: FontWeight.Bold, size: 12),
                                        }
                                    }
                                },
                            }
                        }
                    },
                };
            }

            public object TooltipContent => singer;

            public ITooltip GetCustomTooltip() => new SingerToolTip();
        }
    }
}
