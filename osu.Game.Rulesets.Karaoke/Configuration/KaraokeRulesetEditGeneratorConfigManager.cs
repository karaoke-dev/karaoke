// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using System.Collections.Generic;
using osu.Framework.Bindables;
using osu.Game.Configuration;
using osu.Game.Rulesets.Karaoke.Edit.Generator;
using osu.Game.Rulesets.Karaoke.Edit.Generator.Beatmaps.Pages;
using osu.Game.Rulesets.Karaoke.Edit.Generator.Beatmaps.Stages.Classic;
using osu.Game.Rulesets.Karaoke.Edit.Generator.Beatmaps.Stages.Preview;
using osu.Game.Rulesets.Karaoke.Edit.Generator.Lyrics.Language;
using osu.Game.Rulesets.Karaoke.Edit.Generator.Lyrics.Notes;
using osu.Game.Rulesets.Karaoke.Edit.Generator.Lyrics.ReferenceLyric;
using osu.Game.Rulesets.Karaoke.Edit.Generator.Lyrics.RomajiTags.Ja;
using osu.Game.Rulesets.Karaoke.Edit.Generator.Lyrics.RubyTags.Ja;
using osu.Game.Rulesets.Karaoke.Edit.Generator.Lyrics.TimeTags.Ja;
using osu.Game.Rulesets.Karaoke.Edit.Generator.Lyrics.TimeTags.Zh;

namespace osu.Game.Rulesets.Karaoke.Configuration;

public class KaraokeRulesetEditGeneratorConfigManager : InMemoryConfigManager<KaraokeRulesetEditGeneratorSetting>
{
    protected override void InitialiseDefaults()
    {
        base.InitialiseDefaults();

        // Beatmap page
        SetDefault<PageGeneratorConfig>();

        // Classic stage.
        SetDefault<ClassicLyricLayoutCategoryGeneratorConfig>();
        SetDefault<ClassicLyricTimingInfoGeneratorConfig>();
        SetDefault<ClassicStageInfoGeneratorConfig>();

        // Preview stage.
        SetDefault<PreviewStageInfoGeneratorConfig>();

        // Language detection
        SetDefault<ReferenceLyricDetectorConfig>();

        // Language detection
        SetDefault<LanguageDetectorConfig>();

        // Note generator
        SetDefault<NoteGeneratorConfig>();

        // Romaji generator
        SetDefault<JaRomajiTagGeneratorConfig>();

        // Ruby generator
        SetDefault<JaRubyTagGeneratorConfig>();

        // Time tag generator
        SetDefault<JaTimeTagGeneratorConfig>();
        SetDefault<ZhTimeTagGeneratorConfig>();
    }

    protected void SetDefault<T>() where T : GeneratorConfig, new()
    {
        var defaultValue = CreateDefaultConfig<T>();
        var setting = GetSettingByType<T>();

        SetDefault(setting, defaultValue);
    }

    protected static T CreateDefaultConfig<T>() where T : GeneratorConfig, new() => new();

    protected static KaraokeRulesetEditGeneratorSetting GetSettingByType<TValue>() =>
        typeof(TValue) switch
        {
            Type t when t == typeof(PageGeneratorConfig) => KaraokeRulesetEditGeneratorSetting.BeatmapPageGeneratorConfig,
            Type t when t == typeof(ClassicLyricLayoutCategoryGeneratorConfig) => KaraokeRulesetEditGeneratorSetting.ClassicLyricLayoutCategoryGeneratorConfig,
            Type t when t == typeof(ClassicLyricTimingInfoGeneratorConfig) => KaraokeRulesetEditGeneratorSetting.ClassicLyricTimingInfoGeneratorConfig,
            Type t when t == typeof(ClassicStageInfoGeneratorConfig) => KaraokeRulesetEditGeneratorSetting.ClassicStageInfoGeneratorConfig,
            Type t when t == typeof(PreviewStageInfoGeneratorConfig) => KaraokeRulesetEditGeneratorSetting.PreviewStageInfoGeneratorConfig,
            Type t when t == typeof(ReferenceLyricDetectorConfig) => KaraokeRulesetEditGeneratorSetting.ReferenceLyricDetectorConfig,
            Type t when t == typeof(LanguageDetectorConfig) => KaraokeRulesetEditGeneratorSetting.LanguageDetectorConfig,
            Type t when t == typeof(NoteGeneratorConfig) => KaraokeRulesetEditGeneratorSetting.NoteGeneratorConfig,
            Type t when t == typeof(JaRomajiTagGeneratorConfig) => KaraokeRulesetEditGeneratorSetting.JaRomajiTagGeneratorConfig,
            Type t when t == typeof(JaRubyTagGeneratorConfig) => KaraokeRulesetEditGeneratorSetting.JaRubyTagGeneratorConfig,
            Type t when t == typeof(JaTimeTagGeneratorConfig) => KaraokeRulesetEditGeneratorSetting.JaTimeTagGeneratorConfig,
            Type t when t == typeof(ZhTimeTagGeneratorConfig) => KaraokeRulesetEditGeneratorSetting.ZhTimeTagGeneratorConfig,
            _ => throw new NotSupportedException()
        };

    public TValue Get<TValue>() where TValue : GeneratorConfig, new()
    {
        var lookup = GetSettingByType<TValue>();
        return Get<TValue>(lookup);
    }

    public GeneratorConfig GetGeneratorConfig(KaraokeRulesetEditGeneratorSetting lookup)
    {
        if (!ConfigStore.TryGetValue(lookup, out IBindable? obj))
            throw new KeyNotFoundException();

        var prop = obj.GetType().GetProperty("Value");
        if (prop?.GetValue(obj) is not GeneratorConfig generatorConfig)
            throw new InvalidCastException();

        return generatorConfig;
    }
}

public enum KaraokeRulesetEditGeneratorSetting
{
    // Beatmap
    BeatmapPageGeneratorConfig,

    // Classic stage.
    ClassicLyricLayoutCategoryGeneratorConfig,
    ClassicLyricTimingInfoGeneratorConfig,
    ClassicStageInfoGeneratorConfig,

    // Preview stage.
    PreviewStageInfoGeneratorConfig,

    // Reference lyric detection.
    ReferenceLyricDetectorConfig,

    // Language detection
    LanguageDetectorConfig,

    // Note generator
    NoteGeneratorConfig,

    // Romaji generator
    JaRomajiTagGeneratorConfig,

    // Ruby generator
    JaRubyTagGeneratorConfig,

    // Time tag generator
    JaTimeTagGeneratorConfig,
    ZhTimeTagGeneratorConfig
}
