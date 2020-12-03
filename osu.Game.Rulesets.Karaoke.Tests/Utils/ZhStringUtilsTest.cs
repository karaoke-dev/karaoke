// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using NUnit.Framework;
using osu.Game.Rulesets.Karaoke.Utils;

namespace osu.Game.Rulesets.Karaoke.Tests.Utils
{
    [TestFixture]
    public class ZhStringUtilsTest
    {
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
            Assert.AreEqual(ZhStringUtils.IsChinese(c), result);
        }
    }
}
