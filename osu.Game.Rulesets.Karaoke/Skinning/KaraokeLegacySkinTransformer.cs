// Copyright (c) ppy Pty Ltd <contact@ppy.sh>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using System.IO;
using System.Reflection;
using osu.Framework.Audio.Sample;
using osu.Framework.Bindables;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Textures;
using osu.Game.Audio;
using osu.Game.IO;
using osu.Game.Rulesets.Karaoke.Beatmaps.Formats;
using osu.Game.Rulesets.Karaoke.Beatmaps.Objects;
using osu.Game.Skinning;

namespace osu.Game.Rulesets.Karaoke.Skinning
{
    public class KaraokeLegacySkinTransformer : ISkin
    {
        private readonly ISkin source;

        private KaroakeSkin skin;

        public KaraokeLegacySkinTransformer(ISkinSource source)
        {
            this.source = source;

            // TODO : need a better way to load resource
            var assembly = Assembly.GetExecutingAssembly();
            var resourceName = @"osu.Game.Rulesets.Karaoke.Resources.Skin.default.nkmproj";

            using (Stream stream = assembly.GetManifestResourceStream(resourceName))
            using (var reader = new LineBufferedReader(stream))
            {
                skin = new NicoKaraDecoder().Decode(reader);
            }
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
                case KaraokeFontLookup fontLookup:
                    return SkinUtils.As<TValue>(new Bindable<KaraokeFont>(skin.DefinedFonts[fontLookup.FontIndex]));
                case KaraokeLayoutLookup layoutLookup:
                    return SkinUtils.As<TValue>(new Bindable<KaraokeLayout>(skin.DefinedLayouts[layoutLookup.LayoutIndex]));
            }

            return source.GetConfig<TLookup, TValue>(lookup);
        }

        private bool hasFont(string fontName) => source.GetTexture($"{fontName}-0") != null;
    }
}
