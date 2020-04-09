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

        /// <summary>
        /// Whether texture for the keys exists.
        /// Used to determine if the karaoke ruleset is skinned.
        /// </summary>
        private Lazy<bool> hasKeyTexture;

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

            // todo : need to check skin is really exist
            hasKeyTexture = new Lazy<bool>(() => true);
        }

        public Drawable GetDrawableComponent(ISkinComponent component)
        {
            if (!(component is KaraokeSkinComponent karaokeComponent))
                return null;

            if (!isLegacySkin.Value || !hasKeyTexture.Value)
                return null;

            switch (karaokeComponent.Component)
            {
                case KaraokeSkinComponents.ColumnBackground:
                    return new LegacyColumnBackground();

                case KaraokeSkinComponents.StageBackground:
                    return new LegacyStageBackground();

                case KaraokeSkinComponents.JudgementLine:
                    return new LegacyJudgementLine();

                case KaraokeSkinComponents.Note:
                    return new LegacyNotePiece();
            }

            return null;
        }

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
