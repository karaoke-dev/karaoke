﻿// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using System.Collections.Generic;
using System.Linq;
using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Extensions.Color4Extensions;
using osu.Framework.Extensions.IEnumerableExtensions;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Animations;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Layout;
using osu.Game.Rulesets.Karaoke.Objects.Drawables;
using osu.Game.Rulesets.Karaoke.Skinning.Components;
using osu.Game.Rulesets.Objects.Drawables;
using osu.Game.Rulesets.UI.Scrolling;
using osu.Game.Skinning;
using osuTK;
using osuTK.Graphics;

namespace osu.Game.Rulesets.Karaoke.Skinning
{
    public class LegacyNotePiece : LegacyKaraokeColumnElement
    {
        protected readonly Bindable<Color4> AccentColour = new Bindable<Color4>();
        protected readonly Bindable<Color4> HitColour = new Bindable<Color4>();

        private readonly IBindable<ScrollingDirection> direction = new Bindable<ScrollingDirection>();
        private readonly LayoutValue subtractionCache = new LayoutValue(Invalidation.DrawSize);
        private readonly IBindable<bool> isHitting = new Bindable<bool>();
        private readonly IBindable<bool> display = new Bindable<bool>();
        private readonly IBindable<int> styleIndex = new Bindable<int>();

        public LegacyNotePiece()
        {
            Anchor = Anchor.Centre;
            Origin = Anchor.Centre;
            RelativeSizeAxes = Axes.Both;

            AddLayout(subtractionCache);
        }

        private LayerContainer background;
        private LayerContainer foreground;
        private LayerContainer border;

        [BackgroundDependencyLoader]
        private void load(DrawableHitObject drawableObject, ISkinSource skin, IScrollingInfo scrollingInfo)
        {
            InternalChildren = new[]
            {
                background = createLayer("Background layer", skin, LegacyKaraokeSkinNoteLayer.Background),
                foreground = createLayer("Foreground layer", skin, LegacyKaraokeSkinNoteLayer.Foreground),
                border = createLayer("Border layer", skin, LegacyKaraokeSkinNoteLayer.Border)
            };

            direction.BindTo(scrollingInfo.Direction);
            direction.BindValueChanged(OnDirectionChanged, true);

            if (drawableObject != null)
            {
                var holdNote = (DrawableNote)drawableObject;

                isHitting.BindTo(holdNote.IsHitting);
                display.BindTo(holdNote.Display);
                styleIndex.BindTo(holdNote.StyleIndex);
            }

            AccentColour.BindValueChanged(onAccentChanged);
            HitColour.BindValueChanged(onAccentChanged);
            isHitting.BindValueChanged(onIsHittingChanged, true);
            display.BindValueChanged(_ => onAccentChanged(), true);
            styleIndex.BindValueChanged(value => applySkin(skin, value.NewValue), true);
        }

        private void onIsHittingChanged(ValueChangedEvent<bool> isHitting)
        {
            // Update animate
            InternalChildren.OfType<LayerContainer>().ForEach(x =>
            {
                x.Reset();
                x.IsPlaying = isHitting.NewValue;
            });

            // Foreground sparkle
            foreground.ClearTransforms(false, nameof(foreground.Colour));
            foreground.Alpha = 0;
            if (isHitting.NewValue)
            {
                foreground.Alpha = 1;

                const float animation_length = 50;

                // wait for the next sync point
                double synchronisedOffset = animation_length * 2 - Time.Current % (animation_length * 2);
                using (foreground.BeginDelayedSequence(synchronisedOffset))
                    foreground.FadeColour(AccentColour.Value.Lighten(0.7f), animation_length).Then().FadeColour(foreground.Colour, animation_length).Loop();
            }
        }

        private void applySkin(ISkinSource skin, int styleIndex)
        {
            var noteSkin = skin?.GetConfig<KaraokeSkinLookup, NoteSkin>(new KaraokeSkinLookup(KaraokeSkinConfiguration.NoteStyle, styleIndex))?.Value;
            if (noteSkin == null)
                return;

            AccentColour.Value = noteSkin.NoteColor;
            HitColour.Value = noteSkin.BlinkColor;
        }

        protected override void Update()
        {
            base.Update();

            if (!subtractionCache.IsValid && DrawWidth > 0)
            {
                // TODO : maybe do something
                subtractionCache.Validate();
            }
        }

        protected virtual void OnDirectionChanged(ValueChangedEvent<ScrollingDirection> direction)
        {
            if (direction.NewValue == ScrollingDirection.Left)
            {
                InternalChildren.ForEach(x => Scale = Vector2.One);
            }
            else
            {
                InternalChildren.ForEach(x => Scale = new Vector2(-1, 1));
            }
        }

        private LayerContainer createLayer(string name, ISkin skin, LegacyKaraokeSkinNoteLayer layer)
        {
            return new LayerContainer
            {
                RelativeSizeAxes = Axes.Both,
                Name = name,
                Children = new[]
                {
                    GetSpriteFromLookup(skin, LegacyKaraokeSkinConfigurationLookups.NoteHeadImage, layer).With(d =>
                    {
                        if (d == null)
                            return;

                        d.Name = "Head";
                        d.Anchor = Anchor.CentreLeft;
                        d.Origin = Anchor.Centre;
                    }),
                    GetSpriteFromLookup(skin, LegacyKaraokeSkinConfigurationLookups.NoteBodyImage, layer).With(d =>
                    {
                        if (d == null)
                            return;

                        d.Name = "Body";
                        d.Anchor = Anchor.Centre;
                        d.Origin = Anchor.Centre;
                        d.Size = Vector2.One;
                        d.FillMode = FillMode.Stretch;
                        d.RelativeSizeAxes = Axes.X;
                        d.Depth = 1;

                        d.Height = d.Texture?.DisplayHeight ?? 0;
                    }),
                    GetSpriteFromLookup(skin, LegacyKaraokeSkinConfigurationLookups.NoteTailImage, layer).With(d =>
                    {
                        if (d == null)
                            return;

                        d.Name = "Tail";
                        d.Anchor = Anchor.CentreRight;
                        d.Origin = Anchor.Centre;
                    }),
                }
            };
        }

        protected Sprite GetSpriteFromLookup(ISkin skin, LegacyKaraokeSkinConfigurationLookups lookup, LegacyKaraokeSkinNoteLayer layer)
        {
            var name = GetTextureNameFromLookup(lookup, layer);

            switch (layer)
            {
                case LegacyKaraokeSkinNoteLayer.Background:
                case LegacyKaraokeSkinNoteLayer.Border:
                    return getSpriteByName(name) ?? new Sprite();

                case LegacyKaraokeSkinNoteLayer.Foreground:
                    return getSpriteByName(name)
                           ?? getSpriteByName(GetTextureNameFromLookup(lookup, LegacyKaraokeSkinNoteLayer.Background))
                           ?? new Sprite();

                default:
                    return null;
            }

            Sprite getSpriteByName(string name) => (Sprite)skin.GetAnimation(name, true, true).With(d =>
            {
                if (d == null)
                    return;

                if (d is TextureAnimation animation)
                    animation.IsPlaying = false;
            });
        }

        protected string GetTextureNameFromLookup(LegacyKaraokeSkinConfigurationLookups lookup, LegacyKaraokeSkinNoteLayer layer)
        {
            string suffix;

            switch (lookup)
            {
                case LegacyKaraokeSkinConfigurationLookups.NoteBodyImage:
                    suffix = "body";
                    break;

                case LegacyKaraokeSkinConfigurationLookups.NoteHeadImage:
                    suffix = "head";
                    break;

                case LegacyKaraokeSkinConfigurationLookups.NoteTailImage:
                    suffix = "tail";
                    break;

                default:
                    throw new ArgumentOutOfRangeException($"{nameof(lookup)} should be body, head or tail.");
            }

            string layerSuffix = string.Empty;

            switch (layer)
            {
                case LegacyKaraokeSkinNoteLayer.Border:
                    layerSuffix = "border";
                    break;

                case LegacyKaraokeSkinNoteLayer.Background:
                    layerSuffix = "background";
                    break;
            }

            return $"karaoke-note-{layerSuffix}-{suffix}";
        }

        private void onAccentChanged() => onAccentChanged(new ValueChangedEvent<Color4>(AccentColour.Value, AccentColour.Value));

        private void onAccentChanged(ValueChangedEvent<Color4> accent)
        {
            foreground.Colour = HitColour.Value;
            background.Colour = display.Value ? accent.NewValue : new Color4(23, 41, 46, 255);

            subtractionCache.Invalidate();
        }

        private class LayerContainer : Container
        {
            public IEnumerable<TextureAnimation> AnimateChildren => Children.OfType<TextureAnimation>();

            public bool IsPlaying
            {
                set => AnimateChildren.ForEach(d => d.IsPlaying = value);
            }

            public void Reset() => AnimateChildren.ForEach(d => d.GotoFrame(0));
        }
    }
}
