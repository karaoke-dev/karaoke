// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using NUnit.Framework;
using osu.Game.Rulesets.Karaoke.Utils;

namespace osu.Game.Rulesets.Karaoke.Tests.Utils;

public class VersionUtilsTest
{
    [Test]
    public void TestGetVersion()
    {
        var expected = new Version(1, 0, 0, 0);
        var actual = VersionUtils.GetVersion();
        Assert.That(actual, Is.Not.Null);
        Assert.That(actual.Major, Is.EqualTo(expected.Major));
        Assert.That(actual.Minor, Is.EqualTo(expected.Minor));
        Assert.That(actual.Build, Is.EqualTo(expected.Build));
        Assert.That(actual.Revision, Is.EqualTo(expected.Revision));
    }

    [Test]
    public void TestMajorVersionName()
    {
        const string expected = "UwU";
        string actual = VersionUtils.MajorVersionName;
        Assert.That(actual, Is.EqualTo(expected));
    }

    [Test]
    public void TestIsDeployedBuild()
    {
        // should not be deploy build if not build by github action.
        const bool expected = false;
        bool actual = VersionUtils.IsDeployedBuild;
        Assert.That(actual, Is.EqualTo(expected));
    }

    [Test]
    public void TestGetDisplayVersion()
    {
        const string expected = "1.0.0-UwU";
        string actual = VersionUtils.DisplayVersion;
        Assert.That(actual, Is.EqualTo(expected));
    }
}
