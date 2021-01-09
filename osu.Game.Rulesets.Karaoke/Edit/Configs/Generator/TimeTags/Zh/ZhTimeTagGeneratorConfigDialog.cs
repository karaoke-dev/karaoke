// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Bindables;
using osu.Game.Rulesets.Karaoke.Configuration;
using osu.Game.Rulesets.Karaoke.Edit.Generator.TimeTags.Zh;

namespace osu.Game.Rulesets.Karaoke.Edit.Configs.Generator.TimeTags.Zh
{
    public class ZhTimeTagGeneratorConfigDialog : TimeTagGeneratorConfigDialog<ZhTimeTagGeneratorConfig>
    {
        protected override KaraokeRulesetEditGeneratorSetting Config => KaraokeRulesetEditGeneratorSetting.ZhTimeTagGeneratorConfig;

        protected override GeneratorConfigSection[] CreateConfigSection(Bindable<ZhTimeTagGeneratorConfig> current)
        {
            return null;
        }
    }
}
