// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Linq;
using ArchUnitNET.Domain;
using ArchUnitNET.Domain.Extensions;
using NUnit.Framework;
using osu.Game.Rulesets.Edit.Checks.Components;
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
            Assert.That(allChecks.Length, Is.Not.Zero, "No check found");

            foreach (var check in allChecks)
            {
                // need to make sure that all checks have a test class.
                var matchedTest = allCheckTests.FirstOrDefault(x => x.NameContains(check.Name));
                Assert.That(matchedTest, Is.Not.Null, $"Check {check} should have a test class.");

                // need to make sure that all issue template should be tested.
                var innerIssueTemplates = check.GetInnerClasses();

                foreach (var issueTemplate in innerIssueTemplates)
                {
                    Assert.That(check.GetTypeDependencies().Contains(issueTemplate), $"Seems {issueTemplate} is not tested.");
                }
            }
        });
    }

    [Test]
    [Project.KaraokeTest(true)]
    public void CheckTestClassAndMethod()
    {
        var architecture = GetProjectArchitecture(new Project.KaraokeAttribute());
        var baseCheck = architecture.GetInterfaceOfType(typeof(ICheck));
        var baseCheckTest = architecture.GetClassOfType(typeof(BaseCheckTest<>));

        var assertOkMethod = baseCheckTest.GetMethodMembersContainsName("AssertOk").FirstOrDefault();
        var assertNotOkMethod = baseCheckTest.GetMethodMembersContainsName("AssertNotOk").FirstOrDefault();

        var allCheckTests = architecture.Classes.Where(x => x.InheritedClasses.Contains(baseCheckTest) && x.IsAbstract == false).ToArray();

        Assertion(() =>
        {
            Assert.That(assertOkMethod, Is.Not.Null, "AssertOk method not found");
            Assert.That(assertNotOkMethod, Is.Not.Null, "AssertNotOk method not found");

            Assert.That(allCheckTests.Length, Is.Not.Zero, "No check test found");

            foreach (var checkTest in allCheckTests)
            {
                // check the class naming.
                Assert.That(isTestClassValid(checkTest, baseCheck), $"Test class {checkTest} should have correct naming");

                // check the test method naming in the test case.
                var testMethods = checkTest.GetAllTestMembers(architecture).ToArray();
                Assert.That(testMethods.Length, Is.Not.Zero, $"No test method in the {checkTest}");

                foreach (var testMethod in testMethods)
                {
                    Assert.That(isTestNamingValid(testMethod), $"Test method {testMethod} should have correct naming");
                    Assert.That(isTestMethod(testMethod), $"Test method {testMethod} should call {assertOkMethod} or {assertNotOkMethod} method.");
                }
            }

            return;

            static bool isTestClassValid(Class testClass, Interface baseCheck)
            {
                var testCheck = testClass.GetGenericTypes().OfType<Class>().First(x => x.ImplementsInterface(baseCheck));
                return testClass.NameStartsWith(testCheck.Name);
            }

            static bool isTestNamingValid(IMember testMethod)
            {
                var calledMethods = testMethod.GetMethodCallDependencies().FirstOrDefault(x => x.TargetMember.NameStartsWith("AssertNotOk"));

                if (calledMethods != null)
                {
                    // todo: should get the generic type from the AssertNotOk method. the end of the method should be the issue template.
                    return testMethod.NameStartsWith("TestCheck");
                }

                return true;
            }

            static bool isTestMethod(IMember testMethod)
            {
                var calledMethods = testMethod.GetCalledMethods().ToArray();
                return calledMethods.Any(x => x.NameStartsWith("AssertOk") || x.NameStartsWith("AssertNotOk"));
            }
        });
    }
}
