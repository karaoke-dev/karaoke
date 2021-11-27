// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osuTK.Graphics;

namespace osu.Game.Rulesets.Karaoke.Screens.Settings.Previews
{
    public abstract class SettingsSubsectionPreview : Container
    {
        private const double transition_time = 1000;

        private readonly Container boxContainer;
        private readonly Box background;
        private readonly Container content;

        protected override Container<Drawable> Content => content;

        protected virtual Color4 ThemeColor => getColourFor(GetType());

        protected SettingsSubsectionPreview()
        {
            RelativeSizeAxes = Axes.Both;
            Anchor = Anchor.Centre;
            Origin = Anchor.Centre;

            InternalChildren = new Drawable[]
            {
                boxContainer = new Container
                {
                    CornerRadius = 20,
                    Masking = true,
                    RelativeSizeAxes = Axes.Both,
                    Anchor = Anchor.Centre,
                    Origin = Anchor.Centre,
                    Children = new Drawable[]
                    {
                        background = new Box
                        {
                            RelativeSizeAxes = Axes.Both,

                            Colour = ThemeColor,
                            Alpha = 0.2f,
                            Blending = BlendingParameters.Additive,
                        },
                        content = new Container
                        {
                            RelativeSizeAxes = Axes.Both,
                        }
                    }
                },
            };
        }

        protected override void LoadComplete()
        {
            base.LoadComplete();

            boxContainer.Hide();
            boxContainer.ScaleTo(0.2f);
            boxContainer.RotateTo(-20);

            using (BeginDelayedSequence(100))
            {
                boxContainer.ScaleTo(1, transition_time, Easing.OutElastic);
                boxContainer.RotateTo(0, transition_time / 2, Easing.OutQuint);
                boxContainer.FadeIn(transition_time, Easing.OutExpo);
            }
        }

        private bool showBackground;

        protected bool ShowBackground
        {
            get => showBackground;
            set
            {
                showBackground = value;

                if (showBackground)
                {
                    background.Show();
                }
                else
                {
                    background.Hide();
                }
            }
        }

        private static Color4 getColourFor(object type)
        {
            int hash = type.GetHashCode();
            byte r = (byte)Math.Clamp(((hash & 0xFF0000) >> 16) * 0.8f, 20, 255);
            byte g = (byte)Math.Clamp(((hash & 0x00FF00) >> 8) * 0.8f, 20, 255);
            byte b = (byte)Math.Clamp((hash & 0x0000FF) * 0.8f, 20, 255);
            return new Color4(r, g, b, 255);
        }
    }
}
