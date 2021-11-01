// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Input.Events;
using osu.Game.Graphics;
using osu.Game.Graphics.UserInterfaceV2;

namespace osu.Game.Rulesets.Karaoke.Edit.Lyrics.Extends.Components
{
    public abstract class LabelledObjectFieldSwitchButton<T> : LabelledSwitchButton where T : class
    {
        [Resolved]
        private OsuColour colours { get; set; }

        protected readonly BindableList<T> SelectedItems = new();

        private readonly T item;

        protected LabelledObjectFieldSwitchButton(T item)
        {
            this.item = item;

            if (Component is ObjectFieldSwitchButton objectFieldSwitchButton)
            {
                // apply current text from text-tag.
                objectFieldSwitchButton.Current.Value = GetFieldValue(item);

                // should change preview text box if selected ruby/romaji changed.
                objectFieldSwitchButton.OnCommit += (sender, value) =>
                {
                    ApplyValue(item, value);
                };
            }

            // change style if focus.
            SelectedItems.BindCollectionChanged((_, _) =>
            {
                var highLight = SelectedItems.Contains(item);

                Component.BorderColour = highLight ? colours.Yellow : colours.Blue;
            });

            if (InternalChildren[1] is not FillFlowContainer fillFlowContainer)
                return;

            // change padding to place delete button.
            fillFlowContainer.Padding = new MarginPadding
            {
                Horizontal = CONTENT_PADDING_HORIZONTAL,
                Vertical = CONTENT_PADDING_VERTICAL,
                Right = CONTENT_PADDING_HORIZONTAL + CONTENT_PADDING_HORIZONTAL,
            };
        }

        protected abstract bool GetFieldValue(T item);

        protected abstract void ApplyValue(T item, bool value);

        protected override void OnFocus(FocusEvent e)
        {
            // do not trigger origin focus event if this drawable has been removed.
            // usually cause by user clicking the delete button.
            if (Parent == null)
                return;

            base.OnFocus(e);
        }

        protected override SwitchButton CreateComponent() => new ObjectFieldSwitchButton
        {
            Selected = () =>
            {
                // not trigger again if already focus.
                if (SelectedItems.Contains(item) && SelectedItems.Count == 1)
                    return;

                // trigger selected.
                SelectedItems.Clear();
                SelectedItems.Add(item);
            }
        };

        private class ObjectFieldSwitchButton : SwitchButton
        {
            public Action Selected;

            public Action<SwitchButton, bool> OnCommit;

            protected override bool OnHover(HoverEvent e)
            {
                Selected?.Invoke();
                return base.OnHover(e);
            }

            protected override void OnUserChange(bool value)
            {
                base.OnUserChange(value);
                OnCommit?.Invoke(this, value);
            }
        }
    }
}
