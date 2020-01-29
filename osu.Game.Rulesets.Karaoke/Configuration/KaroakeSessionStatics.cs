// Copyright (c) ppy Pty Ltd <contact@ppy.sh>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Linq;
using osu.Game.Beatmaps;
using osu.Game.Configuration;
using osu.Game.Rulesets.Karaoke.Objects;

namespace osu.Game.Rulesets.Karaoke.Configuration
{
    public class KaroakeSessionStatics : InMemoryConfigManager<KaraokeRulesetSession>
    {
        private readonly KaraokeRulesetConfigManager rulesetConfigManager;

        public KaroakeSessionStatics(KaraokeRulesetConfigManager config, IBeatmap beatmap)
        {
            rulesetConfigManager = config;

            // Translate
            var useTranslate = getvalue<bool>(KaraokeRulesetSetting.UseTranslate);
            var preferLanguage = getvalue<string>(KaraokeRulesetSetting.PreferLanguage);
            var availableTranslate = beatmap.HitObjects.OfType<TranslateDictionary>().FirstOrDefault()?.Translates?.Keys;
            var selectedLanguage = availableTranslate?.FirstOrDefault(t => t == preferLanguage) ?? availableTranslate?.FirstOrDefault();
            Set(KaraokeRulesetSession.UseTranslate, useTranslate);
            Set(KaraokeRulesetSession.PreferLanguage, selectedLanguage);

            // Pitch
            var overridePitch = getvalue<bool>(KaraokeRulesetSetting.OverridePitchAtGameplay);
            var pitchValue = getvalue<int>(KaraokeRulesetSetting.Pitch);
            Set(KaraokeRulesetSession.Pitch, overridePitch ? pitchValue : 0, -10, 10);

            var overrideVocalPitch = getvalue<bool>(KaraokeRulesetSetting.OverrideVocalPitchAtGameplay);
            var vocalPitchValue = getvalue<int>(KaraokeRulesetSetting.VocalPitch);
            Set(KaraokeRulesetSession.VocalPitch, overrideVocalPitch ? vocalPitchValue : 0, -10, 10);

            var overrideSaitenPitch = getvalue<bool>(KaraokeRulesetSetting.OverrideSaitenPitchAtGameplay);
            var saitenPitchValue = getvalue<int>(KaraokeRulesetSetting.SaitenPitch);
            Set(KaraokeRulesetSession.SaitenPitch, overrideSaitenPitch ? saitenPitchValue : 0, -8, 8);

            // Playback
            var overridePlaybackSpeed = getvalue<bool>(KaraokeRulesetSetting.OverridePlaybackSpeedAtGameplay);
            var playbackSpeedValue = getvalue<int>(KaraokeRulesetSetting.PlaybackSpeed);
            Set(KaraokeRulesetSession.PlaybackSpeed, overridePlaybackSpeed ? playbackSpeedValue : 0, -10, 10);

            // Practice
            Set<LyricLine>(KaraokeRulesetSession.NowLyric, null);
        }

        private T getvalue<T>(KaraokeRulesetSetting setting) => rulesetConfigManager.GetBindable<T>(setting).Value;
    }

    public enum KaraokeRulesetSession
    {
        // Translate
        UseTranslate,
        PreferLanguage,

        // Pitch
        Pitch,
        VocalPitch,
        SaitenPitch,

        // Playback
        PlaybackSpeed,

        // Practice
        NowLyric,
    }
}
