// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

#nullable disable

using osu.Framework.Allocation;
using osu.Framework.Development;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Graphics.Textures;
using osu.Game.Graphics;
using osu.Game.Graphics.Sprites;
using osu.Game.Rulesets.Karaoke.Utils;
using osuTK;
using osuTK.Graphics;

namespace osu.Game.Rulesets.Karaoke.Screens.Settings
{
    public class KaraokeVersionManager : VisibilityContainer
    {
        [BackgroundDependencyLoader]
        private void load(OsuColour colours, TextureStore textures)
        {
            AutoSizeAxes = Axes.Both;
            Anchor = Anchor.BottomCentre;
            Origin = Anchor.BottomCentre;

            Alpha = 0;

            FillFlowContainer mainFill;

            Children = new Drawable[]
            {
                mainFill = new FillFlowContainer
                {
                    AutoSizeAxes = Axes.Both,
                    Direction = FillDirection.Vertical,
                    Children = new Drawable[]
                    {
                        new FillFlowContainer
                        {
                            AutoSizeAxes = Axes.Both,
                            Direction = FillDirection.Horizontal,
                            Spacing = new Vector2(5),
                            Anchor = Anchor.TopCentre,
                            Origin = Anchor.TopCentre,
                            Children = new Drawable[]
                            {
                                new OsuSpriteText
                                {
                                    Font = OsuFont.GetFont(weight: FontWeight.Bold),
                                    Text = new KaraokeRuleset().ShortName
                                },
                                new OsuSpriteText
                                {
                                    Colour = DebugUtils.IsDebugBuild ? colours.Red : Color4.White,
                                    Text = VersionUtils.DisplayVersion
                                },
                            }
                        },
                    }
                }
            };

            if (!VersionUtils.IsDeployedBuild)
            {
                mainFill.AddRange(new Drawable[]
                {
                    new OsuSpriteText
                    {
                        Anchor = Anchor.TopCentre,
                        Origin = Anchor.TopCentre,
                        Font = OsuFont.Numeric.With(size: 12),
                        Colour = colours.Yellow,
                        Text = @"Development Build"
                    },
                    new Sprite
                    {
                        Anchor = Anchor.TopCentre,
                        Origin = Anchor.TopCentre,
                        Texture = textures.Get(@"Menu/dev-build-footer"),
                    },
                });
            }
        }

        protected override void PopIn()
        {
            this.FadeIn(1400, Easing.OutQuint);
        }

        protected override void PopOut()
        {
            this.FadeOut(500, Easing.OutQuint);
        }
    }
}
