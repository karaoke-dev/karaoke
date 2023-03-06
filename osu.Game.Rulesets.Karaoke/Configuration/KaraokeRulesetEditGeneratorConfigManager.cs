// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Game.Configuration;
using osu.Game.Rulesets.Karaoke.Edit.Generator;
using osu.Game.Rulesets.Karaoke.Edit.Generator.Beatmaps.Pages;
using osu.Game.Rulesets.Karaoke.Edit.Generator.Lyrics.Language;
using osu.Game.Rulesets.Karaoke.Edit.Generator.Lyrics.Notes;
using osu.Game.Rulesets.Karaoke.Edit.Generator.Lyrics.ReferenceLyric;
using osu.Game.Rulesets.Karaoke.Edit.Generator.Lyrics.RomajiTags.Ja;
using osu.Game.Rulesets.Karaoke.Edit.Generator.Lyrics.RubyTags.Ja;
using osu.Game.Rulesets.Karaoke.Edit.Generator.Lyrics.TimeTags.Ja;
using osu.Game.Rulesets.Karaoke.Edit.Generator.Lyrics.TimeTags.Zh;

namespace osu.Game.Rulesets.Karaoke.Configuration
{
    public class KaraokeRulesetEditGeneratorConfigManager : InMemoryConfigManager<KaraokeRulesetEditGeneratorSetting>
    {
        protected override void InitialiseDefaults()
        {
            base.InitialiseDefaults();

            // Beatmap page
            SetDefault(KaraokeRulesetEditGeneratorSetting.BeatmapPageGeneratorConfig, CreateDefaultConfig<PageGeneratorConfig>());

            // Language detection
            SetDefault(KaraokeRulesetEditGeneratorSetting.ReferenceLyricDetectorConfig, CreateDefaultConfig<ReferenceLyricDetectorConfig>());

            // Language detection
            SetDefault(KaraokeRulesetEditGeneratorSetting.LanguageDetectorConfig, CreateDefaultConfig<LanguageDetectorConfig>());

            // Note generator
            SetDefault(KaraokeRulesetEditGeneratorSetting.NoteGeneratorConfig, CreateDefaultConfig<NoteGeneratorConfig>());

            // Romaji generator
            SetDefault(KaraokeRulesetEditGeneratorSetting.JaRomajiTagGeneratorConfig, CreateDefaultConfig<JaRomajiTagGeneratorConfig>());

            // Ruby generator
            SetDefault(KaraokeRulesetEditGeneratorSetting.JaRubyTagGeneratorConfig, CreateDefaultConfig<JaRubyTagGeneratorConfig>());

            // Time tag generator
            SetDefault(KaraokeRulesetEditGeneratorSetting.JaTimeTagGeneratorConfig, CreateDefaultConfig<JaTimeTagGeneratorConfig>());
            SetDefault(KaraokeRulesetEditGeneratorSetting.ZhTimeTagGeneratorConfig, CreateDefaultConfig<ZhTimeTagGeneratorConfig>());

            // Language detection
            SetDefault(KaraokeRulesetEditGeneratorSetting.ReferenceLyricDetectorConfig, CreateDefaultConfig<ReferenceLyricDetectorConfig>());
        }

        protected static T CreateDefaultConfig<T>() where T : GeneratorConfig, new() => new();
    }

    public enum KaraokeRulesetEditGeneratorSetting
    {
        // Beatmap
        BeatmapPageGeneratorConfig,

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
}
