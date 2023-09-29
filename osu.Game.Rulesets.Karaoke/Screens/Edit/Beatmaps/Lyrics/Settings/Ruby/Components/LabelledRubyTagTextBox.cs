// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using System.Diagnostics;
using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Input.Bindings;
using osu.Framework.Input.Events;
using osu.Game.Graphics;
using osu.Game.Graphics.UserInterface;
using osu.Game.Rulesets.Karaoke.Edit.ChangeHandlers.Lyrics;
using osu.Game.Rulesets.Karaoke.Objects;
using osu.Game.Rulesets.Karaoke.Objects.Utils;
using osu.Game.Rulesets.Karaoke.Screens.Edit.Beatmaps.Lyrics.States.Modes;
using osu.Game.Rulesets.Karaoke.Screens.Edit.Components.UserInterface;
using osuTK;

namespace osu.Game.Rulesets.Karaoke.Screens.Edit.Beatmaps.Lyrics.Settings.Ruby.Components;

public partial class LabelledRubyTagTextBox : LabelledObjectFieldTextBox<RubyTag>
{
    protected const float DELETE_BUTTON_SIZE = 20f;

    [Resolved]
    private ILyricRubyTagsChangeHandler rubyTagsChangeHandler { get; set; } = null!;

    [Resolved]
    private IEditRubyModeState editRubyModeState { get; set; } = null!;

    private readonly IndexShiftingPart indexShiftingPart;

    public LabelledRubyTagTextBox(Lyric lyric, RubyTag rubyTag)
        : base(rubyTag)
    {
        Debug.Assert(lyric.RubyTags.Contains(rubyTag));

        if (InternalChildren[1] is not FillFlowContainer fillFlowContainer)
            throw new ArgumentNullException(nameof(fillFlowContainer));

        // change padding to place delete button.
        fillFlowContainer.Padding = new MarginPadding
        {
            Horizontal = CONTENT_PADDING_HORIZONTAL,
            Vertical = CONTENT_PADDING_VERTICAL,
            Right = CONTENT_PADDING_HORIZONTAL + DELETE_BUTTON_SIZE + CONTENT_PADDING_HORIZONTAL,
        };

        // add delete button.
        AddInternal(new Container
        {
            RelativeSizeAxes = Axes.X,
            AutoSizeAxes = Axes.Y,
            Padding = new MarginPadding
            {
                Top = CONTENT_PADDING_VERTICAL + 10,
                Right = CONTENT_PADDING_HORIZONTAL,
            },
            Child = new DeleteIconButton
            {
                Anchor = Anchor.TopRight,
                Origin = Anchor.TopRight,
                Size = new Vector2(DELETE_BUTTON_SIZE),
                Action = () => removeRubyTag(rubyTag),
                Hover = hover =>
                {
                    if (hover)
                    {
                        // trigger selected if hover on delete button.
                        TriggerSelect(rubyTag);
                    }
                },
            },
        });

        // add the index shifting component at the bottom of the text box.
        AddInternal(new Container
        {
            RelativeSizeAxes = Axes.X,
            AutoSizeAxes = Axes.Y,
            Padding = new MarginPadding
            {
                Top = CONTENT_PADDING_VERTICAL + 45,
                Right = CONTENT_PADDING_HORIZONTAL + DELETE_BUTTON_SIZE + CONTENT_PADDING_HORIZONTAL,
            },
            Child = indexShiftingPart = new IndexShiftingPart
            {
                Anchor = Anchor.TopRight,
                Origin = Anchor.TopRight,
                Width = 120,
                Selected = selected =>
                {
                    if (selected)
                        TriggerSelect(rubyTag);
                },
                Action = (indexType, action) =>
                {
                    int index = getNewIndex(rubyTag, indexType);
                    int newIndex = calculateNewIndex(index, action);
                    if (TextTagUtils.OutOfRange(lyric.Text, newIndex))
                        return;

                    switch (indexType)
                    {
                        case AdjustIndex.Start:
                            if (TextTagUtils.ValidNewStartIndex(rubyTag, newIndex))
                                setIndex(rubyTag, newIndex, null);
                            break;

                        case AdjustIndex.End:
                            if (TextTagUtils.ValidNewEndIndex(rubyTag, newIndex))
                                setIndex(rubyTag, null, newIndex);
                            break;

                        default:
                            throw new InvalidOperationException();
                    }

                    static int getNewIndex(RubyTag rubyTag, AdjustIndex index) =>
                        index switch
                        {
                            AdjustIndex.Start => rubyTag.StartIndex,
                            AdjustIndex.End => rubyTag.EndIndex,
                            _ => throw new InvalidOperationException(),
                        };

                    static int calculateNewIndex(int index, AdjustAction action) =>
                        action switch
                        {
                            AdjustAction.Decrease => index - 1,
                            AdjustAction.Increase => index + 1,
                            _ => throw new InvalidOperationException(),
                        };
                },
            },
        });
    }

    private void setIndex(RubyTag item, int? startIndex, int? endIndex)
        => rubyTagsChangeHandler.SetIndex(item, startIndex, endIndex);

    private void removeRubyTag(RubyTag rubyTag)
        => rubyTagsChangeHandler.Remove(rubyTag);

    [BackgroundDependencyLoader]
    private void load()
    {
        SelectedItems.BindTo(editRubyModeState.SelectedItems);
    }

    protected sealed override string GetFieldValue(RubyTag item)
        => item.Text;

    protected override void TriggerSelect(RubyTag item)
        => editRubyModeState.Select(item);

    protected override void ApplyValue(RubyTag item, string value)
        => rubyTagsChangeHandler.SetText(item, value);

    protected override bool IsFocused(Drawable focusedDrawable)
        => base.IsFocused(focusedDrawable) || focusedDrawable == indexShiftingPart;

    public new CompositeDrawable TabbableContentContainer
    {
        set
        {
            base.TabbableContentContainer = value;
            indexShiftingPart.TabbableContentContainer = value;
        }
    }

    private partial class IndexShiftingPart : TabbableContainer, IKeyBindingHandler<KaraokeEditAction>
    {
        private const int button_size = 20;
        private const int button_spacing = 5;

        public override bool AcceptsFocus => true;

        public Action<AdjustIndex, AdjustAction>? Action;

        private readonly Box background;
        private readonly IconButton reduceStartIndexButton;
        private readonly IconButton increaseStartIndexButton;
        private readonly IconButton reduceEndIndexButton;
        private readonly IconButton increaseEndIndexButton;

        public Action<bool>? Selected;

        public IndexShiftingPart()
        {
            AutoSizeAxes = Axes.Y;
            Masking = true;
            CornerRadius = 5;

            Children = new Drawable[]
            {
                background = new Box
                {
                    RelativeSizeAxes = Axes.Both,
                    Alpha = 0,
                },
                new Container
                {
                    RelativeSizeAxes = Axes.X,
                    AutoSizeAxes = Axes.Y,
                    Padding = new MarginPadding(5),
                    Children = new[]
                    {
                        reduceStartIndexButton = new IconButton
                        {
                            Size = new Vector2(button_size),
                            Anchor = Anchor.CentreLeft,
                            Origin = Anchor.CentreLeft,
                            Icon = FontAwesome.Regular.CaretSquareLeft,
                            Action = () => Action?.Invoke(AdjustIndex.Start, AdjustAction.Decrease),
                        },
                        increaseStartIndexButton = new IconButton
                        {
                            Size = new Vector2(button_size),
                            Anchor = Anchor.CentreLeft,
                            Origin = Anchor.CentreLeft,
                            X = button_size + button_spacing,
                            Icon = FontAwesome.Regular.CaretSquareRight,
                            Action = () => Action?.Invoke(AdjustIndex.Start, AdjustAction.Increase),
                        },
                        reduceEndIndexButton = new IconButton
                        {
                            Size = new Vector2(button_size),
                            Anchor = Anchor.CentreRight,
                            Origin = Anchor.CentreRight,
                            X = -button_size - button_spacing,
                            Icon = FontAwesome.Regular.CaretSquareLeft,
                            Action = () => Action?.Invoke(AdjustIndex.End, AdjustAction.Decrease),
                        },
                        increaseEndIndexButton = new IconButton
                        {
                            Size = new Vector2(button_size),
                            Anchor = Anchor.CentreRight,
                            Origin = Anchor.CentreRight,
                            Icon = FontAwesome.Regular.CaretSquareRight,
                            Action = () => Action?.Invoke(AdjustIndex.End, AdjustAction.Increase),
                        },
                    },
                },
            };
        }

        [BackgroundDependencyLoader]
        private void load(OsuColour colours)
        {
            background.Colour = colours.Yellow;
        }

        protected override void OnFocus(FocusEvent e)
        {
            background.FadeTo(0.6f, 100);
            Selected?.Invoke(true);
            base.OnFocus(e);
        }

        protected override void OnFocusLost(FocusLostEvent e)
        {
            background.FadeOut(100);
            Selected?.Invoke(false);
            base.OnFocusLost(e);
        }

        public bool OnPressed(KeyBindingPressEvent<KaraokeEditAction> e)
        {
            if (!HasFocus)
                return false;

            return e.Action switch
            {
                KaraokeEditAction.EditRubyTagReduceStartIndex => reduceStartIndexButton.TriggerClick(),
                KaraokeEditAction.EditRubyTagIncreaseStartIndex => increaseStartIndexButton.TriggerClick(),
                KaraokeEditAction.EditRubyTagReduceEndIndex => reduceEndIndexButton.TriggerClick(),
                KaraokeEditAction.EditRubyTagIncreaseEndIndex => increaseEndIndexButton.TriggerClick(),
                _ => false,
            };
        }

        public void OnReleased(KeyBindingReleaseEvent<KaraokeEditAction> e)
        {
        }
    }

    private enum AdjustIndex
    {
        Start,

        End,
    }

    private enum AdjustAction
    {
        Decrease,

        Increase,
    }
}
