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

namespace osu.Game.Rulesets.Karaoke.Skinning
{
    public class LegacyJudgementLine : LegacyKaraokeElement
    {
        private readonly IBindable<ScrollingDirection> direction = new Bindable<ScrollingDirection>();

        private Container directionContainer;

        public LegacyJudgementLine()
        {
            RelativeSizeAxes = Axes.Both;
        }

        [BackgroundDependencyLoader]
        private void load(ISkinSource skin, IScrollingInfo scrollingInfo)
        {
            string targetImage = GetKaraokeSkinConfig<string>(skin, LegacyKaraokeSkinConfigurationLookups.HitTargetImage)?.Value
                                 ?? "karaoke-stage-hint";

            bool showJudgementLine = GetKaraokeSkinConfig<bool>(skin, LegacyKaraokeSkinConfigurationLookups.ShowJudgementLine)?.Value
                                     ?? true;

            InternalChild = directionContainer = new Container
            {
                Origin = Anchor.TopCentre,
                RelativeSizeAxes = Axes.X,
                AutoSizeAxes = Axes.Y,
                Children = new Drawable[]
                {
                    new Sprite
                    {
                        Texture = skin.GetTexture(targetImage),
                        Scale = new Vector2(1, 0.9f * 1.6025f),
                        RelativeSizeAxes = Axes.Y,
                        Height = 1
                    },
                    new Box
                    {
                        Anchor = Anchor.TopCentre,
                        RelativeSizeAxes = Axes.Y,
                        Width = 1,
                        Alpha = showJudgementLine ? 0.9f : 0
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
                directionContainer.Anchor = Anchor.CentreLeft;
                directionContainer.Scale = new Vector2(-1, 1);
            }
            else
            {
                directionContainer.Anchor = Anchor.CentreRight;
                directionContainer.Scale = Vector2.One;
            }
        }
    }
}
