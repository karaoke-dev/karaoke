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
using osu.Game.Graphics.UserInterface;
using osu.Game.Overlays;
using osu.Game.Rulesets.Karaoke.Edit.Lyrics.Components.Infos.FixedInfo;
using osu.Game.Rulesets.Karaoke.Edit.Lyrics.Components.Infos.MainInfo;
using osu.Game.Rulesets.Karaoke.Edit.Lyrics.Components.Infos.SubInfo;
using osu.Game.Rulesets.Karaoke.Objects;
using osuTK;

namespace osu.Game.Rulesets.Karaoke.Edit.Lyrics.Components.Infos
{
    public class InfoControl : Container, IHasContextMenu
    {
        private const int max_height = 120;

        private readonly Box headerBackground;
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
                headerBackground = new Box
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
                        new TimeInfoContainer(Lyric)
                        {
                            RelativeSizeAxes = Axes.X,
                            Height = 36,
                        },
                        new GridContainer
                        {
                            RelativeSizeAxes = Axes.X,
                            AutoSizeAxes = Axes.Y,
                            ColumnDimensions = new []
                            {
                                new Dimension(GridSizeMode.Distributed),
                                new Dimension(GridSizeMode.Absolute, 28),
                            },
                            Content = new []
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

            bindableLyricFastEditMode.BindValueChanged(e =>
            {
                CreateBadge(e.NewValue);
            });
        }

        [BackgroundDependencyLoader]
        private void load(OsuColour colours, LyricEditorStateManager stateManager)
        {
            headerBackground.Colour = colours.Gray2;

            bindableMode.BindTo(stateManager.BindableMode);
            bindableLyricFastEditMode.BindTo(stateManager.BindableFastEditMode);
        }

        protected void CreateBadge(LyricFastEditMode mode)
        {
            subInfoContainer.Clear();
            var subInfo = createSubInfo();
            if (subInfo == null)
                return;

            subInfo.Margin = new MarginPadding { Right = 5 };
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
