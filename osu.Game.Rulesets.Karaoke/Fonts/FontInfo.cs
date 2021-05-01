// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Linq;

namespace osu.Game.Rulesets.Karaoke.Fonts
{
    public class FontInfo
    {
        public string FileName { get; }

        public string Family { get; }

        public string Weight { get; }

        public bool UserImport { get; }

        public FontInfo(string fileName, bool userImport = false)
        {
            FileName = fileName;
            UserImport = userImport;

            var parts = fileName.Split('-');

            switch (parts.Length)
            {
                case 1:
                    Family = parts[0];
                    break;

                default:
                    Weight = fileName.Split('-').LastOrDefault();
                    Family = string.Join('-', parts.Take(parts.Length - 1));
                    break;
            }
        }
    }
}
