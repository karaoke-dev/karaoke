// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Extensions.Color4Extensions;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Input.Events;
using osu.Game.Graphics;
using osu.Game.Graphics.Containers;
using osu.Game.Graphics.UserInterface;
using osu.Game.Overlays;
using osuTK;
using osuTK.Graphics;
using osu.Game.Rulesets.Karaoke.Skinning.Components;

namespace osu.Game.Rulesets.Karaoke.Edit.Layout.Components
{
    public class DrawableLayoutListItem : OsuRearrangeableListItem<KaraokeLayout>
    {
        private const float item_height = 35;
        private const float button_width = item_height * 0.75f;

        /// <summary>
        /// Whether the <see cref="KaraokeLayout"/> currently exists inside the <see cref="LayoutManager"/>.
        /// </summary>
        public IBindable<bool> IsCreated => isCreated;

        private readonly Bindable<bool> isCreated = new Bindable<bool>();

        /// <summary>
        /// Creates a new <see cref="KaraokeLayout"/>.
        /// </summary>
        /// <param name="item">The <see cref="KaraokeLayout"/>.</param>
        /// <param name="isCreated">Whether <paramref name="item"/> currently exists inside the <see cref="LayoutManager"/>.</param>
        public DrawableLayoutListItem(KaraokeLayout item, bool isCreated)
            : base(item)
        {
            this.isCreated.Value = isCreated;

            ShowDragHandle.BindTo(this.isCreated);
        }

        protected override Drawable CreateContent() => new ItemContent(Model)
        {
            IsCreated = { BindTarget = isCreated }
        };

        /// <summary>
        /// The main content of the <see cref="DrawableLayoutListItem"/>.
        /// </summary>
        private class ItemContent : CircularContainer
        {
            public readonly Bindable<bool> IsCreated = new Bindable<bool>();

            private readonly KaraokeLayout layout;

            [Resolved(CanBeNull = true)]
            private LayoutManager layoutManager { get; set; }

            private Container textBoxPaddingContainer;
            private ItemTextBox textBox;

            public ItemContent(KaraokeLayout layout)
            {
                this.layout = layout;

                RelativeSizeAxes = Axes.X;
                Height = item_height;
                Masking = true;
            }

            [BackgroundDependencyLoader]
            private void load(OsuColour colours)
            {
                Children = new Drawable[]
                {
                    new DeleteButton(layout)
                    {
                        Anchor = Anchor.CentreRight,
                        Origin = Anchor.CentreRight,
                        IsCreated = { BindTarget = IsCreated },
                        IsTextBoxHovered = v => textBox.ReceivePositionalInputAt(v)
                    },
                    textBoxPaddingContainer = new Container
                    {
                        RelativeSizeAxes = Axes.Both,
                        Padding = new MarginPadding { Right = button_width },
                        Children = new Drawable[]
                        {
                            textBox = new ItemTextBox
                            {
                                RelativeSizeAxes = Axes.Both,
                                Size = Vector2.One,
                                CornerRadius = item_height / 2,
                                PlaceholderText = IsCreated.Value ? string.Empty : "Create a new layout"
                            },
                        }
                    },
                };
            }

            protected override void LoadComplete()
            {
                base.LoadComplete();

                textBox.Current.Value = layout.Name;
                textBox.Current.BindValueChanged(x =>
                {
                    // Update name
                    layoutManager.UpdateLayoutName(layout, x.NewValue);

                    // Create new layout
                    createNewLayout();
                }, true);
                IsCreated.BindValueChanged(created => textBoxPaddingContainer.Padding = new MarginPadding { Right = created.NewValue ? button_width : 0 }, true);
            }

            private void createNewLayout()
            {
                if (IsCreated.Value)
                    return;

                if (string.IsNullOrEmpty(textBox.Current.Value))
                    return;

                // Add the new layout and disable our placeholder. If all text is removed, the placeholder should not show back again.
                layoutManager?.AddLayout(layout);
                textBox.PlaceholderText = string.Empty;

                // When this item changes from placeholder to non-placeholder (via changing containers), its text box will lose focus, so it needs to be re-focused.
                Schedule(() => GetContainingInputManager().ChangeFocus(textBox));

                IsCreated.Value = true;
            }

            private class ItemTextBox : OsuTextBox
            {
                protected override float LeftRightPadding => item_height / 2;

                [BackgroundDependencyLoader]
                private void load(OsuColour colours)
                {
                    BackgroundUnfocused = colours.GreySeafoamDarker.Darken(0.5f);
                    BackgroundFocused = colours.GreySeafoam;
                }
            }

            public class DeleteButton : CompositeDrawable
            {
                public readonly IBindable<bool> IsCreated = new Bindable<bool>();

                public Func<Vector2, bool> IsTextBoxHovered;

                [Resolved(CanBeNull = true)]
                private DialogOverlay dialogOverlay { get; set; }

                [Resolved(CanBeNull = true)]
                private LayoutManager layoutManager { get; set; }

                private readonly KaraokeLayout layout;

                private Drawable fadeContainer;
                private Drawable background;

                public DeleteButton(KaraokeLayout layout)
                {
                    this.layout = layout;
                    RelativeSizeAxes = Axes.Y;

                    Width = button_width + item_height / 2; // add corner radius to cover with fill
                }

                [BackgroundDependencyLoader]
                private void load(OsuColour colours)
                {
                    InternalChild = fadeContainer = new Container
                    {
                        RelativeSizeAxes = Axes.Both,
                        Alpha = 0.1f,
                        Children = new[]
                        {
                            background = new Box
                            {
                                RelativeSizeAxes = Axes.Both,
                                Colour = colours.Red
                            },
                            new SpriteIcon
                            {
                                Anchor = Anchor.CentreRight,
                                Origin = Anchor.Centre,
                                X = -button_width * 0.6f,
                                Size = new Vector2(10),
                                Icon = FontAwesome.Solid.Trash
                            }
                        }
                    };
                }

                protected override void LoadComplete()
                {
                    base.LoadComplete();
                    IsCreated.BindValueChanged(created => Alpha = created.NewValue ? 1 : 0, true);
                }

                public override bool ReceivePositionalInputAt(Vector2 screenSpacePos) => base.ReceivePositionalInputAt(screenSpacePos) && !IsTextBoxHovered(screenSpacePos);

                protected override bool OnHover(HoverEvent e)
                {
                    fadeContainer.FadeTo(1f, 100, Easing.Out);
                    return false;
                }

                protected override void OnHoverLost(HoverLostEvent e)
                {
                    fadeContainer.FadeTo(0.1f, 100);
                }

                protected override bool OnClick(ClickEvent e)
                {
                    background.FlashColour(Color4.White, 150);

                    if (!layoutManager?.IsLayoutModified(layout) ?? false)
                        deleteLayout();
                    else
                        dialogOverlay?.Push(new DeleteLayoutDialog(layout, deleteLayout));

                    return true;
                }

                private void deleteLayout() => layoutManager?.RemoveLayout(layout);
            }
        }
    }
}
