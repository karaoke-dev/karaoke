// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Bindables;
using osu.Game.Rulesets.Karaoke.Configuration;
using osu.Game.Rulesets.Karaoke.Edit.Generator.Beatmaps.Pages;

namespace osu.Game.Rulesets.Karaoke.Screens.Edit.Beatmaps.Configs.Generator.Beatmaps.Pages
{
    public partial class PageGeneratorConfigPopover : GeneratorConfigPopover<PageGeneratorConfig>
    {
        protected override KaraokeRulesetEditGeneratorSetting Config => KaraokeRulesetEditGeneratorSetting.BeatmapPageGeneratorConfig;

        protected override GeneratorConfigSection[] CreateConfigSection(Bindable<PageGeneratorConfig> current)
        {
            return new GeneratorConfigSection[]
            {
                new GenericSection(current),
            };
        }
    }
}
