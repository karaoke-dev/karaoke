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
using osu.Framework.Input.Events;
using osu.Game.Graphics;
using osu.Game.Graphics.Sprites;
using osu.Game.Graphics.UserInterface;
using osu.Game.Overlays;
using osu.Game.Rulesets.Karaoke.Edit.ChangeHandlers.Lyrics;
using osu.Game.Rulesets.Karaoke.Edit.Lyrics.CaretPosition;
using osu.Game.Rulesets.Karaoke.Edit.Lyrics.Rows.Components;
using osu.Game.Rulesets.Karaoke.Edit.Lyrics.Rows.Components.Carets;
using osu.Game.Rulesets.Karaoke.Edit.Lyrics.Rows.Components.FixedInfo;
using osu.Game.Rulesets.Karaoke.Edit.Lyrics.Rows.Components.Parts;
using osu.Game.Rulesets.Karaoke.Edit.Lyrics.Rows.Components.SubInfo;
using osu.Game.Rulesets.Karaoke.Edit.Lyrics.States;
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

            [Resolved(canBeNull: true)]
            private DialogOverlay dialogOverlay { get; set; }

            [Resolved]
            private ILyricsChangeHandler lyricsChangeHandler { get; set; }

            [Resolved]
            private ILyricCaretState lyricCaretState { get; set; }

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

            protected void CreateBadge(LyricEditorMode mode)
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
                        case LyricEditorMode.View:
                        case LyricEditorMode.Manage:
                        case LyricEditorMode.Typing:
                            return null;

                        case LyricEditorMode.Language:
                            return new LanguageInfo(Lyric);

                        case LyricEditorMode.EditRuby:
                        case LyricEditorMode.EditRomaji:
                            return new LanguageInfo(Lyric);

                        case LyricEditorMode.CreateTimeTag:
                            return new LanguageInfo(Lyric);

                        case LyricEditorMode.RecordTimeTag:
                        case LyricEditorMode.AdjustTimeTag:
                            return new TimeTagInfo(Lyric);

                        case LyricEditorMode.EditNote:
                            return null;

                        case LyricEditorMode.Singer:
                            return new SingerInfo(Lyric);

                        default:
                            throw new ArgumentOutOfRangeException(nameof(mode));
                    }
                }
            }

            public MenuItem[] ContextMenuItems
            {
                get
                {
                    if (bindableMode.Value != LyricEditorMode.Manage)
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
            private readonly EditorLyricPiece lyricPiece;

            private readonly Container timeTagContainer;
            private readonly Container<DrawableCaret> caretContainer;

            private readonly IBindable<LyricEditorMode> bindableMode = new Bindable<LyricEditorMode>();

            [Resolved]
            private ILyricsChangeHandler lyricsChangeHandler { get; set; }

            [Resolved]
            private ILyricCaretState lyricCaretState { get; set; }

            public Lyric Lyric { get; }

            public SingleLyricEditor(Lyric lyric)
            {
                Lyric = lyric;
                CornerRadius = 5;
                AutoSizeAxes = Axes.Y;
                Padding = new MarginPadding { Bottom = 10 };
                Children = new Drawable[]
                {
                    lyricPiece = new EditorLyricPiece(lyric),
                    timeTagContainer = new Container
                    {
                        RelativeSizeAxes = Axes.Both,
                    },
                    caretContainer = new Container<DrawableCaret>
                    {
                        RelativeSizeAxes = Axes.Both,
                    }
                };

                lyricPiece.TimeTagsBindable.BindCollectionChanged((_, _) =>
                {
                    ScheduleAfterChildren(UpdateTimeTags);
                }, true);

                bindableMode.BindValueChanged(e =>
                {
                    // initial default caret.
                    InitializeCaret(e.NewValue);

                    // Initial blueprint container.
                    InitializeBlueprint(e.NewValue);
                });
            }

            protected override bool OnMouseMove(MouseMoveEvent e)
            {
                if (!lyricCaretState.CaretEnabled)
                    return false;

                var mode = bindableMode.Value;
                float position = ToLocalSpace(e.ScreenSpaceMousePosition).X;

                switch (mode)
                {
                    case LyricEditorMode.View:
                        break;

                    case LyricEditorMode.Manage:
                        int cuttingLyricStringIndex = Math.Clamp(TextIndexUtils.ToStringIndex(lyricPiece.GetHoverIndex(position)), 0, Lyric.Text.Length - 1);
                        lyricCaretState.MoveHoverCaretToTargetPosition(new TextCaretPosition(Lyric, cuttingLyricStringIndex));
                        break;

                    case LyricEditorMode.Typing:
                        int typingStringIndex = TextIndexUtils.ToStringIndex(lyricPiece.GetHoverIndex(position));
                        lyricCaretState.MoveHoverCaretToTargetPosition(new TextCaretPosition(Lyric, typingStringIndex));
                        break;

                    case LyricEditorMode.Language:
                        break;

                    case LyricEditorMode.EditRuby:
                        lyricCaretState.MoveHoverCaretToTargetPosition(new NavigateCaretPosition(Lyric));
                        break;

                    case LyricEditorMode.EditRomaji:
                        lyricCaretState.MoveHoverCaretToTargetPosition(new NavigateCaretPosition(Lyric));
                        break;

                    case LyricEditorMode.CreateTimeTag:
                        var textIndex = lyricPiece.GetHoverIndex(position);
                        lyricCaretState.MoveHoverCaretToTargetPosition(new TimeTagIndexCaretPosition(Lyric, textIndex));
                        break;

                    case LyricEditorMode.RecordTimeTag:
                        var timeTag = lyricPiece.GetHoverTimeTag(position);
                        lyricCaretState.MoveHoverCaretToTargetPosition(new TimeTagCaretPosition(Lyric, timeTag));
                        break;

                    case LyricEditorMode.AdjustTimeTag:
                    case LyricEditorMode.EditNote:
                    case LyricEditorMode.Singer:
                        lyricCaretState.MoveHoverCaretToTargetPosition(new NavigateCaretPosition(Lyric));
                        break;

                    default:
                        throw new ArgumentOutOfRangeException(nameof(mode));
                }

                return base.OnMouseMove(e);
            }

            protected override void OnHoverLost(HoverLostEvent e)
            {
                if (!lyricCaretState.CaretEnabled)
                    return;

                // lost hover caret and time-tag caret
                lyricCaretState.ClearHoverCaretPosition();
                base.OnHoverLost(e);
            }

            protected override bool OnClick(ClickEvent e)
            {
                if (!lyricCaretState.CaretEnabled)
                    return false;

                // place hover caret to target position.
                var position = lyricCaretState.BindableHoverCaretPosition.Value;
                if (position == null)
                    return false;

                lyricCaretState.MoveCaretToTargetPosition(position);

                return true;
            }

            protected override bool OnDoubleClick(DoubleClickEvent e)
            {
                var mode = bindableMode.Value;
                var position = lyricCaretState.BindableCaretPosition.Value;

                switch (position)
                {
                    case TextCaretPosition textCaretPosition:
                        if (mode == LyricEditorMode.Manage)
                            lyricsChangeHandler.Split(textCaretPosition.Index);
                        return true;

                    default:
                        return false;
                }
            }

            [BackgroundDependencyLoader]
            private void load(EditorClock clock, ILyricEditorState state)
            {
                lyricPiece.Clock = clock;
                bindableMode.BindTo(state.BindableMode);
            }

            protected void InitializeBlueprint(LyricEditorMode mode)
            {
                // remove all exist blueprint container
                RemoveAll(x => x is RubyBlueprintContainer or RomajiBlueprintContainer or TimeTagBlueprintContainer);

                // create preview and real caret
                var blueprintContainer = createBlueprintContainer(mode, Lyric);
                if (blueprintContainer == null)
                    return;

                AddInternal(blueprintContainer);

                static Drawable createBlueprintContainer(LyricEditorMode mode, Lyric lyric) =>
                    mode switch
                    {
                        LyricEditorMode.EditRuby => new RubyBlueprintContainer(lyric),
                        LyricEditorMode.EditRomaji => new RomajiBlueprintContainer(lyric),
                        LyricEditorMode.AdjustTimeTag => new TimeTagBlueprintContainer(lyric),
                        _ => null
                    };
            }

            protected void InitializeCaret(LyricEditorMode mode)
            {
                caretContainer.Clear();

                // create preview and real caret
                addCaret(false);
                addCaret(true);

                void addCaret(bool isPreview)
                {
                    var caret = createCaret(mode, isPreview);
                    if (caret == null)
                        return;

                    caret.Hide();

                    caretContainer.Add(caret);
                }

                static DrawableCaret createCaret(LyricEditorMode mode, bool isPreview)
                {
                    switch (mode)
                    {
                        case LyricEditorMode.View:
                            return null;

                        case LyricEditorMode.Manage:
                            return new DrawableLyricSplitterCaret(isPreview);

                        case LyricEditorMode.Typing:
                            return new DrawableLyricInputCaret(isPreview);

                        case LyricEditorMode.Language:
                        case LyricEditorMode.EditRuby:
                        case LyricEditorMode.EditRomaji:
                            return null;

                        case LyricEditorMode.CreateTimeTag:
                            return new DrawableTimeTagEditCaret(isPreview);

                        case LyricEditorMode.RecordTimeTag:
                            return new DrawableTimeTagRecordCaret(isPreview);

                        case LyricEditorMode.AdjustTimeTag:
                        case LyricEditorMode.EditNote:
                        case LyricEditorMode.Singer:
                            return null;

                        default:
                            throw new ArgumentOutOfRangeException(nameof(mode));
                    }
                }
            }

            protected void UpdateTimeTags()
            {
                timeTagContainer.Clear();
                var timeTags = lyricPiece.TimeTagsBindable;
                if (timeTags == null)
                    return;

                foreach (var timeTag in timeTags)
                {
                    var position = lyricPiece.GetTimeTagPosition(timeTag);
                    timeTagContainer.Add(new DrawableTimeTag(Lyric, timeTag)
                    {
                        Position = position
                    });
                }
            }
        }
    }
}
