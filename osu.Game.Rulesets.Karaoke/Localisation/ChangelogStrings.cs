// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Localisation;

namespace osu.Game.Rulesets.Karaoke.Localisation
{
    public static class ChangelogStrings
    {
        private const string prefix = @"osu.Game.Rulesets.Karaoke.Localisation.ChangelogSection";

        /// <summary>
        /// "view current changelog"
        /// </summary>
        public static LocalisableString ViewCurrentChangelog => new TranslatableString(getKey(@"view_current_changelog"), @"View current changelog");

        private static string getKey(string key) => $@"{prefix}:{key}";
    }
}
