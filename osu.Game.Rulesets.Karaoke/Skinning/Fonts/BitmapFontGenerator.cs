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
            var originFont = store.BitmapFont;
            var characters = GenerateCharacters(chars);

            return new BitmapFont
            {
                Info = copyObject(originFont.Info),
                Common = copyObject(originFont.Common),
                Pages = GeneratePages(characters.Values.ToArray()),
                Characters = characters,
                KerningPairs = GenerateKerningPairs(chars)
            };
        }

        internal IDictionary<int, string> GeneratePages(Character[] characters)
        {
            if (store.BitmapFont == null)
                throw new ArgumentNullException(nameof(store.BitmapFont));

            if (characters == null || characters.Length == 0)
                return new Dictionary<int, string>();

            var maxStorePage = store.BitmapFont.Pages.Max(x => x.Key);
            var maxPage = characters.Max(x => x.Page);
            if (maxPage > maxStorePage)
                throw new ArgumentOutOfRangeException(nameof(maxPage));

            return store.BitmapFont.Pages
                        .Where(x => x.Key <= maxPage)
                        .ToDictionary(x => x.Key, x => x.Value);
        }

        internal IDictionary<int, Character> GenerateCharacters(char[] chars)
        {
            if (store.BitmapFont == null)
                throw new ArgumentNullException(nameof(store.BitmapFont));

            chars = chars?.Distinct().ToArray();
            if (chars == null || chars.Length < 1)
                return new Dictionary<int, Character>();

            var charCodeList = chars.Select(x => (int)x).ToList();
            var filteredCharacters = store.BitmapFont.Characters
                                          .Where(x => charCodeList.Contains(x.Key))
                                          .ToDictionary(x => x.Key, x => copyObject(x.Value));

            // todo : should adjust position and some property.
            return filteredCharacters;
        }

        internal IDictionary<KerningPair, int> GenerateKerningPairs(char[] chars)
        {
            if (store.BitmapFont == null)
                throw new ArgumentNullException(nameof(store.BitmapFont));

            chars = chars?.Distinct().ToArray();
            if (chars == null || chars.Length < 2)
                return new Dictionary<KerningPair, int>();

            // kerning pairs is the spacing between two chars.
            // should query all the kerning that contain chars.
            var charCodeList = chars.Select(x => (int)x).ToList();
            var originKerningPairs = store.BitmapFont.KerningPairs;
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
