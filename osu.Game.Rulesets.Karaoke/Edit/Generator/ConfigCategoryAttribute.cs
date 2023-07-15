// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using osu.Framework.Localisation;

namespace osu.Game.Rulesets.Karaoke.Edit.Generator;

public class ConfigCategoryAttribute : Attribute, IEquatable<ConfigCategoryAttribute>
{
    public LocalisableString Category { get; }

    public ConfigCategoryAttribute(string category)
    {
        Category = category;
    }

    public bool Equals(ConfigCategoryAttribute? other)
    {
        return Category == other?.Category;
    }

    public override bool Equals(object? obj)
    {
        return obj switch
        {
            ConfigCategoryAttribute category => Equals(category),
            _ => false,
        };
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(base.GetHashCode(), Category);
    }
}
