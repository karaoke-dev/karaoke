// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using System.ComponentModel;
using osu.Framework.Bindables;
using osu.Framework.Graphics;
using osu.Game.Beatmaps;
using osu.Game.Rulesets.Karaoke.Skinning.Default;
using osu.Game.Rulesets.Scoring;
using osu.Game.Skinning;

namespace osu.Game.Rulesets.Karaoke.Skinning.Legacy
{
    /// <summary>
    /// Not inherit the <see cref="LegacySkinTransformer"/> because:
    /// 1. Karaoke ruleset does not have the legacy skin.
    /// 2. There's not much logic in the <see cref="LegacySkinTransformer"/>
    /// </summary>
    public class KaraokeLegacySkinTransformer : KaraokeDefaultSkinTransformer
    {
        private readonly Lazy<bool> isLegacySkin;
        private readonly KaraokeBeatmapSkin karaokeSkin;

        public KaraokeLegacySkinTransformer(ISkin skin, IBeatmap beatmap)
            : base(skin, beatmap)
        {
            // we should get config by default karaoke skin.
            // if has resource or texture, then try to get from legacy skin.
            karaokeSkin = new KaraokeBeatmapSkin(new SkinInfo(), new InternalSkinStorageResourceProvider("Default"));
            isLegacySkin = new Lazy<bool>(() => GetConfig<SkinConfiguration.LegacySetting, decimal>(SkinConfiguration.LegacySetting.Version) != null);
        }

        public override Drawable? GetDrawableComponent(ISkinComponentLookup lookup)
        {
            switch (lookup)
            {
                case GameplaySkinComponentLookup<HitResult> resultComponent:
                    return getResult(resultComponent.Component);

                case KaraokeSkinComponentLookup karaokeComponent:
                    if (!isLegacySkin.Value)
                        return null;

                    return karaokeComponent.Component switch
                    {
                        KaraokeSkinComponents.ColumnBackground => new LegacyColumnBackground(),
                        KaraokeSkinComponents.StageBackground => new LegacyStageBackground(),
                        KaraokeSkinComponents.JudgementLine => new LegacyJudgementLine(),
                        KaraokeSkinComponents.Note => new LegacyNotePiece(),
                        KaraokeSkinComponents.HitExplosion => new LegacyHitExplosion(),
                        _ => throw new InvalidEnumArgumentException(nameof(karaokeComponent.Component))
                    };

                default:
                    return base.GetDrawableComponent(lookup);
            }
        }

        private Drawable? getResult(HitResult result)
        {
            // todo : get real component
            return null;
        }

        public override IBindable<TValue>? GetConfig<TLookup, TValue>(TLookup lookup)
            => karaokeSkin.GetConfig<TLookup, TValue>(lookup);
    }
}
