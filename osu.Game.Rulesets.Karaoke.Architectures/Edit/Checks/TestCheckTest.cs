// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Linq;
using ArchUnitNET.Domain;
using ArchUnitNET.Domain.Extensions;
using NUnit.Framework;
using osu.Game.Rulesets.Karaoke.Tests.Editor.Checks;

namespace osu.Game.Rulesets.Karaoke.Architectures.Edit.Checks;

public class TestCheckTest : BaseTest
{
    [Test]
    [Project.KaraokeTest(true)]
    public void CheckShouldContainsTest()
    {
        var architecture = GetProjectArchitecture(new Project.KaraokeAttribute());

        var baseCheckTest = architecture.GetClassOfType(typeof(BaseCheckTest<>));
        var allChecks = architecture.Classes
                                    .Where(x => x.Namespace.RelativeNameMatches(new Project.KaraokeAttribute(), "Edit.Checks"))
                                    .Where(x => x.IsNested == false && x.IsAbstract == false)
                                    .ToArray();
        var allCheckTests = architecture.Classes.Where(x => x.InheritedClasses.Contains(baseCheckTest)).ToArray();

        Assertion(() =>
        {
            Assert.NotZero(allChecks.Length, "No check found");

            foreach (var check in allChecks)
            {
                // need to make sure that all checks have a test class.
                var matchedTest = allCheckTests.FirstOrDefault(x => x.NameContains(check.Name));
                Assert.IsTrue(matchedTest != null, $"Check {check} should have a test class.");

                // need to make sure that all issue template should be tested.
                var innerIssueTemplates = check.GetInnerClasses();

                foreach (var issueTemplate in innerIssueTemplates)
                {
                    Assert.IsTrue(check.GetTypeDependencies().Contains(issueTemplate), $"Seems {issueTemplate} is not tested.");
                }
            }
        });
    }

    [Test]
    [Project.KaraokeTest(true)]
    public void CheckTestMethod()
    {
        var architecture = GetProjectArchitecture();
        var baseCheckTest = architecture.GetClassOfType(typeof(BaseCheckTest<>));

        var assertOkMethod = baseCheckTest.GetMethodMembersContainsName("AssertOk").FirstOrDefault();
        var assertNotOkMethod = baseCheckTest.GetMethodMembersContainsName("AssertNotOk").FirstOrDefault();

        var allCheckTests = architecture.Classes.Where(x => x.InheritedClasses.Contains(baseCheckTest) && x.IsAbstract == false).ToArray();

        Assertion(() =>
        {
            Assert.NotNull(assertOkMethod, "AssertOk method not found");
            Assert.NotNull(assertNotOkMethod, "AssertNotOk method not found");

            Assert.NotZero(allCheckTests.Length, "No check test found");

            foreach (var checkTest in allCheckTests)
            {
                var testMethods = checkTest.GetAllTestMembers(architecture).ToArray();
                Assert.NotZero(testMethods.Length, $"No test method  in the {checkTest}");

                foreach (var testMethod in testMethods)
                {
                    Assert.IsTrue(isTestMethod(testMethod), $"Test method {testMethod} should call {assertOkMethod} or {assertNotOkMethod} method.");
                }
            }

            return;

            static bool isTestMethod(IMember testMethod)
            {
                var calledMethods = testMethod.GetCalledMethods().ToArray();
                return calledMethods.Any(x => x.NameStartsWith("AssertOk") || x.NameStartsWith("AssertNotOk"));
            }
        });
    }
}
