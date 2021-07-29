// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using osu.Game.Rulesets.Karaoke.IO.Stores;
using SharpFNT;

namespace osu.Game.Rulesets.Karaoke.Skinning.Fonts
{
    public class BitmapFontGenerator
    {
        private readonly KaraokeGlyphStore store;

        public BitmapFontGenerator(KaraokeGlyphStore store)
        {
            this.store = store;
        }

        public BitmapFont Generate(char[] chars)
        {
            var bitmapFont = store.BitmapFont;
            if (bitmapFont == null)
                throw new ArgumentNullException(nameof(bitmapFont));

            var characters = GenerateCharacters(chars);

            return new BitmapFont
            {
                Info = copyObject(bitmapFont.Info),
                Common = copyObject(bitmapFont.Common),
                Pages = GeneratePages(characters.Values.ToArray()),
                Characters = characters,
                KerningPairs = GenerateKerningPairs(chars)
            };
        }

        internal IDictionary<int, string> GeneratePages(Character[] characters)
        {
            var bitmapFont = store.BitmapFont;
            if (bitmapFont == null)
                throw new ArgumentNullException(nameof(bitmapFont));

            if (characters == null || characters.Length == 0)
                return new Dictionary<int, string>();

            var maxStorePage = bitmapFont.Pages.Max(x => x.Key);
            var maxPage = characters.Max(x => x.Page);
            if (maxPage > maxStorePage)
                throw new ArgumentOutOfRangeException(nameof(maxPage));

            return bitmapFont.Pages
                             .Where(x => x.Key <= maxPage)
                             .ToDictionary(x => x.Key, x => x.Value);
        }

        internal IDictionary<int, Character> GenerateCharacters(char[] chars)
        {
            var bitmapFont = store.BitmapFont;
            if (bitmapFont == null)
                throw new ArgumentNullException(nameof(bitmapFont));

            chars = chars?.Distinct().ToArray();
            if (chars == null || chars.Length < 1)
                return new Dictionary<int, Character>();

            // got the characters need to be precessed.
            var charCodeList = chars.Select(x => (int)x).ToList();
            var filteredCharacters = bitmapFont.Characters
                                               .Where(x => charCodeList.Contains(x.Key))
                                               .ToDictionary(x => x.Key, x => copyObject(x.Value));

            // first, sort by character height.
            var processingCharacters = filteredCharacters.OrderByDescending(x => x.Value.Height);

            // second, give then a suitable width and height.
            var pageSize = new
            {
                Width = bitmapFont.Common.ScaleWidth,
                Height = bitmapFont.Common.ScaleHeight,
            };
            var padding = new
            {
                Top = bitmapFont.Info.PaddingUp,
                Bottom = bitmapFont.Info.PaddingDown,
                Left = bitmapFont.Info.PaddingLeft,
                Right = bitmapFont.Info.PaddingRight,
            };
            var spacing = new
            {
                X = bitmapFont.Info.SpacingHorizontal,
                Y = bitmapFont.Info.SpacingVertical,
            };

            var page = 0;
            var currentTopLeftPosition = new
            {
                X = padding.Left,
                Y = padding.Top,
            };

            foreach (var (_, character) in processingCharacters)
            {
                if (currentTopLeftPosition.Y + character.Height > pageSize.Width - padding.Bottom)
                {
                    // it's time to change to next page.
                    page++;
                    currentTopLeftPosition = new
                    {
                        X = padding.Left,
                        Y = padding.Top,
                    };
                }
                else if (currentTopLeftPosition.X + character.Width > pageSize.Height - padding.Right)
                {
                    // it's time to change to next line.
                    currentTopLeftPosition = new
                    {
                        X = padding.Left,
                        Y = currentTopLeftPosition.Y + spacing.Y,
                    };
                }

                // memo:
                // x-offset = how many pixels to shift the character right (= extra blank pixels to left of character)
                // y-offset = how many pixels to shift the character down (= extra blank pixels above character)
                // x-advance = total width of character in pixels (so adds xadvance-xoffset-width extra blank pixels to the right of character)
                // so we need to do in here is just change the position.
                character.Page = page;
                character.X = currentTopLeftPosition.X;
                character.Y = currentTopLeftPosition.Y;

                // assign next position for drawing.
                currentTopLeftPosition = new
                {
                    X = currentTopLeftPosition.X + character.Width + spacing.X,
                    Y = currentTopLeftPosition.Y,
                };
            }

            return processingCharacters.OrderBy(x => x.Key).ToDictionary(x => x.Key, x => x.Value);
        }

        internal IDictionary<KerningPair, int> GenerateKerningPairs(char[] chars)
        {
            var bitmapFont = store.BitmapFont;
            if (bitmapFont == null)
                throw new ArgumentNullException(nameof(bitmapFont));

            chars = chars?.Distinct().ToArray();
            if (chars == null || chars.Length < 2)
                return new Dictionary<KerningPair, int>();

            // kerning pairs is the spacing between two chars.
            // should query all the kerning that contain chars.
            var charCodeList = chars.Select(x => (int)x).ToList();
            var originKerningPairs = bitmapFont.KerningPairs;
            return originKerningPairs
                   .Where(x => charCodeList.Contains(x.Key.First) && charCodeList.Contains(x.Key.Second))
                   .ToDictionary(x => x.Key, x => x.Value);
        }

        private static T copyObject<T>(T obj)
        {
            var str = JsonConvert.SerializeObject(obj);
            return JsonConvert.DeserializeObject<T>(str);
        }
    }
}
