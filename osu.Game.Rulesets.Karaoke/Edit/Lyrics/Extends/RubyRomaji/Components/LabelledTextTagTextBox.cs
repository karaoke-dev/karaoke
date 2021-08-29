// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Input.Events;
using osu.Game.Graphics;
using osu.Game.Graphics.UserInterface;
using osu.Game.Graphics.UserInterfaceV2;
using osu.Game.Rulesets.Karaoke.Edit.Components.UserInterfaceV2;
using osu.Game.Rulesets.Karaoke.Objects.Types;
using osuTK;

namespace osu.Game.Rulesets.Karaoke.Edit.Lyrics.Extends.RubyRomaji.Components
{
    public abstract class LabelledTextTagTextBox<T> : LabelledTextBox where T : ITextTag
    {
        protected const float DELETE_BUTTON_SIZE = 20f;

        [Resolved]
        private OsuColour colours { get; set; }

        protected readonly BindableList<T> SelectedTextTag = new();

        private readonly T textTag;

        public Action OnDeleteButtonClick;

        protected LabelledTextTagTextBox(T textTag)
        {
            this.textTag = textTag;

            // apply current text from text-tag.
            Component.Text = textTag.Text;

            // should change preview text box if selected ruby/romaji changed.
            OnCommit += (sender, newText) =>
            {
                textTag.Text = sender.Text;
            };

            // change style if focus.
            SelectedTextTag.BindCollectionChanged((_, _) =>
            {
                var highLight = SelectedTextTag.Contains(textTag);

                Component.BorderColour = highLight ? colours.Yellow : colours.Blue;
                Component.BorderThickness = highLight ? 3 : 0;
            });

            if (!(InternalChildren[1] is FillFlowContainer fillFlowContainer))
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
                    Action = () => OnDeleteButtonClick?.Invoke(),
                    Hover = hover =>
                    {
                        if (hover)
                        {
                            // trigger selected if hover on delete button.
                            SelectedTextTag.Add(textTag);
                        }
                        else
                        {
                            // do not clear current selected if typing.
                            if (Component.HasFocus)
                                return;

                            SelectedTextTag.Remove(textTag);
                        }
                    }
                }
            });
        }

        protected override void OnFocus(FocusEvent e)
        {
            // do not trigger origin focus event if this drawable has been removed.
            // usually cause by user clicking the delete button.
            if (Parent == null)
                return;

            base.OnFocus(e);
        }

        protected override OsuTextBox CreateTextBox() => new TextTagTextBox
        {
            CommitOnFocusLost = true,
            Anchor = Anchor.Centre,
            Origin = Anchor.Centre,
            RelativeSizeAxes = Axes.X,
            CornerRadius = CORNER_RADIUS,
            Selected = () =>
            {
                // not trigger again if already focus.
                if (SelectedTextTag.Contains(textTag) && SelectedTextTag.Count == 1)
                    return;

                // trigger selected.
                SelectedTextTag.Clear();
                SelectedTextTag.Add(textTag);
            }
        };

        internal class TextTagTextBox : OsuTextBox
        {
            public Action Selected;

            protected override void OnFocus(FocusEvent e)
            {
                Selected?.Invoke();
                base.OnFocus(e);
            }
        }
    }
}
