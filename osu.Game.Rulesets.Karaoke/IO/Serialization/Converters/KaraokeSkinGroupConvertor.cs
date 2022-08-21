// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using System.Reflection;
using osu.Game.Rulesets.Karaoke.Skinning.Groups;

namespace osu.Game.Rulesets.Karaoke.IO.Serialization.Converters
{
    public class KaraokeSkinGroupConvertor : GenericTypeConvertor<IGroup>
    {
        protected override Type GetTypeByName(string name)
        {
            // only get name from font
            var assembly = Assembly.GetExecutingAssembly();
            return assembly.GetType($"osu.Game.Rulesets.Karaoke.Skinning.Groups.{name}");
        }
    }
}
