// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Audio.Sample;
using osu.Framework.Bindables;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Textures;
using osu.Game.Audio;
using osu.Game.IO;
using osu.Game.Rulesets.Karaoke.Beatmaps.Formats;
using osu.Game.Rulesets.Karaoke.Skinning;
using osu.Game.Rulesets.Karaoke.Skinning.Components;
using osu.Game.Skinning;
using System;
using System.Linq;
using System.Reflection;

namespace osu.Game.Rulesets.Karaoke.Edit.LyricEditor
{
    /// <summary>
    /// This karaoke skin is using in lyric editor only.
    /// </summary>
    public class KaraokeLyricEditorSkin : ISkin
    {
        public const int MIN_FONT_SIZE = 10;
        public const int MAX_FONT_SIZE = 45;

        private readonly Bindable<KaraokeFont> bindableFont;
        private readonly Bindable<KaraokeLayout> bindableLayout;

        public KaraokeLyricEditorSkin()
        {
            // TODO : need a better way to load resource
            var assembly = Assembly.GetExecutingAssembly();
            const string resource_name = @"osu.Game.Rulesets.Karaoke.Resources.Skin.lyric-editor.skin";

            using (var stream = assembly.GetManifestResourceStream(resource_name))
            using (var reader = new LineBufferedReader(stream))
            {
                var skin = new KaraokeSkinDecoder().Decode(reader);

                bindableFont = new Bindable<KaraokeFont>(skin.Fonts.FirstOrDefault());
                bindableLayout = new Bindable<KaraokeLayout>(skin.Layouts.FirstOrDefault());
            }
        }

        public float FontSize
        {
            get => bindableFont.Value.LyricTextFontInfo.LyricTextFontInfo.CharSize;
            set
            {
                var textSize = Math.Max(Math.Min(value, MAX_FONT_SIZE), MIN_FONT_SIZE);
                var changePercentage = textSize / FontSize;
                bindableFont.Value.LyricTextFontInfo.LyricTextFontInfo.CharSize *= changePercentage;
                bindableFont.Value.RubyTextFontInfo.LyricTextFontInfo.CharSize *= changePercentage;
                bindableFont.Value.RomajiTextFontInfo.LyricTextFontInfo.CharSize *= changePercentage;
                bindableFont.Value.ShadowOffset *= changePercentage;
                bindableFont.TriggerChange();
            }
        }

        public Drawable GetDrawableComponent(ISkinComponent component) => null;

        public SampleChannel GetSample(ISampleInfo sampleInfo) => null;

        public Texture GetTexture(string componentName) => null;

        public IBindable<TValue> GetConfig<TLookup, TValue>(TLookup lookup)
        {
            if (!(lookup is KaraokeSkinLookup skinLookup))
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
