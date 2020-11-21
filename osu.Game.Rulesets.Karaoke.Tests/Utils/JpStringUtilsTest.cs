// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using NUnit.Framework;
using osu.Game.Rulesets.Karaoke.Utils;

namespace osu.Game.Rulesets.Karaoke.Tests.Utils
{
    [TestFixture]
    public class JpStringUtilsTest
    {
        [TestCase("ハナビ", "はなび")]
        [TestCase("タイカイ", "たいかい")]
        [TestCase("花火大会", "花火大会")]
        public void TestToHiragana(string text, string actual)
        {
            var katakana = JpStringUtils.ToHiragana(text);
            Assert.AreEqual(katakana, actual);
        }

        [TestCase("はなび", "ハナビ")]
        [TestCase("たいかい", "タイカイ")]
        [TestCase("花火大会", "花火大会")]
        public void TestToKatakana(string text, string actual)
        {
            var katakana = JpStringUtils.ToKatakana(text);
            Assert.AreEqual(katakana, actual);
        }
    }
}
