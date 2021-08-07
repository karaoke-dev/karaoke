// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using System.IO;
using System.Linq;
using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Graphics;
using osu.Framework.IO.Stores;
using osu.Framework.Platform;
using osu.Game.IO.Archives;
using osu.Game.Rulesets.Karaoke.IO.Archives;
using osu.Game.Rulesets.Karaoke.IO.Stores;
using osu.Game.Rulesets.Karaoke.Utils;

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
                new FontInfo("OpenSans-Regular"),
                new FontInfo("OpenSans-Bold"),
                new FontInfo("OpenSans-RegularItalic"),
                new FontInfo("OpenSans-BoldItalic"),

                new FontInfo("Roboto-Regular"),
                new FontInfo("Roboto-Bold"),
                new FontInfo("RobotoCondensed-Regular"),
                new FontInfo("RobotoCondensed-Bold"),
                // From osu.game
                new FontInfo("osuFont"),

                new FontInfo("Torus-Regular"),
                new FontInfo("Torus-Light"),
                new FontInfo("Torus-SemiBold"),
                new FontInfo("Torus-Bold"),

                new FontInfo("Inter-Regular"),
                new FontInfo("Inter-RegularItalic"),
                new FontInfo("Inter-Light"),
                new FontInfo("Inter-LightItalic"),
                new FontInfo("Inter-SemiBold"),
                new FontInfo("Inter-SemiBoldItalic"),
                new FontInfo("Inter-Bold"),
                new FontInfo("Inter-BoldItalic"),

                new FontInfo("Noto-Basic"),
                new FontInfo("Noto-Hangul"),
                new FontInfo("Noto-CJK-Basic"),
                new FontInfo("Noto-CJK-Compatibility"),
                new FontInfo("Noto-Thai"),

                new FontInfo("Venera-Light"),
                new FontInfo("Venera-Bold"),
                new FontInfo("Venera-Black"),

                new FontInfo("Compatibility"),
            });
        }

        [BackgroundDependencyLoader]
        private void load()
        {
            foreach (var fontType in EnumUtils.GetValues<FontType>())
            {
                var path = getPathByFontType(fontType);
                var extension = getExtensionByFontType(fontType);
                loadFontList(path, extension);
            }

            void loadFontList(string path, string extension)
            {
                var storage = host.Storage;
                if (!storage.ExistsDirectory(path))
                    return;

                var fontFiles = storage.GetFiles(path, $"*.{extension}").ToList();
                Fonts.AddRange(fontFiles.Select(x =>
                {
                    var fontName = Path.GetFileNameWithoutExtension(x);
                    return new FontInfo(fontName, true);
                }));
            }
        }

        public IGlyphStore GetGlyphStore(FontInfo fontInfo)
        {
            if (!fontInfo.UserImport)
                return null;

            var storage = host.Storage;
            if (!storage.ExistsDirectory(font_base_path))
                return null;

            var fontName = fontInfo.FontName;

            var fntGlyphStore = getFntGlyphStore(storage, fontName);
            if (fntGlyphStore != null)
                return fntGlyphStore;

            // todo : might be able to check the font type.
            return getTtfGlyphStore(storage, fontName);
        }

        private FntGlyphStore getFntGlyphStore(Storage storage, string fontName)
        {
            var path = Path.Combine(getPathByFontType(FontType.Fnt), fontName);
            var pathWithExtension = Path.ChangeExtension(path, getExtensionByFontType(FontType.Fnt));

            if (!storage.Exists(pathWithExtension))
                return null;

            var resources = new CachedFontArchiveReader(storage.GetStream(pathWithExtension), fontName);
            return new FntGlyphStore(new ResourceStore<byte[]>(resources), $"{fontName}", host.CreateTextureLoaderStore(resources));
        }

        private TtfGlyphStore getTtfGlyphStore(Storage storage, string fontName)
        {
            var path = Path.Combine(getPathByFontType(FontType.Ttf), fontName);
            var pathWithExtension = Path.ChangeExtension(path, getExtensionByFontType(FontType.Ttf));

            if (!storage.Exists(pathWithExtension))
                return null;

            var resources = new LegacyFileArchiveReader(pathWithExtension);
            return new TtfGlyphStore(new ResourceStore<byte[]>(resources), $"{fontName}");
        }

        private static string getPathByFontType(FontType type)
        {
            switch (type)
            {
                case FontType.Fnt:
                    return $"{font_base_path}/fnt";

                case FontType.Ttf:
                    return $"{font_base_path}/ttf";

                default:
                    throw new ArgumentOutOfRangeException(nameof(type));
            }
        }

        private static string getExtensionByFontType(FontType type)
        {
            switch (type)
            {
                case FontType.Fnt:
                    return $"fnt";

                case FontType.Ttf:
                    return $"ttf";

                default:
                    throw new ArgumentOutOfRangeException(nameof(type));
            }
        }
    }

    public enum FontType
    {
        Fnt,

        Ttf,
    }
}
