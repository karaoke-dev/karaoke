// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using NUnit.Framework;
using osu.Framework.Graphics.Textures;
using osu.Framework.IO.Stores;
using osu.Game.Rulesets.Karaoke.Tests.Resources;

namespace osu.Game.Rulesets.Karaoke.Tests.IO.Stores
{
    /// <summary>
    /// Use <see cref="GlyphStore"/> as test case actual result for comparing.
    /// </summary>
    /// <typeparam name="TGlyphStore"></typeparam>
    public abstract class BaseGlyphStoreTest<TGlyphStore> where TGlyphStore : IGlyphStore
    {
        protected TGlyphStore CustomizeGlyphStore { get; private set; }

        protected GlyphStore GlyphStore { get; private set; }

        [OneTimeSetUp]
        protected void OneTimeSetUp()
        {
            // create and load glyph store.
            var fontResourceStore = new NamespacedResourceStore<byte[]>(TestResources.GetStore(), "Resources.Testing.Fonts.Fnt.OpenSans");
            GlyphStore = new GlyphStore(fontResourceStore, FontName);
            GlyphStore.LoadFontAsync().Wait();

            // create load load customize glyph store.
            var customizeFontResourceStore = new NamespacedResourceStore<byte[]>(TestResources.GetStore(), $"Resources.Testing.Fonts.{FontType}");
            CustomizeGlyphStore = CreateFontStore(customizeFontResourceStore, FontName);
            CustomizeGlyphStore.LoadFontAsync().Wait();
        }

        protected abstract string FontType { get; }

        protected abstract string FontName { get; }

        protected abstract TGlyphStore CreateFontStore(ResourceStore<byte[]> store, string assetName);

        [Test]
        public void CompareFontNameWithOrigin()
        {
            string expected = GlyphStore.FontName;
            string actual = CustomizeGlyphStore.FontName;
            Assert.AreEqual(expected, actual);
        }

        [TestCase('a')]
        [TestCase(' ')]
        [TestCase('ㄅ')] // should not have those texts in store.
        [TestCase('あ')] // should not have those texts in store.
        public void CompareHasGlyphWithOrigin(char c)
        {
            bool expected = GlyphStore.HasGlyph(c);
            bool actual = CustomizeGlyphStore.HasGlyph(c);
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void CompareGetBaseHeightWithOrigin()
        {
            float? expected = GlyphStore.Baseline;
            float? actual = CustomizeGlyphStore.Baseline;
            Assert.AreEqual(expected, actual);
        }

        [TestCase('a')]
        [TestCase('A')]
        [TestCase('1')]
        [TestCase(' ')]
        [TestCase('@')]
        [TestCase('#')]
        public void CompareGetCharacterGlyphWithOrigin(char c)
        {
            var expected = GlyphStore.Get(c);
            var actual = CustomizeGlyphStore.Get(c);

            // because get character glyph should make sure that this glyph store contains char, so will not be null.
            Assert.IsNotNull(expected);
            Assert.IsNotNull(actual);

            // test all property should be matched.
            Assert.AreEqual(expected.Character, actual.Character);
            Assert.AreEqual(expected.XOffset, actual.XOffset);
            Assert.AreEqual(expected.YOffset, actual.YOffset);
            Assert.AreEqual(expected.XAdvance, actual.XAdvance);
        }

        [TestCase('a', 'a')]
        [TestCase('a', 'b')]
        [TestCase('i', 'v')] // todo: should got a result that is not zero.
        [TestCase('t', 'i')]
        [TestCase('a', 'あ')]
        public void CompareGetKerningWithOrigin(char left, char right)
        {
            int expected = GlyphStore.GetKerning(left, right);
            int actual = CustomizeGlyphStore.GetKerning(left, right);
            Assert.AreEqual(expected, actual);
        }

        [TestCase('a')]
        [TestCase('A')]
        [TestCase('1')]
        [TestCase(' ')]
        [TestCase('@')]
        [TestCase('#')]
        public void CompareGetTextureUploadWithOrigin(char c)
        {
            var expected = GlyphStore.Get(new string(new[] { c }));
            var actual = (CustomizeGlyphStore as IResourceStore<TextureUpload>)?.Get(new string(new[] { c }));

            Assert.IsNotNull(expected);
            Assert.IsNotNull(actual);

            // todo : should test with pixel perfect, but it's ok to pass if size is almost the same.
            Assert.AreEqual(expected.Width, actual.Width);
            Assert.AreEqual(expected.Height, actual.Height);
        }
    }
}
