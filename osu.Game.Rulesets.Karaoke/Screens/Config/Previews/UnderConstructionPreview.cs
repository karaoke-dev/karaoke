// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using osu.Framework.Extensions.Color4Extensions;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Graphics.Sprites;
using osu.Game.Graphics;
using osu.Game.Graphics.Sprites;
using osuTK;
using osuTK.Graphics;

namespace osu.Game.Rulesets.Karaoke.Screens.Config.Previews
{
    public class UnderConstructionMessage : SettingsSubsectionPreview
    {
        private const double transition_time = 1000;

        public FillFlowContainer TextContainer { get; }

        private readonly Container boxContainer;

        public UnderConstructionMessage(string name)
        {
            RelativeSizeAxes = Axes.Both;
            Size = new Vector2(0.3f);
            Anchor = Anchor.Centre;
            Origin = Anchor.Centre;

            var colour = getColourFor(name);

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
                        new Box
                        {
                            RelativeSizeAxes = Axes.Both,

                            Colour = colour,
                            Alpha = 0.2f,
                            Blending = BlendingParameters.Additive,
                        },
                        TextContainer = new FillFlowContainer
                        {
                            AutoSizeAxes = Axes.Both,
                            Anchor = Anchor.Centre,
                            Origin = Anchor.Centre,
                            Direction = FillDirection.Vertical,
                            Children = new Drawable[]
                            {
                                new SpriteIcon
                                {
                                    Icon = FontAwesome.Solid.UniversalAccess,
                                    Anchor = Anchor.TopCentre,
                                    Origin = Anchor.TopCentre,
                                    Size = new Vector2(50),
                                },
                                new OsuSpriteText
                                {
                                    Anchor = Anchor.TopCentre,
                                    Origin = Anchor.TopCentre,
                                    Text = name,
                                    Colour = colour.Lighten(0.8f),
                                    Font = OsuFont.GetFont(size: 36),
                                },
                                new OsuSpriteText
                                {
                                    Anchor = Anchor.TopCentre,
                                    Origin = Anchor.TopCentre,
                                    Text = "this preview is not yet ready for use!",
                                    Font = OsuFont.GetFont(size: 20),
                                },
                                new OsuSpriteText
                                {
                                    Anchor = Anchor.TopCentre,
                                    Origin = Anchor.TopCentre,
                                    Text = "please check back a bit later.",
                                    Font = OsuFont.GetFont(size: 14),
                                },
                            }
                        },
                    }
                },
            };
        }

        protected override void LoadComplete()
        {
            base.LoadComplete();

            TextContainer.Position = new Vector2(DrawSize.X / 16, 0);

            boxContainer.Hide();
            boxContainer.ScaleTo(0.2f);
            boxContainer.RotateTo(-20);

            using (BeginDelayedSequence(300, true))
            {
                boxContainer.ScaleTo(1, transition_time, Easing.OutElastic);
                boxContainer.RotateTo(0, transition_time / 2, Easing.OutQuint);

                TextContainer.MoveTo(Vector2.Zero, transition_time, Easing.OutExpo);
                boxContainer.FadeIn(transition_time, Easing.OutExpo);
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
