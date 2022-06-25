// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using NUnit.Framework;
using osu.Game.Rulesets.Karaoke.Utils;

namespace osu.Game.Rulesets.Karaoke.Tests.Utils
{
    [TestFixture]
    public class CharUtilsTest
    {
        [TestCase(' ', true)]
        [TestCase('　', true)]
        [TestCase('ぴ', false)]
        public void TestIsSpacing(char c, bool expected)
        {
            bool actual = CharUtils.IsSpacing(c);
            Assert.AreEqual(expected, actual);
        }

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
        public void TestIsKana(char c, bool expected)
        {
            bool actual = CharUtils.IsKana(c);
            Assert.AreEqual(expected, actual);
        }

        [TestCase('A', true)]
        [TestCase('a', true)]
        [TestCase('Ｚ', true)]
        [TestCase('ｚ', true)]
        [TestCase('1', false)]
        [TestCase('文', false)]
        public void TestIsLatin(char c, bool expected)
        {
            bool actual = CharUtils.IsLatin(c);
            Assert.AreEqual(expected, actual);
        }

        [TestCase(':', true)]
        [TestCase('"', true)]
        [TestCase('&', true)]
        [TestCase('#', true)]
        [TestCase('@', true)]
        [TestCase('A', false)]
        public void TestIsAsciiSymbol(char c, bool expected)
        {
            bool actual = CharUtils.IsAsciiSymbol(c);
            Assert.AreEqual(expected, actual);
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
        public void TestIsChinese(char c, bool expected)
        {
            bool actual = CharUtils.IsChinese(c);
            Assert.AreEqual(expected, actual);
        }
    }
}
