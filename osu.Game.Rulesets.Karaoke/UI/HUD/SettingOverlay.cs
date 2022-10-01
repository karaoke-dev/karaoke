// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

#nullable disable

using System;
using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osu.Game.Graphics.Containers;
using osu.Game.Overlays;
using osuTK;

namespace osu.Game.Rulesets.Karaoke.UI.HUD
{
    /// <summary>
    /// Present setting at right side
    /// </summary>
    public abstract class SettingOverlay : OsuFocusedOverlayContainer
    {
        public const float SETTING_MARGIN = 20;
        public const float SETTING_SPACING = 20;
        public const float TRANSITION_LENGTH = 600;

        protected override bool DimMainContent => false;

        protected override Container<Drawable> Content => content;

        private readonly FillFlowContainer<Drawable> content;

        protected abstract OverlayColourScheme OverlayColourScheme { get; }

        [Cached]
        private readonly OverlayColourProvider colourProvider;

        protected SettingOverlay()
        {
            RelativeSizeAxes = Axes.Y;

            colourProvider = new OverlayColourProvider(OverlayColourScheme);

            InternalChildren = new Drawable[]
            {
                new Box
                {
                    Name = "Background",
                    RelativeSizeAxes = Axes.Both,
                    Colour = colourProvider.Background4,
                },
                new Container
                {
                    RelativeSizeAxes = Axes.Y,
                    AutoSizeAxes = Axes.X,
                    Child = content = new FillFlowContainer<Drawable>
                    {
                        AutoSizeAxes = Axes.Both,
                        Direction = FillDirection.Vertical,
                        Spacing = new Vector2(SETTING_SPACING),
                        Margin = new MarginPadding(SETTING_MARGIN),
                    }
                }
            };
        }

        private OverlayDirection direction;

        public OverlayDirection Direction
        {
            get => direction;
            set
            {
                if (direction == value)
                    return;

                direction = value;

                switch (direction)
                {
                    case OverlayDirection.Left:
                        Anchor = Anchor.CentreLeft;
                        Origin = Anchor.CentreLeft;
                        break;

                    case OverlayDirection.Right:
                        Anchor = Anchor.CentreRight;
                        Origin = Anchor.CentreRight;
                        break;

                    default:
                        throw new ArgumentOutOfRangeException(nameof(direction));
                }

                if (State.Value == Visibility.Hidden)
                {
                    X = getHideXPosition();
                }
            }
        }

        protected override void LoadComplete()
        {
            base.LoadComplete();

            // todo : fix the case that should not affect by other overlay.
            OverlayActivationMode.UnbindAll();

            // Use lazy way to force open overlay
            // Will create ruleset own overlay eventually.
            ((Bindable<OverlayActivation>)OverlayActivationMode).Value = OverlayActivation.All;
        }

        [BackgroundDependencyLoader]
        private void load()
        {
            AutoSizeAxes = Axes.X;
        }

        protected override void PopIn()
        {
            base.PopIn();

            this.MoveToX(0, TRANSITION_LENGTH, Easing.OutQuint);
            this.FadeTo(1, TRANSITION_LENGTH, Easing.OutQuint);
        }

        protected override void PopOut()
        {
            base.PopOut();

            float width = getHideXPosition();
            this.MoveToX(width, TRANSITION_LENGTH, Easing.OutQuint);
            this.FadeTo(0, TRANSITION_LENGTH, Easing.OutQuint);
        }

        private float getHideXPosition() =>
            direction switch
            {
                OverlayDirection.Left => -DrawWidth,
                OverlayDirection.Right => DrawWidth,
                _ => throw new ArgumentOutOfRangeException()
            };

        public SettingButton CreateToggleButton()
            => CreateButton().With(x =>
            {
                x.BackgroundColour = colourProvider.Colour1;
            });

        protected abstract SettingButton CreateButton();
    }

    public enum OverlayDirection
    {
        Left,

        Right
    }
}
