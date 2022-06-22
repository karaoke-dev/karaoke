// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

#nullable disable

using osu.Framework.Localisation;

namespace osu.Game.Rulesets.Karaoke.Localisation
{
    public static class KaraokeSettingsSubsectionStrings
    {
        private const string prefix = @"osu.Game.Rulesets.Karaoke.Resources.Localisation.KaraokeSettingsSubsection";

        /// <summary>
        /// "Scrolling direction"
        /// </summary>
        public static LocalisableString ScrollingDirection => new TranslatableString(getKey(@"scrolling_direction"), @"Scrolling direction");

        /// <summary>
        /// "Adjust the scroll direction in the scoring area. Will show that in the gameplay if the beatmap is support the scoring."
        /// </summary>
        public static LocalisableString ScrollingDirectionTooltip => new TranslatableString(
            getKey(@"scrolling_direction_tooltip"),
            @"Adjust the scroll direction in the scoring area. Will show that in the gameplay if the beatmap is support the scoring."
        );

        /// <summary>
        /// "Scroll speed"
        /// </summary>
        public static LocalisableString ScrollSpeed => new TranslatableString(getKey(@"scroll_speed"), @"Scroll speed");

        /// <summary>
        /// "Show cursor while playing"
        /// </summary>
        public static LocalisableString ShowCursorWhilePlaying => new TranslatableString(getKey(@"show_cursor_while_playing"), @"Show cursor while playing");

        /// <summary>
        /// "Will not showing the cursor while gameplay if not select this option."
        /// </summary>
        public static LocalisableString ShowCursorWhilePlayingTooltip => new TranslatableString(getKey(@"show_cursor_while_playing_tooltip"), @"Will not showing the cursor while gameplay if not select this option.");

        /// <summary>
        /// "Translate"
        /// </summary>
        public static LocalisableString Translate => new TranslatableString(getKey(@"translate"), @"Translate");

        /// <summary>
        /// "Show the translation under the lyric if contains in the beatmap."
        /// </summary>
        public static LocalisableString TranslateTooltip => new TranslatableString(getKey(@"translate_tooltip"), @"Show the translation under the lyric if contains in the beatmap.");

        /// <summary>
        /// "Prefer language"
        /// </summary>
        public static LocalisableString PreferLanguage => new TranslatableString(getKey(@"prefer_language"), @"Prefer language");

        /// <summary>
        /// "Select prefer translate language."
        /// </summary>
        public static LocalisableString PreferLanguageTooltip => new TranslatableString(getKey(@"prefer_language_tooltip"), @"Select prefer translate language.");

        /// <summary>
        /// "Microphone device"
        /// </summary>
        public static LocalisableString MicrophoneDevice => new TranslatableString(getKey(@"microphone_device"), @"Microphone device");

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
        public static LocalisableString OpenRulesetSettingsTooltip => new TranslatableString(getKey(@"open_ruleset_settings_tooltip"), @"Open ruleset settings for adjusting more configs.");

        /// <summary>
        /// "Change log"
        /// </summary>
        public static LocalisableString ChangeLog => new TranslatableString(getKey(@"change_log"), @"Change log");

        /// <summary>
        /// "Let&#39;s see what karaoke! changed."
        /// </summary>
        public static LocalisableString ChangeLogTooltip => new TranslatableString(getKey(@"change_log_tooltip"), @"Let's see what karaoke! changed.");

        private static string getKey(string key) => $@"{prefix}:{key}";
    }
}
