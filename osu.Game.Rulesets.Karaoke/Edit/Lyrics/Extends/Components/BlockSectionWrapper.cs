// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Extensions.Color4Extensions;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Cursor;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Localisation;
using osu.Game.Graphics;
using osu.Game.Graphics.Sprites;
using osuTK;
using osuTK.Graphics;

namespace osu.Game.Rulesets.Karaoke.Edit.Lyrics.Extends.Components
{
    public class BlockSectionWrapper : CompositeDrawable, IHasTooltip
    {
        public BlockSectionWrapper(IconUsage iconUsage, LocalisableString name, LocalisableString description, LocalisableString tooltip)
        {
            TooltipText = tooltip;

            RelativeSizeAxes = Axes.Both;
            Padding = new MarginPadding { Top = 30 };

            InternalChildren = new Drawable[]
            {
                new Container
                {
                    RelativeSizeAxes = Axes.Both,
                    Masking = true,
                    CornerRadius = 15,
                    Child = new Box
                    {
                        RelativeSizeAxes = Axes.Both,
                        Alpha = 0.3f,
                        Colour = Color4.Black
                    },
                },
                new BlockSectionMessage(iconUsage, name, description)
                {
                    Anchor = Anchor.Centre,
                    Origin = Anchor.Centre
                }
            };
        }

        public LocalisableString TooltipText { get; }

        public class BlockSectionMessage : CompositeDrawable
        {
            private FillFlowContainer textContainer { get; }

            private readonly Container boxContainer;

            private const double transition_time = 1000;

            public BlockSectionMessage(IconUsage iconUsage, LocalisableString name, LocalisableString description)
            {
                AutoSizeAxes = Axes.Both;

                Anchor = Anchor.Centre;
                Origin = Anchor.Centre;

                var colour = Color4.Gray;

                InternalChildren = new Drawable[]
                {
                    boxContainer = new Container
                    {
                        CornerRadius = 12,
                        Masking = true,
                        AutoSizeAxes = Axes.Both,
                        Anchor = Anchor.Centre,
                        Origin = Anchor.Centre,
                        Children = new Drawable[]
                        {
                            new Box
                            {
                                RelativeSizeAxes = Axes.Both,

                                Colour = colour.Darken(0.8f),
                                Alpha = 0.8f,
                            },
                            textContainer = new FillFlowContainer
                            {
                                AutoSizeAxes = Axes.Both,
                                Anchor = Anchor.Centre,
                                Origin = Anchor.Centre,
                                Padding = new MarginPadding(20),
                                Direction = FillDirection.Vertical,
                                Children = new Drawable[]
                                {
                                    new SpriteIcon
                                    {
                                        Icon = iconUsage,
                                        Anchor = Anchor.TopCentre,
                                        Origin = Anchor.TopCentre,
                                        Size = new Vector2(32),
                                    },
                                    new OsuSpriteText
                                    {
                                        Anchor = Anchor.TopCentre,
                                        Origin = Anchor.TopCentre,
                                        Text = name,
                                        Colour = colour,
                                        Font = OsuFont.GetFont(size: 18),
                                    },
                                    new OsuSpriteText
                                    {
                                        Anchor = Anchor.TopCentre,
                                        Origin = Anchor.TopCentre,
                                        Text = description,
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

                textContainer.Position = new Vector2(DrawSize.X / 16, 0);

                boxContainer.Hide();
                boxContainer.ScaleTo(0.2f);
                boxContainer.RotateTo(-20);

                using (BeginDelayedSequence(300))
                {
                    boxContainer.ScaleTo(1, transition_time, Easing.OutElastic);
                    boxContainer.RotateTo(0, transition_time / 2, Easing.OutQuint);

                    textContainer.MoveTo(Vector2.Zero, transition_time, Easing.OutExpo);
                    boxContainer.FadeIn(transition_time, Easing.OutExpo);
                }
            }
        }
    }
}
