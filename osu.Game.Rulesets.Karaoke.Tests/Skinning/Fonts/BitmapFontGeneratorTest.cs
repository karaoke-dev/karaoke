// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using System.Linq;
using NUnit.Framework;
using osu.Framework.IO.Stores;
using osu.Game.Rulesets.Karaoke.IO.Stores;
using osu.Game.Rulesets.Karaoke.Skinning.Fonts;
using osu.Game.Rulesets.Karaoke.Tests.Asserts;
using osu.Game.Rulesets.Karaoke.Tests.Resources;
using SharpFNT;

namespace osu.Game.Rulesets.Karaoke.Tests.Skinning.Fonts
{
    public class BitmapFontGeneratorTest
    {
        private KaraokeGlyphStore glyphStore;

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            var fontResourceStore = new NamespacedResourceStore<byte[]>(TestResources.GetStore(), "Resources.Testing.Fonts.OpenSans");
            glyphStore = new KaraokeGlyphStore(fontResourceStore, $"OpenSans-Regular");
            glyphStore.LoadFontAsync().Wait();

            // make sure that
            var normalGlyph = glyphStore.Get('a');
            if (normalGlyph == null)
                throw new ArgumentNullException(nameof(normalGlyph));
        }

        [TestCase("A", 1)]
        [TestCase("a", 1)]
        [TestCase("カラオケ", 0)]
        [TestCase(null, 1)]
        public void TestGenerate(string chars, int charAmount)
        {
            var generator = createGenerator();
            var result = generator.Generate(chars?.ToArray());
            var originFontInfo = glyphStore.BitmapFont;

            // info and common should just copy.
            ObjectAssert.ArePropertyEqual(result.Info, originFontInfo.Info);
            ObjectAssert.ArePropertyEqual(result.Common, originFontInfo.Common);

            // should have page if have char.
            Assert.NotNull(result.Pages);

            if (!string.IsNullOrEmpty(chars) && charAmount > 0)
            {
                Assert.NotZero(result.Pages.Count);
            }

            // should have chars if have char.
            Assert.NotNull(result.Characters);

            if (!string.IsNullOrEmpty(chars))
            {
                Assert.AreEqual(result.Characters.Count, charAmount);
            }

            // kerning pairs amount might be zero but cannot be null.
            Assert.NotNull(result.KerningPairs);
        }

        [TestCase(new int[] { }, new string[] { })]
        [TestCase(new[] { 0 }, new[] { "OpenSans_0.png" })] // max store page is start from 0.
        [TestCase(new[] { 0, 1 }, new[] { "OpenSans_0.png" })] // should not have the case that more then origin page number.
        public void TestGeneratePage(int[] pages, string[] pageNames)
        {
            var generator = createGenerator();
            var characters = pages.Select(x => new Character
            {
                Page = x
            }).ToArray();

            try
            {
                var result = generator.GeneratePages(characters);
                Assert.AreEqual(result.Values.ToArray(), pageNames);
            }
            catch (Exception e)
            {
                var storePage = glyphStore.BitmapFont.Pages.Max(x => x.Key);
                Assert.Greater(pageNames.Length, storePage);
            }
        }

        [TestCase("A")]
        public void TestGenerateCharactersPosition(string chars)
        {
            // todo : implement is needed.
            var generator = createGenerator();
            var result = generator.GenerateCharacters(chars.ToArray());
        }

        [TestCase("カラオケ", 0)]
        [TestCase("からおけ", 0)]
        [TestCase("カラオケ(karaoke)", 7)]
        public void TestGenerateCharactersIfNotExist(string chars, int amount)
        {
            var generator = createGenerator();
            var result = generator.GenerateCharacters(chars.ToArray());
            Assert.AreEqual(result.Count, amount);
        }

        [TestCase("", 0)]
        [TestCase(null, 0)]
        [TestCase("a", 0)] // should not have any kerning pair if has zero or one char.
        [TestCase("aaaaa", 0)] // same char should not have kerning pair.
        [TestCase("ab", 0)] // don't worry. some of pairs does not have kerning pair.
        [TestCase("AB", 1)]
        [TestCase("ABC", 3)]
        public void TestGenerateKerningPairs(string chars, int amount)
        {
            var generator = createGenerator();
            var result = generator.GenerateKerningPairs(chars?.ToArray());
            Assert.AreEqual(result.Count, amount);
        }

        [Test]
        public void TestGenerateKerningPairsWithAllChars()
        {
            var generator = createGenerator();
            var chars = glyphStore.BitmapFont.Characters.Keys.Select(x => (char)x).ToArray();
            var kerningPairs = glyphStore.BitmapFont.KerningPairs;

            // make sure that no kerning is missing.
            var result = generator.GenerateKerningPairs(chars);
            Assert.AreEqual(result.Count, kerningPairs.Count);
        }

        private BitmapFontGenerator createGenerator()
        {
            return new(glyphStore);
        }
    }
}
