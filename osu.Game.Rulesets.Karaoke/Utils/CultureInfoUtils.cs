// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Globalization;

namespace osu.Game.Rulesets.Karaoke.Utils
{
    public static class CultureInfoUtils
    {
        /// <summary>
        /// Get all the languages that are not related to the country
        /// </summary>
        /// <returns></returns>
        public static CultureInfo[] GetAvailableLanguages()
        {
            // todo: should make sure that all the language's LCID or ISC code are not duplicated.
            return CultureInfo.GetCultures(CultureTypes.NeutralCultures);
        }

        public static bool IsLanguage(CultureInfo cultureInfo)
            => (cultureInfo.CultureTypes & CultureTypes.NeutralCultures) != 0;

        public static string GetLanguageDisplayText(CultureInfo cultureInfo)
            => cultureInfo.NativeName;
    }
}
