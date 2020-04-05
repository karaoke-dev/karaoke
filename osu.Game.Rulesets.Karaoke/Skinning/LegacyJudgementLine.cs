// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Graphics.Sprites;
using osu.Game.Rulesets.UI.Scrolling;
using osu.Game.Skinning;
using osuTK;

namespace osu.Game.Rulesets.Karaoke.Skinning
{
    public class LegacyJudgementLine : LegacyKaraokeElement
    {
        public LegacyJudgementLine()
        {
            RelativeSizeAxes = Axes.Both;
        }

        [BackgroundDependencyLoader]
        private void load(ISkinSource skin, IScrollingInfo scrollingInfo)
        {
            string targetImage = GetKaraokeSkinConfig<string>(skin, LegacyKaraokeSkinConfigurationLookups.HitTargetImage)?.Value
                                 ?? "karaoke-stage-hint";

            bool showJudgementLine = GetKaraokeSkinConfig<bool>(skin, LegacyKaraokeSkinConfigurationLookups.ShowJudgementLine)?.Value
                                     ?? true;

            InternalChildren = new Drawable[]
            {
                new Sprite
                {
                    Texture = skin.GetTexture(targetImage),
                    Scale = new Vector2(1, 0.9f * 1.6025f),
                    RelativeSizeAxes = Axes.Y,
                    Height = 1
                },
                new Box
                {
                    Anchor = Anchor.TopCentre,
                    RelativeSizeAxes = Axes.Y,
                    Width = 1,
                    Alpha = showJudgementLine ? 0.9f : 0
                }
            };
        }
    }
}
