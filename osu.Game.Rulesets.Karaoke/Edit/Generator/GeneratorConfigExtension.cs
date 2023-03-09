// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace osu.Game.Rulesets.Karaoke.Edit.Generator;

public static class GeneratorConfigExtension
{
    public static IDictionary<ConfigCategoryAttribute, (ConfigSourceAttribute, PropertyInfo)[]> GetOrderedConfigsSourceDictionary(this GeneratorConfig config, ConfigCategoryAttribute defaultCategory)
    {
        return GetOrderedConfigsSourceProperties(config)
               .GroupBy(attr => attr.Item2)
               .ToDictionary(
                   group => group.Key ?? defaultCategory,
                   group => group.Select(x => (x.Item1, x.Item3)).ToArray()
               );
    }

    public static ICollection<(ConfigSourceAttribute, ConfigCategoryAttribute?, PropertyInfo)> GetOrderedConfigsSourceProperties(this GeneratorConfig config)
        => GetConfigSourceProperties(config)
           .OrderBy(attr => attr.Item1)
           .ToArray();

    public static IEnumerable<(ConfigSourceAttribute, ConfigCategoryAttribute?, PropertyInfo)> GetConfigSourceProperties(this GeneratorConfig config)
    {
        var type = config.GetType();

        foreach (var property in type.GetProperties(BindingFlags.GetProperty | BindingFlags.Public | BindingFlags.Instance))
        {
            var configSourceAttribute = property.GetCustomAttribute<ConfigSourceAttribute>(true);
            var configCategoryAttribute = property.GetCustomAttribute<ConfigCategoryAttribute>(true);

            if (configSourceAttribute == null)
                continue;

            yield return (configSourceAttribute, configCategoryAttribute, property);
        }
    }
}
