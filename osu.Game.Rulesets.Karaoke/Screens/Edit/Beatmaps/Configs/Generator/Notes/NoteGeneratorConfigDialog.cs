// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

#nullable disable

using osu.Framework.Bindables;
using osu.Game.Overlays;
using osu.Game.Rulesets.Karaoke.Configuration;
using osu.Game.Rulesets.Karaoke.Edit.Generator.Lyrics.Notes;

namespace osu.Game.Rulesets.Karaoke.Screens.Edit.Beatmaps.Configs.Generator.Notes
{
    public partial class NoteGeneratorConfigDialog : GeneratorConfigDialog<NoteGeneratorConfig>
    {
        protected override KaraokeRulesetEditGeneratorSetting Config => KaraokeRulesetEditGeneratorSetting.NoteGeneratorConfig;

        protected override OverlayColourScheme OverlayColourScheme => OverlayColourScheme.Green;

        protected override string Title => "Layout generator config";

        protected override string Description => "Change config for layout generator.";

        protected override GeneratorConfigSection[] CreateConfigSection(Bindable<NoteGeneratorConfig> current)
        {
            return null;
        }
    }
}
