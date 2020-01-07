// Copyright (c) ppy Pty Ltd <contact@ppy.sh>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Linq;
using osu.Framework.Extensions.IEnumerableExtensions;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Graphics.Sprites;
using osu.Game.Graphics.Sprites;
using osu.Game.Overlays.Mods;
using osu.Game.Rulesets.Judgements;
using osu.Game.Rulesets.Mods;
using osu.Game.Rulesets.Scoring;
using osu.Game.Screens.Play;
using osuTK;
using osuTK.Graphics;

namespace osu.Game.Rulesets.Karaoke.Mods
{
    public class KaraokeModWindowsUpdate : ModSuddenDeath, IApplicableToHUD
    {
        public override string Name => "Windows update";
        public override string Acronym => "WD";
        public override IconUsage Icon => FontAwesome.Brands.Windows;
        public override string Description => "Once you missed, windows will upppppdate your osu!";

        private HUDOverlay overlay;
        private WindowsUpdateContainer windowsUpdateContainer;

        public void ApplyToHUD(HUDOverlay overlay)
        {
            this.overlay = overlay;
        }

        protected override bool FailCondition(HealthProcessor healthProcessor, JudgementResult result)
        {
            var displayWindowsUpdateScreen = base.FailCondition(healthProcessor, result);

            if (displayWindowsUpdateScreen && windowsUpdateContainer == null)
            {
                overlay.Add(windowsUpdateContainer = new WindowsUpdateContainer
                {
                    RelativeSizeAxes = Axes.Both
                });
            }

            return false;
        }

        public class WindowsUpdateContainer : Container
        {
            public WindowsUpdateContainer()
            {
                Children = new Drawable[]
                {
                    new Box
                    {
                        Name = "Background",
                        RelativeSizeAxes = Axes.Both,
                        Colour = new Color4(0, 120, 215, 255)
                    },
                    new LoadingIcon
                    {
                        Name = "Loading icon",
                        Anchor = Anchor.Centre,
                        Origin = Anchor.Centre,
                        Colour = Color4.White,
                        Y = -80,
                    },
                    new OsuSpriteText
                    {
                        Name = "Update progress text",
                        Text = "Working on updates 87 % complete.",
                        Anchor = Anchor.Centre,
                        Origin = Anchor.Centre,
                        Scale = new Vector2(1.5f),
                    },
                    new OsuSpriteText
                    {
                        Name = "Take a while text",
                        Text = "Don't turn off your PC. This will take a while.",
                        Anchor = Anchor.Centre,
                        Origin = Anchor.Centre,
                        Scale = new Vector2(1.5f),
                        Y = -25,
                    },
                    new OsuSpriteText
                    {
                        Name = "Restart text",
                        Text = "Your PC will restart several times.",
                        Anchor = Anchor.BottomCentre,
                        Origin = Anchor.Centre,
                        Scale = new Vector2(1.3f),
                        Y = -30,
                    }
                };
            }

            public class LoadingIcon : ModButton
            {
                public LoadingIcon()
                    : base(new KaraokeModWindowsUpdate())
                {
                    // Hide the text on the bottom.
                    Children.OfType<OsuSpriteText>().ForEach(x => x.Hide());
                }
            }
        }
    }
}
