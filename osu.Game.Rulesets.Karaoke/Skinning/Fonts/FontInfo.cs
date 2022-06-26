// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Linq;

namespace osu.Game.Rulesets.Karaoke.Skinning.Fonts
{
    public readonly struct FontInfo
    {
        public string FontName { get; }

        public string Family { get; }

        public string? Weight { get; }

        public FontFormat FontFormat { get; }

        public FontInfo(string fontName, FontFormat fontFormat)
        {
            FontName = fontName;
            FontFormat = fontFormat;

            string[] parts = fontName.Split('-');

            switch (parts.Length)
            {
                case 1:
                    Family = parts[0];
                    Weight = null;
                    break;

                default:
                    Family = string.Join('-', parts.Take(parts.Length - 1));
                    Weight = fontName.Split('-').LastOrDefault();
                    break;
            }
        }
    }

    public enum FontFormat
    {
        Internal,

        Fnt,

        Ttf,
    }
}
