// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using System.Linq;

namespace osu.Game.Rulesets.Karaoke.Fonts
{
    public class FontInfo
    {
        public string FileName { get; set; }

        public string Family { get; set; }

        public string Weight { get; set; }

        public FontInfo(string fileName)
        {
            FileName = fileName;

            var parts = fileName.Split('-');
            switch (parts.Length)
            {
                case 1:
                    Family = parts[0];
                    break;

                default:
                    Weight = fileName.Split('-').LastOrDefault();
                    Family = string.Join('-', parts.Take(parts.Count() - 1));
                    break;
            }
        }
    }
}
