// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Game.Configuration;
using osu.Game.Rulesets.Karaoke.Edit.Generator.Languages;
using osu.Game.Rulesets.Karaoke.Edit.Generator.Notes;
using osu.Game.Rulesets.Karaoke.Edit.Generator.RomajiTags.Ja;
using osu.Game.Rulesets.Karaoke.Edit.Generator.RubyTags.Ja;
using osu.Game.Rulesets.Karaoke.Edit.Generator.TimeTags.Ja;
using osu.Game.Rulesets.Karaoke.Edit.Generator.TimeTags.Zh;
using osu.Game.Rulesets.Karaoke.Edit.Generator.Types;

namespace osu.Game.Rulesets.Karaoke.Configuration
{
    public class KaraokeRulesetEditGeneratorConfigManager : InMemoryConfigManager<KaraokeRulesetEditGeneratorSetting>
    {
        protected override void InitialiseDefaults()
        {
            base.InitialiseDefaults();

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
        }

        protected static T CreateDefaultConfig<T>() where T : IHasConfig<T>, new()
            => new T().CreateDefaultConfig();
    }

    public enum KaraokeRulesetEditGeneratorSetting
    {
        // Language detection
        LanguageDetectorConfig,

        // Layout generator
        LayoutGeneratorConfig,

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
