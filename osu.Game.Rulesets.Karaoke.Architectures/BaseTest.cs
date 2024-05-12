// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using System.Linq;
using ArchUnitNET.Domain;
using ArchUnitNET.Loader;
using NUnit.Framework;

namespace osu.Game.Rulesets.Karaoke.Architectures;

public abstract class BaseTest
{
    private Project.ProjectAttribute? executeProject;

    [SetUp]
    public void SetUp()
    {
        executeProject = null;
    }

    #region Utility

    protected Project.ProjectAttribute GetExecuteProject()
    {
        return executeProject ??= MethodUtils.GetExecuteProject();
    }

    protected Architecture GetProjectArchitecture(params Project.ProjectAttribute[] extraProjects)
    {
        // trying to get the callstack with test attribute
        var rootObject = GetExecuteProject().RootObjectType;
        var assembly = rootObject.Assembly;

        // note:
        // 1. only load the test assembly because loading too much assembly will cause the test to be slow.
        // 2. should not filter the namespace in here because it will cause inner class or inherit class cannot be found.
        return new ArchLoader()
               .LoadAssembly(assembly)
               .LoadAssemblies(extraProjects.Select(x => x.RootObjectType.Assembly).ToArray())
               .Build();
    }

    protected void Assertion(Action assert)
    {
        // check the execute project type
        var executeType = GetExecuteProject().ExecuteType;

        switch (executeType)
        {
            case Project.ExecuteType.Check:
            {
                assert();
                break;
            }

            case Project.ExecuteType.Report:
            {
                Assert.Multiple(() =>
                {
                    assert();

                    int totalCount = TestContext.CurrentContext.AssertCount;
                    int failedCount = TestContext.CurrentContext.Result.Assertions.Count();
                    Console.WriteLine("=================================");
                    Console.WriteLine($"There are {failedCount} failed in {totalCount} test step.");
                });

                break;
            }

            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    #endregion
}
