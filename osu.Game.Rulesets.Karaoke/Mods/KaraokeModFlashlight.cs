// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using System.Linq;
using JetBrains.Annotations;
using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Caching;
using osu.Framework.Graphics;
using osu.Game.Rulesets.Karaoke.Objects;
using osu.Game.Rulesets.Karaoke.UI;
using osu.Game.Rulesets.Mods;
using osu.Game.Rulesets.UI;
using osu.Game.Rulesets.UI.Scrolling;
using osuTK;

namespace osu.Game.Rulesets.Karaoke.Mods
{
    public class KaraokeModFlashlight : ModFlashlight<KaraokeHitObject>
    {
        public override double ScoreMultiplier => 1;
        public override Type[] IncompatibleMods => new[] { typeof(ModHidden) };

        public override void ApplyToDrawableRuleset(DrawableRuleset<KaraokeHitObject> drawableRuleset)
        {
            base.ApplyToDrawableRuleset(drawableRuleset);

            var notePlayfield = (drawableRuleset as DrawableKaraokeRuleset)?.Playfield?.NotePlayfield;
            if (notePlayfield == null)
                return;

            var flashlight = drawableRuleset.KeyBindingInputManager.Children.OfType<KaraokeFlashlight>().FirstOrDefault();
            if (flashlight == null)
                return;

            flashlight.RelativeSizeAxes = Axes.X;

            // notePlayfield.Height - 30*2;
            flashlight.Height = 190;
            flashlight.Y = 80;
        }

        public override Flashlight CreateFlashlight() => new KaraokeFlashlight();

        private class KaraokeFlashlight : Flashlight
        {
            private readonly Cached flashlightProperties = new Cached();

            private readonly IBindable<ScrollingDirection> direction = new Bindable<ScrollingDirection>();
            private readonly IBindable<double> timeRange = new Bindable<double>();

            public override bool Invalidate(Invalidation invalidation = Invalidation.All, Drawable source = null, bool shallPropagate = true)
            {
                if ((invalidation & Invalidation.DrawSize) > 0)
                {
                    flashlightProperties.Invalidate();
                }

                return base.Invalidate(invalidation, source, shallPropagate);
            }

            protected override void Update()
            {
                base.Update();

                if (flashlightProperties.IsValid)
                    return;

                FlashlightSize = new Vector2(DrawSize.X * flashLightMultiple, DrawHeight);
                FlashlightPosition = new Vector2(DrawPosition.X, 0) + (scrollingDirection == KaraokeScrollingDirection.Right ? DrawSize : new Vector2());
                flashlightProperties.Validate();
            }

            [BackgroundDependencyLoader(true)]
            private void load([NotNull] IScrollingInfo scrollingInfo)
            {
                direction.BindTo(scrollingInfo.Direction);
                direction.BindValueChanged(OnDirectionChanged, true);

                timeRange.BindTo(scrollingInfo.TimeRange);
                timeRange.BindValueChanged(OnTimeRangeChanged, true);
            }

            private KaraokeScrollingDirection scrollingDirection;

            protected virtual void OnDirectionChanged(ValueChangedEvent<ScrollingDirection> e)
            {
                scrollingDirection = (KaraokeScrollingDirection)e.NewValue;
                flashlightProperties.Invalidate();
            }

            private float flashLightMultiple;

            protected virtual void OnTimeRangeChanged(ValueChangedEvent<double> e)
            {
                flashLightMultiple = 3000 / (float)e.NewValue;
                flashlightProperties.Invalidate();
            }

            protected override void OnComboChange(ValueChangedEvent<int> e)
            {
            }

            protected override string FragmentShader => "RectangularFlashlight";
        }
    }
}
