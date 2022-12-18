// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

#nullable disable

using osu.Framework.Bindables;
using osu.Game.Rulesets.Karaoke.Configuration;
using osu.Game.Rulesets.Karaoke.Edit.Generator.Lyrics.RubyTags.Ja;

namespace osu.Game.Rulesets.Karaoke.Screens.Edit.Beatmaps.Configs.Generator.RubyTags.Ja
{
    public partial class JaRubyTagGeneratorConfigPopover : GeneratorConfigPopover<JaRubyTagGeneratorConfig>
    {
        protected override KaraokeRulesetEditGeneratorSetting Config => KaraokeRulesetEditGeneratorSetting.JaRubyTagGeneratorConfig;

        protected override GeneratorConfigSection[] CreateConfigSection(Bindable<JaRubyTagGeneratorConfig> current)
        {
            return new GeneratorConfigSection[]
            {
                new GenericSection(current)
            };
        }
    }
}
