// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using System.Linq;
using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Input.Events;
using osu.Game.Graphics;
using osu.Game.Graphics.Containers;
using osu.Game.Graphics.UserInterface;
using osu.Game.Graphics.UserInterfaceV2;
using osu.Game.Rulesets.Karaoke.Extensions;
using osuTK.Graphics;

namespace osu.Game.Rulesets.Karaoke.Screens.Edit.Beatmaps.Lyrics.Settings;

public abstract partial class LabelledObjectFieldTextBox<T> : LabelledTextBox where T : class
{
    protected readonly IBindableList<T> SelectedItems = new BindableList<T>();

    protected new ObjectFieldTextBox Component => (ObjectFieldTextBox)base.Component;

    private readonly T item;

    protected LabelledObjectFieldTextBox(T item)
    {
        this.item = item;

        // apply current value from the field in the item.
        Current.Value = GetFieldValue(item);

        // should change preview text box if selected string property changed.
        OnCommit += (sender, newText) =>
        {
            if (!newText)
                return;

            ApplyValue(item, sender.Text);
        };

        // change style if focus.
        SelectedItems.BindCollectionChanged((_, _) =>
        {
            bool highLight = SelectedItems.Contains(item);
            Component.HighLight = highLight;

            if (SelectedItems.Contains(item) && SelectedItems.Count == 1)
                focus();
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

    protected abstract void TriggerSelect(T item);

    protected abstract string GetFieldValue(T item);

    protected abstract void ApplyValue(T item, string value);

    protected override OsuTextBox CreateTextBox() => new ObjectFieldTextBox
    {
        CommitOnFocusLost = true,
        Anchor = Anchor.Centre,
        Origin = Anchor.Centre,
        RelativeSizeAxes = Axes.X,
        CornerRadius = CORNER_RADIUS,
        Selected = selected =>
        {
            if (selected)
                TriggerSelect(item);
        },
    };

    private void focus()
    {
        Schedule(() =>
        {
            var focusedDrawable = GetContainingInputManager().FocusedDrawable;
            if (focusedDrawable == null)
                return;

            // Make sure that view is visible in the scroll container.
            // Give the top spacing larger space to let use able to see the previous item or the description text.
            var parentScrollContainer = this.FindClosestParent<OsuScrollContainer>();
            parentScrollContainer.ScrollIntoViewWithSpacing(this, new MarginPadding { Top = 150, Bottom = 50 });

            if (IsFocused(focusedDrawable))
                return;

            GetContainingFocusManager().ChangeFocus(Component);
        });
    }

    protected virtual bool IsFocused(Drawable focusedDrawable)
        => focusedDrawable == Component;

    protected partial class ObjectFieldTextBox : OsuTextBox
    {
        [Resolved]
        private OsuColour colours { get; set; } = null!;

        public Action<bool>? Selected;

        protected override void OnFocus(FocusEvent e)
        {
            base.OnFocus(e);
            Selected?.Invoke(true);
        }

        protected override void OnFocusLost(FocusLostEvent e)
        {
            base.OnFocusLost(e);

            // should not change the border size because still need to highlight the textarea without focus.
            BorderThickness = 3f;

            // note: should trigger commit event first in the base class.
            Selected?.Invoke(false);
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
