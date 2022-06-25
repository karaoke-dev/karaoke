// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using osu.Framework.Development;

namespace osu.Game.Rulesets.Karaoke.Utils
{
    public static class VersionUtils
    {
        public static Version GetVersion()
            => AssemblyUtils.GetAssemblyByName("osu.Game.Rulesets.Karaoke")?.GetName().Version ?? new Version();

        /// <summary>
        /// Get the major version of this ruleset.
        /// Will be a noun or word.
        /// </summary>
        /// <returns>Major version name</returns>
        public static string MajorVersionName => "UwU";

        public static bool IsDeployedBuild => GetVersion().Major > 1;

        public static string DisplayVersion
        {
            get
            {
                var assemblyVersion = GetVersion();
                bool isDeployedBuild = assemblyVersion.Major > 0;
                if (!isDeployedBuild)
                    return @"local " + (DebugUtils.IsDebugBuild ? @"debug" : @"release");

                return $@"{assemblyVersion.Major}.{assemblyVersion.Minor}.{assemblyVersion.Build}-{MajorVersionName}";
            }
        }
    }
}
