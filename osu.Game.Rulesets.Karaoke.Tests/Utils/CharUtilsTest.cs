// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using NUnit.Framework;
using osu.Game.Rulesets.Karaoke.Utils;

namespace osu.Game.Rulesets.Karaoke.Tests.Utils
{
    [TestFixture]
    public class CharUtilsTest
    {
        [TestCase('ひ', true)]
        [TestCase('び', true)]
        [TestCase('ぴ', true)]
        [TestCase('カ', true)]
        [TestCase('ガ', true)]
        [TestCase('゠', true)]
        [TestCase('・', true)]
        [TestCase('ー', true)]
        [TestCase('a', false)]
        [TestCase('1', false)]
        public void TestIsKana(char c, bool match)
        {
            var isKana = CharUtils.IsKana(c);
            Assert.AreEqual(isKana, match);
        }

        [TestCase('A', true)]
        [TestCase('a', true)]
        [TestCase('Ｚ', true)]
        [TestCase('ｚ', true)]
        [TestCase('1', false)]
        [TestCase('文', false)]
        public void TestIsLatin(char c, bool match)
        {
            var isLatin = CharUtils.IsLatin(c);
            Assert.AreEqual(isLatin, match);
        }

        [TestCase(':', true)]
        [TestCase('"', true)]
        [TestCase('&', true)]
        [TestCase('#', true)]
        [TestCase('@', true)]
        [TestCase('A', false)]
        public void TestIsAsciiSymbol(char c, bool match)
        {
            var isAsciiSymbol = CharUtils.IsAsciiSymbol(c);
            Assert.AreEqual(isAsciiSymbol, match);
        }

        [TestCase('你', true)]
        [TestCase('好', true)]
        [TestCase('世', true)]
        [TestCase('界', true)]
        [TestCase('A', false)]
        [TestCase('a', false)]
        [TestCase('Ａ', false)]
        [TestCase('ａ', false)]
        [TestCase('~', false)]
        [TestCase('～', false)]
        [TestCase('ハ', false)]
        [TestCase('は', false)]
        [TestCase('ハ', false)]
        public void TestIsChinese(char c, bool result)
        {
            var isChinese = CharUtils.IsChinese(c);
            Assert.AreEqual(isChinese, result);
        }
    }
}
