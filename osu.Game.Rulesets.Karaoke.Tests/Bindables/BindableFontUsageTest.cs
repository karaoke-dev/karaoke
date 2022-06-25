// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using NUnit.Framework;
using osu.Framework.Graphics.Sprites;
using osu.Game.Rulesets.Karaoke.Bindables;

namespace osu.Game.Rulesets.Karaoke.Tests.Bindables
{
    public class BindableFontUsageTest
    {
        [TestCase("family=f weight=w size=10 italics=true fixedWidth=true", "f", 10, "w", true, true)]
        [TestCase("Font=f-w Size=10 Italics=true FixedWidth=true", "f", 10, "w", true, true)]
        public void TestParsingString(string value, string family, float size, string weight = null!, bool italics = false, bool fixedWidth = false)
        {
            var bindable = new BindableFontUsage();
            bindable.Parse(value);

            var expected = new FontUsage(family, size, weight, italics, fixedWidth);
            var actual = bindable.Value;
            Assert.AreEqual(expected, actual);
        }
    }
}
