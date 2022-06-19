// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using osu.Game.Beatmaps;
using osu.Game.Configuration;
using osu.Game.Rulesets.Karaoke.Beatmaps;
using osu.Game.Rulesets.Karaoke.Objects;
using osu.Game.Rulesets.Karaoke.UI.Components;

namespace osu.Game.Rulesets.Karaoke.Configuration
{
    public class KaraokeSessionStatics : InMemoryConfigManager<KaraokeRulesetSession>
    {
        private readonly KaraokeRulesetConfigManager rulesetConfigManager;

        public KaraokeSessionStatics(KaraokeRulesetConfigManager config, IBeatmap beatmap)
        {
            rulesetConfigManager = config;

            // Translate
            bool useTranslate = getValue<bool>(KaraokeRulesetSetting.UseTranslate);
            var preferLanguage = getValue<CultureInfo>(KaraokeRulesetSetting.PreferLanguage);
            var availableTranslate = beatmap?.AvailableTranslates();
            var selectedLanguage = availableTranslate?.FirstOrDefault(t => EqualityComparer<CultureInfo>.Default.Equals(t, preferLanguage)) ?? availableTranslate?.FirstOrDefault();
            SetDefault(KaraokeRulesetSession.UseTranslate, useTranslate);
            SetDefault(KaraokeRulesetSession.PreferLanguage, selectedLanguage);

            bool displayRuby = getValue<bool>(KaraokeRulesetSetting.DisplayRuby);
            bool displayRomaji = getValue<bool>(KaraokeRulesetSetting.DisplayRomaji);
            SetDefault(KaraokeRulesetSession.DisplayRuby, displayRuby);
            SetDefault(KaraokeRulesetSession.DisplayRomaji, displayRomaji);

            // Pitch
            bool overridePitch = getValue<bool>(KaraokeRulesetSetting.OverridePitchAtGameplay);
            int pitchValue = getValue<int>(KaraokeRulesetSetting.Pitch);
            SetDefault(KaraokeRulesetSession.Pitch, overridePitch ? pitchValue : 0, -10, 10);

            bool overrideVocalPitch = getValue<bool>(KaraokeRulesetSetting.OverrideVocalPitchAtGameplay);
            int vocalPitchValue = getValue<int>(KaraokeRulesetSetting.VocalPitch);
            SetDefault(KaraokeRulesetSession.VocalPitch, overrideVocalPitch ? vocalPitchValue : 0, -10, 10);

            bool overrideScoringPitch = getValue<bool>(KaraokeRulesetSetting.OverrideScoringPitchAtGameplay);
            int scoringPitchValue = getValue<int>(KaraokeRulesetSetting.ScoringPitch);
            SetDefault(KaraokeRulesetSession.ScoringPitch, overrideScoringPitch ? scoringPitchValue : 0, -8, 8);

            // Playback
            bool overridePlaybackSpeed = getValue<bool>(KaraokeRulesetSetting.OverridePlaybackSpeedAtGameplay);
            int playbackSpeedValue = getValue<int>(KaraokeRulesetSetting.PlaybackSpeed);
            SetDefault(KaraokeRulesetSession.PlaybackSpeed, overridePlaybackSpeed ? playbackSpeedValue : 0, -10, 10);

            // Practice
            SetDefault<Lyric[]>(KaraokeRulesetSession.SingingLyrics, null);

            // Scoring status
            SetDefault(KaraokeRulesetSession.ScoringStatus, ScoringStatusMode.NotInitialized);
        }

        private T getValue<T>(KaraokeRulesetSetting setting) => rulesetConfigManager.Get<T>(setting);
    }

    public enum KaraokeRulesetSession
    {
        // Translate
        UseTranslate,
        PreferLanguage,

        // Ruby/Romaji
        DisplayRuby,
        DisplayRomaji,

        // Pitch
        Pitch,
        VocalPitch,
        ScoringPitch,

        // Playback
        PlaybackSpeed,

        // Practice
        SingingLyrics,

        // Saiten status
        ScoringStatus,
    }
}
