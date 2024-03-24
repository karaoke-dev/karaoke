// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using osu.Game.Beatmaps;
using osu.Game.Configuration;
using osu.Game.Rulesets.Karaoke.Objects;
using osu.Game.Rulesets.Karaoke.UI.Components;

namespace osu.Game.Rulesets.Karaoke.Configuration;

public class KaraokeSessionStatics : InMemoryConfigManager<KaraokeRulesetSession>
{
    private readonly KaraokeRulesetConfigManager rulesetConfigManager;

    public KaraokeSessionStatics(KaraokeRulesetConfigManager config, IBeatmap? beatmap)
    {
        rulesetConfigManager = config;

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
        SetDefault(KaraokeRulesetSession.SingingLyrics, Array.Empty<Lyric>());

        // Scoring status
        SetDefault(KaraokeRulesetSession.ScoringStatus, ScoringStatusMode.NotInitialized);
    }

    private T getValue<T>(KaraokeRulesetSetting setting) => rulesetConfigManager.Get<T>(setting);
}

public enum KaraokeRulesetSession
{
    // Pitch
    Pitch,
    VocalPitch,
    ScoringPitch,

    // Playback
    PlaybackSpeed,

    // Practice
    SingingLyrics,

    // Scoring status
    ScoringStatus,
}
