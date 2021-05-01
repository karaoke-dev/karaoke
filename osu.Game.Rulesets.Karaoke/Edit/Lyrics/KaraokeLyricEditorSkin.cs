// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using osu.Framework.Bindables;
using osu.Game.Rulesets.Karaoke.Skinning;
using osu.Game.Skinning;

namespace osu.Game.Rulesets.Karaoke.Edit.Lyrics
{
    /// <summary>
    /// This karaoke skin is using in lyric editor only.
    /// </summary>
    public class KaraokeLyricEditorSkin : KaraokeInternalSkin
    {
        public const int MIN_FONT_SIZE = 10;
        public const int MAX_FONT_SIZE = 45;

        protected readonly Bindable<float> BindableColumnHeight = new Bindable<float>(12);
        protected readonly Bindable<float> BindableJudgementAresPrecentage = new Bindable<float>(0.05f);

        protected override string ResourceName => @"osu.Game.Rulesets.Karaoke.Resources.Skin.editor.skin";

        public KaraokeLyricEditorSkin()
        {
            FontSize = 26;
        }

        public float FontSize
        {
            get => BindableFont.Value.LyricTextFontInfo.LyricTextFontInfo.CharSize;
            set
            {
                var textSize = Math.Max(Math.Min(value, MAX_FONT_SIZE), MIN_FONT_SIZE);
                var changePercentage = textSize / FontSize;
                BindableFont.Value.LyricTextFontInfo.LyricTextFontInfo.CharSize *= changePercentage;
                BindableFont.Value.RubyTextFontInfo.LyricTextFontInfo.CharSize *= changePercentage;
                BindableFont.Value.RomajiTextFontInfo.LyricTextFontInfo.CharSize *= changePercentage;
                BindableFont.Value.ShadowOffset *= changePercentage;
                BindableFont.TriggerChange();
            }
        }

        public override IBindable<TValue> GetConfig<TLookup, TValue>(TLookup lookup)
        {
            if (lookup is KaraokeSkinConfigurationLookup skinConfigurationLookup)
            {
                switch (skinConfigurationLookup.Lookup)
                {
                    // should use customize height for note playfield in lyric editor.
                    case LegacyKaraokeSkinConfigurationLookups.ColumnHeight:
                        return SkinUtils.As<TValue>(BindableColumnHeight);

                    // not have note playfield judgement spacing in lyric editor.
                    case LegacyKaraokeSkinConfigurationLookups.JudgementAresPrecentage:
                        return SkinUtils.As<TValue>(BindableJudgementAresPrecentage);
                }
            }

            return base.GetConfig<TLookup, TValue>(lookup);
        }
    }
}
