// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Graphics.Sprites;
using osu.Game.Rulesets.Karaoke.Fonts;

namespace osu.Game.Rulesets.Karaoke.Utils
{
    public static class FontUsageUtils
    {
        public static FontInfo ToFontInfo(FontUsage fontUsage, bool userImport = true)
            => new FontInfo(fontUsage.FontName, userImport);
    }
}
