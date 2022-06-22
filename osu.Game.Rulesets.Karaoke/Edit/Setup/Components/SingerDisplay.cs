// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

#nullable disable

using System;
using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Cursor;
using osu.Framework.Graphics.UserInterface;
using osu.Game.Graphics;
using osu.Game.Graphics.Containers;
using osu.Game.Graphics.Sprites;
using osu.Game.Graphics.UserInterface;
using osu.Game.Rulesets.Karaoke.Beatmaps.Metadatas;
using osu.Game.Rulesets.Karaoke.Graphics.Cursor;
using osu.Game.Rulesets.Karaoke.Graphics.Sprites;
using osu.Game.Rulesets.Karaoke.Utils;
using osuTK;

namespace osu.Game.Rulesets.Karaoke.Edit.Setup.Components
{
    /// <summary>
    /// A component which displays a singer along with related description text.
    /// </summary>
    public class SingerDisplay : CompositeDrawable, IHasCurrentValue<Singer>
    {
        /// <summary>
        /// Invoked when the user has requested the singer corresponding to this <see cref="SingerDisplay"/>
        /// to be removed from its palette.
        /// </summary>
        public event Action<SingerDisplay> DeleteRequested;

        private readonly BindableWithCurrent<Singer> current = new();

        private OsuSpriteText singerName;

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
                        DeleteRequested = () => DeleteRequested?.Invoke(this)
                    },
                    singerName = new OsuSpriteText
                    {
                        Anchor = Anchor.TopCentre,
                        Origin = Anchor.TopCentre
                    }
                }
            };

            Current.BindValueChanged(singer => singerName.Text = singer.NewValue?.Name, true);
        }

        private class SingerCircle : OsuClickableContainer, IHasContextMenu, IHasCustomTooltip<Singer>
        {
            public Bindable<Singer> Current { get; } = new();

            public Action DeleteRequested { get; set; }

            private readonly DrawableSingerAvatar singerAvatar;

            [Resolved]
            private OsuColour colours { get; set; }

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
                    }
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
                new OsuMenuItem("Delete", MenuItemType.Destructive, () => DeleteRequested?.Invoke())
            };

            public ITooltip<Singer> GetCustomTooltip() => new SingerToolTip();

            public Singer TooltipContent => Current.Value;
        }
    }
}
