// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using osu.Framework.Graphics.Textures;
using osu.Game.Rulesets.Karaoke.IO.Stores;
using SharpFNT;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;

namespace osu.Game.Rulesets.Karaoke.Skinning.Fonts
{
    public class BitmapFontImageGenerator
    {
        private readonly FntGlyphStore store;

        public BitmapFontImageGenerator(FntGlyphStore store)
        {
            this.store = store;
        }

        public TextureUpload[] Generate(BitmapFont bitmapFont)
        {
            var pages = bitmapFont?.Characters?.GroupBy(x => x.Value.Page)
                                  .ToDictionary(x => x.Key, x
                                      => x.ToDictionary(v => v.Key, v => v.Value));

            if (pages == null || !pages.Any())
                return Array.Empty<TextureUpload>();

            return pages.Select(x => GeneratePage(bitmapFont.Info, bitmapFont.Common, x.Value)).ToArray();
        }

        internal TextureUpload GeneratePage([NotNull] BitmapFontInfo originInfo, [NotNull] BitmapFontCommon common, IDictionary<int, Character> characters)
        {
            int width = common.ScaleWidth;
            int height = common.ScaleHeight;

            int paddingTop = originInfo.PaddingUp;
            int paddingLeft = originInfo.PaddingLeft;

            // should check that all image is in the range.
            if (characters.Any(x => x.Value.X + x.Value.Width > width))
                throw new ArgumentOutOfRangeException(nameof(characters));

            if (characters.Any(x => x.Value.Y + x.Value.Height > height))
                throw new ArgumentOutOfRangeException(nameof(characters));

            // start drawing all the characters into single image.
            var page = new Image<Rgba32>(SixLabors.ImageSharp.Configuration.Default, width, height, new Rgba32(255, 255, 255, 0));

            foreach ((int c, var character) in characters)
            {
                // get the character image from source, and should make sure that image is exist.
                var characterImage = store.Get(new string(new[] { (char)c }));
                if (characterImage == null)
                    throw new ArgumentNullException(nameof(characterImage));

                // paste this shit into here.
                pasteImage(page, characterImage, paddingLeft + character.X, paddingTop + character.Y);
            }

            return new TextureUpload(page);
        }

        private static void pasteImage(Image<Rgba32> page, TextureUpload character, int startFromX, int startFromY)
        {
            int characterWidth = character.Width;
            int characterHeight = character.Height;
            var rowData = character.Data;

            for (int y = 0; y < characterHeight; y++)
            {
                var pixelRowSpan = page.GetPixelRowSpan(startFromY + y);
                int readOffset = y * character.Width;

                for (int x = 0; x < characterWidth; x++)
                {
                    pixelRowSpan[startFromX + x] = rowData[readOffset + x];
                }
            }
        }
    }
}
