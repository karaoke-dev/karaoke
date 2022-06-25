// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using NUnit.Framework;
using osu.Game.Rulesets.Karaoke.Utils;

namespace osu.Game.Rulesets.Karaoke.Tests.Utils
{
    [TestFixture]
    public class FontUtilsTest
    {
        [TestCase(10, "10 px")]
        [TestCase(10.5f, "10.5 px")]
        [TestCase(-3, "-3 px")]
        public void TestGetText(float font, string expected)
        {
            string actual = FontUtils.GetText(font);
            Assert.AreEqual(expected, actual);
        }
    }
}
