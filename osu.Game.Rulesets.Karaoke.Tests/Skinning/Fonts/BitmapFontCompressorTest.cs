// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using System.Linq;
using NUnit.Framework;
using osu.Framework.Extensions;
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
        private BitmapFont font = null!;

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            var fontResourceStore = new NamespacedResourceStore<byte[]>(TestResources.GetStore(), "Resources.Testing.Fonts.Fnt.OpenSans");
            var glyphStore = new FntGlyphStore(fontResourceStore, "OpenSans-Regular");
            glyphStore.LoadFontAsync().WaitSafely();

            // make sure glyph are loaded.
            var normalGlyph = glyphStore.Get('a');
            if (normalGlyph == null)
                throw new ArgumentNullException(nameof(normalGlyph));

            font = glyphStore.BitmapFont;
        }

        [TestCase("A", 1)]
        [TestCase("a", 1)]
        [TestCase("カラオケ", 0)]
        [TestCase("", 1)]
        public void TestCompress(string chars, int charAmount)
        {
            var result = BitmapFontCompressor.Compress(font, chars.ToArray());

            // info and common should just copy.
            ObjectAssert.ArePropertyEqual(font.Info, result.Info);
            ObjectAssert.ArePropertyEqual(font.Common, result.Common);

            // should have page if have char.
            Assert.IsNotNull(result.Pages);

            if (!string.IsNullOrEmpty(chars) && charAmount > 0)
            {
                Assert.NotZero(result.Pages.Count);
            }

            // should have chars if have char.
            Assert.IsNotNull(result.Characters);

            if (!string.IsNullOrEmpty(chars))
            {
                Assert.AreEqual(result.Characters.Count, charAmount);
            }

            // kerning pairs amount might be zero but cannot be null.
            Assert.IsNotNull(result.KerningPairs);
        }

        [TestCase(new int[] { }, new string[] { })]
        [TestCase(new[] { 0 }, new[] { "OpenSans_0.png" })] // max store page is start from 0.
        [TestCase(new[] { 0, 1 }, new[] { "OpenSans_0.png" })] // should not have the case that more then origin page number.
        public void TestGeneratePage(int[] pages, string[] expected)
        {
            var characters = pages.Select(x => new Character { Page = x }).ToArray();

            try
            {
                string[] actual = BitmapFontCompressor.GeneratePages(font.Pages, characters).Values.ToArray();
                Assert.AreEqual(expected, actual);
            }
            catch
            {
                int expectedPageSize = expected.Length;
                int storePage = font.Pages.Max(x => x.Key);
                Assert.Greater(expectedPageSize, storePage);
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
                var expected = characters[c];
                Assert.IsNotNull(expected);
                Assert.AreEqual(expected.Width, character.Width);
                Assert.AreEqual(expected.Height, character.Height);
                Assert.AreEqual(expected.XOffset, character.XOffset);
                Assert.AreEqual(expected.YOffset, character.YOffset);
                Assert.AreEqual(expected.XAdvance, character.XAdvance);
                Assert.AreEqual(expected.Channel, character.Channel);

                // test previous position should smaller the current one.
                var previousChar = result.Values.GetPrevious(character);
                if (previousChar == null)
                    return;

                // all the test case can be finished in single line.
                Assert.AreEqual(character.X, previousChar.X + previousChar.Width + spacing);
                Assert.AreEqual(topPadding, previousChar.Y);
                Assert.AreEqual(0, previousChar.Page);
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
                Assert.AreEqual(leftPadding, previousChar.X);
                Assert.AreEqual(character.Y, previousChar.Y + previousChar.Height + spacing);
                Assert.AreEqual(0, previousChar.Page);
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
                Assert.AreEqual(leftPadding, previousChar.X);
                Assert.AreEqual(topPadding, previousChar.Y);
                Assert.AreEqual(page, previousChar.Page);
                page++;
            }
        }

        [Test]
        public void TestGenerateAllCharacters()
        {
            // make sure that no characters is missing.
            // not checking position because algorithm might not save as original one.
            char[] chars = font.Characters.Keys.Select(x => (char)x).ToArray();
            var result = BitmapFontCompressor.GenerateCharacters(font.Info, font.Common, font.Characters, chars);

            int expected = font.Characters.Count;
            int actual = result.Count;
            Assert.AreEqual(expected, actual);
        }

        [TestCase("カラオケ", 0)]
        [TestCase("からおけ", 0)]
        [TestCase("カラオケ(karaoke)", 7)]
        public void TestGenerateCharactersIfNotExist(string chars, int expected)
        {
            var result = BitmapFontCompressor.GenerateCharacters(font.Info, font.Common, font.Characters, chars.ToArray());

            int actual = result.Count;
            Assert.AreEqual(expected, actual);
        }

        [TestCase("", 0)]
        [TestCase("a", 0)] // should not have any kerning pair if has zero or one char.
        [TestCase("aaaaa", 0)] // same char should not have kerning pair.
        [TestCase("ab", 0)] // don't worry. some of pairs does not have kerning pair.
        [TestCase("AB", 1)]
        [TestCase("ABC", 3)]
        public void TestGenerateKerningPairs(string chars, int expected)
        {
            var result = BitmapFontCompressor.GenerateKerningPairs(font.KerningPairs, chars.ToArray());

            int actual = result.Count;
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void TestGenerateKerningPairsWithAllChars()
        {
            // make sure that no kerning is missing.
            char[] chars = font.Characters.Keys.Select(x => (char)x).ToArray();
            var kerningPairs = font.KerningPairs;
            var result = BitmapFontCompressor.GenerateKerningPairs(kerningPairs, chars);

            int expected = kerningPairs.Count;
            int actual = result.Count;
            Assert.AreEqual(expected, actual);
        }
    }
}
