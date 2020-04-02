// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Graphics.Sprites;
using osu.Game.Rulesets.UI.Scrolling;
using osu.Game.Skinning;
using osuTK;
using osuTK.Graphics;

namespace osu.Game.Rulesets.Karaoke.Skinning
{
    public class LegacyColumnBackground : LegacyKaraokeColumnElement
    {
        private readonly IBindable<ScrollingDirection> direction = new Bindable<ScrollingDirection>();

        private Container lightContainer;
        private Sprite light;

        public LegacyColumnBackground()
        {
            RelativeSizeAxes = Axes.Both;
        }

        [BackgroundDependencyLoader]
        private void load(ISkinSource skin, IScrollingInfo scrollingInfo)
        {
            string lightImage = GetKaraokeSkinConfig<string>(skin, LegacyKaraokeSkinConfigurationLookups.LightImage)?.Value
                                ?? "karaoke-stage-light";

            float leftLineWidth = GetKaraokeSkinConfig<float>(skin, LegacyKaraokeSkinConfigurationLookups.LeftLineWidth)
                ?.Value ?? 1;
            float rightLineWidth = GetKaraokeSkinConfig<float>(skin, LegacyKaraokeSkinConfigurationLookups.RightLineWidth)
                ?.Value ?? 1;

            bool hasLeftLine = false;
            bool hasRightLine = false;

            float lightPosition = GetKaraokeSkinConfig<float>(skin,LegacyKaraokeSkinConfigurationLookups.LightPosition)?.Value
                                  ?? 0;

            InternalChildren = new Drawable[]
            {
                new Box
                {
                    RelativeSizeAxes = Axes.Both,
                    Colour = Color4.Black
                },
                new Box
                {
                    RelativeSizeAxes = Axes.X,
                    Height = leftLineWidth,
                    Alpha = hasLeftLine ? 1 : 0
                },
                new Box
                {
                    Anchor = Anchor.BottomRight,
                    Origin = Anchor.BottomRight,
                    RelativeSizeAxes = Axes.X,
                    Height = rightLineWidth,
                    Alpha = hasRightLine ? 1 : 0
                },
                lightContainer = new Container
                {
                    Origin = Anchor.CentreLeft,
                    RelativeSizeAxes = Axes.Both,
                    Padding = new MarginPadding { Left = lightPosition },
                    Child = light = new Sprite
                    {
                        Anchor = Anchor.CentreLeft,
                        Origin = Anchor.CentreLeft,
                        Texture = skin.GetTexture(lightImage),
                        RelativeSizeAxes = Axes.Y,
                        Height = 1,
                        Alpha = 0
                    }
                }
            };

            direction.BindTo(scrollingInfo.Direction);
            direction.BindValueChanged(onDirectionChanged, true);
        }

        private void onDirectionChanged(ValueChangedEvent<ScrollingDirection> direction)
        {
            if (direction.NewValue == ScrollingDirection.Left)
            {
                lightContainer.Anchor = Anchor.CentreLeft;
                lightContainer.Scale = new Vector2(1, -1);
            }
            else
            {
                lightContainer.Anchor = Anchor.CentreRight;
                lightContainer.Scale = Vector2.One;
            }
        }
    }
}
