// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using System.Diagnostics.CodeAnalysis;
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
        public const string FONT_BASE_PATH = @"fonts";

        [Resolved, AllowNull]
        private GameHost host { get; set; }

        private Storage storage => host.Storage.GetStorageForDirectory(FONT_BASE_PATH);

        private readonly FontFormat[] supportedFormat = { FontFormat.Fnt, FontFormat.Ttf };

        public readonly BindableList<FontInfo> Fonts = new();

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

        private FileSystemWatcher watcher = null!;

        [BackgroundDependencyLoader]
        private void load()
        {
            foreach (var fontFormat in supportedFormat)
            {
                // will create folder if not exist.
                string path = getPathByFontType(fontFormat);
                string extension = getExtensionByFontType(fontFormat);

                var fontFiles = storage.GetStorageForDirectory(path)
                                       .GetFiles(string.Empty, $"*.{extension}").ToList();

                foreach (string fontFile in fontFiles)
                {
                    addFontToList(fontFile, fontFormat);
                }
            }

            watcher = new FileSystemWatcher(storage.GetFullPath(string.Empty))
            {
                EnableRaisingEvents = true,
                IncludeSubdirectories = true,
                NotifyFilter = NotifyFilters.LastWrite | NotifyFilters.CreationTime | NotifyFilters.FileName,
            };

            watcher.Renamed += onChange;
            watcher.Deleted += onChange;
            watcher.Created += onChange;

            void onChange(object sender, FileSystemEventArgs args)
            {
                // check is valid format.
                string extension = Path.GetExtension(args.FullPath);
                bool validFormat = supportedFormat.Any(x => $".{getPathByFontType(x)}" == extension);
                if (!validFormat)
                    return;

                // then doing action by type.
                switch (args.ChangeType)
                {
                    case WatcherChangeTypes.Created:
                        addFontToList(args.FullPath);
                        return;

                    case WatcherChangeTypes.Deleted:
                        removeFontFromList(args.FullPath);
                        return;

                    case WatcherChangeTypes.Renamed:
                        if (args is not RenamedEventArgs renamedEventArgs)
                            throw new InvalidCastException(nameof(args));

                        removeFontFromList(renamedEventArgs.OldFullPath);
                        addFontToList(renamedEventArgs.FullPath);

                        return;
                }
            }
        }

        private void addFontToList(string path)
        {
            var fontFormat = getFontTypeByExtension(Path.GetExtension(path));
            addFontToList(path, fontFormat);
        }

        private void removeFontFromList(string path)
        {
            var fontFormat = getFontTypeByExtension(Path.GetExtension(path));
            removeFontFromList(path, fontFormat);
        }

        private void addFontToList(string path, FontFormat fontFormat)
        {
            string fontName = Path.GetFileNameWithoutExtension(path);
            var fontInfo = new FontInfo(fontName, fontFormat);
            Fonts.Add(fontInfo);
        }

        private void removeFontFromList(string path, FontFormat fontFormat)
        {
            string fontName = Path.GetFileNameWithoutExtension(path);
            var matchedFont = Fonts.FirstOrDefault(x => x.FontName == fontName && x.FontFormat == fontFormat);

            if (!Fonts.Contains(matchedFont))
                return;

            Fonts.Remove(matchedFont);
        }

        public FontFormat? CheckFontFormat(FontUsage fontUsage)
        {
            string fontName = fontUsage.FontName;
            if (Fonts.All(x => x.FontName != fontName))
                return null;

            return Fonts.FirstOrDefault(x => x.FontName == fontName).FontFormat;
        }

        public IResourceStore<TextureUpload>? GetGlyphStore(FontInfo fontInfo)
        {
            // do not import if this font is system font.
            var fontFormat = fontInfo.FontFormat;
            if (fontFormat == FontFormat.Internal)
                return null;

            string fontName = fontInfo.FontName;
            return fontFormat switch
            {
                FontFormat.Fnt => getFntGlyphStore(fontName),
                FontFormat.Ttf => getTtfGlyphStore(fontName),
                FontFormat.Internal or _ => throw new ArgumentOutOfRangeException(nameof(fontFormat))
            };
        }

        private FntGlyphStore? getFntGlyphStore(string fontName)
        {
            string path = Path.Combine(getPathByFontType(FontFormat.Fnt), fontName);
            string pathWithExtension = Path.ChangeExtension(path, getExtensionByFontType(FontFormat.Fnt));

            if (!storage.Exists(pathWithExtension))
                return null;

            var resources = new CachedFontArchiveReader(storage.GetStream(pathWithExtension), fontName);
            return new FntGlyphStore(new ResourceStore<byte[]>(resources), $"{fontName}", host.CreateTextureLoaderStore(resources));
        }

        private TtfGlyphStore? getTtfGlyphStore(string fontName)
        {
            string path = Path.Combine(getPathByFontType(FontFormat.Ttf), fontName);
            string pathWithExtension = Path.ChangeExtension(path, getExtensionByFontType(FontFormat.Ttf));

            if (!storage.Exists(pathWithExtension))
                return null;

            var resources = new StorageBackedResourceStore(storage.GetStorageForDirectory(getPathByFontType(FontFormat.Ttf)));
            return new TtfGlyphStore(new ResourceStore<byte[]>(resources), $"{fontName}");
        }

        private static string getPathByFontType(FontFormat type) =>
            type switch
            {
                FontFormat.Fnt => "fnt",
                FontFormat.Ttf => "ttf",
                FontFormat.Internal or _ => throw new ArgumentOutOfRangeException(nameof(type))
            };

        private static string getExtensionByFontType(FontFormat type) =>
            type switch
            {
                FontFormat.Fnt => "zipfnt",
                FontFormat.Ttf => "ttf",
                FontFormat.Internal or _ => throw new ArgumentOutOfRangeException(nameof(type))
            };

        private static FontFormat getFontTypeByExtension(string extension) =>
            extension switch
            {
                ".zipfnt" => FontFormat.Fnt,
                ".ttf" => FontFormat.Ttf,
                _ => throw new FormatException(nameof(extension)),
            };

        protected override void Dispose(bool isDisposing)
        {
            base.Dispose(isDisposing);

            watcher?.Dispose();
        }
    }
}
