// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

#nullable disable

using System;
using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Input.Bindings;
using osu.Framework.Input.Events;
using osu.Game.Graphics;
using osu.Game.Graphics.UserInterface;
using osu.Game.Rulesets.Karaoke.Edit.Components.UserInterface;
using osu.Game.Rulesets.Karaoke.Edit.Lyrics.Extends.Components;
using osu.Game.Rulesets.Karaoke.Objects;
using osu.Game.Rulesets.Karaoke.Objects.Types;
using osu.Game.Rulesets.Karaoke.Utils;
using osuTK;

namespace osu.Game.Rulesets.Karaoke.Edit.Lyrics.Extends.RubyRomaji.Components
{
    public abstract class LabelledTextTagTextBox<T> : LabelledObjectFieldTextBox<T> where T : class, ITextTag
    {
        protected const float DELETE_BUTTON_SIZE = 20f;

        private readonly IndexShiftingPart indexShiftingPart;

        protected LabelledTextTagTextBox(Lyric lyric, T textTag)
            : base(textTag)
        {
            if (InternalChildren[1] is not FillFlowContainer fillFlowContainer)
                return;

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
                    Action = () => RemoveTextTag(textTag),
                    Hover = hover =>
                    {
                        if (hover)
                        {
                            // trigger selected if hover on delete button.
                            SelectedItems.Add(textTag);
                        }
                        else
                        {
                            // do not clear current selected if typing.
                            if (Component.HasFocus)
                                return;

                            SelectedItems.Remove(textTag);
                        }
                    }
                }
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
                        {
                            // not trigger again if already focus.
                            if (SelectedItems.Contains(Item) && SelectedItems.Count == 1)
                                return;

                            // trigger selected.
                            SelectedItems.Clear();
                            SelectedItems.Add(Item);
                        }
                        else
                        {
                            SelectedItems.Remove(Item);
                        }
                    },
                    Action = (indexType, action) =>
                    {
                        int index = getNewIndex(textTag, indexType);
                        int newIndex = calculateNewIndex(index, action);
                        if (TextTagUtils.OutOfRange(lyric.Text, newIndex))
                            return;

                        switch (indexType)
                        {
                            case AdjustIndex.Start:
                                if (TextTagUtils.ValidNewStartIndex(textTag, newIndex))
                                    SetIndex(textTag, newIndex, null);
                                break;

                            case AdjustIndex.End:
                                if (TextTagUtils.ValidNewEndIndex(textTag, newIndex))
                                    SetIndex(textTag, null, newIndex);
                                break;

                            default:
                                throw new InvalidOperationException();
                        }

                        static int getNewIndex(T textTag, AdjustIndex index) =>
                            index switch
                            {
                                AdjustIndex.Start => textTag.StartIndex,
                                AdjustIndex.End => textTag.EndIndex,
                                _ => throw new InvalidOperationException()
                            };

                        static int calculateNewIndex(int index, AdjustAction action) =>
                            action switch
                            {
                                AdjustAction.Decrease => index - 1,
                                AdjustAction.Increase => index + 1,
                                _ => throw new InvalidOperationException()
                            };
                    },
                }
            });
        }

        protected sealed override string GetFieldValue(T item)
            => item.Text;

        protected abstract void SetIndex(T item, int? startIndex, int? endIndex);

        protected abstract void RemoveTextTag(T item);

        public new CompositeDrawable TabbableContentContainer
        {
            set
            {
                base.TabbableContentContainer = value;
                indexShiftingPart.TabbableContentContainer = value;
            }
        }

        private class IndexShiftingPart : TabbableContainer, IKeyBindingHandler<KaraokeEditAction>
        {
            private const int button_size = 20;
            private const int button_spacing = 5;

            public override bool AcceptsFocus => true;

            public Action<AdjustIndex, AdjustAction> Action;

            private readonly Box background;
            private readonly IconButton reduceStartIndexButton;
            private readonly IconButton increaseStartIndexButton;
            private readonly IconButton reduceEndIndexButton;
            private readonly IconButton increaseEndIndexButton;

            public Action<bool> Selected;

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
                                Action = () => Action?.Invoke(AdjustIndex.Start, AdjustAction.Decrease)
                            },
                            increaseStartIndexButton = new IconButton
                            {
                                Size = new Vector2(button_size),
                                Anchor = Anchor.CentreLeft,
                                Origin = Anchor.CentreLeft,
                                X = button_size + button_spacing,
                                Icon = FontAwesome.Regular.CaretSquareRight,
                                Action = () => Action?.Invoke(AdjustIndex.Start, AdjustAction.Increase)
                            },
                            reduceEndIndexButton = new IconButton
                            {
                                Size = new Vector2(button_size),
                                Anchor = Anchor.CentreRight,
                                Origin = Anchor.CentreRight,
                                X = -button_size - button_spacing,
                                Icon = FontAwesome.Regular.CaretSquareLeft,
                                Action = () => Action?.Invoke(AdjustIndex.End, AdjustAction.Decrease)
                            },
                            increaseEndIndexButton = new IconButton
                            {
                                Size = new Vector2(button_size),
                                Anchor = Anchor.CentreRight,
                                Origin = Anchor.CentreRight,
                                Icon = FontAwesome.Regular.CaretSquareRight,
                                Action = () => Action?.Invoke(AdjustIndex.End, AdjustAction.Increase)
                            },
                        }
                    }
                };
            }

            [BackgroundDependencyLoader]
            private void load(OsuColour colour)
            {
                background.Colour = colour.Yellow;
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

                switch (e.Action)
                {
                    case KaraokeEditAction.EditTextTagReduceStartIndex:
                        reduceStartIndexButton.TriggerClick();
                        return true;

                    case KaraokeEditAction.EditTextTagIncreaseStartIndex:
                        increaseStartIndexButton.TriggerClick();
                        return true;

                    case KaraokeEditAction.EditTextTagReduceEndIndex:
                        reduceEndIndexButton.TriggerClick();
                        return true;

                    case KaraokeEditAction.EditTextTagIncreaseEndIndex:
                        increaseEndIndexButton.TriggerClick();
                        return true;

                    default:
                        return false;
                }
            }

            public void OnReleased(KeyBindingReleaseEvent<KaraokeEditAction> e)
            {
            }
        }

        private enum AdjustIndex
        {
            Start,

            End
        }

        private enum AdjustAction
        {
            Decrease,

            Increase
        }
    }
}
