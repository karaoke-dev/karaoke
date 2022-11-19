// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using NUnit.Framework;
using osu.Game.Rulesets.Karaoke.Edit.Components.Issues;
using osu.Game.Rulesets.Karaoke.Tests.Helper;

namespace osu.Game.Rulesets.Karaoke.Tests.Editor.Components.Issues;

public class IssueIconTest
{
    [Test]
    public void TestGetIconUsageByIssueTemplate()
    {
        // This test case is just make sure that not crash.
        var availableIssueTemplates = TestCaseCheckHelper.GetAllAvailableIssueTemplates();

        foreach (var availableIssueTemplate in availableIssueTemplates)
        {
            Assert.DoesNotThrow(() => IssueIcon.GetIconUsageByIssueTemplate(availableIssueTemplate));
        }
    }

    [Test]
    public void TestGetIconUsageByCheck()
    {
        // This test case is just make sure that issue will always has icon.
        var availableChecks = TestCaseCheckHelper.GetAllAvailableChecks();

        foreach (var availableCheck in availableChecks)
        {
            Assert.NotNull(IssueIcon.GetIconUsageByCheck(availableCheck));
        }
    }
}
