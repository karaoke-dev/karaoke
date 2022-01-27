// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using System.Linq;
using JetBrains.Annotations;
using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Graphics;
using osu.Framework.Layout;
using osu.Game.Configuration;
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

        [SettingSource("Flashlight size", "Multiplier applied to the default flashlight size.")]
        public override BindableNumber<float> SizeMultiplier { get; } = new()
        {
            MinValue = 0.5f,
            MaxValue = 3f,
            Default = 1f,
            Value = 1f,
            Precision = 0.1f
        };

        [SettingSource("Change size based on combo", "Decrease the flashlight size as combo increases.")]
        public override BindableBool ComboBasedSize { get; } = new()
        {
            Default = false,
            Value = false
        };

        public override float DefaultFlashlightSize => 50;

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

        protected override Flashlight CreateFlashlight() => new KaraokeFlashlight(this);

        internal class KaraokeFlashlight : Flashlight
        {
            private readonly LayoutValue flashlightProperties = new(Invalidation.DrawSize);

            private readonly IBindable<ScrollingDirection> direction = new Bindable<ScrollingDirection>();
            private readonly IBindable<double> timeRange = new Bindable<double>();

            public KaraokeFlashlight(KaraokeModFlashlight modFlashlight)
                : base(modFlashlight)
            {
                AddLayout(flashlightProperties);
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
                this.TransformTo(nameof(FlashlightSize), new Vector2(DrawWidth, GetSizeFor(e.NewValue)), FLASHLIGHT_FADE_DURATION);
            }

            protected override string FragmentShader => "RectangularFlashlight";
        }
    }
}
