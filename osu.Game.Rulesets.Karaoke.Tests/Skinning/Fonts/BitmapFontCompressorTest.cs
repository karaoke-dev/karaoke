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

namespace osu.Game.Rulesets.Karaoke.Tests.Skinning.Fonts;

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

        font = glyphStore.BitmapFont ?? throw new InvalidOperationException("Font should not be null in the test case.");
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
        Assert.That(result.Pages, Is.Not.Null);

        if (!string.IsNullOrEmpty(chars) && charAmount > 0)
        {
            Assert.That(result.Pages.Count, Is.Not.EqualTo(0));
        }

        // should have chars if have char.
        Assert.That(result.Characters, Is.Not.Null);

        if (!string.IsNullOrEmpty(chars))
        {
        Assert.That(result.Characters.Count, Is.EqualTo(charAmount));
        }

        // kerning pairs amount might be zero but cannot be null.
        Assert.That(result.KerningPairs, Is.Not.Null);
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
            Assert.That(expected, Is.EqualTo(actual));
        }
        catch
        {
            int expectedPageSize = expected.Length;
            int storePage = font.Pages.Max(x => x.Key);
            Assert.That(expectedPageSize, Is.GreaterThan(storePage));
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
            Assert.That(expected, Is.Not.Null);
            Assert.That(character.Width, Is.EqualTo(expected.Width));
            Assert.That(character.Height, Is.EqualTo(expected.Height));
            Assert.That(character.XOffset, Is.EqualTo(expected.XOffset));
            Assert.That(character.YOffset, Is.EqualTo(expected.YOffset));
            Assert.That(character.XAdvance, Is.EqualTo(expected.XAdvance));
            Assert.That(character.Channel, Is.EqualTo(expected.Channel));

            // test previous position should smaller the current one.
            var previousChar = result.Values.GetPrevious(character);
            if (previousChar == null)
                return;

            // all the test case can be finished in single line.
            Assert.That(character.X, Is.EqualTo(previousChar.X + previousChar.Width + spacing));
            Assert.That(previousChar.Y, Is.EqualTo(topPadding));
            Assert.That(previousChar.Page, Is.EqualTo(0));
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
            Assert.That(previousChar.X, Is.EqualTo(leftPadding));
            Assert.That(character.Y, Is.EqualTo(previousChar.Y + previousChar.Height + spacing));
            Assert.That(previousChar.Page, Is.EqualTo(0));
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
            Assert.That(previousChar.X, Is.EqualTo(leftPadding));
            Assert.That(previousChar.Y, Is.EqualTo(topPadding));
            Assert.That(previousChar.Page, Is.EqualTo(page));
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
        Assert.That(expected, Is.EqualTo(actual));
    }

    [TestCase("カラオケ", 0)]
    [TestCase("からおけ", 0)]
    [TestCase("カラオケ(karaoke)", 7)]
    public void TestGenerateCharactersIfNotExist(string chars, int expected)
    {
        var result = BitmapFontCompressor.GenerateCharacters(font.Info, font.Common, font.Characters, chars.ToArray());

        int actual = result.Count;
        Assert.That(expected, Is.EqualTo(actual));
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
        Assert.That(expected, Is.EqualTo(actual));
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
        Assert.That(expected, Is.EqualTo(actual));
    }
}
