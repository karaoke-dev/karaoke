// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Threading.Tasks;
using JetBrains.Annotations;
using osu.Framework.Graphics.Textures;
using osu.Framework.IO.Stores;
using osu.Framework.Logging;
using osu.Framework.Text;
using SixLabors.Fonts;

namespace osu.Game.Rulesets.Karaoke.IO.Stores
{
    public class OtfGlyphStore : IResourceStore<TextureUpload>, IGlyphStore
    {
        protected readonly string AssetName;

        protected readonly IResourceStore<TextureUpload> TextureLoader;

        public readonly string FontName;

        protected readonly ResourceStore<byte[]> Store;

        [CanBeNull]
        public Font Font => completionSource.Task.Result;

        private IFontInstance fontInstance => Font?.Instance;

        private readonly TaskCompletionSource<Font> completionSource = new TaskCompletionSource<Font>();

        private readonly IGlyphRenderer glyphRenderer;

        /// <summary>
        /// Create a new glyph store.
        /// </summary>
        /// <param name="store">The store to provide font resources.</param>
        /// <param name="assetName">The base name of thße font.</param>
        /// <param name="textureLoader">An optional platform-specific store for loading textures. Should load for the store provided in <param ref="param"/>.</param>
        public OtfGlyphStore(ResourceStore<byte[]> store, string assetName = null, IResourceStore<TextureUpload> textureLoader = null)
        {
            Store = new ResourceStore<byte[]>(store);

            Store.AddExtension("otf");

            AssetName = assetName;
            TextureLoader = textureLoader;

            FontName = assetName?.Split('/').Last();
        }

        private Task fontLoadTask;

        public Task LoadFontAsync() => fontLoadTask ??= Task.Factory.StartNew(() =>
        {
            try
            {
                Font font;

                using (var s = Store.GetStream($@"{AssetName}"))
                {
                    var fonts = new FontCollection();
                    var fontFamily = fonts.Install(s);
                    font = new Font(fontFamily, 12);
                }

                completionSource.SetResult(font);
            }
            catch (Exception ex)
            {
                Logger.Error(ex, $"Couldn't load font asset from {AssetName}.");
                completionSource.SetResult(null);
                throw;
            }
        }, TaskCreationOptions.PreferFairness);

        public bool HasGlyph(char c) => fontInstance?.GetGlyph(c) != null;

        public int GetBaseHeight() => fontInstance?.LineHeight ?? 0;

        public int? GetBaseHeight(string name)
        {
            if (name != FontName)
                return null;

            return GetBaseHeight();
        }

        [CanBeNull]
        public CharacterGlyph Get(char character)
        {
            if (fontInstance == null)
                return null;

            var glyphInstance = fontInstance.GetGlyph(character);
            // todo : get x and y offset.
            return new CharacterGlyph(character, 0, 0, glyphInstance.AdvanceWidth, this);
        }

        public int GetKerning(char left, char right)
        {
            if (fontInstance == null)
                return 0;

            var leftGlyphInstance = fontInstance.GetGlyph(left);
            var rightGlyphInstance = fontInstance.GetGlyph(right);

            return (int)fontInstance.GetOffset(leftGlyphInstance, rightGlyphInstance).X;
        }

        Task<CharacterGlyph> IResourceStore<CharacterGlyph>.GetAsync(string name) => Task.Run(() => ((IGlyphStore)this).Get(name[0]));

        CharacterGlyph IResourceStore<CharacterGlyph>.Get(string name) => Get(name[0]);

        public TextureUpload Get(string name)
        {
            if (fontInstance == null) return null;

            if (name.Length > 1 && !name.StartsWith($@"{FontName}/", StringComparison.Ordinal))
                return null;

            var glyphInstance = fontInstance.GetGlyph(name.Last());
            return LoadCharacter(glyphInstance);
        }

        public virtual async Task<TextureUpload> GetAsync(string name)
        {
            if (name.Length > 1 && !name.StartsWith($@"{FontName}/", StringComparison.Ordinal))
                return null;

            return !(await completionSource.Task.ConfigureAwait(false)).Instance.GetGlyph() ? null : LoadCharacter(c);
        }

        protected int LoadedGlyphCount;

        protected virtual TextureUpload LoadCharacter(GlyphInstance character)
        {
            LoadedGlyphCount++;

            character.RenderTo(glyphRenderer, 0, Vector2.Zero, new Vector2(72), fontInstance.LineHeight);

            return new TextureUpload(img);
        }

        public Stream GetStream(string name) => throw new NotSupportedException();

        public IEnumerable<string> GetAvailableResources() => throw new NotSupportedException();

        #region IDisposable Support

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
        }

        #endregion
    }
}
