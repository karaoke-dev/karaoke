// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

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
using osuTK.Graphics;

namespace osu.Game.Rulesets.Karaoke.Skinning
{
    public class KaraokeLegacySkinTransformer : ISkin
    {
        private readonly ISkin source;

        private readonly KaraokeSkin skin;

        public KaraokeLegacySkinTransformer(ISkinSource source)
        {
            this.source = source;

            // TODO : need a better way to load resource
            var assembly = Assembly.GetExecutingAssembly();
            const string resource_name = @"osu.Game.Rulesets.Karaoke.Resources.Skin.default.skin";

            using (var stream = assembly.GetManifestResourceStream(resource_name))
            using (var reader = new LineBufferedReader(stream))
            {
                skin = new KaraokeSkinDecoder().Decode(reader);
            }

            // TODO : get note style from file
            skin.DefinedNoteSkins = new List<NoteSkin>
            {
                new NoteSkin
                {
                    Name = "Note skin 0",
                    NoteColor = new Color4(68, 170, 221, 255),
                    BlinkColor = new Color4(255, 102, 170, 255),
                    TextColor = Color4.White,
                    BoldText = true,
                }
            };
        }

        public Drawable GetDrawableComponent(ISkinComponent component)
        {
            if (!(component is KaraokeSkinComponent karaokeComponent))
                return null;

            switch (karaokeComponent.Component)
            {
                case KaraokeSkinComponents.Snow:
                    if (source.GetTexture("snow") != null)
                        return new LegacySnow();

                    return null;
            }

            return null;
        }

        public Texture GetTexture(string componentName) => source.GetTexture(componentName);

        public SampleChannel GetSample(ISampleInfo sample) => source.GetSample(sample);

        public IBindable<TValue> GetConfig<TLookup, TValue>(TLookup lookup)
        {
            switch (lookup)
            {
                case KaraokeSkinLookup skinLookup:
                {
                    var config = skinLookup.Config;
                    var lookupNumber = skinLookup.Lookup;

                    switch (config)
                    {
                        case KaraokeSkinConfiguration.LyricStyle:
                            return SkinUtils.As<TValue>(new Bindable<KaraokeFont>(skin.DefinedFonts[lookupNumber]));

                        case KaraokeSkinConfiguration.LyricLayout:
                            return SkinUtils.As<TValue>(new Bindable<KaraokeLayout>(skin.DefinedLayouts[lookupNumber]));

                        case KaraokeSkinConfiguration.NoteStyle:
                            return SkinUtils.As<TValue>(new Bindable<NoteSkin>(skin.DefinedNoteSkins[lookupNumber]));
                    }

                    break;
                }

                case KaraokeIndexLookup indexLookup:
                    switch (indexLookup)
                    {
                        case KaraokeIndexLookup.Layout:
                            var layoutDictionary = skin.DefinedLayouts.ToDictionary(k => skin.DefinedLayouts.IndexOf(k), y => y.Name);
                            return SkinUtils.As<TValue>(new Bindable<Dictionary<int, string>>(layoutDictionary));

                        case KaraokeIndexLookup.Style:
                            var fontDictionary = skin.DefinedFonts.ToDictionary(k => skin.DefinedFonts.IndexOf(k), y => y.Name);
                            return SkinUtils.As<TValue>(new Bindable<Dictionary<int, string>>(fontDictionary));

                        case KaraokeIndexLookup.Note:
                            var noteDictionary = skin.DefinedNoteSkins.ToDictionary(k => skin.DefinedNoteSkins.IndexOf(k), y => y.Name);
                            return SkinUtils.As<TValue>(new Bindable<Dictionary<int, string>>(noteDictionary));
                    }

                    break;
            }

            return source.GetConfig<TLookup, TValue>(lookup);
        }

        private bool hasFont(string fontName) => source.GetTexture($"{fontName}-0") != null;
    }
}
