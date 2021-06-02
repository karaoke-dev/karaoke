// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using System.Collections.Generic;
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
using osu.Game.Rulesets.Karaoke.Edit.Lyrics.Rows.Components.FixedInfo;
using osu.Game.Rulesets.Karaoke.Edit.Lyrics.Rows.Components.SubInfo;
using osu.Game.Rulesets.Karaoke.Objects;
using osu.Game.Rulesets.Karaoke.Utils;
using osuTK;

namespace osu.Game.Rulesets.Karaoke.Edit.Lyrics.Rows.Components
{
    public class InfoControl : Container, IHasContextMenu
    {
        private const int max_height = 120;

        private readonly Box background;
        private readonly Box headerBackground;
        private readonly OsuSpriteText timeRange;
        private readonly Container subInfoContainer;

        private readonly Bindable<Mode> bindableMode = new Bindable<Mode>();

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

            bindableMode.BindValueChanged(e =>
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
        }

        protected void CreateBadge(Mode mode)
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
                    case Mode.ViewMode:
                    case Mode.EditMode:
                    case Mode.TypingMode:
                    case Mode.RubyRomajiMode:
                    case Mode.EditNoteMode:
                        return null;

                    case Mode.RecordMode:
                    case Mode.TimeTagEditMode:
                        return new TimeTagInfo(Lyric);

                    case Mode.Layout:
                        return new LayoutInfo(Lyric);

                    case Mode.Singer:
                        return new SingerInfo(Lyric);

                    case Mode.Language:
                        return new LanguageInfo(Lyric);

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

                var menuItems = new List<MenuItem>
                {
                    new OsuMenuItem("Create new lyric", MenuItemType.Standard, () =>
                    {
                        // add new lyric with below of current lyric.
                        var targetOrder = Lyric.Order;
                        lyricManager.CreateLyric(targetOrder);
                    })
                };

                // use lazy way to check lyric is not in first
                if (Lyric.Order > 1)
                {
                    menuItems.Add(new OsuMenuItem("Combine with previous lyric", MenuItemType.Standard, () =>
                    {
                        lyricManager.CombineWithPreviousLyric(Lyric);
                    }));
                }

                menuItems.Add(new OsuMenuItem("Delete", MenuItemType.Destructive, () =>
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
                }));

                return menuItems.ToArray();
            }
        }
    }
}
