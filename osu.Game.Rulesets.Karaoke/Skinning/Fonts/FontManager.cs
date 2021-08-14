// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using System.IO;
using System.Linq;
using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Graphics.Textures;
using osu.Framework.IO.Stores;
using osu.Framework.Platform;
using osu.Game.Rulesets.Karaoke.IO.Archives;
using osu.Game.Rulesets.Karaoke.IO.Stores;

namespace osu.Game.Rulesets.Karaoke.Skinning.Fonts
{
    public class FontManager : Component
    {
        private const string font_base_path = @"fonts";

        [Resolved]
        private GameHost host { get; set; }

        public readonly BindableList<FontInfo> Fonts = new BindableList<FontInfo>();

        public FontManager()
        {
            Fonts.AddRange(new[]
            {
                // From osu-framework
                new FontInfo("OpenSans-Regular", FontFormat.Internal),
                new FontInfo("OpenSans-Bold", FontFormat.Internal),
                new FontInfo("OpenSans-RegularItalic", FontFormat.Internal),
                new FontInfo("OpenSans-BoldItalic", FontFormat.Internal),

                new FontInfo("Roboto-Regular", FontFormat.Internal),
                new FontInfo("Roboto-Bold", FontFormat.Internal),
                new FontInfo("RobotoCondensed-Regular", FontFormat.Internal),
                new FontInfo("RobotoCondensed-Bold", FontFormat.Internal),
                // From osu.game
                new FontInfo("osuFont", FontFormat.Internal),

                new FontInfo("Torus-Regular", FontFormat.Internal),
                new FontInfo("Torus-Light", FontFormat.Internal),
                new FontInfo("Torus-SemiBold", FontFormat.Internal),
                new FontInfo("Torus-Bold", FontFormat.Internal),

                new FontInfo("Inter-Regular", FontFormat.Internal),
                new FontInfo("Inter-RegularItalic", FontFormat.Internal),
                new FontInfo("Inter-Light", FontFormat.Internal),
                new FontInfo("Inter-LightItalic", FontFormat.Internal),
                new FontInfo("Inter-SemiBold", FontFormat.Internal),
                new FontInfo("Inter-SemiBoldItalic", FontFormat.Internal),
                new FontInfo("Inter-Bold", FontFormat.Internal),
                new FontInfo("Inter-BoldItalic", FontFormat.Internal),

                new FontInfo("Noto-Basic", FontFormat.Internal),
                new FontInfo("Noto-Hangul", FontFormat.Internal),
                new FontInfo("Noto-CJK-Basic", FontFormat.Internal),
                new FontInfo("Noto-CJK-Compatibility", FontFormat.Internal),
                new FontInfo("Noto-Thai", FontFormat.Internal),

                new FontInfo("Venera-Light", FontFormat.Internal),
                new FontInfo("Venera-Bold", FontFormat.Internal),
                new FontInfo("Venera-Black", FontFormat.Internal),

                new FontInfo("Compatibility", FontFormat.Internal),
            });
        }

        [BackgroundDependencyLoader]
        private void load()
        {
            var supportedFormat = new[] { FontFormat.Fnt, FontFormat.Ttf };

            foreach (var fontFormat in supportedFormat)
            {
                // check if dictionary is exist.
                var path = getPathByFontType(fontFormat);
                var extension = getExtensionByFontType(fontFormat);
                var storage = host.Storage;
                if (!storage.ExistsDirectory(path))
                    return;

                var fontFiles = storage.GetFiles(path, $"*.{extension}").ToList();
                Fonts.AddRange(fontFiles.Select(x =>
                {
                    var fontName = Path.GetFileNameWithoutExtension(x);
                    return new FontInfo(fontName, fontFormat);
                }));
            }
        }

        public FontFormat? CheckFontFormat(FontUsage fontUsage)
        {
            var fontName = fontUsage.FontName;
            if (Fonts.All(x => x.FontName != fontName))
                return null;

            return Fonts.FirstOrDefault(x => x.FontName == fontName).FontFormat;
        }

        public IResourceStore<TextureUpload> GetGlyphStore(FontInfo fontInfo)
        {
            // do not import if this font is system font.
            var fontFormat = fontInfo.FontFormat;
            if (fontFormat == FontFormat.Internal)
                return null;

            var storage = host.Storage;
            if (!storage.ExistsDirectory(font_base_path))
                return null;

            var fontName = fontInfo.FontName;
            return fontInfo.FontFormat switch
            {
                FontFormat.Fnt => getFntGlyphStore(storage, fontName),
                FontFormat.Ttf => getTtfGlyphStore(storage, fontName),
                _ => throw new ArgumentOutOfRangeException(nameof(fontFormat))
            };
        }

        private FntGlyphStore getFntGlyphStore(Storage storage, string fontName)
        {
            var path = Path.Combine(getPathByFontType(FontFormat.Fnt), fontName);
            var pathWithExtension = Path.ChangeExtension(path, getExtensionByFontType(FontFormat.Fnt));

            if (!storage.Exists(pathWithExtension))
                return null;

            var resources = new CachedFontArchiveReader(storage.GetStream(pathWithExtension), fontName);
            return new FntGlyphStore(new ResourceStore<byte[]>(resources), $"{fontName}", host.CreateTextureLoaderStore(resources));
        }

        private TtfGlyphStore getTtfGlyphStore(Storage storage, string fontName)
        {
            var path = Path.Combine(getPathByFontType(FontFormat.Ttf), fontName);
            var pathWithExtension = Path.ChangeExtension(path, getExtensionByFontType(FontFormat.Ttf));

            if (!storage.Exists(pathWithExtension))
                return null;

            var resources = new StorageBackedResourceStore(storage.GetStorageForDirectory(getPathByFontType(FontFormat.Ttf)));
            return new TtfGlyphStore(new ResourceStore<byte[]>(resources), $"{fontName}");
        }

        private static string getPathByFontType(FontFormat type) =>
            type switch
            {
                FontFormat.Fnt => $"{font_base_path}/fnt",
                FontFormat.Ttf => $"{font_base_path}/ttf",
                _ => throw new ArgumentOutOfRangeException(nameof(type))
            };

        private static string getExtensionByFontType(FontFormat type) =>
            type switch
            {
                FontFormat.Fnt => "zipfnt",
                FontFormat.Ttf => "ttf",
                _ => throw new ArgumentOutOfRangeException(nameof(type))
            };
    }
}
