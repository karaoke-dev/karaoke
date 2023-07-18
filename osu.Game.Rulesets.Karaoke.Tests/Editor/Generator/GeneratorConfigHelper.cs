// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using osu.Framework.Bindables;
using osu.Framework.Extensions.TypeExtensions;
using osu.Game.Rulesets.Karaoke.Edit.Generator;

namespace osu.Game.Rulesets.Karaoke.Tests.Editor.Generator;

public class GeneratorConfigHelper
{
    public static void ClearValue(GeneratorConfig config)
    {
        foreach (var property in getAllBindableProperty(config))
        {
            object propertyInstance = property.GetValue(config, null)!;

            // set the default value to the value.
            var propertyType = propertyInstance.GetType();
            propertyType.GetProperty(nameof(Bindable<object>.Value))!.SetValue(propertyInstance, default);
        }
    }

    private static IEnumerable<PropertyInfo> getAllBindableProperty(GeneratorConfig config)
    {
        var properties = config.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance);

        foreach (var property in properties)
        {
            object propertyInstance = property.GetValue(config, null)!;
            var propertyType = propertyInstance.GetType();

            if (propertyType.EnumerateBaseTypes().All(t => !t.IsGenericType || t.GetGenericTypeDefinition() != typeof(Bindable<>)))
                continue;

            yield return property;
        }
    }
}
