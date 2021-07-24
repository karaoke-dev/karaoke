// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System.IO;
using osu.Framework.IO.Stores;
using osu.Framework.Platform;
using osu.Game.Rulesets.Karaoke.Fonts;
using osu.Game.Rulesets.Karaoke.IO.Archives;

namespace osu.Game.Rulesets.Karaoke.Extensions
{
    public static class GameHostExtensions
    {
        private const string font_base_path = @"fonts\cached";

        private const string font_extension = "cached";

        public static GlyphStore CreateGlyphStore(this GameHost host, FontInfo fontInfo)
        {
            var storage = host.Storage;
            if (!storage.ExistsDirectory(font_base_path))
                return null;

            var fontName = fontInfo.FontName;
            var path = Path.Combine(font_base_path, fontName);
            var pathWithExtension = Path.ChangeExtension(path, font_extension);

            if (!storage.Exists(pathWithExtension))
                return null;

            var resources = new CachedFontArchiveReader(storage.GetStream(pathWithExtension), fontName);
            return new GlyphStore(new ResourceStore<byte[]>(resources), $"{fontName}", host.CreateTextureLoaderStore(resources));
        }
    }
}
