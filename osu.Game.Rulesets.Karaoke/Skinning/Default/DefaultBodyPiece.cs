// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

#nullable disable

using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Extensions.Color4Extensions;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Layout;
using osu.Game.Rulesets.Karaoke.Objects.Drawables;
using osu.Game.Rulesets.Objects.Drawables;
using osuTK.Graphics;

namespace osu.Game.Rulesets.Karaoke.Skinning.Default
{
    public class DefaultBodyPiece : Container
    {
        public const float CORNER_RADIUS = 5;

        public readonly Bindable<Color4> AccentColour = new();
        public readonly Bindable<Color4> HitColour = new();

        private readonly LayoutValue subtractionCache = new(Invalidation.DrawSize);
        private readonly IBindable<bool> isHitting = new Bindable<bool>();
        private readonly IBindable<bool> display = new Bindable<bool>();
        private readonly IBindableList<int> singer = new BindableList<int>();

        protected Drawable Background { get; private set; }
        protected Drawable Foreground { get; private set; }

        public DefaultBodyPiece()
        {
            CornerRadius = CORNER_RADIUS;
            Masking = true;

            AddLayout(subtractionCache);
        }

        [BackgroundDependencyLoader]
        private void load(DrawableHitObject drawableObject)
        {
            InternalChildren = new[]
            {
                Background = new Box
                {
                    RelativeSizeAxes = Axes.Both
                },
                Foreground = new Box
                {
                    RelativeSizeAxes = Axes.Both
                }
            };

            var note = (DrawableNote)drawableObject;

            isHitting.BindTo(note.IsHitting);
            display.BindTo(note.DisplayBindable);
            singer.BindTo(note.SingersBindable);

            AccentColour.BindValueChanged(onAccentChanged);
            HitColour.BindValueChanged(onAccentChanged);
            isHitting.BindValueChanged(_ => onAccentChanged(), true);
            display.BindValueChanged(_ => onAccentChanged(), true);
        }

        private void onAccentChanged() => onAccentChanged(new ValueChangedEvent<Color4>(AccentColour.Value, AccentColour.Value));

        private void onAccentChanged(ValueChangedEvent<Color4> accent)
        {
            Foreground.Colour = HitColour.Value;
            Background.Colour = display.Value ? AccentColour.Value : new Color4(23, 41, 46, 255);

            Foreground.ClearTransforms(false, nameof(Foreground.Colour));
            Foreground.Alpha = 0;

            if (isHitting.Value)
            {
                Foreground.Alpha = 1;

                const float animation_length = 50;

                // wait for the next sync point
                double synchronisedOffset = animation_length * 2 - Time.Current % (animation_length * 2);
                using (Foreground.BeginDelayedSequence(synchronisedOffset))
                    Foreground.FadeColour(accent.NewValue.Lighten(0.7f), animation_length).Then().FadeColour(Foreground.Colour, animation_length).Loop();
            }

            subtractionCache.Invalidate();
        }
    }
}
