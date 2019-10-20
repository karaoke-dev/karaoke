// Copyright (c) ppy Pty Ltd <contact@ppy.sh>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using osuTK;

namespace osu.Game.Rulesets.Karaoke.Beatmaps.Objects
{
    public class KaraokeFont
    {
        public string Name { get; set; }

        public TextBrushInfo FrontTextBrushInfo { get; set; }

        public TextBrushInfo BackTextBrushInfo { get; set; }

        public TextFontInfo LyricTextFontInfo { get; set; }

        public TextFontInfo RubyTextFontInfo { get; set; }

        public class TextBrushInfo
        {
            public BrushInfo TextBrush { get; set; }

            public BrushInfo BorderBrush { get; set; }

            public BrushInfo ShadowBrush { get; set; }
        }

        public class TextFontInfo
        {
            public FontInfo LyricTextFontInfo { get; set; }

            public FontInfo NakaTextFontInfo { get; set; }

            public FontInfo EnTextFontInfo { get; set; }
        }

        public bool UseShadow { get; set; }

        public Vector2 ShadowOffset { get; set; }
    }
}
