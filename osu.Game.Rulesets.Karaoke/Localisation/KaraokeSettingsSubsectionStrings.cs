// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Localisation;

namespace osu.Game.Rulesets.Karaoke.Localisation
{
    public static class KaraokeSettingsSubsectionStrings
    {
        private const string prefix = @"osu.Game.Rulesets.Karaoke.Resources.Localisation.KaraokeSettingsSubsection";

        /// <summary>
        /// "Scroll speed"
        /// </summary>
        public static LocalisableString ScrollSpeed => new TranslatableString(getKey(@"scroll_speed"), @"Scroll speed");

        /// <summary>
        /// "Show cursor while playing"
        /// </summary>
        public static LocalisableString ShowCursorWhilePlaying => new TranslatableString(getKey(@"show_cursor_while_playing"), @"Show cursor while playing");

        /// <summary>
        /// "Translate"
        /// </summary>
        public static LocalisableString Translate => new TranslatableString(getKey(@"translate"), @"Translate");

        /// <summary>
        /// "Prefer language"
        /// </summary>
        public static LocalisableString PreferLanguage => new TranslatableString(getKey(@"prefer_language"), @"Prefer language");

        /// <summary>
        /// "Select prefer translate language."
        /// </summary>
        public static LocalisableString SelectPreferTranslateLanguage => new TranslatableString(getKey(@"select_prefer_translate_language"), @"Select prefer translate language.");

        /// <summary>
        /// "Microphone devices"
        /// </summary>
        public static LocalisableString MicrophoneDevices => new TranslatableString(getKey(@"microphone_devices"), @"Microphone devices");

        /// <summary>
        /// "Practice preempt time"
        /// </summary>
        public static LocalisableString PracticePreemptTime => new TranslatableString(getKey(@"practice_preempt_time"), @"Practice preempt time");

        /// <summary>
        /// "Open ruleset settings"
        /// </summary>
        public static LocalisableString OpenRulesetSettings => new TranslatableString(getKey(@"open_ruleset_settings"), @"Open ruleset settings");

        /// <summary>
        /// "Open ruleset settings for adjusting more configs."
        /// </summary>
        public static LocalisableString OpenRulesetSettingsForAdjustingMoreConfigs =>
            new TranslatableString(getKey(@"open_ruleset_settings_for_adjusting_more_configs"), @"Open ruleset settings for adjusting more configs.");

        /// <summary>
        /// "Change log"
        /// </summary>
        public static LocalisableString ChangeLog => new TranslatableString(getKey(@"change_log"), @"Change log");

        /// <summary>
        /// "Let&#39;s see what karaoke! changed."
        /// </summary>
        public static LocalisableString LetsSeeWhatKaraokeChanged => new TranslatableString(getKey(@"lets_see_what_karaoke_changed"), @"Let's see what karaoke! changed.");

        private static string getKey(string key) => $@"{prefix}:{key}";
    }
}
