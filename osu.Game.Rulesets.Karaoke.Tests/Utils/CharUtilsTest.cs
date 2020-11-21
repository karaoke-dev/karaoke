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
            var isKaka = CharUtils.IsKana(c);
            Assert.AreEqual(isKaka, match);
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
        public void TestIsASCIISymbol(char c, bool match)
        {
            var isAsciiSymbol = CharUtils.IsAsciiSymbol(c);
            Assert.AreEqual(isAsciiSymbol, match);
        }
    }
}
