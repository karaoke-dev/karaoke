// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Localisation;

namespace osu.Game.Rulesets.Karaoke.Localisation
{
    public static class CommonStrings
    {
        private const string prefix = @"osu.Game.Rulesets.Karaoke.Resources.Localisation.Common";

        /// <summary>
        /// "karaoke!"
        /// </summary>
        public static LocalisableString RulesetName => new TranslatableString(getKey(@"karaoke"), @"karaoke!");

        private static string getKey(string key) => $@"{prefix}:{key}";
    }
}
