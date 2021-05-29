// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Extensions.Color4Extensions;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Sprites;
using osu.Game.Graphics;
using osu.Game.Graphics.Sprites;
using osuTK;

namespace osu.Game.Rulesets.Karaoke.Screens.Config.Previews
{
    public class UnderConstructionMessage : SettingsSubsectionPreview
    {
        private const double transition_time = 1000;

        public FillFlowContainer TextContainer { get; }

        public UnderConstructionMessage(string name)
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
                        Colour = ThemeColor.Lighten(0.8f),
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
            };
        }

        protected override void LoadComplete()
        {
            base.LoadComplete();

            TextContainer.Position = new Vector2(DrawSize.X / 16, 0);

            using (BeginDelayedSequence(100, true))
            {
                TextContainer.MoveTo(Vector2.Zero, transition_time, Easing.OutExpo);
            }
        }
    }
}
