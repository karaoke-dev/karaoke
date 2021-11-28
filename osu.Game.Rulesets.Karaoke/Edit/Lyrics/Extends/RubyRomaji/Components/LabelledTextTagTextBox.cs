// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Input.Events;
using osu.Game.Rulesets.Karaoke.Edit.Components.UserInterface;
using osu.Game.Rulesets.Karaoke.Edit.Lyrics.Extends.Components;
using osu.Game.Rulesets.Karaoke.Objects.Types;
using osuTK;

namespace osu.Game.Rulesets.Karaoke.Edit.Lyrics.Extends.RubyRomaji.Components
{
    public abstract class LabelledTextTagTextBox<T> : LabelledObjectFieldTextBox<T> where T : class, ITextTag
    {
        protected const float DELETE_BUTTON_SIZE = 20f;

        public Action OnDeleteButtonClick;

        protected LabelledTextTagTextBox(T textTag)
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
                    Action = () => OnDeleteButtonClick?.Invoke(),
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
        }

        protected override string GetFieldValue(T item)
            => item.Text;

        protected override void ApplyValue(T item, string value)
            => item.Text = value;

        protected override void OnFocus(FocusEvent e)
        {
            // do not trigger origin focus event if this drawable has been removed.
            // usually cause by user clicking the delete button.
            if (Parent == null)
                return;

            base.OnFocus(e);
        }
    }
}
