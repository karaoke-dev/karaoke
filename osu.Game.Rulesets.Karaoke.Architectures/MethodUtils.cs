// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using NUnit.Framework;

namespace osu.Game.Rulesets.Karaoke.Architectures;

public class MethodUtils
{
    public static Project.ProjectAttribute GetExecuteProject()
    {
        // trying to get the callstack with test attribute
        var stackTrace = new StackTrace();
        var frames = stackTrace.GetFrames();

        foreach (var frame in frames)
        {
            var method = frame.GetMethod();
            if (method == null)
                continue;

            var attributes = method.CustomAttributes;
            if (attributes.All(x => x.AttributeType != typeof(TestAttribute)))
                continue;

            return getDefaultAttributeByMethod(method);
        }

        throw new InvalidOperationException("Test method is not in the callstack.");

        static Project.ProjectAttribute getDefaultAttributeByMethod(MethodBase method)
        {
            var projects = method.CustomAttributes
                                 .Where(x => x.AttributeType.BaseType == typeof(Project.ProjectAttribute))
                                 .Select(x =>
                                 {
                                     object?[] constructorParams = x.ConstructorArguments.Select(args => args.Value).ToArray();
                                     object? instance = Activator.CreateInstance(x.AttributeType, constructorParams);

                                     if (instance is not Project.ProjectAttribute propertyAttribute)
                                         throw new InvalidOperationException("The attribute is not found.");

                                     return propertyAttribute;
                                 });

            return projects.Single(x => x.Execute);
        }
    }
}
