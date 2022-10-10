// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
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
using osu.Game.Rulesets.Karaoke.Edit.ChangeHandlers.Lyrics;
using osu.Game.Rulesets.Karaoke.Edit.Lyrics.LyricList.Rows.Info.Badge;
using osu.Game.Rulesets.Karaoke.Edit.Lyrics.LyricList.Rows.Info.FixedInfo;
using osu.Game.Rulesets.Karaoke.Edit.Lyrics.States;
using osu.Game.Rulesets.Karaoke.Edit.Lyrics.States.Modes;
using osu.Game.Rulesets.Karaoke.Edit.Utils;
using osu.Game.Rulesets.Karaoke.Objects;
using osu.Game.Rulesets.Karaoke.Utils;
using osuTK;

namespace osu.Game.Rulesets.Karaoke.Edit.Lyrics.LyricList.Rows.Info
{
    public class InfoControl : Container, IHasContextMenu
    {
        private const int max_height = 120;

        private readonly Box background;
        private readonly Box headerBackground;
        private readonly OsuSpriteText timeRange;
        private readonly Container subInfoContainer;

        private readonly IBindable<ModeWithSubMode> bindableModeAndSubMode = new Bindable<ModeWithSubMode>();

        [Resolved]
        private IDialogOverlay? dialogOverlay { get; set; }

        [Resolved, AllowNull]
        private ILyricsChangeHandler lyricsChangeHandler { get; set; }

        [Resolved, AllowNull]
        private ILyricCaretState lyricCaretState { get; set; }

        [Resolved, AllowNull]
        private LyricEditorColourProvider colourProvider { get; set; }

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

            bindableModeAndSubMode.BindValueChanged(e =>
            {
                initializeBadge(e.NewValue.Mode, e.NewValue.SubMode);

                if (ValueChangedEventUtils.EditModeChanged(e) || !IsLoaded)
                    Schedule(() => updateColour(e.NewValue.Mode));
            }, true);
        }

        [BackgroundDependencyLoader]
        private void load(ILyricEditorState state)
        {
            bindableModeAndSubMode.BindTo(state.BindableModeAndSubMode);
        }

        private void updateColour(LyricEditorMode mode)
        {
            background.Colour = colourProvider.Background2(mode);
            headerBackground.Colour = colourProvider.Background5(mode);
        }

        private void initializeBadge(LyricEditorMode mode, Enum? subMode)
        {
            subInfoContainer.Clear();
            var subInfo = createSubInfo();
            if (subInfo == null)
                return;

            subInfo.Margin = new MarginPadding { Right = 15 };
            subInfo.Anchor = Anchor.TopRight;
            subInfo.Origin = Anchor.TopRight;
            subInfoContainer.Add(subInfo);

            Drawable? createSubInfo()
            {
                switch (mode)
                {
                    case LyricEditorMode.View:
                    case LyricEditorMode.Texting:
                        return null;

                    case LyricEditorMode.Reference:
                        return new ReferenceLyricInfo(Lyric);

                    case LyricEditorMode.Language:
                        return new LanguageInfo(Lyric);

                    case LyricEditorMode.EditRuby:
                    case LyricEditorMode.EditRomaji:
                        return new LanguageInfo(Lyric);

                    case LyricEditorMode.EditTimeTag:
                        if (subMode is not TimeTagEditMode timeTagEditMode)
                            throw new NullReferenceException();

                        return createTimeTagModeSubInfo(timeTagEditMode, Lyric);

                    case LyricEditorMode.EditNote:
                        return null;

                    case LyricEditorMode.Singer:
                        return new SingerInfo(Lyric);

                    default:
                        throw new ArgumentOutOfRangeException(nameof(mode));
                }

                static Drawable createTimeTagModeSubInfo(TimeTagEditMode editMode, Lyric lyric)
                {
                    switch (editMode)
                    {
                        case TimeTagEditMode.Create:
                            return new LanguageInfo(lyric);

                        case TimeTagEditMode.Recording:
                        case TimeTagEditMode.Adjust:
                            return new TimeTagInfo(lyric);

                        default:
                            throw new ArgumentOutOfRangeException(nameof(subMode));
                    }
                }
            }
        }

        public MenuItem[] ContextMenuItems
        {
            get
            {
                var editMode = bindableModeAndSubMode.Value.Mode;
                if (editMode != LyricEditorMode.Texting)
                    return Array.Empty<MenuItem>();

                // should select lyric if trying to interact with context menu.
                lyricCaretState.MoveCaretToTargetPosition(Lyric);

                var menuItems = new List<MenuItem>
                {
                    new OsuMenuItem("Create new lyric", MenuItemType.Standard, () =>
                    {
                        lyricsChangeHandler.CreateAtPosition();
                    })
                };

                // use lazy way to check lyric is not in first
                if (Lyric.Order > 1)
                {
                    menuItems.Add(new OsuMenuItem("Combine with previous lyric", MenuItemType.Standard, () =>
                    {
                        lyricsChangeHandler.Combine();
                    }));
                }

                menuItems.Add(new OsuMenuItem("Delete", MenuItemType.Destructive, () =>
                {
                    if (dialogOverlay == null)
                    {
                        // todo : remove lyric directly in test case because pop-up dialog is not registered.
                        lyricsChangeHandler.Remove();
                    }
                    else
                    {
                        dialogOverlay.Push(new DeleteLyricDialog(isOk =>
                        {
                            if (isOk)
                                lyricsChangeHandler.Remove();
                        }));
                    }
                }));

                return menuItems.ToArray();
            }
        }
    }
}
