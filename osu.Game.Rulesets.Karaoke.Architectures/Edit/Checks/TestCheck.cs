// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Linq;
using ArchUnitNET.Domain.Extensions;
using NUnit.Framework;
using osu.Game.Rulesets.Edit.Checks.Components;

namespace osu.Game.Rulesets.Karaoke.Architectures.Edit.Checks;

public class TestCheck : BaseTest
{
    [Test]
    [Project.Karaoke(true)]
    public void CheckCheckClassNamingAndInherit()
    {
        var architecture = GetProjectArchitecture();

        var baseIssue = architecture.GetClassOfType(typeof(Issue));
        var issues = architecture.Classes.Where(x => x.Namespace.RelativeNameMatches(GetExecuteProject(), "Edit.Checks.Issues")).ToArray();

        var checkOrIssueTemplate = architecture.Classes.Where(x => x.Namespace.RelativeNameMatches(GetExecuteProject(), "Edit.Checks")).ToArray();

        var baseCheck = architecture.GetInterfaceOfType(typeof(ICheck));
        var checks = checkOrIssueTemplate.Where(x => x.IsNested == false).ToArray();

        var baseIssueTemplate = architecture.GetClassOfType(typeof(IssueTemplate));
        var issueTemplates = checkOrIssueTemplate.Where(x => x.IsNested).ToArray();

        Assertion(() =>
        {
            // issues.
            Assert.NotZero(issues.Length, $"{nameof(Issue)} amount is weird.");

            foreach (var checkClass in issues)
            {
                Assert.True(checkClass.InheritedClasses.Contains(baseIssue), $"Class inherit is invalid: {checkClass}");
                Assert.True(checkClass.NameEndsWith("Issue"), $"Class name is invalid: {checkClass}");
            }

            // checks
            Assert.NotZero(checks.Length, $"{nameof(ICheck)} amount is weird.");

            foreach (var check in checks)
            {
                Assert.True(check.ImplementsInterface(baseCheck), $"Class inherit is invalid: {check}");
                Assert.True(check.NameStartsWith("Check"), $"Class name is invalid: {check}");
            }

            // issue templates.
            Assert.NotZero(issueTemplates.Length, $"{nameof(IssueTemplate)} amount is weird");

            foreach (var checkClass in issueTemplates)
            {
                Assert.True(checkClass.InheritedClasses.Contains(baseIssueTemplate), $"Class inherit is invalid: {checkClass}");
                Assert.True(checkClass.NameStartsWith("IssueTemplate"), $"Class name is invalid: {checkClass}");
            }
        });
    }
}
