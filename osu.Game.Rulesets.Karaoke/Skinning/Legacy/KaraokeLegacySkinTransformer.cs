// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using System.ComponentModel;
using osu.Framework.Audio;
using osu.Framework.Bindables;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Textures;
using osu.Framework.IO.Stores;
using osu.Game.Beatmaps;
using osu.Game.Database;
using osu.Game.IO;
using osu.Game.Rulesets.Karaoke.UI.HUD;
using osu.Game.Rulesets.Karaoke.Utils;
using osu.Game.Rulesets.Scoring;
using osu.Game.Skinning;

namespace osu.Game.Rulesets.Karaoke.Skinning.Legacy
{
    public class KaraokeLegacySkinTransformer : LegacySkinTransformer
    {
        private readonly Lazy<bool> isLegacySkin;
        private readonly KaraokeBeatmapSkin defaultKaraokeSkin;

        public KaraokeLegacySkinTransformer(ISkin source, IBeatmap beatmap)
            : base(source)
        {
            // we should get config by default karaoke skin.
            // if has resource or texture, then try to get from legacy skin.
            defaultKaraokeSkin = new KaraokeBeatmapSkin(new SkinInfo(), new InternalSkinStorageResourceProvider("Default"));
            isLegacySkin = new Lazy<bool>(() => GetConfig<SkinConfiguration.LegacySetting, decimal>(SkinConfiguration.LegacySetting.Version) != null);
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

            SkinnableTargetComponentsContainer getTargetComponentsContainerFromOtherPlace() =>
                Skin switch
                {
                    LegacySkin legacySkin => new TempLegacySkin(legacySkin.SkinInfo.Value).GetDrawableComponent(component) as SkinnableTargetComponentsContainer,
                    _ => throw new InvalidCastException()
                };
        }

        private Drawable getResult(HitResult result)
        {
            // todo : get real component
            return null;
        }

        public override IBindable<TValue> GetConfig<TLookup, TValue>(TLookup lookup)
        {
            var config = defaultKaraokeSkin.GetConfig<TLookup, TValue>(lookup);
            return config ?? base.GetConfig<TLookup, TValue>(lookup);
        }

        // it's a temp class for just getting SkinnableTarget.MainHUDComponents
        private class TempLegacySkin : LegacySkin
        {
            public TempLegacySkin(SkinInfo skin)
                : base(skin, null, null, default(string))
            {
            }
        }

        private class InternalSkinStorageResourceProvider : IStorageResourceProvider
        {
            public InternalSkinStorageResourceProvider(string skinName)
            {
                var assembly = AssemblyUtils.GetAssemblyByName("osu.Game.Rulesets.Karaoke");
                Files = Resources = new NamespacedResourceStore<byte[]>(new DllResourceStore(assembly), $"Resources/Skin/{skinName}");
            }

            public IResourceStore<TextureUpload> CreateTextureLoaderStore(IResourceStore<byte[]> underlyingStore)
            {
                throw new NotImplementedException();
            }

            public AudioManager AudioManager => null;
            public IResourceStore<byte[]> Files { get; }
            public IResourceStore<byte[]> Resources { get; }
            public RealmAccess RealmAccess => null;
        }
    }
}
