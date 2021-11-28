// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using NUnit.Framework;
using osu.Framework.Graphics.Sprites;
using osu.Game.Rulesets.Karaoke.Utils;

namespace osu.Game.Rulesets.Karaoke.Tests.Utils
{
    public class TypeUtilsTest
    {
        [Test]
        public void TestChangeTypeToSameType()
        {
            // test string
            Assert.AreEqual(TypeUtils.ChangeType<string>("123"), "123");

            // test number
            Assert.AreEqual(TypeUtils.ChangeType<int>(456), 456);

            // test another number
            Assert.AreEqual(TypeUtils.ChangeType<float>(789f), 789f);

            // test struct
            Assert.AreEqual(TypeUtils.ChangeType<FontUsage>(new FontUsage("123")), new FontUsage("123"));

            // test class, should use same instance.
            var testClass = new TestClass(123);
            Assert.AreEqual(TypeUtils.ChangeType<TestClass>(testClass), testClass);
        }

        [Test]
        public void TestChangeTypeToDifferentType()
        {
            // test convert to number
            Assert.AreEqual(TypeUtils.ChangeType<double>(123), Convert.ToDouble(123));

            // test convert to string
            Assert.AreEqual(TypeUtils.ChangeType<string>(123), Convert.ToString(123));

            // test convert to nullable
            Assert.AreEqual(TypeUtils.ChangeType<double?>(123d), 123);
            Assert.AreEqual(TypeUtils.ChangeType<double?>(null), default(double?));
        }

        private class TestClass
        {
            private readonly int value;

            public TestClass(int value)
            {
                this.value = value;
            }
        }
    }
}
