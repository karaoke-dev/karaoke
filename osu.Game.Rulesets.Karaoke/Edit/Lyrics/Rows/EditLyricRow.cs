// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using System.Collections.Generic;
using System.Linq;
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
using osu.Game.Rulesets.Karaoke.Edit.Lyrics.CaretPosition;
using osu.Game.Rulesets.Karaoke.Edit.Lyrics.Rows.Components;
using osu.Game.Rulesets.Karaoke.Edit.Lyrics.Rows.Components.Carets;
using osu.Game.Rulesets.Karaoke.Edit.Lyrics.Rows.Components.FixedInfo;
using osu.Game.Rulesets.Karaoke.Edit.Lyrics.Rows.Components.Parts;
using osu.Game.Rulesets.Karaoke.Edit.Lyrics.Rows.Components.SubInfo;
using osu.Game.Rulesets.Karaoke.Objects;
using osu.Game.Rulesets.Karaoke.Utils;
using osu.Game.Screens.Edit;
using osuTK;

namespace osu.Game.Rulesets.Karaoke.Edit.Lyrics.Rows
{
    public class EditLyricRow : LyricEditorRow
    {
        private const int min_height = 75;
        private const int continuous_spacing = 20;

        public EditLyricRow(Lyric lyric)
            : base(lyric)
        {
            AutoSizeAxes = Axes.Y;
        }

        protected override Drawable CreateLyricInfo(Lyric lyric)
        {
            // todo : need to refactor this part.
            var isContinuous = lyric.LayoutIndex == -1;
            var continuousSpacing = isContinuous ? continuous_spacing : 0;

            return new InfoControl(lyric)
            {
                Margin = new MarginPadding
                {
                    Left = continuousSpacing,
                },
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

            private readonly Bindable<LyricEditorMode> bindableMode = new Bindable<LyricEditorMode>();

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

                        case LyricEditorMode.EditRubyRomaji:
                            return null;

                        case LyricEditorMode.CreateTimeTag:
                        case LyricEditorMode.RecordTimeTag:
                        case LyricEditorMode.AdjustTimeTag:
                            return new TimeTagInfo(Lyric);

                        case LyricEditorMode.EditNote:
                            return null;

                        case LyricEditorMode.Layout:
                            return new LayoutInfo(Lyric);

                        case LyricEditorMode.Singer:
                            return new SingerInfo(Lyric);

                        default:
                            throw new IndexOutOfRangeException(nameof(mode));
                    }
                }
            }

            public MenuItem[] ContextMenuItems
            {
                get
                {
                    if (bindableMode.Value != LyricEditorMode.Manage)
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

        public class SingleLyricEditor : Container
        {
            [Cached]
            private readonly EditorLyricPiece lyricPiece;

            private readonly Container timeTagContainer;
            private readonly Container caretContainer;

            [Resolved(canBeNull: true)]
            private LyricManager lyricManager { get; set; }

            [Resolved]
            private ILyricEditorState state { get; set; }

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
                    caretContainer = new Container
                    {
                        RelativeSizeAxes = Axes.Both,
                    }
                };

                lyricPiece.TimeTagsBindable.BindValueChanged(e =>
                {
                    ScheduleAfterChildren(UpdateTimeTags);
                }, true);
            }

            protected override bool OnMouseMove(MouseMoveEvent e)
            {
                if (lyricManager == null)
                    return false;

                if (!state.CaretEnabled)
                    return false;

                var position = ToLocalSpace(e.ScreenSpaceMousePosition).X;

                switch (state.Mode)
                {
                    case LyricEditorMode.Manage:
                        var cuttingLyricStringIndex = Math.Clamp(TextIndexUtils.ToStringIndex(lyricPiece.GetHoverIndex(position)), 0, Lyric.Text.Length - 1);
                        state.MoveHoverCaretToTargetPosition(new TextCaretPosition(Lyric, cuttingLyricStringIndex));
                        break;

                    case LyricEditorMode.Typing:
                        var typingStringIndex = TextIndexUtils.ToStringIndex(lyricPiece.GetHoverIndex(position));
                        state.MoveHoverCaretToTargetPosition(new TextCaretPosition(Lyric, typingStringIndex));
                        break;

                    case LyricEditorMode.EditRubyRomaji:
                        state.MoveHoverCaretToTargetPosition(new NavigateCaretPosition(Lyric));
                        break;

                    case LyricEditorMode.CreateTimeTag:
                        var textIndex = lyricPiece.GetHoverIndex(position);
                        state.MoveHoverCaretToTargetPosition(new TimeTagIndexCaretPosition(Lyric, textIndex));
                        break;

                    case LyricEditorMode.RecordTimeTag:
                        var timeTag = lyricPiece.GetHoverTimeTag(position);
                        state.MoveHoverCaretToTargetPosition(new TimeTagCaretPosition(Lyric, timeTag));
                        break;

                    case LyricEditorMode.AdjustTimeTag:
                    case LyricEditorMode.EditNote:
                    case LyricEditorMode.Layout:
                    case LyricEditorMode.Singer:
                        state.MoveHoverCaretToTargetPosition(new NavigateCaretPosition(Lyric));
                        break;

                    default:
                        throw new ArgumentOutOfRangeException(nameof(state.Mode));
                }

                return base.OnMouseMove(e);
            }

            protected override void OnHoverLost(HoverLostEvent e)
            {
                if (!state.CaretEnabled)
                    return;

                // lost hover caret and time-tag caret
                state.ClearHoverCaretPosition();
                base.OnHoverLost(e);
            }

            protected override bool OnClick(ClickEvent e)
            {
                if (!state.CaretEnabled)
                    return false;

                // place hover caret to target position.
                var position = state.BindableHoverCaretPosition.Value;
                if (position == null)
                    return false;

                state.MoveCaretToTargetPosition(position);

                return true;
            }

            protected override bool OnDoubleClick(DoubleClickEvent e)
            {
                if (state.Mode != LyricEditorMode.Manage)
                    return false;

                // todo : not really sure is ok to split time-tag by double click?
                // need to make an ux research.
                var position = state.BindableCaretPosition.Value;

                if (position is TextCaretPosition textCaretPosition)
                {
                    lyricManager?.SplitLyric(Lyric, textCaretPosition.Index);
                    return true;
                }

                throw new NotSupportedException(nameof(position));
            }

            [BackgroundDependencyLoader]
            private void load(EditorClock clock)
            {
                lyricPiece.Clock = clock;
                state.BindableMode.BindValueChanged(e =>
                {
                    // initial default caret.
                    InitializeCaret(e.NewValue);

                    // Initial blueprint container.
                    InitializeBlueprint(e.NewValue);
                }, true);

                // update change if caret changed.
                state.BindableHoverCaretPosition.BindValueChanged(e =>
                {
                    UpdateCaretPosition(e.NewValue, true);
                });
                state.BindableCaretPosition.BindValueChanged(e =>
                {
                    UpdateCaretPosition(e.NewValue, false);
                });
            }

            protected void InitializeBlueprint(LyricEditorMode mode)
            {
                // remove all exist blueprint container
                RemoveAll(x => x is RubyRomajiBlueprintContainer || x is TimeTagBlueprintContainer);

                // create preview and real caret
                var blueprintContainer = createBlueprintContainer(mode, Lyric);
                if (blueprintContainer == null)
                    return;

                AddInternal(blueprintContainer);

                static Drawable createBlueprintContainer(LyricEditorMode mode, Lyric lyric)
                {
                    switch (mode)
                    {
                        case LyricEditorMode.EditRubyRomaji:
                            return new RubyRomajiBlueprintContainer(lyric);

                        case LyricEditorMode.AdjustTimeTag:
                            return new TimeTagBlueprintContainer(lyric);

                        default:
                            return null;
                    }
                }
            }

            protected void InitializeCaret(LyricEditorMode mode)
            {
                caretContainer.Clear();

                // create preview and real caret
                addCaret(false);
                addCaret(true);

                void addCaret(bool isPreview)
                {
                    var caret = createCaret(mode);
                    if (caret == null)
                        return;

                    caret.Hide();

                    if (caret is IDrawableCaret drawableCaret)
                        drawableCaret.Preview = isPreview;

                    caretContainer.Add(caret);
                }

                static Drawable createCaret(LyricEditorMode mode)
                {
                    switch (mode)
                    {
                        case LyricEditorMode.View:
                            return null;

                        case LyricEditorMode.Manage:
                            return new DrawableLyricSplitterCaret();

                        case LyricEditorMode.Typing:
                            return new DrawableLyricInputCaret();

                        case LyricEditorMode.Language:
                        case LyricEditorMode.EditRubyRomaji:
                            return null;

                        case LyricEditorMode.CreateTimeTag:
                            return new DrawableTimeTagEditCaret();

                        case LyricEditorMode.RecordTimeTag:
                            return new DrawableTimeTagRecordCaret();

                        case LyricEditorMode.AdjustTimeTag:
                        case LyricEditorMode.EditNote:
                        case LyricEditorMode.Layout:
                        case LyricEditorMode.Singer:
                            return null;

                        default:
                            throw new IndexOutOfRangeException(nameof(mode));
                    }
                }
            }

            protected void UpdateCaretPosition(ICaretPosition position, bool hover)
            {
                if (position == null)
                    return;

                var caret = caretContainer.OfType<IDrawableCaret>().FirstOrDefault(x => x.Preview == hover);
                if (caret == null)
                    return;

                if (position.Lyric != Lyric)
                {
                    caret.Hide();
                    return;
                }

                Vector2 caretPosition = getCaretPosition();

                // set position
                if (caret is DrawableLyricInputCaret inputCaret)
                {
                    inputCaret.DisplayAt(caretPosition, null);
                }
                else if (caret is Drawable drawable)
                {
                    drawable.Position = caretPosition;
                }

                // todo : should have a better way to set height to input or split caret
                if (caret is DrawableLyricInputCaret || caret is DrawableLyricSplitterCaret)
                {
                    if (caret is Drawable drawable)
                    {
                        var textHeight = lyricPiece.GetTextHeight();
                        drawable.Height = textHeight;
                    }
                }

                // set other property
                if (caret is IHasCaretPosition hasCaretPosition)
                {
                    hasCaretPosition.CaretPosition = position;
                }
                else if (caret is IHasTimeTag hasTimeTag)
                {
                    hasTimeTag.TimeTag = (position as TimeTagCaretPosition)?.TimeTag;
                }

                caret.Show();

                Vector2 getCaretPosition()
                {
                    var textHeight = lyricPiece.GetTextHeight();

                    switch (position)
                    {
                        case TextCaretPosition textCaretPosition:
                            var end = textCaretPosition.Index == textCaretPosition.Lyric?.Text?.Length;
                            var originPosition = lyricPiece.GetTextIndexPosition(TextIndexUtils.FromStringIndex(textCaretPosition.Index, end));
                            return new Vector2(originPosition.X, originPosition.Y - textHeight);

                        case TimeTagIndexCaretPosition indexCaretPosition:
                            return lyricPiece.GetTextIndexPosition(indexCaretPosition.Index);

                        case TimeTagCaretPosition timeTagCaretPosition:
                            var timeTag = timeTagCaretPosition.TimeTag;
                            return lyricPiece.GetTimeTagPosition(timeTag);

                        default:
                            throw new NotSupportedException(nameof(position));
                    }
                }
            }

            protected void UpdateTimeTags()
            {
                timeTagContainer.Clear();
                var timeTags = lyricPiece.TimeTagsBindable.Value;
                if (timeTags == null)
                    return;

                foreach (var timeTag in timeTags)
                {
                    var position = lyricPiece.GetTimeTagPosition(timeTag);
                    timeTagContainer.Add(new DrawableTimeTag(new TimeTagCaretPosition(Lyric, timeTag))
                    {
                        Position = position
                    });
                }
            }
        }
    }
}
