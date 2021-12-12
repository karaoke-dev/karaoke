// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using System.Linq;
using NUnit.Framework;
using osu.Framework.Extensions.IEnumerableExtensions;
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
        private BitmapFont font;

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            var fontResourceStore = new NamespacedResourceStore<byte[]>(TestResources.GetStore(), "Resources.Testing.Fonts.Fnt.OpenSans");
            var glyphStore = new FntGlyphStore(fontResourceStore, "OpenSans-Regular");
            glyphStore.LoadFontAsync().Wait();

            // make sure glyph are loaded.
            var normalGlyph = glyphStore.Get('a');
            if (normalGlyph == null)
                throw new ArgumentNullException(nameof(normalGlyph));

            font = glyphStore.BitmapFont;
        }

        [TestCase("A", 1)]
        [TestCase("a", 1)]
        [TestCase("カラオケ", 0)]
        [TestCase(null, 1)]
        public void TestCompress(string chars, int charAmount)
        {
            var result = BitmapFontCompressor.Compress(font, chars?.ToArray());

            // info and common should just copy.
            ObjectAssert.ArePropertyEqual(result.Info, font.Info);
            ObjectAssert.ArePropertyEqual(result.Common, font.Common);

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
            var characters = pages.Select(x => new Character { Page = x }).ToArray();

            try
            {
                var result = BitmapFontCompressor.GeneratePages(font.Pages, characters);
                Assert.AreEqual(result.Values.ToArray(), pageNames);
            }
            catch
            {
                int storePage = font.Pages.Max(x => x.Key);
                Assert.Greater(pageNames.Length, storePage);
            }
        }

        [TestCase("A")]
        [TestCase("ABC")]
        [TestCase("abc")]
        [TestCase("!!!!")]
        [TestCase("カラオケ")] // should not have any text if cannot get character in origin font.
        public void TestGenerateCharactersPropertyWithSingleLine(string chars)
        {
            var characters = font.Characters;
            int spacing = font.Info.SpacingHorizontal;
            int topPadding = font.Info.PaddingUp;

            var result = BitmapFontCompressor.GenerateCharacters(font.Info, font.Common, font.Characters, chars.ToArray());

            foreach ((int c, var character) in result)
            {
                // check some property should be same as origin character.
                var originCharacter = characters[c];
                Assert.IsNotNull(originCharacter);
                Assert.AreEqual(character.Width, originCharacter.Width);
                Assert.AreEqual(character.Height, originCharacter.Height);
                Assert.AreEqual(character.XOffset, originCharacter.XOffset);
                Assert.AreEqual(character.YOffset, originCharacter.YOffset);
                Assert.AreEqual(character.XAdvance, originCharacter.XAdvance);
                Assert.AreEqual(character.Channel, originCharacter.Channel);

                // test previous position should smaller the current one.
                var previousChar = result.Values.GetPrevious(character);
                if (previousChar == null)
                    return;

                // all the test case can be finished in single line.
                Assert.AreEqual(previousChar.X + previousChar.Width + spacing, character.X);
                Assert.AreEqual(previousChar.Y, topPadding);
                Assert.AreEqual(previousChar.Page, 0);
            }
        }

        [TestCase("A")]
        [TestCase("ABC")]
        [TestCase("abc")]
        [TestCase("1234567890")]
        [TestCase("!!!!")]
        [TestCase("カラオケ")] // should not have any text if cannot get character in origin font.
        public void TestGenerateCharactersPropertyWithMultiLine(string chars)
        {
            // make sure that will change new line if print next chars.
            var bitmapFontCommon = new BitmapFontCommon
            {
                ScaleWidth = 0,
                ScaleHeight = int.MaxValue,
            };
            int spacing = font.Info.SpacingVertical;
            int leftPadding = font.Info.PaddingUp;

            var result = BitmapFontCompressor.GenerateCharacters(font.Info, bitmapFontCommon, font.Characters, chars.ToArray());

            foreach (var (_, character) in result)
            {
                // test previous position should smaller the current one.
                var previousChar = result.Values.GetPrevious(character);
                if (previousChar == null)
                    return;

                // all the test case can be finished in different line.
                Assert.AreEqual(previousChar.X, leftPadding);
                Assert.AreEqual(previousChar.Y + previousChar.Height + spacing, character.Y);
                Assert.AreEqual(previousChar.Page, 0);
            }
        }

        [TestCase("A")]
        [TestCase("ABC")]
        [TestCase("abc")]
        [TestCase("1234567890")]
        [TestCase("!!!!")]
        [TestCase("カラオケ")] // should not have any text if cannot get character in origin font.
        public void TestGenerateCharactersPropertyWithMultiPage(string chars)
        {
            // make sure that will change new page if print next chars.
            var bitmapFontCommon = new BitmapFontCommon
            {
                ScaleWidth = 0,
                ScaleHeight = 0,
            };
            int page = 0;
            int topPadding = font.Info.PaddingUp;
            int leftPadding = font.Info.PaddingUp;

            var result = BitmapFontCompressor.GenerateCharacters(font.Info, bitmapFontCommon, font.Characters, chars.ToArray());

            foreach (var (_, character) in result)
            {
                // test previous position should smaller the current one.
                var previousChar = result.Values.GetPrevious(character);
                if (previousChar == null)
                    return;

                // all the test case can be finished in single line, so just test x position.
                Assert.AreEqual(previousChar.X, leftPadding);
                Assert.AreEqual(previousChar.Y, topPadding);
                Assert.AreEqual(previousChar.Page, page);
                page++;
            }
        }

        [Test]
        public void TestGenerateAllCharacters()
        {
            char[] chars = font.Characters.Keys.Select(x => (char)x).ToArray();
            var characters = font.Characters;

            // make sure that no characters is missing.
            // not checking position because algorithm might not save as original one.
            var result = BitmapFontCompressor.GenerateCharacters(font.Info, font.Common, font.Characters, chars);
            Assert.AreEqual(result.Count, characters.Count);
        }

        [TestCase("カラオケ", 0)]
        [TestCase("からおけ", 0)]
        [TestCase("カラオケ(karaoke)", 7)]
        public void TestGenerateCharactersIfNotExist(string chars, int amount)
        {
            var result = BitmapFontCompressor.GenerateCharacters(font.Info, font.Common, font.Characters, chars.ToArray());
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
            var result = BitmapFontCompressor.GenerateKerningPairs(font.KerningPairs, chars?.ToArray());
            Assert.AreEqual(result.Count, amount);
        }

        [Test]
        public void TestGenerateKerningPairsWithAllChars()
        {
            char[] chars = font.Characters.Keys.Select(x => (char)x).ToArray();
            var kerningPairs = font.KerningPairs;

            // make sure that no kerning is missing.
            var result = BitmapFontCompressor.GenerateKerningPairs(kerningPairs, chars);
            Assert.AreEqual(result.Count, kerningPairs.Count);
        }
    }
}
