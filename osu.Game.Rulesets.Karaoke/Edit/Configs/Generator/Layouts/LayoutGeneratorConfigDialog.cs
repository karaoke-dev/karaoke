// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Bindables;
using osu.Game.Rulesets.Karaoke.Configuration;
using osu.Game.Rulesets.Karaoke.Edit.Generator.Layouts;

namespace osu.Game.Rulesets.Karaoke.Edit.Configs.Generator.Layouts
{
    public class LayoutGeneratorConfigDialog : GeneratorConfigDialog<LayoutGeneratorConfig>
    {
        protected override KaraokeRulesetEditGeneratorSetting Config => KaraokeRulesetEditGeneratorSetting.LayoutGeneratorConfig;

        protected override GeneratorConfigSection[] CreateConfigSection(Bindable<LayoutGeneratorConfig> current, LayoutGeneratorConfig defaultConfig)
        {
            throw new System.NotImplementedException();
        }
    }
}
