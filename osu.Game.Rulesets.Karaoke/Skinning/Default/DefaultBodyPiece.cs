// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using JetBrains.Annotations;
using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Extensions.Color4Extensions;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Layout;
using osu.Game.Rulesets.Karaoke.Objects.Drawables;
using osu.Game.Rulesets.Karaoke.Skinning.Metadatas.Notes;
using osu.Game.Rulesets.Objects.Drawables;
using osu.Game.Skinning;
using osuTK.Graphics;

namespace osu.Game.Rulesets.Karaoke.Skinning.Default
{
    public class DefaultBodyPiece : Container
    {
        public const float CORNER_RADIUS = 5;

        protected readonly Bindable<Color4> AccentColour = new Bindable<Color4>();
        protected readonly Bindable<Color4> HitColour = new Bindable<Color4>();

        private readonly LayoutValue subtractionCache = new LayoutValue(Invalidation.DrawSize);
        private readonly IBindable<bool> isHitting = new Bindable<bool>();
        private readonly IBindable<bool> display = new Bindable<bool>();
        private readonly IBindable<int[]> singer = new Bindable<int[]>();

        protected Drawable Background { get; private set; }
        protected Drawable Foreground { get; private set; }

        public DefaultBodyPiece()
        {
            CornerRadius = CORNER_RADIUS;
            Masking = true;

            AddLayout(subtractionCache);
        }

        [BackgroundDependencyLoader(true)]
        private void load([CanBeNull] DrawableHitObject drawableObject, ISkinSource skin)
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

            if (drawableObject != null)
            {
                var holdNote = (DrawableNote)drawableObject;

                isHitting.BindTo(holdNote.IsHitting);
                display.BindTo(holdNote.DisplayBindable);
                singer.BindTo(holdNote.SingersBindable);
            }

            AccentColour.BindValueChanged(onAccentChanged);
            HitColour.BindValueChanged(onAccentChanged);
            isHitting.BindValueChanged(_ => onAccentChanged(), true);
            display.BindValueChanged(_ => onAccentChanged(), true);
            singer.BindValueChanged(value => applySingerStyle(skin, value.NewValue), true);
        }

        private void applySingerStyle(ISkinSource skin, int[] singers)
        {
            var noteSkin = skin?.GetConfig<KaraokeSkinLookup, NoteSkin>(new KaraokeSkinLookup(KaraokeSkinConfiguration.NoteStyle, singers))?.Value;
            if (noteSkin == null)
                return;

            AccentColour.Value = noteSkin.NoteColor;
            HitColour.Value = noteSkin.BlinkColor;
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
