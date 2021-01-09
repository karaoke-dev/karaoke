// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Bindables;
using osu.Game.Overlays;
using osu.Game.Rulesets.Karaoke.Configuration;
using osu.Game.Rulesets.Karaoke.Edit.Generator.Languages;

namespace osu.Game.Rulesets.Karaoke.Edit.Configs.Generator.Languages
{
    public class LanguageDetectorConfigDialog : GeneratorConfigDialog<LanguageDetectorConfig>
    {
        protected override KaraokeRulesetEditGeneratorSetting Config => KaraokeRulesetEditGeneratorSetting.LanguageDetectorConfig;

        protected override OverlayColourScheme OverlayColourScheme => OverlayColourScheme.Orange;

        protected override string Title => "Language generator config";

        protected override string Description => "Change config for language generator.";

        protected override GeneratorConfigSection[] CreateConfigSection(Bindable<LanguageDetectorConfig> current)
        {
            return null;
        }
    }
}
