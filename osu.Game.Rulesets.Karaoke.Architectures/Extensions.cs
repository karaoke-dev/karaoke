// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using ArchUnitNET.Domain;
using ArchUnitNET.Domain.Dependencies;
using ArchUnitNET.Domain.Extensions;
using NUnit.Framework;
using osu.Game.Rulesets.Karaoke.Tests;
using Attribute = ArchUnitNET.Domain.Attribute;

namespace osu.Game.Rulesets.Karaoke.Architectures;

public static class Extensions
{
    #region Architecture

    public static IEnumerable<Class> GetAllTestClass(this Architecture architecture)
    {
        var testProject = new Project.KaraokeTestAttribute();
        var testClasses = architecture.Classes.Where(x =>
                                      {
                                          if (x.Namespace.RelativeNameStartsWith(testProject, "Helper"))
                                              return false;

                                          return x.Namespace.RelativeNameStartsWith(testProject, "");
                                      })
                                      .Except(new[]
                                      {
                                          architecture.GetClassOfType(typeof(KaraokeTestBrowser)),
                                          architecture.GetClassOfType(typeof(VisualTestRunner)),
                                      }).ToArray();

        if (testClasses.Length == 0)
            throw new InvalidOperationException("No test class found in the project.");

        return testClasses;
    }

    #endregion

    #region Class

    public static IEnumerable<Class> GetInnerClasses(this Class @class)
    {
        // do the same things as type.GetNestedTypes();
        // see: https://learn.microsoft.com/en-us/dotnet/api/system.type.getnestedtypes?view=net-8.0&redirectedfrom=MSDN#overloads
        // note: cannot use @class.GetNestedTypes(); because it will return the nested class in the same file.
        return @class.GetTypeDependencies().Where(x => x.IsNested).OfType<Class>().Distinct();
    }

    public static IEnumerable<IMember> GetAllTestMembers(this Class @class, Architecture architecture)
    {
        var testAttribute = architecture.GetAttributeOfType(typeof(TestAttribute));
        var testCaseAttribute = architecture.GetAttributeOfType(typeof(TestCaseAttribute));
        return @class.MembersIncludingInherited.Where(x => x.Attributes.Contains(testAttribute) || x.Attributes.Contains(testCaseAttribute));
    }

    public static bool HasAttributeInSelfOrChild(this Class @class, Attribute attribute)
    {
        return @class.Attributes.Contains(attribute) || @class.InheritedClasses.Any(c => c.Attributes.Contains(attribute));
    }

    #endregion

    #region Name

    public static IEnumerable<IType> GetGenericTypes(this Class @class)
    {
        return @class.GetInheritsBaseClassDependencies().SelectMany(x => x.TargetGenericArguments.Select(arg => arg.Type));
    }

    public static bool RelativeNameStartsWith(
        this IHasName cls,
        Project.ProjectAttribute project,
        string pattern,
        StringComparison stringComparison = StringComparison.CurrentCulture)
    {
        string relativeNamespace = getRelativeNamespace(project, cls.FullName);
        return relativeNamespace.StartsWith(pattern, stringComparison);
    }

    public static bool RelativeNameMatches(this IHasName cls,
                                           Project.ProjectAttribute project,
                                           string pattern,
                                           bool useRegularExpressions = false)
    {
        string relativeNamespace = getRelativeNamespace(project, cls.FullName);

        if (!useRegularExpressions)
            return string.Equals(relativeNamespace, pattern, StringComparison.OrdinalIgnoreCase);

        return Regex.IsMatch(relativeNamespace, pattern);
    }

    private static string getRelativeNamespace(Project.ProjectAttribute project, string fullNamespace)
    {
        string? rootObjectNamespace = project.RootObjectType.Namespace;
        if (rootObjectNamespace == null)
            throw new NotSupportedException("Root object namespace should not be null.");

        if (!fullNamespace.StartsWith(rootObjectNamespace, StringComparison.Ordinal))
            throw new NotSupportedException("The namespace of the class is not in the root object namespace.");

        // remove the start namespace with dot.
        return fullNamespace == rootObjectNamespace
            ? string.Empty
            : fullNamespace[$"{rootObjectNamespace}.".Length..];
    }

    #endregion

    #region Type

    public static IEnumerable<MethodMember> GetMethodMembersContainsName(this IType type, string name)
    {
        return type.GetMethodMembers().WhereNameContains<MethodMember>(name);
    }

    public static IEnumerable<TType> WhereNameContains<TType>(this IEnumerable<TType> source, string name) where TType : IHasName
    {
        return source.Where(hasName => hasName.Name.Contains(name));
    }

    #endregion
}
