// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Collections.Generic;
using JetBrains.Annotations;
using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Extensions.Color4Extensions;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Layout;
using osu.Game.Rulesets.Karaoke.Objects.Drawables;
using osu.Game.Rulesets.Karaoke.Skinning.Elements;
using osu.Game.Rulesets.Objects.Drawables;
using osu.Game.Skinning;
using osuTK.Graphics;

namespace osu.Game.Rulesets.Karaoke.Skinning.Default
{
    public class DefaultBodyPiece : Container
    {
        public const float CORNER_RADIUS = 5;

        protected readonly Bindable<Color4> AccentColour = new();
        protected readonly Bindable<Color4> HitColour = new();

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
            singer.BindCollectionChanged((_, _) => applySingerStyle(skin, singer), true);
        }

        private void applySingerStyle(ISkinSource skin, IEnumerable<int> singers)
        {
            var noteSkin = skin?.GetConfig<KaraokeSkinLookup, NoteStyle>(new KaraokeSkinLookup(ElementType.NoteStyle, singers))?.Value;
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
