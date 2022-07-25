// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

#nullable disable

using osu.Framework.Bindables;
using osu.Game.Overlays;
using osu.Game.Rulesets.Karaoke.Configuration;
using osu.Game.Rulesets.Karaoke.Edit.Generator.ReferenceLyric;

namespace osu.Game.Rulesets.Karaoke.Edit.Configs.Generator.ReferenceLyric
{
    public class ReferenceLyricGeneratorConfigDialog : GeneratorConfigDialog<ReferenceLyricDetectorConfig>
    {
        protected override KaraokeRulesetEditGeneratorSetting Config => KaraokeRulesetEditGeneratorSetting.NoteGeneratorConfig;

        protected override OverlayColourScheme OverlayColourScheme => OverlayColourScheme.Green;

        protected override string Title => "Reference lyric config";

        protected override string Description => "Change config for reference lyric detector.";

        protected override GeneratorConfigSection[] CreateConfigSection(Bindable<ReferenceLyricDetectorConfig> current)
        {
            return new GeneratorConfigSection[]
            {
                new GenericSection(current),
            };
        }
    }
}
