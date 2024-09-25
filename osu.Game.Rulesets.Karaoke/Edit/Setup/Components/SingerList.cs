// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Cursor;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Graphics.UserInterface;
using osu.Game.Graphics;
using osu.Game.Graphics.Containers;
using osu.Game.Graphics.Sprites;
using osu.Game.Graphics.UserInterface;
using osu.Game.Rulesets.Karaoke.Beatmaps.Metadatas;
using osu.Game.Rulesets.Karaoke.Beatmaps.Utils;
using osu.Game.Rulesets.Karaoke.Graphics.Cursor;
using osu.Game.Rulesets.Karaoke.Graphics.Drawables;
using osuTK;

namespace osu.Game.Rulesets.Karaoke.Edit.Setup.Components;

/// <summary>
/// A component which displays a collection of singers in individual <see cref="SingerDisplay"/>s.
/// </summary>
public partial class SingerList : CompositeDrawable
{
    public BindableList<Singer> Singers { get; } = new();

    private string singerNamePrefix = "Singer";

    public string SingerNamePrefix
    {
        get => singerNamePrefix;
        set
        {
            if (singerNamePrefix == value)
                return;

            singerNamePrefix = value;

            if (IsLoaded)
                reindexItems();
        }
    }

    private FillFlowContainer singers = null!;

    private IEnumerable<SingerDisplay> singerDisplays => singers.OfType<SingerDisplay>();

    [BackgroundDependencyLoader]
    private void load()
    {
        RelativeSizeAxes = Axes.X;
        AutoSizeAxes = Axes.Y;
        AutoSizeDuration = fade_duration;
        AutoSizeEasing = Easing.OutQuint;

        InternalChild = singers = new FillFlowContainer
        {
            RelativeSizeAxes = Axes.X,
            AutoSizeAxes = Axes.Y,
            Spacing = new Vector2(10),
            Direction = FillDirection.Full,
        };
    }

    protected override void LoadComplete()
    {
        base.LoadComplete();

        Singers.BindCollectionChanged((_, args) =>
        {
            if (args.Action != NotifyCollectionChangedAction.Replace)
                updateSingers();
        }, true);
        FinishTransforms(true);
    }

    private const int fade_duration = 200;

    private void updateSingers()
    {
        singers.Clear();

        for (int i = 0; i < Singers.Count; ++i)
        {
            // copy to avoid accesses to modified closure.
            int singerIndex = i;
            SingerDisplay display;

            singers.Add(display = new SingerDisplay
            {
                Current = { Value = Singers[singerIndex] },
            });

            // todo : might check does this like works because singer is object.
            display.Current.BindValueChanged(singer => Singers[singerIndex] = singer.NewValue);
            display.DeleteRequested += singerDeletionRequested;
        }

        singers.Add(new AddSingerButton
        {
            // todo : use better way to create singer with right id.
            Action = () => Singers.Add(new Singer
            {
                Name = "New singer",
            }),
        });

        reindexItems();
    }

    // todo : might have dialog to ask should delete singer or not if contains lyric.
    private void singerDeletionRequested(SingerDisplay display) => Singers.RemoveAt(singers.IndexOf(display));

    private void reindexItems()
    {
        int index = 1;

        foreach (var singerDisplay in singerDisplays)
        {
            // todo : might call singer manager to update singer id?
            index += 1;
        }
    }

    /// <summary>
    /// A component which displays a singer along with related description text.
    /// </summary>
    public partial class SingerDisplay : CompositeDrawable, IHasCurrentValue<Singer>
    {
        /// <summary>
        /// Invoked when the user has requested the singer corresponding to this <see cref="SingerDisplay"/>.<br/>
        /// to be removed from its palette.
        /// </summary>
        public event Action<SingerDisplay>? DeleteRequested;

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
            Width = 100;

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
                        DeleteRequested = () => DeleteRequested?.Invoke(this),
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

        private partial class SingerCircle : OsuClickableContainer, IHasContextMenu, IHasCustomTooltip<Singer>
        {
            public Bindable<Singer> Current { get; } = new();

            public Action? DeleteRequested { get; init; }

            private readonly DrawableSingerAvatar singerAvatar;

            public SingerCircle()
            {
                RelativeSizeAxes = Axes.X;
                Height = 100;
                CornerRadius = 50;
                Masking = true;
                BorderThickness = 5;
                Action = () =>
                {
                    // todo: show edit singer dialog.
                };

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

            public MenuItem[] ContextMenuItems => new MenuItem[]
            {
                new OsuMenuItem("Delete", MenuItemType.Destructive, () => DeleteRequested?.Invoke()),
            };

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
            Width = 100;

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
                        Height = 100,
                        CornerRadius = 50,
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
