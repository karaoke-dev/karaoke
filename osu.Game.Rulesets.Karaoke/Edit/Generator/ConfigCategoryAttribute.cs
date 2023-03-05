// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using osu.Framework.Localisation;

namespace osu.Game.Rulesets.Karaoke.Edit.Generator;

public class ConfigCategoryAttribute : Attribute
{
    public LocalisableString Category { get; }

    public ConfigCategoryAttribute(string category)
    {
        Category = category;
    }
}
