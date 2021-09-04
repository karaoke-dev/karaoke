// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

#nullable disable

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using JetBrains.Annotations;
using osu.Framework.Extensions;
using osu.Framework.Graphics.Textures;
using osu.Framework.IO.Stores;
using osu.Framework.Logging;
using osu.Framework.Text;
using SixLabors.Fonts;
using SixLabors.Fonts.Unicode;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Drawing;
using SixLabors.ImageSharp.Drawing.Processing;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;
using TextBuilder = SixLabors.ImageSharp.Drawing.TextBuilder;

namespace osu.Game.Rulesets.Karaoke.IO.Stores
{
    public class TtfGlyphStore : IResourceStore<TextureUpload>, IGlyphStore
    {
        private const float dpi = 72f;

        protected readonly string AssetName;

        public string FontName { get; }

        public float? Baseline => fontMetrics?.LineHeight;

        protected readonly ResourceStore<byte[]> Store;

        [CanBeNull]
        public Font Font => completionSource.Task.GetResultSafely();

        private FontMetrics fontMetrics => Font?.FontMetrics;

        private readonly TaskCompletionSource<Font> completionSource = new();

        /// <summary>
        /// Create a new glyph store.
        /// </summary>
        /// <param name="store">The store to provide font resources.</param>
        /// <param name="assetName">The base name of thße font.</param>
        public TtfGlyphStore(ResourceStore<byte[]> store, string assetName = null)
        {
            Store = new ResourceStore<byte[]>(store);

            Store.AddExtension("ttf");
            Store.AddExtension("ttc");

            AssetName = assetName;

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
                    var fontFamily = fonts.AddCollection(s, out var description).ToArray();
                    font = new Font(fontFamily[0], 1);
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

        public bool HasGlyph(char c)
        {
            var glyphMetrics = fontMetrics?.GetGlyphMetrics(new CodePoint(c), ColorFontSupport.None).FirstOrDefault();
            return glyphMetrics?.GlyphType != GlyphType.Fallback;
        }

        [CanBeNull]
        public CharacterGlyph Get(char character)
        {
            if (fontMetrics == null)
                return null;

            Debug.Assert(Baseline != null);

            var glyphMetrics = fontMetrics.GetGlyphMetrics(new CodePoint(character), ColorFontSupport.None).FirstOrDefault();
            if (glyphMetrics == null || glyphMetrics.GlyphType == GlyphType.Fallback)
                return null;

            string text = new(new[] { character });
            var style = new TextOptions(Font);
            var bounds = TextMeasurer.MeasureBounds(text, style);

            float xOffset = bounds.Left * dpi;
            float yOffset = bounds.Top * dpi;

            float advanceWidth2 = glyphMetrics.AdvanceWidth * dpi / glyphMetrics.UnitsPerEm;
            return new CharacterGlyph(character, xOffset, yOffset, advanceWidth2, Baseline.Value, this);
        }

        public int GetKerning(char left, char right)
        {
            // todo: implement.
            return 0;
        }

        Task<CharacterGlyph> IResourceStore<CharacterGlyph>.GetAsync(string name, CancellationToken cancellationToken) =>
            Task.Run(() => ((IGlyphStore)this).Get(name[0]), cancellationToken);

        CharacterGlyph IResourceStore<CharacterGlyph>.Get(string name) => Get(name[0]);

        public TextureUpload Get(string name)
        {
            if (fontMetrics == null) return null;

            if (name.Length > 1 && !name.StartsWith($@"{FontName}/", StringComparison.Ordinal))
                return null;

            return !HasGlyph(name.Last()) ? null : LoadCharacter(name.Last());
        }

        public virtual async Task<TextureUpload> GetAsync(string name, CancellationToken cancellationToken = default)
        {
            if (name.Length > 1 && !name.StartsWith($@"{FontName}/", StringComparison.Ordinal))
                return null;

            await completionSource.Task.ConfigureAwait(false);

            return LoadCharacter(name.Last());
        }

        protected int LoadedGlyphCount;

        protected virtual TextureUpload LoadCharacter(char c)
        {
            if (Font == null)
                return null;

            LoadedGlyphCount++;

            // see: https://stackoverflow.com/a/53023454/4105113
            const float texture_scale = dpi;
            var style = new TextOptions(Font);
            string text = new(new[] { c });
            var bounds = TextMeasurer.MeasureBounds(text, style);
            var targetSize = new
            {
                Width = (int)(bounds.Width * texture_scale),
                Height = (int)(bounds.Height * texture_scale),
            };

            // this is the important line, where we render the glyphs to a vector instead of directly to the image
            // this allows further vector manipulation (scaling, translating) etc without the expensive pixel operations.
            var glyphs = TextBuilder.GenerateGlyphs(text, style);

            // should calculate this because it will cut the border if width and height scale is not the same.
            float widthScale = targetSize.Width / glyphs.Bounds.Width;
            float heightScale = targetSize.Height / glyphs.Bounds.Height;
            float minScale = Math.Min(widthScale, heightScale);

            // scale so that it will fit exactly in image shape once rendered
            glyphs = glyphs.Scale(minScale);

            // move the vectorised glyph so that it touch top and left edges
            // could be tweeked to center horizontally & vertically here
            glyphs = glyphs.Translate(-glyphs.Bounds.Location);

            // create image with char.
            var img = new Image<Rgba32>(targetSize.Width, targetSize.Height, new Rgba32(255, 255, 255, 0));
            img.Mutate(i => i.Fill(Color.White, glyphs));
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
