// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using osu.Game.Configuration;

namespace osu.Game.Rulesets.Karaoke.Edit.Generator;

public class ConfigSourceAttribute : SettingSourceAttribute
{
    public ConfigSourceAttribute(Type declaringType, string label, string? description = null)
        : base(declaringType, label, description)
    {
    }

    public ConfigSourceAttribute(string? label, string? description = null)
        : base(label, description)
    {
    }

    public ConfigSourceAttribute(Type declaringType, string label, string description, int orderPosition)
        : base(declaringType, label, description, orderPosition)
    {
    }

    public ConfigSourceAttribute(string label, string description, int orderPosition)
        : base(label, description, orderPosition)
    {
    }
}
