// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Graphics.Textures;
using osu.Framework.Layout;
using osu.Game.Rulesets.UI.Scrolling;
using osu.Game.Skinning;
using osuTK;

namespace osu.Game.Rulesets.Karaoke.Skinning.Legacy
{
    public class LegacyJudgementLine : LegacyKaraokeElement
    {
        private readonly IBindable<ScrollingDirection> direction = new Bindable<ScrollingDirection>();
        private readonly LayoutValue subtractionCache = new LayoutValue(Invalidation.DrawSize);

        public LegacyJudgementLine()
        {
            RelativeSizeAxes = Axes.Y;
            Anchor = Anchor.Centre;
            Origin = Anchor.Centre;

            AddLayout(subtractionCache);
        }

        private Sprite judgementLineBodySprite;

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
                judgementLineBodySprite = new Sprite
                {
                    Anchor = Anchor.Centre,
                    Origin = Anchor.Centre,
                    RelativeSizeAxes = Axes.Y,
                    Name = "Judgement line body",
                    Size = Vector2.One,
                    FillMode = FillMode.Stretch,
                    Depth = 1,
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

            direction.BindTo(scrollingInfo.Direction);
            direction.BindValueChanged(OnDirectionChanged, true);
        }

        protected virtual void OnDirectionChanged(ValueChangedEvent<ScrollingDirection> direction)
        {
            Scale = direction.NewValue == ScrollingDirection.Left ? Vector2.One : new Vector2(-1, 1);
        }

        protected override void Update()
        {
            base.Update();

            if (!subtractionCache.IsValid && DrawHeight > 0)
            {
                if (judgementLineBodySprite.Texture != null)
                {
                    judgementLineBodySprite.Width = getWidth(judgementLineBodySprite);
                }

                subtractionCache.Validate();
            }

            static float getWidth(Sprite s) => s.Texture?.DisplayWidth ?? 0;
        }

        protected Texture GetTextureFromLookup(ISkin skin, LegacyKaraokeSkinConfigurationLookups lookup)
            => skin.GetTexture(GetTextureNameFromLookup(lookup));

        public static string GetTextureNameFromLookup(LegacyKaraokeSkinConfigurationLookups lookup)
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

            return $"karaoke-judgement-line-{suffix}";
        }
    }
}
