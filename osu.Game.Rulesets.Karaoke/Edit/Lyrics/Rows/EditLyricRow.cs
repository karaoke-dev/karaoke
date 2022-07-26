// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

#nullable disable

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
using osu.Game.Rulesets.Karaoke.Edit.ChangeHandlers.Lyrics;
using osu.Game.Rulesets.Karaoke.Edit.Lyrics.Rows.Components;
using osu.Game.Rulesets.Karaoke.Edit.Lyrics.Rows.Components.FixedInfo;
using osu.Game.Rulesets.Karaoke.Edit.Lyrics.Rows.Components.SubInfo;
using osu.Game.Rulesets.Karaoke.Edit.Lyrics.States;
using osu.Game.Rulesets.Karaoke.Edit.Lyrics.States.Modes;
using osu.Game.Rulesets.Karaoke.Objects;
using osu.Game.Rulesets.Karaoke.Utils;
using osu.Game.Screens.Edit;
using osuTK;

namespace osu.Game.Rulesets.Karaoke.Edit.Lyrics.Rows
{
    public class EditLyricRow : LyricEditorRow
    {
        private const int min_height = 75;

        public EditLyricRow(Lyric lyric)
            : base(lyric)
        {
            AutoSizeAxes = Axes.Y;
        }

        protected override Drawable CreateLyricInfo(Lyric lyric)
        {
            return new InfoControl(lyric)
            {
                // todo : cannot use relative size to both because it will cause size cannot roll-back if make lyric smaller.
                RelativeSizeAxes = Axes.X,
                Height = min_height,
            };
        }

        protected override Drawable CreateContent(Lyric lyric)
        {
            return new SingleLyricEditor(lyric)
            {
                Margin = new MarginPadding { Left = 10 },
                RelativeSizeAxes = Axes.X,
            };
        }

        public class InfoControl : Container, IHasContextMenu
        {
            private const int max_height = 120;

            private readonly Box background;
            private readonly Box headerBackground;
            private readonly OsuSpriteText timeRange;
            private readonly Container subInfoContainer;

            private readonly IBindable<LyricEditorMode> bindableMode = new Bindable<LyricEditorMode>();
            private readonly IBindable<TimeTagEditMode> bindableTimeTagEditMode = new Bindable<TimeTagEditMode>();

            [Resolved(canBeNull: true)]
            private IDialogOverlay dialogOverlay { get; set; }

            [Resolved]
            private ILyricsChangeHandler lyricsChangeHandler { get; set; }

            [Resolved]
            private ILyricCaretState lyricCaretState { get; set; }

            [Resolved]
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

                bindableMode.BindValueChanged(e =>
                {
                    Schedule(updateColour);
                    initializeBadge();
                }, true);

                bindableTimeTagEditMode.BindValueChanged(_ =>
                {
                    initializeBadge();
                });
            }

            [BackgroundDependencyLoader]
            private void load(ILyricEditorState state, ITimeTagModeState timeTagModeState)
            {
                bindableMode.BindTo(state.BindableMode);
                bindableTimeTagEditMode.BindTo(timeTagModeState.BindableEditMode);
            }

            private void updateColour()
            {
                background.Colour = colourProvider.Background2(bindableMode.Value);
                headerBackground.Colour = colourProvider.Background5(bindableMode.Value);
            }

            private void initializeBadge()
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
                    var mode = bindableMode.Value;

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
                            return createTimeTagModeSubInfo();

                        case LyricEditorMode.EditNote:
                            return null;

                        case LyricEditorMode.Singer:
                            return new SingerInfo(Lyric);

                        default:
                            throw new ArgumentOutOfRangeException(nameof(mode));
                    }

                    Drawable createTimeTagModeSubInfo()
                    {
                        var timeTagEditMode = bindableTimeTagEditMode.Value;

                        switch (timeTagEditMode)
                        {
                            case TimeTagEditMode.Create:
                                return new LanguageInfo(Lyric);

                            case TimeTagEditMode.Recording:
                            case TimeTagEditMode.Adjust:
                                return new TimeTagInfo(Lyric);

                            default:
                                throw new ArgumentOutOfRangeException(nameof(timeTagEditMode));
                        }
                    }
                }
            }

            public MenuItem[] ContextMenuItems
            {
                get
                {
                    if (bindableMode.Value != LyricEditorMode.Texting)
                        return null;

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

        public class SingleLyricEditor : Container
        {
            [Cached]
            private readonly EditorKaraokeSpriteText karaokeSpriteText;

            public SingleLyricEditor(Lyric lyric)
            {
                CornerRadius = 5;
                AutoSizeAxes = Axes.Y;
                Padding = new MarginPadding { Bottom = 10 };
                Children = new Drawable[]
                {
                    karaokeSpriteText = new EditorKaraokeSpriteText(lyric),
                    new TimeTagLayer(lyric)
                    {
                        RelativeSizeAxes = Axes.Both,
                    },
                    new CaretLayer(lyric)
                    {
                        RelativeSizeAxes = Axes.Both,
                    },
                    new BlueprintLayer(lyric)
                    {
                        RelativeSizeAxes = Axes.Both,
                    }
                };
            }

            [BackgroundDependencyLoader]
            private void load(EditorClock clock)
            {
                karaokeSpriteText.Clock = clock;
            }
        }
    }
}
