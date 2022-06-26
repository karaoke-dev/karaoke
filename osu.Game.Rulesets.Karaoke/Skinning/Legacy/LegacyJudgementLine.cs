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
        private readonly LayoutValue subtractionCache = new(Invalidation.DrawSize);

        public LegacyJudgementLine()
        {
            RelativeSizeAxes = Axes.Y;
            Anchor = Anchor.Centre;
            Origin = Anchor.Centre;

            AddLayout(subtractionCache);
        }

        private Sprite judgementLineBodySprite = null!;

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
                    Texture = getTextureFromLookup(skin, LegacyKaraokeSkinConfigurationLookups.JudgementLineHeadImage)
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
                    Texture = getTextureFromLookup(skin, LegacyKaraokeSkinConfigurationLookups.JudgementLineBodyImage)
                },
                new Sprite
                {
                    Anchor = Anchor.BottomCentre,
                    Origin = Anchor.Centre,
                    Name = "Judgement line tail",
                    Texture = getTextureFromLookup(skin, LegacyKaraokeSkinConfigurationLookups.JudgementLineTailImage)
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

        private static Texture? getTextureFromLookup(ISkin skin, LegacyKaraokeSkinConfigurationLookups lookup)
            => skin.GetTexture(getTextureNameFromLookup(lookup));

        private static string getTextureNameFromLookup(LegacyKaraokeSkinConfigurationLookups lookup)
        {
            string suffix = lookup switch
            {
                LegacyKaraokeSkinConfigurationLookups.JudgementLineBodyImage => "body",
                LegacyKaraokeSkinConfigurationLookups.JudgementLineHeadImage => "head",
                LegacyKaraokeSkinConfigurationLookups.JudgementLineTailImage => "tail",
                _ => throw new ArgumentOutOfRangeException(nameof(lookup))
            };

            return $"karaoke-judgement-line-{suffix}";
        }
    }
}
