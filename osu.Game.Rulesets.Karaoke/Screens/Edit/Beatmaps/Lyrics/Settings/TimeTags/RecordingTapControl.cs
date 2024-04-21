// Copyright (c) ppy Pty Ltd <contact@ppy.sh>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Input.Events;
using osu.Game.Graphics.UserInterface;
using osu.Game.Overlays;
using osu.Game.Screens.Edit.Timing;
using osuTK;

namespace osu.Game.Rulesets.Karaoke.Screens.Edit.Beatmaps.Lyrics.Settings.TimeTags;

public partial class RecordingTapControl : CompositeDrawable
{
    private readonly BindableBool isHandlingTapping = new();

    private readonly MetronomeDisplay metronome = null!;

    [BackgroundDependencyLoader]
    private void load(OverlayColourProvider colourProvider)
    {
        RelativeSizeAxes = Axes.X;
        AutoSizeAxes = Axes.Y;

        InternalChildren = new Container
        {
            new Container
            {
                RelativeSizeAxes = Axes.Y,
                Anchor = Anchor.Centre,
                Origin = Anchor.CentreRight,
                Height = 0.98f,
                Width = TapButton.SIZE / 1.3f,
                Masking = true,
                CornerRadius = 15,
                Children = new Drawable[]
                {
                    new InlineButton(FontAwesome.Solid.Stop, Anchor.TopLeft)
                    {
                        BackgroundColour = colourProvider.Background1,
                        RelativeSizeAxes = Axes.Both,
                        Height = 0.49f,
                        Action = reset,
                    },
                    new InlineButton(FontAwesome.Solid.Play, Anchor.BottomLeft)
                    {
                        BackgroundColour = colourProvider.Background1,
                        RelativeSizeAxes = Axes.Both,
                        Height = 0.49f,
                        Anchor = Anchor.BottomLeft,
                        Origin = Anchor.BottomLeft,
                        Action = start,
                    },
                },
            },
            new TapButton
            {
                Anchor = Anchor.Centre,
                Origin = Anchor.Centre,
                IsHandlingTapping = { BindTarget = isHandlingTapping },
            },
        };

        isHandlingTapping.BindValueChanged(handling =>
        {
            metronome.EnableClicking = !handling.NewValue;

            if (handling.NewValue)
                start();
        }, true);
    }

    private void start()
    {
    }

    private void reset()
    {
    }

    private partial class InlineButton : OsuButton
    {
        private readonly IconUsage icon;
        private readonly Anchor anchor;

        private SpriteIcon spriteIcon = null!;

        [Resolved]
        private OverlayColourProvider colourProvider { get; set; } = null!;

        public InlineButton(IconUsage icon, Anchor anchor)
        {
            this.icon = icon;
            this.anchor = anchor;
        }

        protected override void LoadComplete()
        {
            base.LoadComplete();

            Content.CornerRadius = 0;
            Content.Masking = false;

            BackgroundColour = colourProvider.Background2;

            Content.Add(new Container
            {
                RelativeSizeAxes = Axes.Both,
                Padding = new MarginPadding(15),
                Children = new Drawable[]
                {
                    spriteIcon = new SpriteIcon
                    {
                        Icon = icon,
                        Size = new Vector2(22),
                        Anchor = anchor,
                        Origin = anchor,
                        Colour = colourProvider.Background1,
                    },
                },
            });
        }

        protected override bool OnMouseDown(MouseDownEvent e)
        {
            // scale looks bad so don't call base.
            return false;
        }

        protected override bool OnHover(HoverEvent e)
        {
            spriteIcon.FadeColour(colourProvider.Content2, 200, Easing.OutQuint);
            return base.OnHover(e);
        }

        protected override void OnHoverLost(HoverLostEvent e)
        {
            spriteIcon.FadeColour(colourProvider.Background1, 200, Easing.OutQuint);
            base.OnHoverLost(e);
        }
    }
}
