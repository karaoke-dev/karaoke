// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Linq;

namespace osu.Game.Rulesets.Karaoke.Fonts
{
    public class FontInfo
    {
        public string FontName { get; }

        public string Family { get; }

        public string Weight { get; }

        public bool UserImport { get; }

        public FontInfo(string fontName, bool userImport = false)
        {
            FontName = fontName;
            UserImport = userImport;

            var parts = fontName.Split('-');

            switch (parts.Length)
            {
                case 1:
                    Family = parts[0];
                    break;

                default:
                    Weight = fontName.Split('-').LastOrDefault();
                    Family = string.Join('-', parts.Take(parts.Length - 1));
                    break;
            }
        }
    }
}
