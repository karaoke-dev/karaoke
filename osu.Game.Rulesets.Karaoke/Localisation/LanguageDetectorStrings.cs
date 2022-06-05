using osu.Framework.Localisation;

namespace osu.Game.Rulesets.Karaoke.Localisation
{
    public static class LanguageDetectorStrings
    {
        private const string prefix = @"osu.Game.Rulesets.Karaoke.Localisation.LanguageDetector";

        /// <summary>
        /// "Lyric should not be empty."
        /// </summary>
        public static LocalisableString LyricShouldNotBeEmpty => new TranslatableString(getKey(@"lyric_should_not_be_empty"), @"Lyric should not be empty.");

        private static string getKey(string key) => $@"{prefix}:{key}";
    }
}