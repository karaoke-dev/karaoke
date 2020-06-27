// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Audio.Sample;
using osu.Framework.Bindables;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Textures;
using osu.Game.Audio;
using osu.Game.Rulesets.Karaoke.Skinning;
using osu.Game.Rulesets.Karaoke.Skinning.Components;
using osu.Game.Skinning;
using System;

namespace osu.Game.Rulesets.Karaoke.Edit.LyricEditor
{
    /// <summary>
    /// This karaoke skin is using in lyric editor only.
    /// </summary>
    public class KaraokeLyricEditorSkin : ISkin
    {
        private readonly Bindable<KaraokeFont> bindableFont;
        private readonly Bindable<KaraokeLayout> bindableLayout;

        public KaraokeLyricEditorSkin()
        {
            bindableFont = new Bindable<KaraokeFont>();
            bindableLayout = new Bindable<KaraokeLayout>();
        }

        public Drawable GetDrawableComponent(ISkinComponent component) => null;

        public SampleChannel GetSample(ISampleInfo sampleInfo) => null;

        public Texture GetTexture(string componentName) => null;

        public IBindable<TValue> GetConfig<TLookup, TValue>(TLookup lookup)
        {
            if(!(lookup is KaraokeSkinLookup skinLookup))
                throw new NotSupportedException();

            var config = skinLookup.Config;

            switch (config)
            {
                case KaraokeSkinConfiguration.LyricStyle:
                    return SkinUtils.As<TValue>(bindableFont);
                case KaraokeSkinConfiguration.LyricLayout:
                    return SkinUtils.As<TValue>(bindableLayout);
                default:
                    throw new NotSupportedException();
            }
        }
    }
}
