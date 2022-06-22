// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

#nullable disable

using System;
using NUnit.Framework;
using osu.Game.Rulesets.Karaoke.Utils;

namespace osu.Game.Rulesets.Karaoke.Tests.Utils
{
    public class VersionUtilsTest
    {
        [Test]
        public void TestGetVersion()
        {
            var expected = new Version(1, 0, 0, 0);
            var actual = VersionUtils.GetVersion();
            Assert.IsNotNull(actual);
            Assert.AreEqual(expected.Major, actual.Major);
            Assert.AreEqual(expected.Minor, actual.Minor);
            Assert.AreEqual(expected.Build, actual.Build);
            Assert.AreEqual(expected.Revision, actual.Revision);
        }

        [Test]
        public void TestMajorVersionName()
        {
            const string expected = "UwU";
            string actual = VersionUtils.MajorVersionName;
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void IsDeployedBuild()
        {
            // should not be deploy build if not build by github action.
            const bool expected = false;
            bool actual = VersionUtils.IsDeployedBuild;
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void TestGetDisplayVersion()
        {
            const string expected = "1.0.0-UwU";
            string actual = VersionUtils.DisplayVersion;
            Assert.AreEqual(expected, actual);
        }
    }
}
