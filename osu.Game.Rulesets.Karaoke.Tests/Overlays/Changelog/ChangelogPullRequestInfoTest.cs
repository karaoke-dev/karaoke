// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Linq;
using NUnit.Framework;
using osu.Game.Rulesets.Karaoke.Overlays.Changelog;

namespace osu.Game.Rulesets.Karaoke.Tests.Overlays.Changelog;

public class ChangelogPullRequestInfoTest
{
    [TestCase("#2152@andy840119", new[] { 2152 }, new[] { "andy840119" })]
    [TestCase("#2152", new[] { 2152 }, new string[] { })]
    [TestCase("@andy840119", new int[] { }, new[] { "andy840119" })]
    [TestCase("#2152#2153", new[] { 2152, 2153 }, new string[] { })]
    [TestCase("#2152#2152", new[] { 2152 }, new string[] { })]
    [TestCase("@andy@andy840119", new int[] { }, new[] { "andy", "andy840119" })]
    [TestCase("@andy840119@andy840119", new int[] { }, new[] { "andy840119" })]
    [TestCase("https://raw.githubusercontent.com/karaoke-dev/karaoke-dev.github.io/master/content/changelog/2023.1212/#2152@andy840119", new[] { 2152 }, new[] { "andy840119" })] // the actual data that will be get in this method.
    public void TestGetPullRequestInfoFromLink(string url, int[] expectedPrs, string[] expectedUserNames)
    {
        var result = ChangelogPullRequestInfo.GetPullRequestInfoFromLink("karaoke", url)!;

        Assert.AreEqual(expectedPrs, result.PullRequests.Select(x => x.Number));
        Assert.AreEqual(expectedUserNames, result.Users.Select(x => x.UserName));
    }

    [TestCase("unknown_repo", "#2152@andy840119")] // "unknown_repo" does not in the repo list.
    [TestCase("karaoke", "")] // there's no pr number or username.
    [TestCase("karaoke", "hello")] // invalid pr number or username.
    [TestCase("karaoke", "#aaa")] // invalid pr number.
    public void TestTestGetPullRequestInfoFromLinkWithNull(string link, string url)
    {
        var result = ChangelogPullRequestInfo.GetPullRequestInfoFromLink(link, url);
        Assert.IsNull(result);
    }
}
