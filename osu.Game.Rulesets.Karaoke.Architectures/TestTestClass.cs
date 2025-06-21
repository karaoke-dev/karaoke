// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Linq;
using ArchUnitNET.Domain.Extensions;
using NUnit.Framework;
using osu.Framework.Testing;
using osu.Game.Tests.Visual;

namespace osu.Game.Rulesets.Karaoke.Architectures;

public class TestTestClass : BaseTest
{
    [Test]
    [Project.KaraokeTest(true)]
    public void CheckTestClassAndMethodNaming()
    {
        var architecture = GetProjectArchitecture();
        var baseTestScenes = new[]
        {
            architecture.GetClassOfType(typeof(EditorClockTestScene)),
            architecture.GetClassOfType(typeof(ModTestScene)),
            architecture.GetClassOfType(typeof(ModFailConditionTestScene)),
            architecture.GetClassOfType(typeof(OsuGridTestScene)),
            architecture.GetClassOfType(typeof(OsuManualInputManagerTestScene)),
            architecture.GetClassOfType(typeof(OsuTestScene)),
            architecture.GetClassOfType(typeof(PlayerTestScene)),
            architecture.GetClassOfType(typeof(ScreenTestScene)),
            architecture.GetClassOfType(typeof(SkinnableTestScene)),
        };
        var headlessTestScene = architecture.GetAttributeOfType(typeof(HeadlessTestAttribute));

        // get all test classes
        var testClasses = architecture.GetAllTestClass().ToArray();

        Assertion(() =>
        {
            foreach (var testClass in testClasses)
            {
                var testMethods = testClass.GetAllTestMembers(architecture).ToArray();

                // skip test class without test method. it might not be the test class.
                if (testMethods.Length == 0)
                    continue;

                // test the test class.
                if (testClass.InheritedClasses.Any(x => baseTestScenes.Contains(x)))
                {
                    if (testClass.HasAttributeInSelfOrChild(headlessTestScene))
                    {
                        Assert.That(testClass.NameEndsWith("Test"), $"Test class {testClass} should end with 'Test'.");
                    }
                    else if (testClass.IsAbstract == true)
                    {
                        bool testEndWithTest = testClass.NameEndsWith("TestScene") || testClass.NameEndsWith("TestScene`2");
                        Assert.That(testEndWithTest, $"Test class {testClass} should end with 'TestScene'.");
                    }
                    else
                    {
                        Assert.That(testClass.NameStartsWith("TestScene"), $"Test class {testClass} should start with 'TestScene'.");
                    }
                }
                else
                {
                    bool testEndWithTest = testClass.NameEndsWith("Test") || testClass.NameEndsWith("Test`1") || testClass.NameEndsWith("Test`2");
                    Assert.That(testEndWithTest, $"Test class {testClass} should end with 'Test'.");
                }

                // test all test methods in the test class.
                foreach (var testMethod in testMethods)
                {
                    Assert.That(testMethod.NameStartsWith("Test"), $"Test method {testMethod} should start with 'Test'.");
                }
            }
        });
    }
}
