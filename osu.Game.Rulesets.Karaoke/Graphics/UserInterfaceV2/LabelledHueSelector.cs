// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

#nullable disable

using System;
using osu.Framework.Bindables;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Effects;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Graphics.UserInterface;
using osu.Game.Graphics.UserInterfaceV2;
using osuTK;

namespace osu.Game.Rulesets.Karaoke.Graphics.UserInterfaceV2
{
    public class LabelledHueSelector : LabelledComponent<LabelledHueSelector.OsuHueSelector, float>
    {
        public LabelledHueSelector()
            : base(true)
        {
        }

        protected override OsuHueSelector CreateComponent()
            => new();

        private static EdgeEffectParameters createShadowParameters() => new()
        {
            Type = EdgeEffectType.Shadow,
            Offset = new Vector2(0, 1),
            Radius = 3,
            Colour = Colour4.Black.Opacity(0.3f)
        };

        /// <summary>
        /// Copied from <see cref="OsuHSVColourPicker"/>
        /// </summary>
        public class OsuHueSelector : HSVColourPicker.HueSelector, IHasCurrentValue<float>
        {
            private const float corner_radius = 10;
            private const float control_border_thickness = 3;

            public Bindable<float> Current
            {
                get => Hue;
                set
                {
                    if (value == null)
                        throw new ArgumentNullException(nameof(value));

                    Hue.UnbindBindings();
                    Hue.BindTo(value);
                }
            }

            public OsuHueSelector()
            {
                SliderBar.CornerRadius = corner_radius;
                SliderBar.Masking = true;
            }

            protected override Drawable CreateSliderNub() => new SliderNub(this);

            private class SliderNub : CompositeDrawable
            {
                private readonly Bindable<float> hue;
                private readonly Box fill;

                public SliderNub(OsuHueSelector osuHueSelector)
                {
                    hue = osuHueSelector.Hue.GetBoundCopy();

                    InternalChild = new CircularContainer
                    {
                        Height = 35,
                        Width = 10,
                        Origin = Anchor.Centre,
                        Anchor = Anchor.Centre,
                        Masking = true,
                        BorderColour = Colour4.White,
                        BorderThickness = control_border_thickness,
                        EdgeEffect = createShadowParameters(),
                        Child = fill = new Box
                        {
                            RelativeSizeAxes = Axes.Both
                        }
                    };
                }

                protected override void LoadComplete()
                {
                    hue.BindValueChanged(h => fill.Colour = Colour4.FromHSV(h.NewValue, 1, 1), true);
                }
            }
        }
    }
}
