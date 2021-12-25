// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Cursor;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Graphics.UserInterface;
using osu.Game.Graphics;
using osu.Game.Graphics.Sprites;
using osu.Game.Graphics.UserInterface;
using osu.Game.Overlays;
using osu.Game.Rulesets.Karaoke.Beatmaps.Metadatas;
using osu.Game.Rulesets.Karaoke.Edit.ChangeHandlers.Singers;
using osu.Game.Rulesets.Karaoke.Edit.Singers.Detail;
using osu.Game.Rulesets.Karaoke.Edit.Singers.Rows.Components;
using osu.Game.Rulesets.Karaoke.Graphics.Cursor;
using osu.Game.Rulesets.Karaoke.Graphics.Sprites;
using osuTK;
using osuTK.Graphics;

namespace osu.Game.Rulesets.Karaoke.Edit.Singers.Rows
{
    public class SingerLyricPlacementColumn : LyricPlacementColumn
    {
        public SingerLyricPlacementColumn(Singer singer)
            : base(singer)
        {
        }

        protected override float SingerInfoSize => 178;

        protected override Drawable CreateSingerInfo(Singer singer)
            => new DrawableSingerInfo(singer);

        protected override Drawable CreateTimeLinePart(Singer singer)
            => new SingerLyricEditor(singer);

        internal class DrawableSingerInfo : CompositeDrawable, IHasCustomTooltip<Singer>, IHasContextMenu
        {
            [Resolved]
            private ISingersChangeHandler singersChangeHandler { get; set; }

            [Resolved]
            private DialogOverlay dialogOverlay { get; set; }

            [Resolved]
            private EditSingerDialog editSingerDialog { get; set; }

            private readonly Box background;
            private readonly DrawableSingerAvatar avatar;
            private readonly OsuSpriteText singerName;
            private readonly OsuSpriteText romajiName;
            private readonly OsuSpriteText englishName;

            private readonly Bindable<Singer> bindableSinger = new();

            public DrawableSingerInfo(Singer singer)
            {
                bindableSinger.Value = singer;
                InternalChildren = new Drawable[]
                {
                    background = new Box
                    {
                        Name = "Background",
                        RelativeSizeAxes = Axes.Both,
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
                                avatar = new DrawableSingerAvatar
                                {
                                    Name = "Avatar",
                                    Size = new Vector2(48),
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
                                        singerName = new OsuSpriteText
                                        {
                                            Name = "Singer name",
                                            Font = OsuFont.GetFont(weight: FontWeight.Bold, size: 20),
                                        },
                                        romajiName = new OsuSpriteText
                                        {
                                            Name = "Romaji name",
                                            Font = OsuFont.GetFont(weight: FontWeight.Bold, size: 9),
                                        },
                                        englishName = new OsuSpriteText
                                        {
                                            Name = "English name",
                                            Font = OsuFont.GetFont(weight: FontWeight.Bold, size: 12),
                                        }
                                    }
                                },
                            }
                        }
                    },
                };

                bindableSinger.BindValueChanged(e =>
                {
                    updateSingerInfo(e.NewValue);
                }, true);

                singer.OrderBindable.BindValueChanged(_ =>
                {
                    updateSingerName(singer);
                });
            }

            private void updateSingerInfo(Singer singer)
            {
                if (singer == null)
                    return;

                // background
                background.Colour = singer.Color ?? new Color4();
                background.Alpha = singer.Color != null ? 0.3f : 0;

                // avatar
                avatar.Singer = singer;

                // metadata
                updateSingerName(singer);
                romajiName.Text = singer.RomajiName;
                englishName.Text = singer.EnglishName != null ? $"({singer.EnglishName})" : "";
            }

            private void updateSingerName(Singer singer)
            {
                singerName.Text = $"#{singer.Order} {singer.Name}";
            }

            public ITooltip<Singer> GetCustomTooltip() => new SingerToolTip();

            public Singer TooltipContent => bindableSinger.Value;

            public MenuItem[] ContextMenuItems => new MenuItem[]
            {
                new OsuMenuItem("Edit singer info", MenuItemType.Standard, () =>
                {
                    editSingerDialog.Current = bindableSinger;
                    editSingerDialog.Show();
                }),
                new OsuMenuItem("Delete", MenuItemType.Destructive, () =>
                {
                    dialogOverlay.Push(new DeleteSingerDialog(isOk =>
                    {
                        if (isOk)
                            singersChangeHandler.Remove(bindableSinger.Value);
                    }));
                }),
            };
        }
    }
}
