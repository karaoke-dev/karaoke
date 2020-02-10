// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using osuTK;

namespace osu.Game.Rulesets.Karaoke.Skinning.Components
{
    public class KaraokeFont
    {
        public string Name { get; set; }

        public TextBrushInfo FrontTextBrushInfo { get; set; }

        public TextBrushInfo BackTextBrushInfo { get; set; }

        public TextFontInfo LyricTextFontInfo { get; set; }

        public TextFontInfo RubyTextFontInfo { get; set; }

        public TextFontInfo RomajiTextFontInfo { get; set; }

        public class TextBrushInfo
        {
            public BrushInfo TextBrush { get; set; }

            public BrushInfo BorderBrush { get; set; }

            public BrushInfo ShadowBrush { get; set; }
        }

        public class TextFontInfo
        {
            public FontInfo LyricTextFontInfo { get; set; }

            // This property might be ignore now
            public FontInfo NakaTextFontInfo { get; set; }

            // This property might be ignore now
            public FontInfo EnTextFontInfo { get; set; }
        }

        public bool UseShadow { get; set; }

        public Vector2 ShadowOffset { get; set; }
    }
}
