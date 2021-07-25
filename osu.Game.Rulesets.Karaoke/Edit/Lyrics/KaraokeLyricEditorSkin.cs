// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using osu.Framework.Graphics.Sprites;
using osu.Game.Rulesets.Karaoke.Skinning;

namespace osu.Game.Rulesets.Karaoke.Edit.Lyrics
{
    /// <summary>
    /// This karaoke skin is using in lyric editor only.
    /// </summary>
    public class KaraokeLyricEditorSkin : KaraokeInternalSkin
    {
        public const int MIN_FONT_SIZE = 10;
        public const int MAX_FONT_SIZE = 45;

        protected override string ResourceName => @"osu.Game.Rulesets.Karaoke.Resources.Skin.editor.skin";

        public KaraokeLyricEditorSkin()
        {
            FontSize = 26;
        }

        public float FontSize
        {
            get => BindableFont.Value.LyricTextFontInfo.LyricTextFontInfo.Size;
            set
            {
                var textSize = Math.Max(Math.Min(value, MAX_FONT_SIZE), MIN_FONT_SIZE);
                var changePercentage = textSize / FontSize;

                BindableFont.Value.LyricTextFontInfo.LyricTextFontInfo
                    = multipleSize(BindableFont.Value.LyricTextFontInfo.LyricTextFontInfo, changePercentage);
                BindableFont.Value.RubyTextFontInfo.LyricTextFontInfo
                    = multipleSize(BindableFont.Value.RubyTextFontInfo.LyricTextFontInfo, changePercentage);
                BindableFont.Value.RomajiTextFontInfo.LyricTextFontInfo
                    = multipleSize(BindableFont.Value.RomajiTextFontInfo.LyricTextFontInfo, changePercentage);

                BindableFont.Value.ShadowOffset *= changePercentage;
                BindableFont.TriggerChange();

                static FontUsage multipleSize(FontUsage origin, float percentage)
                    => origin.With(size: origin.Size * percentage);
            }
        }
    }
}
