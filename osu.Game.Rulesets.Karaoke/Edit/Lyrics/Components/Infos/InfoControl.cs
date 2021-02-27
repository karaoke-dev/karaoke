// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
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
using osu.Game.Rulesets.Karaoke.Edit.Lyrics.Components.Infos.FixedInfo;
using osu.Game.Rulesets.Karaoke.Edit.Lyrics.Components.Infos.SubInfo;
using osu.Game.Rulesets.Karaoke.Objects;
using osu.Game.Rulesets.Karaoke.Utils;
using osuTK;

namespace osu.Game.Rulesets.Karaoke.Edit.Lyrics.Components.Infos
{
    public class InfoControl : Container, IHasContextMenu
    {
        private const int max_height = 120;

        private readonly Box background;
        private readonly Box headerBackground;
        private readonly OsuSpriteText timeRange;
        private readonly Container subInfoContainer;

        private readonly Bindable<Mode> bindableMode = new Bindable<Mode>();
        private readonly Bindable<LyricFastEditMode> bindableLyricFastEditMode = new Bindable<LyricFastEditMode>();

        [Resolved(canBeNull: true)]
        private DialogOverlay dialogOverlay { get; set; }

        [Resolved]
        private LyricManager lyricManager { get; set; }

        public Lyric Lyric { get; }

        public InfoControl(Lyric lyric)
        {
            Lyric = lyric;

            Children = new Drawable[]
            {
                background = new Box
                {
                    RelativeSizeAxes = Axes.X,
                    Height = max_height,
                },
                new FillFlowContainer
                {
                    Direction = FillDirection.Vertical,
                    RelativeSizeAxes = Axes.X,
                    AutoSizeAxes = Axes.Y,
                    Anchor = Anchor.TopRight,
                    Origin = Anchor.TopRight,
                    Spacing = new Vector2(5),
                    Children = new Drawable[]
                    {
                        new Container
                        {
                            RelativeSizeAxes = Axes.X,
                            Height = 36,
                            Children = new Drawable[]
                            {
                                headerBackground = new Box
                                {
                                    RelativeSizeAxes = Axes.Both
                                },
                                timeRange = new OsuSpriteText
                                {
                                    Anchor = Anchor.CentreLeft,
                                    Origin = Anchor.CentreLeft,
                                    Font = OsuFont.GetFont(size: 16, fixedWidth: true),
                                    Margin = new MarginPadding(10),
                                },
                                new InvalidInfo(lyric)
                                {
                                    Anchor = Anchor.CentreRight,
                                    Origin = Anchor.CentreRight,
                                    Margin = new MarginPadding(10),
                                    Scale = new Vector2(1.3f),
                                    Y = 1,
                                },
                            }
                        },
                        new GridContainer
                        {
                            RelativeSizeAxes = Axes.X,
                            AutoSizeAxes = Axes.Y,
                            ColumnDimensions = new[]
                            {
                                new Dimension(),
                                new Dimension(GridSizeMode.Absolute, 28),
                            },
                            Content = new[]
                            {
                                new Drawable[]
                                {
                                    subInfoContainer = new Container
                                    {
                                        RelativeSizeAxes = Axes.X
                                    },
                                    new FillFlowContainer
                                    {
                                        RelativeSizeAxes = Axes.X,
                                        AutoSizeAxes = Axes.Y,
                                        Direction = FillDirection.Vertical,
                                        Spacing = new Vector2(5),
                                        Children = new Drawable[]
                                        {
                                            new OrderInfo(lyric),
                                            new LockInfo(lyric),
                                        }
                                    }
                                }
                            }
                        }
                    }
                },
            };

            timeRange.Text = LyricUtils.LyricTimeFormattedString(lyric);

            bindableLyricFastEditMode.BindValueChanged(e =>
            {
                CreateBadge(e.NewValue);
            });
        }

        [BackgroundDependencyLoader]
        private void load(OsuColour colours, ILyricEditorState state)
        {
            background.Colour = colours.Gray2;
            headerBackground.Colour = colours.Gray3;

            bindableMode.BindTo(state.BindableMode);
            bindableLyricFastEditMode.BindTo(state.BindableFastEditMode);
        }

        protected void CreateBadge(LyricFastEditMode mode)
        {
            subInfoContainer.Clear();
            var subInfo = createSubInfo();
            if (subInfo == null)
                return;

            subInfo.Margin = new MarginPadding { Right = 15 };
            subInfo.Anchor = Anchor.TopRight;
            subInfo.Origin = Anchor.TopRight;
            subInfoContainer.Add(subInfo);

            Drawable createSubInfo()
            {
                switch (mode)
                {
                    case LyricFastEditMode.None:
                        return null;

                    case LyricFastEditMode.Layout:
                        return new LayoutInfo(Lyric);

                    case LyricFastEditMode.Singer:
                        return new SingerInfo(Lyric);

                    case LyricFastEditMode.Language:
                        return new LanguageInfo(Lyric);

                    case LyricFastEditMode.TimeTag:
                        return new TimeTagInfo(Lyric);

                    default:
                        throw new IndexOutOfRangeException(nameof(mode));
                }
            }
        }

        public MenuItem[] ContextMenuItems
        {
            get
            {
                if (bindableMode.Value != Mode.EditMode)
                    return null;

                return new MenuItem[]
                {
                    new OsuMenuItem("Create new lyric", MenuItemType.Standard, () =>
                    {
                        // add new lyric with below of current lyric.
                        var targetOrder = Lyric.Order;
                        lyricManager.CreateLyric(targetOrder);
                    }),
                    new OsuMenuItem("Delete", MenuItemType.Destructive, () =>
                    {
                        if (dialogOverlay == null)
                        {
                            // todo : remove lyric directly in test case because pop-up dialog is not registered.
                            lyricManager.DeleteLyric(Lyric);
                        }
                        else
                        {
                            dialogOverlay.Push(new DeleteLyricDialog(isOk =>
                            {
                                if (isOk)
                                    lyricManager.DeleteLyric(Lyric);
                            }));
                        }
                    }),
                };
            }
        }
    }
}
