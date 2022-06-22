// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

#nullable disable

using NUnit.Framework;
using osu.Game.Rulesets.Karaoke.Tests.Helper;

namespace osu.Game.Rulesets.Karaoke.Tests.Objects
{
    [TestFixture]
    public class ToneCalculationTest
    {
        [TestCase(1, 1, 2)]
        [TestCase(-1, 1, 0)]
        [TestCase(1, -1, 0)]
        [TestCase(1.5, 2.5, 4)]
        [TestCase(-1.5, 2.5, 1)]
        [TestCase(1.5, -2.5, -1)]
        public void TestOperatorPlus(double tone1, double tone2, double tone)
        {
            var expected = TestCaseToneHelper.NumberToTone(tone);
            var actual = TestCaseToneHelper.NumberToTone(tone1) + TestCaseToneHelper.NumberToTone(tone2);
            Assert.AreEqual(expected, actual);
        }

        [TestCase(1, 1, 2)]
        [TestCase(-1, 1, 0)]
        [TestCase(1, -1, 0)]
        public void TestOperatorPlusWithInt(double tone1, int scale1, double tone)
        {
            var expected = TestCaseToneHelper.NumberToTone(tone);
            var actual = TestCaseToneHelper.NumberToTone(tone1) + scale1;
            Assert.AreEqual(expected, actual);
        }

        [TestCase(1, 1, 0)]
        [TestCase(-1, 1, -2)]
        [TestCase(1, -1, 2)]
        [TestCase(1.5, 2.5, -1)]
        [TestCase(-1.5, 2.5, -4)]
        [TestCase(1.5, -2.5, 4)]
        public void TestOperatorMinus(double tone1, double tone2, double tone)
        {
            var expected = TestCaseToneHelper.NumberToTone(tone);
            var actual = TestCaseToneHelper.NumberToTone(tone1) - TestCaseToneHelper.NumberToTone(tone2);
            Assert.AreEqual(expected, actual);
        }

        [TestCase(1, 1, 0)]
        [TestCase(-1, 1, -2)]
        [TestCase(1, -1, 2)]
        public void TestOperatorMinusWithInt(double tone1, int scale1, double tone)
        {
            var expected = TestCaseToneHelper.NumberToTone(tone);
            var actual = TestCaseToneHelper.NumberToTone(tone1) - scale1;
            Assert.AreEqual(expected, actual);
        }

        [TestCase(1, 1)]
        [TestCase(1.5, 1.5)]
        [TestCase(-1.5, -1.5)]
        public void TestOperatorEqual(double tone1, double tone2)
        {
            var expected = TestCaseToneHelper.NumberToTone(tone1);
            var actual = TestCaseToneHelper.NumberToTone(tone2);
            Assert.AreEqual(expected, actual);
        }

        [TestCase(1, 1)]
        [TestCase(-1, -1)]
        public void TestOperatorEqualWithInt(double tone1, int scale1)
        {
            Assert.IsTrue(TestCaseToneHelper.NumberToTone(tone1) == scale1);
        }

        [TestCase(-1, 1)]
        [TestCase(1, -1)]
        [TestCase(1.5, -1.5)]
        [TestCase(1.5, 1)]
        [TestCase(-1.5, -1)]
        [TestCase(-1.5, -2)]
        public void TestOperatorNotEqual(double tone1, double tone2)
        {
            var expected = TestCaseToneHelper.NumberToTone(tone1);
            var actual = TestCaseToneHelper.NumberToTone(tone2);
            Assert.AreNotEqual(expected, actual);
        }

        [TestCase(-1, 1)]
        [TestCase(1, -1)]
        [TestCase(-1.5, -1)]
        [TestCase(-1.5, -2)]
        public void TestOperatorNotEqualWithInt(double tone1, int scale1)
        {
            Assert.IsTrue(TestCaseToneHelper.NumberToTone(tone1) != scale1);
        }

        [TestCase(1, 0, true)]
        [TestCase(1, 0.5, true)]
        [TestCase(1, 1, false)]
        [TestCase(1, 1.5, false)]
        public void TestOperatorGreater(double tone1, double tone2, bool expected)
        {
            bool actual = TestCaseToneHelper.NumberToTone(tone1) > TestCaseToneHelper.NumberToTone(tone2);
            Assert.AreEqual(expected, actual);
        }

        [TestCase(1, 0, true)]
        [TestCase(1, 1, false)]
        public void TestOperatorGreaterWithInt(double tone1, int scale1, bool expected)
        {
            bool actual = TestCaseToneHelper.NumberToTone(tone1) > scale1;
            Assert.AreEqual(expected, actual);
        }

        [TestCase(1, 0, true)]
        [TestCase(1, 0.5, true)]
        [TestCase(1, 1, true)]
        [TestCase(1, 1.5, false)]
        public void TestOperatorGreaterOrEqual(double tone1, double tone2, bool expected)
        {
            bool actual = TestCaseToneHelper.NumberToTone(tone1) >= TestCaseToneHelper.NumberToTone(tone2);
            Assert.AreEqual(expected, actual);
        }

        [TestCase(1, 0, true)]
        [TestCase(1, 1, true)]
        public void TestOperatorGreaterOrEqualWithInt(double tone1, int scale1, bool expected)
        {
            bool actual = TestCaseToneHelper.NumberToTone(tone1) >= scale1;
            Assert.AreEqual(expected, actual);
        }

        [TestCase(-1, 0, true)]
        [TestCase(-1, -0.5, true)]
        [TestCase(-1, -1, false)]
        [TestCase(-1, -1.5, false)]
        public void TestOperatorLess(double tone1, double tone2, bool expected)
        {
            bool actual = TestCaseToneHelper.NumberToTone(tone1) < TestCaseToneHelper.NumberToTone(tone2);
            Assert.AreEqual(expected, actual);
        }

        [TestCase(-1, 0, true)]
        [TestCase(-1, -1, false)]
        public void TestOperatorLessWithInt(double tone1, int scale1, bool expected)
        {
            bool actual = TestCaseToneHelper.NumberToTone(tone1) < scale1;
            Assert.AreEqual(expected, actual);
        }

        [TestCase(-1, 0, true)]
        [TestCase(-1, -0.5, true)]
        [TestCase(-1, -1, true)]
        [TestCase(-1, -1.5, false)]
        public void TestOperatorLessOrEqual(double tone1, double tone2, bool expected)
        {
            bool actual = TestCaseToneHelper.NumberToTone(tone1) <= TestCaseToneHelper.NumberToTone(tone2);
            Assert.AreEqual(expected, actual);
        }

        [TestCase(-1, 0, true)]
        [TestCase(-1, -1, true)]
        public void TestOperatorLessOrEqualWithInt(double tone1, int scale1, bool expected)
        {
            bool actual = TestCaseToneHelper.NumberToTone(tone1) <= scale1;
            Assert.AreEqual(expected, actual);
        }
    }
}
