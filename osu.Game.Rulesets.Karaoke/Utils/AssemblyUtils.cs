// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using System.Linq;
using System.Reflection;

namespace osu.Game.Rulesets.Karaoke.Utils
{
    public static class AssemblyUtils
    {
        public static Assembly? GetAssemblyByName(string name)
        {
            var defaultAssembly = AppDomain.CurrentDomain.GetAssemblies().FirstOrDefault(x => x.GetName().Name == name);

            if (defaultAssembly != null)
                return defaultAssembly;

            // Note: because multiple assembly might be wrapped into single one by ILRepack, so should find by main dll again if not found.
            return AppDomain.CurrentDomain.GetAssemblies().FirstOrDefault(x => x.FullName?.Contains("osu.Game.Rulesets.Karaoke") ?? false);
        }
    }
}
