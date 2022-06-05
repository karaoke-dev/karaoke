using osu.Framework.Localisation;

namespace osu.Game.Rulesets.Karaoke.Localisation.Edit.Generator
{
    public static class NoteGeneratorStrings
    {
        private const string prefix = @"osu.Game.Rulesets.Karaoke.Localisation.NoteGenerator";

        /// <summary>
        /// "Sorry, lyric must have at least two time-tags."
        /// </summary>
        public static LocalisableString SorryLyricMustHaveAtLeastTwoTimeTags => new TranslatableString(getKey(@"sorry_lyric_must_have_at_least_two_time_tags"), @"Sorry, lyric must have at least two time-tags.");

        /// <summary>
        /// "All time-tag should have the time."
        /// </summary>
        public static LocalisableString AllTimeTagShouldHaveTheTime => new TranslatableString(getKey(@"all_time_tag_should_have_the_time"), @"All time-tag should have the time.");

        private static string getKey(string key) => $@"{prefix}:{key}";
    }
}
