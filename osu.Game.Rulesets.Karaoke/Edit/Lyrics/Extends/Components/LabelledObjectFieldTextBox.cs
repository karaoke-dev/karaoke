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
using osuTK.Graphics;

namespace osu.Game.Rulesets.Karaoke.Edit.Lyrics.Extends.Components
{
    public abstract class LabelledObjectFieldTextBox<T> : LabelledTextBox where T : class
    {
        protected readonly BindableList<T> SelectedItems = new();

        protected new ObjectFieldTextBox Component => (ObjectFieldTextBox)base.Component;

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
                bool highLight = SelectedItems.Contains(item);
                Component.HighLight = highLight;
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
            Selected = (selected) =>
            {
                if (selected)
                {
                    // not trigger again if already focus.
                    if (SelectedItems.Contains(item) && SelectedItems.Count == 1)
                        return;

                    // trigger selected.
                    SelectedItems.Clear();
                    SelectedItems.Add(item);
                }
                else
                {
                    SelectedItems.Remove(item);
                }
            }
        };

        protected class ObjectFieldTextBox : OsuTextBox
        {
            [Resolved]
            private OsuColour colours { get; set; }

            public Action<bool> Selected;

            protected override void OnFocus(FocusEvent e)
            {
                Selected?.Invoke(true);
                base.OnFocus(e);
            }

            protected override void OnFocusLost(FocusLostEvent e)
            {
                Selected?.Invoke(false);
                base.OnFocusLost(e);
            }

            private Color4 standardBorderColour;

            [BackgroundDependencyLoader]
            private void load()
            {
                standardBorderColour = BorderColour;
            }

            public bool HighLight
            {
                set
                {
                    BorderColour = value ? colours.Yellow : standardBorderColour;
                    BorderThickness = value ? 3 : 0;
                }
            }
        }
    }
}
