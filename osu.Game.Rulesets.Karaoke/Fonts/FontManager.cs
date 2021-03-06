﻿// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System.IO;
using System.Linq;
using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Graphics;
using osu.Framework.Platform;

namespace osu.Game.Rulesets.Karaoke.Fonts
{
    public class FontManager : Component
    {
        private const string base_path = "fonts\\cached";

        public readonly BindableList<FontInfo> Fonts = new BindableList<FontInfo>();

        private readonly Storage storage;

        public FontManager(Storage storage)
        {
            this.storage = storage;
            Fonts.AddRange(new[]
            {
                // From osu-framework
                new FontInfo("OpenSans-Bold"),
                new FontInfo("OpenSans-BoldItalic"),
                new FontInfo("OpenSans-Regular"),
                new FontInfo("OpenSans-RegularItalic"),
                new FontInfo("Roboto-Bold"),
                new FontInfo("Roboto-Regular"),
                new FontInfo("RobotoCondensed-Bold"),
                new FontInfo("RobotoCondensed-Regular"),
                // From osu.game
                new FontInfo("Noto-Basic"),
                new FontInfo("Noto-CJK-Basic"),
                new FontInfo("Noto-CJK-Compatibility"),
                new FontInfo("Noto-Hangul"),
                new FontInfo("Noto-Thai"),
                new FontInfo("Torus-Bold"),
                new FontInfo("Torus-Light"),
                new FontInfo("Torus-Regular"),
                new FontInfo("Torus-SemiBold"),
                new FontInfo("Venera-Black"),
                new FontInfo("Venera-Bold"),
                new FontInfo("Venera-Light"),
                new FontInfo("Compatibility"),
            });
        }

        [BackgroundDependencyLoader]
        private void load()
        {
            if (!storage.ExistsDirectory(base_path))
                return;

            var fontFiles = storage.GetFiles(base_path, "*.cached").ToList();
            Fonts.AddRange(fontFiles.Select(x =>
            {
                var fontName = Path.GetFileNameWithoutExtension(x);
                return new FontInfo(fontName, true);
            }));
        }
    }
}
