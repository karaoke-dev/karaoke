// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Bindables;
using osu.Game.Rulesets.Karaoke.Configuration;
using osu.Game.Rulesets.Karaoke.Edit.Generator.RomajiTags.Ja;

namespace osu.Game.Rulesets.Karaoke.Edit.Configs.Generator.RomajiTags.Ja
{
    public class JaRomajiTagGeneratorConfigDialog : RomajiTagGeneratorConfigDialog<JaRomajiTagGeneratorConfig>
    {
        protected override KaraokeRulesetEditGeneratorSetting Config => KaraokeRulesetEditGeneratorSetting.JaRomajiTagGeneratorConfig;

        protected override GeneratorConfigSection[] CreateConfigSection(Bindable<JaRomajiTagGeneratorConfig> current)
        {
            return null;
        }
    }
}
