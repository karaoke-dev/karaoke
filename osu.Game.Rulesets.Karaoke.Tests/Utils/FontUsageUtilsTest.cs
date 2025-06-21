// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using NUnit.Framework;
using osu.Framework.Graphics.Sprites;
using osu.Game.Rulesets.Karaoke.Skinning.Fonts;
using osu.Game.Rulesets.Karaoke.Utils;

namespace osu.Game.Rulesets.Karaoke.Tests.Utils;

[TestFixture]
public class FontUsageUtilsTest
{
    [TestCase("OpenSans", null, false, "OpenSans")]
    [TestCase("OpenSans", "Regular", false, "OpenSans-Regular")]
    [TestCase("OpenSans", "Regular", true, "OpenSans-RegularItalic")]
    public void TestToFontInfo(string expectedFamily, string? expectedWeight, bool italics, string expectedFontName)
    {
        var fontUsage = new FontUsage(expectedFontName);
        var fontInfo = FontUsageUtils.ToFontInfo(fontUsage, FontFormat.Internal);
        Assert.That(fontInfo.FontName, Is.EqualTo(expectedFontName));
        Assert.That(fontInfo.Family, Is.EqualTo(expectedFamily));

        // note: font info should not follow rules as fontUsage.
        if (!italics)
        {
            Assert.That(fontInfo.Weight, Is.EqualTo(expectedWeight));
        }
    }
}
