// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

#nullable disable

using System.Collections.Generic;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Graphics.Textures;
using osu.Framework.IO.Stores;
using osu.Game.Rulesets.Karaoke.Skinning.Fonts;
using osu.Game.Rulesets.Karaoke.Utils;

namespace osu.Game.Rulesets.Karaoke.IO.Stores
{
    public class KaraokeLocalFontStore : FontStore
    {
        private readonly Dictionary<FontInfo, IResourceStore<TextureUpload>> fontInfos = new();
        private readonly IResourceStore<TextureUpload> store;
        private readonly FontManager fontManager;

        /// <summary>
        /// Construct a font store to be added to a parent font store via <see cref="AddFont"/>.
        /// </summary>
        /// <param name="fontManager">font manager.</param>
        /// <param name="store">The texture source.</param>
        /// <param name="scaleAdjust">The raw pixel height of the font. Can be used to apply a global scale or metric to font usages.</param>
        public KaraokeLocalFontStore(FontManager fontManager, IResourceStore<TextureUpload> store = null, float scaleAdjust = 100)
            : base(store, scaleAdjust)
        {
            this.store = store;
            this.fontManager = fontManager;
        }

        public void AddFont(FontUsage fontUsage)
        {
            var fontFormat = fontManager.CheckFontFormat(fontUsage);
            if (fontFormat == null)
                return;

            var fontInfo = FontUsageUtils.ToFontInfo(fontUsage, fontFormat.Value);
            addFont(fontInfo);
        }

        private void addFont(FontInfo fontInfo)
        {
            bool hasFont = fontInfos.ContainsKey(fontInfo);
            if (hasFont)
                return;

            var glyphStore = fontManager.GetGlyphStore(fontInfo);
            if (glyphStore == null)
                return;

            AddTextureSource(glyphStore);
            fontInfos.Add(fontInfo, glyphStore);
        }

        private void removeFont(FontInfo fontInfo)
        {
            if (!fontInfos.TryGetValue(fontInfo, out var glyphStore))
                return;

            RemoveTextureStore(glyphStore);
            fontInfos.Remove(fontInfo);
        }

        public void ClearFont()
        {
            foreach (var (fontInfo, _) in fontInfos)
            {
                removeFont(fontInfo);
            }
        }
    }
}
