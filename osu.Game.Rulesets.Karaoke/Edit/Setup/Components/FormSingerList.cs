// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using System.Collections.Specialized;
using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Extensions;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Cursor;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Graphics.UserInterface;
using osu.Framework.Input.Events;
using osu.Framework.Localisation;
using osu.Game.Graphics;
using osu.Game.Graphics.Containers;
using osu.Game.Graphics.Sprites;
using osu.Game.Graphics.UserInterface;
using osu.Game.Graphics.UserInterfaceV2;
using osu.Game.Overlays;
using osu.Game.Rulesets.Karaoke.Beatmaps.Metadatas;
using osu.Game.Rulesets.Karaoke.Beatmaps.Utils;
using osu.Game.Rulesets.Karaoke.Graphics.Cursor;
using osu.Game.Rulesets.Karaoke.Graphics.Drawables;
using osu.Game.Rulesets.Karaoke.Screens.Edit.Beatmaps.Singers.Detail;
using osuTK;

namespace osu.Game.Rulesets.Karaoke.Edit.Setup.Components;

public partial class FormSingerList : CompositeDrawable
{
     public BindableList<Singer> Singers { get; } = new();

    public LocalisableString Caption { get; init; }

    public LocalisableString HintText { get; init; }

    private Box background = null!;
    private FormFieldCaption caption = null!;
    private FillFlowContainer flow = null!;

    [Resolved]
    private OverlayColourProvider colourProvider { get; set; } = null!;

    [BackgroundDependencyLoader]
    private void load()
    {
        RelativeSizeAxes = Axes.X;
        AutoSizeAxes = Axes.Y;

        Masking = true;
        CornerRadius = 5;

        AddSingerButton button;

        InternalChildren = new Drawable[]
        {
            background = new Box
            {
                RelativeSizeAxes = Axes.Both,
                Colour = colourProvider.Background5,
            },
            new FillFlowContainer
            {
                RelativeSizeAxes = Axes.X,
                AutoSizeAxes = Axes.Y,
                Padding = new MarginPadding(9),
                Spacing = new Vector2(7),
                Direction = FillDirection.Vertical,
                Children = new Drawable[]
                {
                    caption = new FormFieldCaption
                    {
                        Caption = Caption,
                        TooltipText = HintText,
                    },
                    flow = new FillFlowContainer
                    {
                        RelativeSizeAxes = Axes.X,
                        AutoSizeAxes = Axes.Y,
                        Direction = FillDirection.Full,
                        Spacing = new Vector2(10),
                        Child = button = new AddSingerButton
                        {
                            Action = singerInsertionRequested,
                        },
                    },
                },
            },
        };

        flow.SetLayoutPosition(button, float.MaxValue);
    }

    protected override void LoadComplete()
    {
        base.LoadComplete();

        Singers.BindCollectionChanged((_, args) =>
        {
            if (args.Action != NotifyCollectionChangedAction.Replace)
                updateSingers();
        }, true);
        updateState();
    }

    protected override bool OnHover(HoverEvent e)
    {
        updateState();
        return true;
    }

    protected override void OnHoverLost(HoverLostEvent e)
    {
        base.OnHoverLost(e);
        updateState();
    }

    private void updateState()
    {
        background.Colour = colourProvider.Background5;
        caption.Colour = colourProvider.Content2;

        BorderThickness = IsHovered ? 2 : 0;

        if (IsHovered)
            BorderColour = colourProvider.Light4;
    }

    private void updateSingers()
    {
        flow.RemoveAll(d => d is SingerDisplay, true);

        foreach (var singer in Singers)
        {
            flow.Add(new SingerDisplay
            {
                Current = { Value = singer },
                DeleteRequested = languageDeletionRequested,
            });
        }
    }

    private void singerInsertionRequested()
    {
        var singer = new Singer
        {
            Name = "New singer",
        };
        Singers.Add(singer);
    }

    private void languageDeletionRequested(Singer singer) => Singers.Remove(singer);

    /// <summary>
    /// A component which displays a singer along with related description text.
    /// </summary>
    private partial class SingerDisplay : CompositeDrawable, IHasCurrentValue<Singer>, IHasContextMenu, IHasPopover
    {
        /// <summary>
        /// Invoked when the user has requested the singer corresponding to this <see cref="SingerDisplay"/>.<br/>
        /// to be removed from its palette.
        /// </summary>
        public Action<Singer>? DeleteRequested;

        private readonly BindableWithCurrent<Singer> current = new();

        private OsuSpriteText singerName = null!;

        public Bindable<Singer> Current
        {
            get => current.Current;
            set => current.Current = value;
        }

        [BackgroundDependencyLoader]
        private void load()
        {
            AutoSizeAxes = Axes.Y;
            Width = 50;

            InternalChild = new FillFlowContainer
            {
                RelativeSizeAxes = Axes.X,
                AutoSizeAxes = Axes.Y,
                Direction = FillDirection.Vertical,
                Spacing = new Vector2(0, 10),
                Children = new Drawable[]
                {
                    new SingerCircle
                    {
                        Current = { BindTarget = Current },
                    },
                    singerName = new OsuSpriteText
                    {
                        Anchor = Anchor.TopCentre,
                        Origin = Anchor.TopCentre,
                    },
                },
            };

            Current.BindValueChanged(singer => singerName.Text = singer.NewValue?.Name ?? "unknown singer", true);
        }

        protected override bool OnClick(ClickEvent e)
        {
            this.ShowPopover();
            return base.OnClick(e);
        }

        public MenuItem[] ContextMenuItems => new MenuItem[]
        {
            new OsuMenuItem("Edit singer info", MenuItemType.Standard, this.ShowPopover),
            new OsuMenuItem("Delete", MenuItemType.Destructive, () =>
            {
                DeleteRequested?.Invoke(Current.Value);
            }),
        };

        public Popover GetPopover() => new SingerEditPopover(Current.Value);

        private partial class SingerCircle : Container, IHasCustomTooltip<Singer>
        {
            public Bindable<Singer> Current { get; } = new();

            private readonly DrawableSingerAvatar singerAvatar;

            public SingerCircle()
            {
                RelativeSizeAxes = Axes.X;
                Height = 50;

                CornerRadius = 25;
                Masking = true;
                BorderThickness = 5;

                Children = new Drawable[]
                {
                    singerAvatar = new DrawableSingerAvatar
                    {
                        Anchor = Anchor.Centre,
                        Origin = Anchor.Centre,
                        RelativeSizeAxes = Axes.Both,
                    },
                };
            }

            protected override void LoadComplete()
            {
                base.LoadComplete();

                Current.BindValueChanged(_ => updateSinger(), true);
            }

            private void updateSinger()
            {
                BorderColour = SingerUtils.GetContentColour(Current.Value);
                singerAvatar.Singer = Current.Value;
            }

            public ITooltip<Singer> GetCustomTooltip() => new SingerToolTip();

            public Singer TooltipContent => Current.Value;
        }
    }

    internal partial class AddSingerButton : CompositeDrawable
    {
        public Action Action
        {
            set => circularButton.Action = value;
        }

        private readonly OsuClickableContainer circularButton;

        public AddSingerButton()
        {
            AutoSizeAxes = Axes.Y;
            Width = 50;

            InternalChild = new FillFlowContainer
            {
                RelativeSizeAxes = Axes.X,
                AutoSizeAxes = Axes.Y,
                Direction = FillDirection.Vertical,
                Spacing = new Vector2(0, 10),
                Children = new Drawable[]
                {
                    circularButton = new OsuClickableContainer
                    {
                        RelativeSizeAxes = Axes.X,
                        Height = 50,
                        CornerRadius = 25,
                        Masking = true,
                        BorderThickness = 5,
                        Children = new Drawable[]
                        {
                            new Box
                            {
                                RelativeSizeAxes = Axes.Both,
                                Colour = Colour4.Transparent,
                                AlwaysPresent = true,
                            },
                            new SpriteIcon
                            {
                                Anchor = Anchor.Centre,
                                Origin = Anchor.Centre,
                                Size = new Vector2(20),
                                Icon = FontAwesome.Solid.Plus,
                            },
                        },
                    },
                    new OsuSpriteText
                    {
                        Anchor = Anchor.TopCentre,
                        Origin = Anchor.TopCentre,
                        Text = "New",
                    },
                },
            };
        }

        [BackgroundDependencyLoader]
        private void load(OsuColour colours)
        {
            circularButton.BorderColour = colours.BlueDarker;
        }
    }
}
