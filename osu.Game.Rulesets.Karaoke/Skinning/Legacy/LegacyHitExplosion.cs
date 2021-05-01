// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Animations;
using osu.Game.Rulesets.UI.Scrolling;
using osu.Game.Skinning;
using osuTK;

namespace osu.Game.Rulesets.Karaoke.Skinning.Legacy
{
    public class LegacyHitExplosion : LegacyKaraokeColumnElement
    {
        private readonly IBindable<ScrollingDirection> direction = new Bindable<ScrollingDirection>();

        private Drawable explosion;

        public LegacyHitExplosion()
        {
            RelativeSizeAxes = Axes.Both;
        }

        [BackgroundDependencyLoader]
        private void load(ISkinSource skin, IScrollingInfo scrollingInfo)
        {
            string imageName = GetKaraokeSkinConfig<string>(skin, LegacyKaraokeSkinConfigurationLookups.ExplosionImage)?.Value
                               ?? GetTextureName();

            float explosionScale = GetKaraokeSkinConfig<float>(skin, LegacyKaraokeSkinConfigurationLookups.ExplosionScale)?.Value
                                   ?? 1;

            // Create a temporary animation to retrieve the number of frames, in an effort to calculate the intended frame length.
            // This animation is discarded and re-queried with the appropriate frame length afterwards.
            var tmp = skin.GetAnimation(imageName, true, false);
            double frameLength = 0;
            if (tmp is IFramedAnimation tmpAnimation && tmpAnimation.FrameCount > 0)
                frameLength = Math.Max(1000 / 60.0, 170.0 / tmpAnimation.FrameCount);

            explosion = skin.GetAnimation(imageName, true, false, frameLength: frameLength).With(d =>
            {
                if (d == null)
                    return;

                d.Origin = Anchor.Centre;
                d.Blending = BlendingParameters.Additive;
                d.Scale = new Vector2(explosionScale);
            });

            if (explosion != null)
                InternalChild = explosion;

            direction.BindTo(scrollingInfo.Direction);
            direction.BindValueChanged(onDirectionChanged, true);
        }

        private void onDirectionChanged(ValueChangedEvent<ScrollingDirection> direction)
        {
            if (explosion != null)
                explosion.Anchor = direction.NewValue == ScrollingDirection.Left ? Anchor.CentreLeft : Anchor.CentreRight;
        }

        protected override void LoadComplete()
        {
            base.LoadComplete();

            explosion?.FadeInFromZero(80)
                     .Then().FadeOut(120);
        }

        public static string GetTextureName() => "karaoke-lighting";
    }
}
