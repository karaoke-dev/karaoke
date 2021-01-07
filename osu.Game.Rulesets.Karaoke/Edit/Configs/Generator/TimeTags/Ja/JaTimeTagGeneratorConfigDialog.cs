// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Game.Rulesets.Karaoke.Configuration;
using osu.Game.Rulesets.Karaoke.Edit.Generator.TimeTags.Ja;

namespace osu.Game.Rulesets.Karaoke.Edit.Configs.Generator.TimeTags.Ja
{
    public class JaTimeTagGeneratorConfigDialog : GeneratorConfigDialog<JaTimeTagGeneratorConfig>
    {
        protected override KaraokeRulesetEditGeneratorSetting Config => KaraokeRulesetEditGeneratorSetting.JaTimeTagGeneratorConfig;
    }
}
