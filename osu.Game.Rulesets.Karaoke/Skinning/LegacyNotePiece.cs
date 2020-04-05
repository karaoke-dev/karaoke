// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Graphics.Textures;
using osu.Game.Rulesets.UI.Scrolling;
using osu.Game.Skinning;
using osuTK;

namespace osu.Game.Rulesets.Karaoke.Skinning
{
    public class LegacyNotePiece : LegacyKaraokeColumnElement
    {
        private readonly IBindable<ScrollingDirection> direction = new Bindable<ScrollingDirection>();

        private Container directionContainer;
        private Sprite noteSprite;

        public LegacyNotePiece()
        {
            Anchor = Anchor.Centre;
            Origin = Anchor.Centre;
            RelativeSizeAxes = Axes.Both;
        }

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
                        new Sprite
                        {
                            Anchor = Anchor.CentreLeft,
                            Origin = Anchor.Centre,
                            Texture = GetTextureFromLookup(skin, LegacyKaraokeSkinConfigurationLookups.NoteHeadImage, LegacyKaraokeSkinNoteLayer.Background)
                        },
                        new Sprite
                        {
                            Anchor = Anchor.Centre,
                            Origin = Anchor.Centre,
                            Texture = GetTextureFromLookup(skin, LegacyKaraokeSkinConfigurationLookups.NoteBodyImage, LegacyKaraokeSkinNoteLayer.Background)
                        },
                        new Sprite
                        {
                            Anchor = Anchor.CentreRight,
                            Origin = Anchor.Centre,
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
                        new Sprite
                        {
                            Anchor = Anchor.CentreLeft,
                            Origin = Anchor.Centre,
                            Texture = GetTextureFromLookup(skin, LegacyKaraokeSkinConfigurationLookups.NoteHeadImage, LegacyKaraokeSkinNoteLayer.Border)
                        },
                        new Sprite
                        {
                            Anchor = Anchor.Centre,
                            Origin = Anchor.Centre,
                            Texture = GetTextureFromLookup(skin, LegacyKaraokeSkinConfigurationLookups.NoteBodyImage, LegacyKaraokeSkinNoteLayer.Border)
                        },
                        new Sprite
                        {
                            Anchor = Anchor.CentreRight,
                            Origin = Anchor.Centre,
                            Texture = GetTextureFromLookup(skin, LegacyKaraokeSkinConfigurationLookups.NoteTailImage, LegacyKaraokeSkinNoteLayer.Border)
                        }
                    }
                },
            };

            directionContainer = new Container
            {
                Anchor = Anchor.Centre,
                Origin = Anchor.Centre,
                RelativeSizeAxes = Axes.Both,
                Child = noteSprite = new Sprite
                {
                    Anchor = Anchor.Centre,
                    Origin = Anchor.Centre,
                    Texture = GetTexture(skin)
                }
            };

            direction.BindTo(scrollingInfo.Direction);
            direction.BindValueChanged(OnDirectionChanged, true);
        }

        protected override void Update()
        {
            base.Update();

            /*
            if (noteSprite.Texture != null)
            {
                var scale = DrawHeight / noteSprite.Texture.DisplayHeight;
                noteSprite.Scale = new Vector2(scale);
            }
            */
        }

        protected virtual void OnDirectionChanged(ValueChangedEvent<ScrollingDirection> direction)
        {
            /*
            if (direction.NewValue == ScrollingDirection.Left)
            {
                directionContainer.Anchor = Anchor.Centre;
                directionContainer.Scale = new Vector2(-1, 1);
            }
            else
            {
                directionContainer.Anchor = Anchor.Centre;
                directionContainer.Scale = Vector2.One;
            }
            */
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
