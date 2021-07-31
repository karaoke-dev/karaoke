// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using System.Linq;
using NUnit.Framework;
using osu.Framework.Graphics.Textures;
using osu.Framework.IO.Stores;
using osu.Game.Rulesets.Karaoke.IO.Stores;
using osu.Game.Rulesets.Karaoke.Skinning.Fonts;
using osu.Game.Rulesets.Karaoke.Tests.Resources;
using SharpFNT;

namespace osu.Game.Rulesets.Karaoke.Tests.Skinning.Fonts
{
    public class BitmapFontImageGeneratorTest
    {
        private TestFntGlyphStore glyphStore;

        private BitmapFont font => glyphStore.BitmapFont;

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            var fontResourceStore = new NamespacedResourceStore<byte[]>(TestResources.GetStore(), "Resources.Testing.Fonts.Fnt.OpenSans");
            glyphStore = new TestFntGlyphStore(fontResourceStore, "OpenSans-Regular");
            glyphStore.LoadFontAsync().Wait();

            // make sure glyph are loaded.
            var normalGlyph = glyphStore.Get('a');
            if (normalGlyph == null)
                throw new ArgumentNullException(nameof(normalGlyph));
        }

        [Test]
        public void TestGenerate()
        {
            var generator = new BitmapFontImageGenerator(glyphStore);

            var result = generator.Generate(font);
            var originPage = glyphStore.GetPageImage(0);

            // test should draw same image as origin resource in glyph store.
            Assert.AreEqual(result.Length, 1);

            // test should draw same image as origin resource in glyph store.
            var originImageData = originPage.Data.ToArray();
            var resultImageData = result.FirstOrDefault()?.Data.ToArray();
            Assert.AreEqual(resultImageData, originImageData);
        }

        [Test]
        public void TestGeneratePage()
        {
            var generator = new BitmapFontImageGenerator(glyphStore);
            var characters = font.Characters.Where(x => x.Value.Page == 0)
                                 .ToDictionary(x => x.Key, x => x.Value);

            // test generate first page.
            var result = generator.GeneratePage(font.Info, font.Common, characters);
            var originPage = glyphStore.GetPageImage(0);

            // test should draw same image as origin resource in glyph store.
            var originImageData = originPage.Data.ToArray();
            var resultImageData = result.Data.ToArray();
            Assert.AreEqual(resultImageData, originImageData);
        }

        private class TestFntGlyphStore : FntGlyphStore
        {
            public TestFntGlyphStore(ResourceStore<byte[]> store, string assetName)
                : base(store, assetName)
            {
            }

            // should expose this image for testing.
            public new TextureUpload GetPageImage(int page)
                => base.GetPageImage(page);
        }
    }
}
