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

namespace osu.Game.Rulesets.Karaoke.Edit.Lyrics.Extends.Components
{
    public abstract class LabelledObjectFieldTextBox<T> : LabelledTextBox where T : class
    {
        [Resolved]
        private OsuColour colours { get; set; }

        protected readonly BindableList<T> SelectedItems = new();

        private readonly T item;

        protected LabelledObjectFieldTextBox(T item)
        {
            this.item = item;

            // apply current text from text-tag.
            Component.Text = GetFieldValue(item);

            // should change preview text box if selected ruby/romaji changed.
            OnCommit += (sender, _) =>
            {
                ApplyValue(item, sender.Text);
            };

            // change style if focus.
            SelectedItems.BindCollectionChanged((_, _) =>
            {
                var highLight = SelectedItems.Contains(item);

                Component.BorderColour = highLight ? colours.Yellow : colours.Blue;
                Component.BorderThickness = highLight ? 3 : 0;
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

        protected abstract string GetFieldValue(T item);

        protected abstract void ApplyValue(T item, string value);

        protected override OsuTextBox CreateTextBox() => new ObjectFieldTextBox
        {
            CommitOnFocusLost = true,
            Anchor = Anchor.Centre,
            Origin = Anchor.Centre,
            RelativeSizeAxes = Axes.X,
            CornerRadius = CORNER_RADIUS,
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

        private class ObjectFieldTextBox : OsuTextBox
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
