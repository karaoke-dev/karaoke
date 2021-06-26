// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Globalization;
using NUnit.Framework;
using osu.Game.Rulesets.Karaoke.Bindables;

namespace osu.Game.Rulesets.Karaoke.Tests.Bindables
{
    public class BindableCultureInfoTest
    {
        [TestCase("ar", 1)]
        [TestCase("en-US", 1033)]
        [TestCase("ja-JP", 1041)]
        public void TestParsingString(string value, int actualLcid)
        {
            var bindable = new BindableCultureInfo();
            bindable.Parse(value);

            Assert.AreEqual(bindable.Value, new CultureInfo(actualLcid));
        }

        [TestCase(1, 1)]
        [TestCase(1033, 1033)]
        [TestCase(1041, 1041)]
        public void TestParsingNumber(int value, int actualLcid)
        {
            var bindable = new BindableCultureInfo();
            bindable.Parse(value);

            Assert.AreEqual(bindable.Value, new CultureInfo(actualLcid));
        }

        [TestCase(1, 1)]
        [TestCase(1033, 1033)]
        [TestCase(1041, 1041)]
        public void TestParsingCultureInfo(int value, int actualLcid)
        {
            var bindable = new BindableCultureInfo();
            bindable.Parse(new CultureInfo(value));

            Assert.AreEqual(bindable.Value, new CultureInfo(actualLcid));
        }
    }
}
