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
using osu.Game.Rulesets.UI.Scrolling;
using osu.Game.Skinning;
using osuTK;

namespace osu.Game.Rulesets.Karaoke.Skinning
{
    public class LegacyNotePiece : LegacyKaraokeColumnElement
    {
        private readonly IBindable<ScrollingDirection> direction = new Bindable<ScrollingDirection>();
        private readonly LayoutValue subtractionCache = new LayoutValue(Invalidation.DrawSize);

        public LegacyNotePiece()
        {
            Anchor = Anchor.Centre;
            Origin = Anchor.Centre;
            RelativeSizeAxes = Axes.Both;

            AddLayout(subtractionCache);
        }

        private Sprite backgroundHeadSprite;
        private Sprite backgroundBodySprite;
        private Sprite backgroundTailSprite;

        private Sprite borderHeadSprite;
        private Sprite borderBodySprite;
        private Sprite borderTailSprite;

        [BackgroundDependencyLoader]
        private void load(ISkinSource skin, IScrollingInfo scrollingInfo)
        {
            InternalChildren = new[]
            {
                new Container
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
                new Container
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
        }

        protected override void Update()
        {
            base.Update();

            if (!subtractionCache.IsValid && DrawWidth > 0)
            {
                if (backgroundBodySprite.Texture != null)
                {
                    // apply background scale
                    var backgroundBodyScale = (DrawWidth - (getWidth(backgroundHeadSprite) + getWidth(backgroundTailSprite)) / 2)
                        / getWidth(backgroundBodySprite);
                    backgroundBodySprite.Scale = new Vector2(backgroundBodyScale, 1);
                    backgroundBodySprite.X = (getWidth(backgroundHeadSprite) - getWidth(backgroundTailSprite)) / 4;
                }

                if (borderBodySprite.Texture != null)
                {
                    // apply border scale
                    var borderBodyScale = (DrawWidth - (getWidth(borderHeadSprite) + getWidth(borderTailSprite)) / 2)
                        / getWidth(borderBodySprite);
                    borderBodySprite.Scale = new Vector2(borderBodyScale, 1);
                    borderBodySprite.X = (getWidth(borderHeadSprite) - getWidth(borderTailSprite)) / 4;
                }

                subtractionCache.Validate();
            }

            float getWidth(Sprite s) => s.Texture?.DisplayWidth ?? 0;
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

        protected virtual Texture GetTexture(ISkinSource skin)
            => GetTextureFromLookup(skin, LegacyKaraokeSkinConfigurationLookups.NoteBodyImage, LegacyKaraokeSkinNoteLayer.Border);

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
    }
}
