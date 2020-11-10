// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

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
            var useTranslate = getValue<bool>(KaraokeRulesetSetting.UseTranslate);
            var preferLanguage = getValue<string>(KaraokeRulesetSetting.PreferLanguage);
            var availableTranslate = beatmap?.AvailableTranslates();
            var selectedLanguage = availableTranslate?.FirstOrDefault(t => t.Name == preferLanguage) ?? availableTranslate?.FirstOrDefault();
            Set(KaraokeRulesetSession.UseTranslate, useTranslate);
            Set(KaraokeRulesetSession.PreferLanguage, selectedLanguage?.Name ?? "");

            var displayRuby = getValue<bool>(KaraokeRulesetSetting.DisplayRuby);
            var displayRomaji = getValue<bool>(KaraokeRulesetSetting.DisplayRomaji);
            Set(KaraokeRulesetSession.DisplayRuby, displayRuby);
            Set(KaraokeRulesetSession.DisplayRomaji, displayRomaji);

            // Pitch
            var overridePitch = getValue<bool>(KaraokeRulesetSetting.OverridePitchAtGameplay);
            var pitchValue = getValue<int>(KaraokeRulesetSetting.Pitch);
            Set(KaraokeRulesetSession.Pitch, overridePitch ? pitchValue : 0, -10, 10);

            var overrideVocalPitch = getValue<bool>(KaraokeRulesetSetting.OverrideVocalPitchAtGameplay);
            var vocalPitchValue = getValue<int>(KaraokeRulesetSetting.VocalPitch);
            Set(KaraokeRulesetSession.VocalPitch, overrideVocalPitch ? vocalPitchValue : 0, -10, 10);

            var overrideSaitenPitch = getValue<bool>(KaraokeRulesetSetting.OverrideSaitenPitchAtGameplay);
            var saitenPitchValue = getValue<int>(KaraokeRulesetSetting.SaitenPitch);
            Set(KaraokeRulesetSession.SaitenPitch, overrideSaitenPitch ? saitenPitchValue : 0, -8, 8);

            // Playback
            var overridePlaybackSpeed = getValue<bool>(KaraokeRulesetSetting.OverridePlaybackSpeedAtGameplay);
            var playbackSpeedValue = getValue<int>(KaraokeRulesetSetting.PlaybackSpeed);
            Set(KaraokeRulesetSession.PlaybackSpeed, overridePlaybackSpeed ? playbackSpeedValue : 0, -10, 10);

            // Practice
            Set<Lyric>(KaraokeRulesetSession.NowLyric, null);

            // Saiten status
            Set(KaraokeRulesetSession.SaitenStatus, SaitenStatusMode.NotInitialized);
        }

        private T getValue<T>(KaraokeRulesetSetting setting) => rulesetConfigManager.GetBindable<T>(setting).Value;
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
        SaitenPitch,

        // Playback
        PlaybackSpeed,

        // Practice
        NowLyric,

        // Saiten status
        SaitenStatus,
    }
}
