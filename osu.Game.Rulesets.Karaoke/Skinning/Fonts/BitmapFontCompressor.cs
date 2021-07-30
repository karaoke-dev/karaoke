// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using Newtonsoft.Json;
using SharpFNT;

namespace osu.Game.Rulesets.Karaoke.Skinning.Fonts
{
    public class BitmapFontCompressor
    {
        public static BitmapFont Compress([NotNull] BitmapFont bitmapFont, char[] chars)
        {
            if (bitmapFont == null)
                throw new ArgumentNullException(nameof(bitmapFont));

            var characters = GenerateCharacters(bitmapFont.Info, bitmapFont.Common, bitmapFont.Characters, chars);

            return new BitmapFont
            {
                Info = copyObject(bitmapFont.Info),
                Common = copyObject(bitmapFont.Common),
                Pages = GeneratePages(bitmapFont.Pages, characters.Values.ToArray()),
                Characters = characters,
                KerningPairs = GenerateKerningPairs(bitmapFont.KerningPairs, chars)
            };
        }

        internal static IDictionary<int, string> GeneratePages([NotNull] IDictionary<int, string> originPages, Character[] characters)
        {
            if (characters == null || characters.Length == 0)
                return new Dictionary<int, string>();

            var maxStorePage = originPages.Max(x => x.Key);
            var maxPage = characters.Max(x => x.Page);
            if (maxPage > maxStorePage)
                throw new ArgumentOutOfRangeException(nameof(maxPage));

            return originPages
                   .Where(x => x.Key <= maxPage)
                   .ToDictionary(x => x.Key, x => x.Value);
        }

        internal static IDictionary<int, Character> GenerateCharacters([NotNull] BitmapFontInfo originInfo, [NotNull] BitmapFontCommon originCommon, [NotNull] IDictionary<int, Character> originCharacters, char[] chars)
        {
            chars = chars?.Distinct().ToArray();
            if (chars == null || chars.Length < 1)
                return new Dictionary<int, Character>();

            // got the characters need to be precessed.
            var charCodeList = chars.Select(x => (int)x).ToList();
            var filteredCharacters = originCharacters
                                     .Where(x => charCodeList.Contains(x.Key))
                                     .ToDictionary(x => x.Key, x => copyObject(x.Value));

            // first, sort by character height.
            var processingCharacters = filteredCharacters.OrderByDescending(x => x.Value.Height).ToArray();

            // second, give then a suitable width and height.
            var pageSize = new
            {
                Width = originCommon.ScaleWidth,
                Height = originCommon.ScaleHeight,
            };
            var padding = new
            {
                Top = originInfo.PaddingUp,
                Bottom = originInfo.PaddingDown,
                Left = originInfo.PaddingLeft,
                Right = originInfo.PaddingRight,
            };
            var spacing = new
            {
                X = originInfo.SpacingHorizontal,
                Y = originInfo.SpacingVertical,
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
                    X = currentTopLeftPosition.X + character.Width + spacing.X, currentTopLeftPosition.Y,
                };
            }

            return processingCharacters.OrderBy(x => x.Key).ToDictionary(x => x.Key, x => x.Value);
        }

        internal static IDictionary<KerningPair, int> GenerateKerningPairs([NotNull] IDictionary<KerningPair, int> originKerningPairs, char[] chars)
        {
            chars = chars?.Distinct().ToArray();
            if (chars == null || chars.Length < 2)
                return new Dictionary<KerningPair, int>();

            // kerning pairs is the spacing between two chars.
            // should query all the kerning that contain chars.
            var charCodeList = chars.Select(x => (int)x).ToList();
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
