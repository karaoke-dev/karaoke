// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

#nullable disable

using osu.Framework.Bindables;
using osu.Game.Rulesets.Karaoke.Configuration;
using osu.Game.Rulesets.Karaoke.Edit.Generator.ReferenceLyric;

namespace osu.Game.Rulesets.Karaoke.Edit.Configs.Generator.ReferenceLyric
{
    public class ReferenceLyricDetectorConfigPopover : GeneratorConfigPopover<ReferenceLyricDetectorConfig>
    {
        protected override KaraokeRulesetEditGeneratorSetting Config => KaraokeRulesetEditGeneratorSetting.ReferenceLyricDetectorConfig;

        protected override GeneratorConfigSection[] CreateConfigSection(Bindable<ReferenceLyricDetectorConfig> current)
        {
            return new GeneratorConfigSection[]
            {
                new GenericSection(current),
            };
        }
    }
}
