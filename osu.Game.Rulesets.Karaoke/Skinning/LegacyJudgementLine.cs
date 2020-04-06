// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Graphics.Textures;
using osu.Game.Rulesets.UI.Scrolling;
using osu.Game.Skinning;
using osuTK;

namespace osu.Game.Rulesets.Karaoke.Skinning
{
    public class LegacyJudgementLine : LegacyKaraokeElement
    {
        public LegacyJudgementLine()
        {
            RelativeSizeAxes = Axes.X;
            Anchor = Anchor.Centre;
            Origin = Anchor.Centre;
        }

        [BackgroundDependencyLoader]
        private void load(ISkinSource skin, IScrollingInfo scrollingInfo)
        {
            InternalChildren = new Drawable[]
            {
                new Sprite
                {
                    Anchor = Anchor.TopCentre,
                    Origin = Anchor.Centre,
                    Name = "Judgement line head",
                    Texture = GetTextureFromLookup(skin, LegacyKaraokeSkinConfigurationLookups.JudgementLineHeadImage)
                },
                new Sprite
                {
                    Anchor = Anchor.Centre,
                    Origin = Anchor.Centre,
                    RelativeSizeAxes = Axes.Y,
                    Name = "Judgement line body",
                    Size = Vector2.One,
                    FillMode = FillMode.Stretch,
                    Texture = GetTextureFromLookup(skin, LegacyKaraokeSkinConfigurationLookups.JudgementLineBodyImage)
                },
                new Sprite
                {
                    Anchor = Anchor.BottomCentre,
                    Origin = Anchor.Centre,
                    Name = "Judgement line tail",
                    Texture = GetTextureFromLookup(skin, LegacyKaraokeSkinConfigurationLookups.JudgementLineTailImage)
                }
            };
        }

        protected Texture GetTextureFromLookup(ISkin skin, LegacyKaraokeSkinConfigurationLookups lookup)
        {
            string suffix;

            switch (lookup)
            {
                case LegacyKaraokeSkinConfigurationLookups.JudgementLineBodyImage:
                    suffix = "body";
                    break;

                case LegacyKaraokeSkinConfigurationLookups.JudgementLineHeadImage:
                    suffix = "head";
                    break;

                case LegacyKaraokeSkinConfigurationLookups.JudgementLineTailImage:
                    suffix = "tail";
                    break;

                default:
                    throw new ArgumentOutOfRangeException($"{nameof(lookup)} should be body, head or tail.");
            }

            string noteImage = $"karaoke-judgement-line-{suffix}";
            return skin.GetTexture(noteImage);
        }
    }
}
