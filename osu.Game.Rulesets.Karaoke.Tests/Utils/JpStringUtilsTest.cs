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

        [TestCase("はなび", "hanabi")]
        [TestCase("たいかい", "taikai")]
        [TestCase("ハナビ", "hanabi")]
        [TestCase("タイカイ", "taikai")]
        [TestCase("花火大会", "花火大会")] // cannot convert kanji to romaji.
        [TestCase("ハナビ wo miru", "hanabi wo miru")]
        [TestCase("タイカイー☆", "taikaii☆")] // it's converted by package, let's skip this checking.
        [TestCase("タイカイ ー☆", "taikai -☆")] // it's converted by package, let's skip this checking.
        public void TestToRomaji(string text, string actual)
        {
            var romaji = JpStringUtils.ToRomaji(text);
            Assert.AreEqual(romaji, actual);
        }
    }
}
