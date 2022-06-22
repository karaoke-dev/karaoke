// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

#nullable disable

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

        [TestCase(TestEnum.Enum1, TestEnum.Enum3)]
        [TestCase(TestEnum.Enum2, TestEnum.Enum1)]
        [TestCase(TestEnum.Enum3, TestEnum.Enum2)]
        public void TestGetPreviousValue(TestEnum current, TestEnum expected)
        {
            var actual = EnumUtils.GetPreviousValue(current);
            Assert.AreEqual(expected, actual);
        }

        [TestCase(TestEnum.Enum1, TestEnum.Enum2)]
        [TestCase(TestEnum.Enum2, TestEnum.Enum3)]
        [TestCase(TestEnum.Enum3, TestEnum.Enum1)]
        public void TestGetNextValue(TestEnum current, TestEnum expected)
        {
            var actual = EnumUtils.GetNextValue(current);
            Assert.AreEqual(expected, actual);
        }

        public enum TestEnum
        {
            Enum1,

            Enum2,

            Enum3,
        }
    }
}
