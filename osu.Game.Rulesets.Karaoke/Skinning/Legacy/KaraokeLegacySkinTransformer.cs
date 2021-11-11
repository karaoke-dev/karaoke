// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using System.ComponentModel;
using System.Reflection;
using osu.Framework.Graphics;
using osu.Game.Beatmaps;
using osu.Game.IO;
using osu.Game.Rulesets.Karaoke.Beatmaps;
using osu.Game.Rulesets.Karaoke.UI.HUD;
using osu.Game.Rulesets.Scoring;
using osu.Game.Skinning;

namespace osu.Game.Rulesets.Karaoke.Skinning.Legacy
{
    public class KaraokeLegacySkinTransformer : LegacySkinTransformer
    {
        private readonly KaraokeBeatmap beatmap;
        private readonly Lazy<bool> isLegacySkin;

        public KaraokeLegacySkinTransformer(ISkin source, IBeatmap beatmap)
            : base(generateDefaultKaraokeSkin(source))
        {
            this.beatmap = (KaraokeBeatmap)beatmap;
            isLegacySkin = new Lazy<bool>(() => GetConfig<SkinConfiguration.LegacySetting, decimal>(SkinConfiguration.LegacySetting.Version) != null);
        }

        private static DefaultKaraokeSkin generateDefaultKaraokeSkin(ISkin skin)
        {
            var resources = getStorageResourceProvider(skin);
            return new DefaultKaraokeSkin(resources);

            static IStorageResourceProvider getStorageResourceProvider(ISkin skin)
            {
                if (skin is not LegacySkin legacySkin)
                    return null;

                var property = typeof(Skin).GetField("resources", BindingFlags.Instance | BindingFlags.NonPublic);
                return property?.GetValue(legacySkin) as IStorageResourceProvider;
            }
        }

        public override Drawable GetDrawableComponent(ISkinComponent component)
        {
            switch (component)
            {
                case SkinnableTargetComponent targetComponent:
                    switch (targetComponent.Target)
                    {
                        case SkinnableTarget.MainHUDComponents:
                            var components = base.GetDrawableComponent(component) as SkinnableTargetComponentsContainer ?? getTargetComponentsContainerFromOtherPlace();
                            components.Add(new SettingButtonsDisplay
                            {
                                Anchor = Anchor.CentreRight,
                                Origin = Anchor.CentreRight,
                            });
                            return components;

                        default:
                            return base.GetDrawableComponent(component);
                    }

                case GameplaySkinComponent<HitResult> resultComponent:
                    return getResult(resultComponent.Component);

                case KaraokeSkinComponent karaokeComponent:
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
                    return base.GetDrawableComponent(component);
            }

            SkinnableTargetComponentsContainer getTargetComponentsContainerFromOtherPlace()
            {
                switch (Skin)
                {
                    case DefaultKaraokeSkin defaultKaraokeSkin:
                        return new TempLegacySkin(defaultKaraokeSkin.SkinInfo).GetDrawableComponent(component) as SkinnableTargetComponentsContainer;

                    default:
                        throw new InvalidCastException();
                }
            }
        }

        private Drawable getResult(HitResult result)
        {
            // todo : get real component
            return null;
        }

        // it's a temp class for just getting SkinnableTarget.MainHUDComponents
        private class TempLegacySkin : LegacySkin
        {
            public TempLegacySkin(SkinInfo skin)
                : base(skin, null, null, default(string))
            {
            }
        }
    }
}
