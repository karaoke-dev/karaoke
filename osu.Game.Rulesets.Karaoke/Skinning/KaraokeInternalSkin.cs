// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using System.Linq;
using System.Reflection;
using osu.Framework.Audio.Sample;
using osu.Framework.Bindables;
using osu.Framework.Graphics;
using osu.Framework.Graphics.OpenGL.Textures;
using osu.Framework.Graphics.Textures;
using osu.Game.Audio;
using osu.Game.IO;
using osu.Game.Rulesets.Karaoke.Beatmaps.Formats;
using osu.Game.Rulesets.Karaoke.Skinning.Metadatas.Fonts;
using osu.Game.Rulesets.Karaoke.Skinning.Metadatas.Layouts;
using osu.Game.Rulesets.Karaoke.Skinning.Metadatas.Notes;
using osu.Game.Skinning;

namespace osu.Game.Rulesets.Karaoke.Skinning
{
    /// <summary>
    /// Use as internal skin and prevent user to adjust
    /// </summary>
    public abstract class KaraokeInternalSkin : ISkin
    {
        protected readonly Bindable<LyricFont> BindableFont;
        protected readonly Bindable<LyricLayout> BindableLayout;
        protected readonly Bindable<NoteSkin> BindableNote;

        protected abstract string ResourceName { get; }

        protected KaraokeInternalSkin()
        {
            // TODO : need a better way to load resource
            var assembly = Assembly.GetExecutingAssembly();

            using (var stream = assembly.GetManifestResourceStream(ResourceName))
            using (var reader = new LineBufferedReader(stream))
            {
                var skin = new KaraokeSkinDecoder().Decode(reader);

                BindableFont = new Bindable<LyricFont>(skin.Fonts.FirstOrDefault());
                BindableLayout = new Bindable<LyricLayout>(skin.Layouts.FirstOrDefault());
                BindableNote = new Bindable<NoteSkin>(skin.NoteSkins.FirstOrDefault());
            }
        }

        public Drawable GetDrawableComponent(ISkinComponent component) => null;

        public ISample GetSample(ISampleInfo sampleInfo) => null;

        public Texture GetTexture(string componentName, WrapMode wrapModeS, WrapMode wrapModeT) => null;

        public virtual IBindable<TValue> GetConfig<TLookup, TValue>(TLookup lookup)
        {
            if (!(lookup is KaraokeSkinLookup skinLookup))
                throw new NotSupportedException();

            var config = skinLookup.Config;

            switch (config)
            {
                case KaraokeSkinConfiguration.LyricStyle:
                    return SkinUtils.As<TValue>(BindableFont);

                case KaraokeSkinConfiguration.LyricLayout:
                    return SkinUtils.As<TValue>(BindableLayout);

                case KaraokeSkinConfiguration.NoteStyle:
                    return SkinUtils.As<TValue>(BindableNote);

                default:
                    throw new NotSupportedException();
            }
        }
    }
}
