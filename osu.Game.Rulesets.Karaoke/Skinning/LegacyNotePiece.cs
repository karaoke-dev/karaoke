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
        private Container border;

        [BackgroundDependencyLoader]
        private void load(DrawableHitObject drawableObject, ISkinSource skin, IScrollingInfo scrollingInfo)
        {
            InternalChildren = new[]
            {
                background = createLayer("Background layer", skin, LegacyKaraokeSkinNoteLayer.Background),
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
            isHitting.BindValueChanged(_ => onAccentChanged(), true);
            display.BindValueChanged(_ => onAccentChanged(), true);
            styleIndex.BindValueChanged(value => applySkin(skin, value.NewValue), true);
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

        private Container createLayer(string name, ISkin skin, LegacyKaraokeSkinNoteLayer layer)
        {
            Sprite body;
            var c = new Container
            {
                RelativeSizeAxes = Axes.Both,
                Name = name,
                Children = new Drawable[]
                {
                    new Sprite
                    {
                        Anchor = Anchor.CentreLeft,
                        Origin = Anchor.Centre,
                        Name = "Head",
                        Texture = GetTextureFromLookup(skin, LegacyKaraokeSkinConfigurationLookups.NoteHeadImage, layer)
                    },
                    body = new Sprite
                    {
                        Anchor = Anchor.Centre,
                        Origin = Anchor.Centre,
                        Name = "Body",
                        Size = Vector2.One,
                        FillMode = FillMode.Stretch,
                        RelativeSizeAxes = Axes.X,
                        Depth = 1,
                        Texture = GetTextureFromLookup(skin, LegacyKaraokeSkinConfigurationLookups.NoteBodyImage, layer)
                    },
                    new Sprite
                    {
                        Anchor = Anchor.CentreRight,
                        Origin = Anchor.Centre,
                        Name = "Tail",
                        Texture = GetTextureFromLookup(skin, LegacyKaraokeSkinConfigurationLookups.NoteTailImage, layer)
                    }
                }
            };
            body.Height = getHeight(body);
            return c;

            float getHeight(Sprite s) => s.Texture?.DisplayHeight ?? 0;
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
