// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using osu.Framework.Audio.Sample;
using osu.Framework.Bindables;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Textures;
using osu.Game.Audio;
using osu.Game.IO;
using osu.Game.Rulesets.Karaoke.Beatmaps.Formats;
using osu.Game.Rulesets.Karaoke.Skinning.Components;
using osu.Game.Skinning;

namespace osu.Game.Rulesets.Karaoke.Skinning
{
    public class KaraokeLegacySkinTransformer : ISkin
    {
        private readonly ISkin source;

        private readonly KaraokeSkin skin;

        private Lazy<bool> isLegacySkin;

        public KaraokeLegacySkinTransformer(ISkinSource source)
        {
            this.source = source;

            if (source != null)
                source.SourceChanged += sourceChanged;
            sourceChanged();

            // TODO : need a better way to load resource
            var assembly = Assembly.GetExecutingAssembly();
            const string resource_name = @"osu.Game.Rulesets.Karaoke.Resources.Skin.default.skin";

            using (var stream = assembly.GetManifestResourceStream(resource_name))
            using (var reader = new LineBufferedReader(stream))
            {
                skin = new KaraokeSkinDecoder().Decode(reader);
            }
        }

        private void sourceChanged()
        {
            isLegacySkin = new Lazy<bool>(() => source?.GetConfig<LegacySkinConfiguration.LegacySetting, decimal>(LegacySkinConfiguration.LegacySetting.Version) != null);
        }

        public Drawable GetDrawableComponent(ISkinComponent component)
        {
            if (!(component is KaraokeSkinComponent karaokeComponent))
                return null;

            if (!isLegacySkin.Value)
                return null;

            switch (karaokeComponent.Component)
            {
                case KaraokeSkinComponents.ColumnBackground:
                    if (textureExist(LegacyColumnBackground.GetTextureName()))
                        return new LegacyColumnBackground();

                    return null;

                case KaraokeSkinComponents.StageBackground:
                    if (textureExist(LegacyStageBackground.GetTextureName()))
                        return new LegacyStageBackground();

                    return null;

                case KaraokeSkinComponents.JudgementLine:
                    var judgementLine = LegacyJudgementLine.GetTextureNameFromLookup(LegacyKaraokeSkinConfigurationLookups.JudgementLineBodyImage);
                    if (textureExist(judgementLine))
                        return new LegacyJudgementLine();

                    return null;

                case KaraokeSkinComponents.Note:
                    var foregroundBody = LegacyNotePiece.GetTextureNameFromLookup(LegacyKaraokeSkinConfigurationLookups.NoteBodyImage, LegacyKaraokeSkinNoteLayer.Foreground);
                    var backgroundBody = LegacyNotePiece.GetTextureNameFromLookup(LegacyKaraokeSkinConfigurationLookups.NoteBodyImage, LegacyKaraokeSkinNoteLayer.Background);
                    if (textureExist(foregroundBody, backgroundBody))
                        return new LegacyNotePiece();

                    return null;

                case KaraokeSkinComponents.HitExplosion:
                    if (animationExist(LegacyHitExplosion.GetTextureName()))
                        return new LegacyHitExplosion();

                    return null;
            }

            return null;
        }

        private bool textureExist(params string[] textureNames)
            => textureNames.All(x => source.GetTexture(x) != null);

        private bool animationExist(params string[] textureNames)
            => textureNames.All(x => source.GetAnimation(x, true, false) != null);

        public Texture GetTexture(string componentName) => source.GetTexture(componentName);

        public SampleChannel GetSample(ISampleInfo sample) => source.GetSample(sample);

        public IBindable<TValue> GetConfig<TLookup, TValue>(TLookup lookup)
        {
            switch (lookup)
            {
                // Lookup skin by type and index
                case KaraokeSkinLookup skinLookup:
                {
                    var config = skinLookup.Config;
                    var lookupNumber = skinLookup.Lookup;

                    switch (config)
                    {
                        case KaraokeSkinConfiguration.LyricStyle:
                            return SkinUtils.As<TValue>(new Bindable<KaraokeFont>(skin.Fonts[lookupNumber]));

                        case KaraokeSkinConfiguration.LyricLayout:
                            return SkinUtils.As<TValue>(new Bindable<KaraokeLayout>(skin.Layouts[lookupNumber]));

                        case KaraokeSkinConfiguration.NoteStyle:
                            return SkinUtils.As<TValue>(new Bindable<NoteSkin>(skin.NoteSkins[lookupNumber]));

                        case KaraokeSkinConfiguration.Singer:
                            return SkinUtils.As<TValue>(new Bindable<Singer>(skin.Singers[lookupNumber]));
                    }

                    break;
                }

                // Lookup list of name by type
                case KaraokeIndexLookup indexLookup:
                    switch (indexLookup)
                    {
                        case KaraokeIndexLookup.Layout:
                            var layoutDictionary = skin.Layouts.ToDictionary(k => skin.Layouts.IndexOf(k), y => y.Name);
                            return SkinUtils.As<TValue>(new Bindable<Dictionary<int, string>>(layoutDictionary));

                        case KaraokeIndexLookup.Style:
                            var fontDictionary = skin.Fonts.ToDictionary(k => skin.Fonts.IndexOf(k), y => y.Name);
                            return SkinUtils.As<TValue>(new Bindable<Dictionary<int, string>>(fontDictionary));

                        case KaraokeIndexLookup.Note:
                            var noteDictionary = skin.NoteSkins.ToDictionary(k => skin.NoteSkins.IndexOf(k), y => y.Name);
                            return SkinUtils.As<TValue>(new Bindable<Dictionary<int, string>>(noteDictionary));

                        case KaraokeIndexLookup.Singer:
                            var singerDictionary = skin.Singers.ToDictionary(k => skin.Singers.IndexOf(k), y => y.Name);
                            return SkinUtils.As<TValue>(new Bindable<Dictionary<int, string>>(singerDictionary));
                    }

                    break;
            }

            return source.GetConfig<TLookup, TValue>(lookup);
        }
    }
}
