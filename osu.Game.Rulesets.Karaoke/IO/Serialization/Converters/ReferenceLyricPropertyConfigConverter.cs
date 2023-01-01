// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using System.Diagnostics;
using System.Reflection;
using osu.Game.Rulesets.Karaoke.Objects.Properties;

namespace osu.Game.Rulesets.Karaoke.IO.Serialization.Converters
{
    public class ReferenceLyricPropertyConfigConverter : GenericTypeConverter<IReferenceLyricPropertyConfig>
    {
        protected override Type GetTypeByName(string name)
        {
            var assembly = Assembly.GetExecutingAssembly();
            var type = assembly.GetType($"osu.Game.Rulesets.Karaoke.Objects.Properties.{name}");
            Debug.Assert(type != null);
            return type;
        }
    }
}
