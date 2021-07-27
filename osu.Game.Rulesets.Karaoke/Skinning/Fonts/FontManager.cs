// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System.IO;
using System.Linq;
using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Graphics;
using osu.Framework.IO.Stores;
using osu.Framework.Platform;
using osu.Game.Rulesets.Karaoke.IO.Archives;

namespace osu.Game.Rulesets.Karaoke.Skinning.Fonts
{
    public class FontManager : Component
    {
        private const string font_base_path = @"fonts\cached";

        private const string font_extension = "cached";

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
            var storage = host.Storage;
            if (!storage.ExistsDirectory(font_base_path))
                return;

            var fontFiles = storage.GetFiles(font_base_path, "*.cached").ToList();
            Fonts.AddRange(fontFiles.Select(x =>
            {
                var fontName = Path.GetFileNameWithoutExtension(x);
                return new FontInfo(fontName, true);
            }));
        }

        public GlyphStore GetGlyphStore(FontInfo fontInfo)
        {
            if (!fontInfo.UserImport)
                return null;

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
