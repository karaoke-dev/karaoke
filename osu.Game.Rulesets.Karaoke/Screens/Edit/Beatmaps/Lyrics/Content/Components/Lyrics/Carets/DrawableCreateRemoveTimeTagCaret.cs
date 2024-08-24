// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Cursor;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Input.Events;
using osu.Framework.Localisation;
using osu.Game.Graphics;
using osu.Game.Graphics.UserInterface;
using osu.Game.Rulesets.Karaoke.Edit.ChangeHandlers.Lyrics;
using osu.Game.Rulesets.Karaoke.Edit.Components.Sprites;
using osu.Game.Rulesets.Karaoke.Objects;
using osu.Game.Rulesets.Karaoke.Screens.Edit.Beatmaps.Lyrics.CaretPosition;
using osu.Game.Rulesets.Karaoke.Screens.Edit.Beatmaps.Lyrics.States.Modes;
using osu.Game.Rulesets.Karaoke.Utils;
using osuTK;

namespace osu.Game.Rulesets.Karaoke.Screens.Edit.Beatmaps.Lyrics.Content.Components.Lyrics.Carets;

public partial class DrawableCreateRemoveTimeTagCaret : DrawableCaret<CreateRemoveTimeTagCaretPosition>
{
    private const float border_spacing = 5;

    private readonly TimeTagsInfo? startTimeTagInfo;
    private readonly TimeTagsInfo? endTimeTagInfo;

    public DrawableCreateRemoveTimeTagCaret(DrawableCaretState state)
        : base(state)
    {
        // todo: should re-design the drawable caret.
        InternalChildren = new Drawable[]
        {
            new Container
            {
                Masking = true,
                BorderThickness = border_spacing,
                BorderColour = Colour4.White,
                RelativeSizeAxes = Axes.Both,
                Alpha = GetAlpha(state),
                Child = new Box
                {
                    RelativeSizeAxes = Axes.Both,
                    Colour = Colour4.White,
                    Alpha = 0.1f,
                },
            },
        };

        if (state != DrawableCaretState.Idle)
            return;

        AddRangeInternal(new[]
        {
            startTimeTagInfo = new TimeTagsInfo(TextIndex.IndexState.Start)
            {
                X = 18,
                Anchor = Anchor.BottomLeft,
                Origin = Anchor.CentreRight,
                Alpha = GetAlpha(state),
            },
            endTimeTagInfo = new TimeTagsInfo(TextIndex.IndexState.End)
            {
                X = -18,
                Anchor = Anchor.BottomRight,
                Origin = Anchor.CentreLeft,
                Alpha = GetAlpha(state),
            },
        });
    }

    protected override void ApplyCaretPosition(CreateRemoveTimeTagCaretPosition caret)
    {
        var rect = LyricPositionProvider.GetRectByCharIndex(caret.CharIndex);

        Position = rect.TopLeft - new Vector2(border_spacing);
        Size = rect.Size + new Vector2(border_spacing * 2);

        startTimeTagInfo?.UpdateCaretPosition(caret);
        endTimeTagInfo?.UpdateCaretPosition(caret);
    }

    protected override void TriggerDisallowEditEffect(OsuColour colour)
    {
        this.FlashColour(colour.Red, 200);
    }

    /// <summary>
    /// List of time-tags info.
    /// Provide the button for able to create and remove the time-tag.
    /// </summary>
    private partial class TimeTagsInfo : CompositeDrawable
    {
        private const int border_radius = 5;

        private Container createButtonArea = null!;
        private FillFlowContainer contents = null!;
        private FillFlowContainer<TimeTagVisualization> drawableTimeTags = null!;

        private readonly TextIndex.IndexState indexState;
        private readonly IBindableList<TimeTag> bindableTimeTags = new BindableList<TimeTag>();

        public TimeTagsInfo(TextIndex.IndexState indexState)
        {
            this.indexState = indexState;

            Masking = true;
            CornerRadius = border_radius;

            AutoSizeAxes = Axes.Both;
        }

        [BackgroundDependencyLoader]
        private void load(LyricEditorColourProvider colourProvider,
                          ILyricEditorState state,
                          ILyricTimeTagsChangeHandler lyricTimeTagsChangeHandler,
                          IEditTimeTagModeState editTimeTagModeState)
        {
            InternalChildren = new Drawable[]
            {
                new Box
                {
                    Name = "Background",
                    RelativeSizeAxes = Axes.Both,
                    Colour = colourProvider.Background2(state.Mode),
                },
                new Container
                {
                    Name = "Background for exist time-tags",
                    RelativeSizeAxes = Axes.Both,
                    Padding = new MarginPadding
                    {
                        Horizontal = 3,
                        Vertical = 5,
                    },
                    Child = new Container
                    {
                        Masking = true,
                        CornerRadius = border_radius,
                        RelativeSizeAxes = Axes.Both,
                        Child = new Box
                        {
                            RelativeSizeAxes = Axes.Both,
                            Colour = colourProvider.Background3(state.Mode),
                        },
                    },
                },
                contents = new FillFlowContainer
                {
                    AutoSizeAxes = Axes.Both,
                    Margin = new MarginPadding(3),
                    Direction = FillDirection.Horizontal,
                    Children = new Drawable[]
                    {
                        createButtonArea = new Container
                        {
                            AutoSizeAxes = Axes.Both,
                            Masking = true,
                            CornerRadius = border_radius,
                            Children = new Drawable[]
                            {
                                new Box
                                {
                                    RelativeSizeAxes = Axes.Both,
                                    Colour = colourProvider.Background5(state.Mode),
                                },
                                new IconButton
                                {
                                    Icon = FontAwesome.Solid.Plus,
                                    Anchor = Anchor.Centre,
                                    Origin = Anchor.Centre,
                                    Margin = new MarginPadding(5),
                                    Size = new Vector2(15),
                                    Action = () =>
                                    {
                                        if (previousCaret == null)
                                            throw new InvalidOperationException();

                                        var textIndex = new TextIndex(previousCaret.Value.CharIndex, indexState);
                                        lyricTimeTagsChangeHandler.AddByPosition(textIndex);
                                        editTimeTagModeState.BindableCreateType.Value = CreateTimeTagType.Mouse;
                                    },
                                },
                            },
                        },
                        drawableTimeTags = new FillFlowContainer<TimeTagVisualization>
                        {
                            Anchor = Anchor.CentreLeft,
                            Origin = Anchor.CentreLeft,
                            AutoSizeAxes = Axes.Both,
                            Spacing = new Vector2(5),
                            Direction = FillDirection.Horizontal,
                        },
                    },
                },
            };

            // update the create button by state.
            float position = TextIndexUtils.GetValueByState(indexState, float.MaxValue, float.MinValue);
            contents.SetLayoutPosition(createButtonArea, position);

            // should update the time-tag visualization if the time-tags are changed.
            bindableTimeTags.BindCollectionChanged((_, _) =>
            {
                redrawTimeTags();
            });
        }

        private CreateRemoveTimeTagCaretPosition? previousCaret;

        public void UpdateCaretPosition(CreateRemoveTimeTagCaretPosition caret)
        {
            if (previousCaret?.Lyric != caret.Lyric)
            {
                // should wait until previous caret is updated.
                Schedule(() =>
                {
                    bindableTimeTags.UnbindBindings();
                    bindableTimeTags.BindTo(caret.Lyric.TimeTagsBindable);
                });
            }

            previousCaret = caret;
            redrawTimeTags();
        }

        private void redrawTimeTags()
        {
            if (previousCaret == null)
                throw new InvalidOperationException();

            var timeTags = previousCaret.Value.GetTimeTagsWithState(indexState);

            drawableTimeTags.Clear();

            for (int i = 0; i < timeTags.Length; i++)
            {
                bool isFirst = i == 0;
                bool isLast = i == timeTags.Length - 1;

                drawableTimeTags.Add(new TimeTagVisualization(timeTags[i])
                {
                    Anchor = Anchor.Centre,
                    Origin = Anchor.Centre,
                    Size = new Vector2(12),
                    Margin = new MarginPadding(5)
                    {
                        Left = isFirst ? 5 : 0,
                        Right = isLast ? 5 : 0,
                    },
                    Alpha = 0.5f,
                });
            }
        }

        /// <summary>
        /// Exist time-tag visualization.
        /// Easy to delete if click on it.
        /// </summary>
        private partial class TimeTagVisualization : CompositeDrawable, IHasTooltip
        {
            [Resolved]
            private ILyricTimeTagsChangeHandler lyricTimeTagsChangeHandler { get; set; } = null!;

            [Resolved]
            private IEditTimeTagModeState editTimeTagModeState { get; set; } = null!;

            private readonly TimeTag timeTag;

            public TimeTagVisualization(TimeTag timeTag)
            {
                this.timeTag = timeTag;

                InternalChildren = new Drawable[]
                {
                    new DrawableTimeTag
                    {
                        Name = "Time tag triangle",
                        Anchor = Anchor.Centre,
                        Origin = Anchor.Centre,
                        RelativeSizeAxes = Axes.Both,
                        TimeTag = timeTag,
                    },
                    // todo: should have delete icon in here?
                };
            }

            protected override bool OnClick(ClickEvent e)
            {
                lyricTimeTagsChangeHandler.Remove(timeTag);
                editTimeTagModeState.BindableCreateType.Value = CreateTimeTagType.Mouse;

                return true;
            }

            public LocalisableString TooltipText => "Click to remove the time-tag.";
        }
    }
}
