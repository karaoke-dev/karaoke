// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Extensions.IEnumerableExtensions;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Graphics.Textures;
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

        private Container background;
        private Sprite backgroundHeadSprite;
        private Sprite backgroundBodySprite;
        private Sprite backgroundTailSprite;

        private Container border;
        private Sprite borderHeadSprite;
        private Sprite borderBodySprite;
        private Sprite borderTailSprite;

        [BackgroundDependencyLoader]
        private void load(DrawableHitObject drawableObject, ISkinSource skin, IScrollingInfo scrollingInfo)
        {
            InternalChildren = new[]
            {
                background = new Container
                {
                    RelativeSizeAxes = Axes.Both,
                    Name = "Background layer",
                    Children = new Drawable[]
                    {
                        backgroundHeadSprite = new Sprite
                        {
                            Anchor = Anchor.CentreLeft,
                            Origin = Anchor.Centre,
                            Name = "Background head",
                            Texture = GetTextureFromLookup(skin, LegacyKaraokeSkinConfigurationLookups.NoteHeadImage, LegacyKaraokeSkinNoteLayer.Background)
                        },
                        backgroundBodySprite = new Sprite
                        {
                            Anchor = Anchor.Centre,
                            Origin = Anchor.Centre,
                            Name = "Background body",
                            Size = Vector2.One,
                            FillMode = FillMode.Stretch,
                            RelativeSizeAxes = Axes.X,
                            Texture = GetTextureFromLookup(skin, LegacyKaraokeSkinConfigurationLookups.NoteBodyImage, LegacyKaraokeSkinNoteLayer.Background)
                        },
                        backgroundTailSprite = new Sprite
                        {
                            Anchor = Anchor.CentreRight,
                            Origin = Anchor.Centre,
                            Name = "Background tail",
                            Texture = GetTextureFromLookup(skin, LegacyKaraokeSkinConfigurationLookups.NoteTailImage, LegacyKaraokeSkinNoteLayer.Background)
                        }
                    }
                },
                border = new Container
                {
                    RelativeSizeAxes = Axes.Both,
                    Name = "Border layer",
                    Children = new Drawable[]
                    {
                        borderHeadSprite = new Sprite
                        {
                            Anchor = Anchor.CentreLeft,
                            Origin = Anchor.Centre,
                            Name = "Border head",
                            Texture = GetTextureFromLookup(skin, LegacyKaraokeSkinConfigurationLookups.NoteHeadImage, LegacyKaraokeSkinNoteLayer.Border)
                        },
                        borderBodySprite = new Sprite
                        {
                            Anchor = Anchor.Centre,
                            Origin = Anchor.Centre,
                            Name = "Border body",
                            Size = Vector2.One,
                            FillMode = FillMode.Stretch,
                            RelativeSizeAxes = Axes.X,
                            Texture = GetTextureFromLookup(skin, LegacyKaraokeSkinConfigurationLookups.NoteBodyImage, LegacyKaraokeSkinNoteLayer.Border)
                        },
                        borderTailSprite = new Sprite
                        {
                            Anchor = Anchor.CentreRight,
                            Origin = Anchor.Centre,
                            Name = "Border tail",
                            Texture = GetTextureFromLookup(skin, LegacyKaraokeSkinConfigurationLookups.NoteTailImage, LegacyKaraokeSkinNoteLayer.Border)
                        }
                    }
                },
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
            isHitting.BindValueChanged(_ => onAccentChanged(), true);
            display.BindValueChanged(_ => onAccentChanged(), true);
            styleIndex.BindValueChanged(value => applySkin(skin, value.NewValue), true);
        }

        private void applySkin(ISkinSource skin, int styleIndex)
        {
            if (skin == null)
                return;

            var noteSkin = skin.GetConfig<KaraokeSkinLookup, NoteSkin>(new KaraokeSkinLookup(KaraokeSkinConfiguration.NoteStyle, styleIndex))?.Value;
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
                if (backgroundBodySprite.Texture != null)
                {
                    backgroundBodySprite.Height = getHeight(backgroundBodySprite);
                }

                if (borderBodySprite.Texture != null)
                {
                    borderBodySprite.Height = getHeight(borderBodySprite);
                }

                subtractionCache.Validate();
            }

            float getHeight(Sprite s) => s.Texture?.DisplayHeight ?? 0;
        }

        protected virtual void OnDirectionChanged(ValueChangedEvent<ScrollingDirection> direction)
        {
            if (direction.NewValue == ScrollingDirection.Left)
            {
                InternalChildren.ForEach(x=> Scale = Vector2.One);
            }
            else
            {
                InternalChildren.ForEach(x => Scale = new Vector2(-1, 1));
            }
        }

        protected Texture GetTextureFromLookup(ISkin skin, LegacyKaraokeSkinConfigurationLookups lookup, LegacyKaraokeSkinNoteLayer layer)
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

            string noteImage = $"karaoke-note-{layerSuffix}-{suffix}";

            return skin.GetTexture(noteImage);
        }

        private void onAccentChanged() => onAccentChanged(new ValueChangedEvent<Color4>(AccentColour.Value, AccentColour.Value));

        private void onAccentChanged(ValueChangedEvent<Color4> accent)
        {
            background.Colour = display.Value ? AccentColour.Value : new Color4(23, 41, 46, 255);

            // todo : implement is hitting and hit color

            subtractionCache.Invalidate();
        }
    }
}
