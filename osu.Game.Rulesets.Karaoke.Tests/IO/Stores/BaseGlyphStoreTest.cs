﻿// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using NUnit.Framework;
using osu.Framework.Extensions;
using osu.Framework.Graphics.Textures;
using osu.Framework.IO.Stores;
using osu.Game.Rulesets.Karaoke.Tests.Resources;

namespace osu.Game.Rulesets.Karaoke.Tests.IO.Stores;

/// <summary>
/// Use <see cref="GlyphStore"/> as test case actual result for comparing.
/// </summary>
/// <typeparam name="TGlyphStore"></typeparam>
public abstract class BaseGlyphStoreTest<TGlyphStore> where TGlyphStore : class, IGlyphStore
{
    protected TGlyphStore CustomizeGlyphStore { get; private set; } = null!;

    protected GlyphStore GlyphStore { get; private set; } = null!;

    [OneTimeSetUp]
    protected void OneTimeSetUp()
    {
        // create and load glyph store.
        var fontResourceStore = new NamespacedResourceStore<byte[]>(TestResources.GetStore(), "Resources.Testing.Fonts.Fnt.OpenSans");
        GlyphStore = new GlyphStore(fontResourceStore, FontName);
        GlyphStore.LoadFontAsync().WaitSafely();

        // create load load customize glyph store.
        var customizeFontResourceStore = new NamespacedResourceStore<byte[]>(TestResources.GetStore(), $"Resources.Testing.Fonts.{FontType}");
        CustomizeGlyphStore = CreateFontStore(customizeFontResourceStore, FontName);
        CustomizeGlyphStore.LoadFontAsync().WaitSafely();
    }

    protected abstract string FontType { get; }

    protected abstract string FontName { get; }

    protected abstract TGlyphStore CreateFontStore(ResourceStore<byte[]> store, string assetName);

    [Test]
    public void TestCompareFontNameWithOrigin()
    {
        string expected = GlyphStore.FontName;
        string actual = CustomizeGlyphStore.FontName;
        Assert.That(actual, Is.EqualTo(expected));
    }

    [TestCase('a')]
    [TestCase(' ')]
    [TestCase('ㄅ')] // should not have those texts in store.
    [TestCase('あ')] // should not have those texts in store.
    public void TestCompareHasGlyphWithOrigin(char c)
    {
        bool expected = GlyphStore.HasGlyph(c);
        bool actual = CustomizeGlyphStore.HasGlyph(c);
        Assert.That(actual, Is.EqualTo(expected));
    }

    [Test]
    public void TestCompareGetBaseHeightWithOrigin()
    {
        float? expected = GlyphStore.Baseline;
        float? actual = CustomizeGlyphStore.Baseline;
        Assert.That(actual, Is.EqualTo(expected));
    }

    [TestCase('a')]
    [TestCase('A')]
    [TestCase('1')]
    [TestCase(' ')]
    [TestCase('@')]
    [TestCase('#')]
    public void TestCompareGetCharacterGlyphWithOrigin(char c)
    {
        var expected = GlyphStore.Get(c)!;
        var actual = CustomizeGlyphStore.Get(c)!;

        // because get character glyph should make sure that this glyph store contains char, so will not be null.
        Assert.That(expected, Is.Not.Null);
        Assert.That(actual, Is.Not.Null);

        // test all property should be matched.
        Assert.That(actual.Character, Is.EqualTo(expected.Character));
        Assert.That(actual.XOffset, Is.EqualTo(expected.XOffset));
        Assert.That(actual.YOffset, Is.EqualTo(expected.YOffset));
        Assert.That(actual.XAdvance, Is.EqualTo(expected.XAdvance));
    }

    [TestCase('a', 'a')]
    [TestCase('a', 'b')]
    [TestCase('i', 'v')] // todo: should got a result that is not zero.
    [TestCase('t', 'i')]
    [TestCase('a', 'あ')]
    public void TestCompareGetKerningWithOrigin(char left, char right)
    {
        int expected = GlyphStore.GetKerning(left, right);
        int actual = CustomizeGlyphStore.GetKerning(left, right);
        Assert.That(actual, Is.EqualTo(expected));
    }

    [TestCase('a')]
    [TestCase('A')]
    [TestCase('1')]
    [TestCase(' ')]
    [TestCase('@')]
    [TestCase('#')]
    public void TestCompareGetTextureUploadWithOrigin(char c)
    {
        var expected = GlyphStore.Get(new string(new[] { c }));
        var actual = (CustomizeGlyphStore as IResourceStore<TextureUpload>)?.Get(new string(new[] { c }));

        if (expected == null || actual == null)
            throw new ArgumentNullException();

        // todo : should test with pixel perfect, but it's ok to pass if size is almost the same.
        Assert.That(actual.Width, Is.EqualTo(expected.Width));
        Assert.That(actual.Height, Is.EqualTo(expected.Height));
    }
}
