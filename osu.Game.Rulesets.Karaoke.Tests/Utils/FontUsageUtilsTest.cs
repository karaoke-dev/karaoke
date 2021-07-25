// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using NUnit.Framework;
using osu.Framework.Graphics.Sprites;
using osu.Game.Rulesets.Karaoke.Utils;

namespace osu.Game.Rulesets.Karaoke.Tests.Utils
{
    [TestFixture]
    public class FontUsageUtilsTest
    {
        [TestCase("OpenSans", null, false, "OpenSans")]
        [TestCase("OpenSans", "Regular", false, "OpenSans-Regular")]
        [TestCase("OpenSans", "Regular", true, "OpenSans-RegularItalic")]
        public void TestToFontInfo(string family, string weight, bool italics, string fontName)
        {
            var fontUsage = new FontUsage(fontName);
            var fontInfo = FontUsageUtils.ToFontInfo(fontUsage);
            Assert.AreEqual(fontInfo.FontName, fontName);
            Assert.AreEqual(fontInfo.Family, family);

            // note: font info should not follow rules as fontUsage.
            if (!italics)
            {
                Assert.AreEqual(fontInfo.Weight, weight);
            }
        }
    }
}
