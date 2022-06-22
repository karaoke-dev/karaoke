// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

#nullable disable

using osu.Framework.Extensions.Color4Extensions;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Sprites;
using osu.Game.Graphics;
using osu.Game.Graphics.Sprites;
using osuTK;

namespace osu.Game.Rulesets.Karaoke.Screens.Settings.Previews
{
    public class DefaultPreview : SettingsSubsectionPreview
    {
        private const double transition_time = 1000;

        public FillFlowContainer TextContainer { get; }

        public DefaultPreview()
        {
            Size = new Vector2(0.3f);

            Child = TextContainer = new FillFlowContainer
            {
                AutoSizeAxes = Axes.Both,
                Anchor = Anchor.Centre,
                Origin = Anchor.Centre,
                Direction = FillDirection.Vertical,
                Children = new Drawable[]
                {
                    new SpriteIcon
                    {
                        Icon = FontAwesome.Solid.Cog,
                        Anchor = Anchor.TopCentre,
                        Origin = Anchor.TopCentre,
                        Size = new Vector2(50),
                    },
                    new OsuSpriteText
                    {
                        Anchor = Anchor.TopCentre,
                        Origin = Anchor.TopCentre,
                        Text = "Welcome to config!",
                        Colour = ThemeColor.Lighten(0.8f),
                        Font = OsuFont.GetFont(size: 32),
                    },
                    new OsuSpriteText
                    {
                        Anchor = Anchor.TopCentre,
                        Origin = Anchor.TopCentre,
                        Text = "Select left size to adjust.",
                        Font = OsuFont.GetFont(size: 20),
                    },
                }
            };
        }

        protected override void LoadComplete()
        {
            base.LoadComplete();

            TextContainer.Position = new Vector2(DrawSize.X / 16, 0);

            using (BeginDelayedSequence(100))
            {
                TextContainer.MoveTo(Vector2.Zero, transition_time, Easing.OutExpo);
            }
        }
    }
}
