// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

#nullable disable

using osu.Framework.Bindables;
using osu.Game.Rulesets.Karaoke.Configuration;
using osu.Game.Rulesets.Karaoke.Edit.Generator.Lyrics.Language;

namespace osu.Game.Rulesets.Karaoke.Screens.Edit.Beatmaps.Configs.Generator.Lyrics.Language
{
    public partial class LanguageDetectorConfigPopover : GeneratorConfigPopover<LanguageDetectorConfig>
    {
        protected override KaraokeRulesetEditGeneratorSetting Config => KaraokeRulesetEditGeneratorSetting.LanguageDetectorConfig;

        protected override GeneratorConfigSection[] CreateConfigSection(Bindable<LanguageDetectorConfig> current)
        {
            return new GeneratorConfigSection[]
            {
                new AcceptLanguagesSection(current),
            };
        }
    }
}
