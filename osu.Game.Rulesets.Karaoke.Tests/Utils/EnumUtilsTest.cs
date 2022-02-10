// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using NUnit.Framework;
using osu.Game.Rulesets.Karaoke.Utils;

namespace osu.Game.Rulesets.Karaoke.Tests.Utils
{
    [TestFixture]
    public class EnumUtilsTest
    {
        [Test]
        public void TestGetValues()
        {
            var expected = new[]
            {
                TestEnum.Enum1,
                TestEnum.Enum2,
                TestEnum.Enum3
            };
            var actual = EnumUtils.GetValues<TestEnum>();
            Assert.AreEqual(expected, actual);
        }

        internal enum TestEnum
        {
            Enum1,

            Enum2,

            Enum3,
        }
    }
}
