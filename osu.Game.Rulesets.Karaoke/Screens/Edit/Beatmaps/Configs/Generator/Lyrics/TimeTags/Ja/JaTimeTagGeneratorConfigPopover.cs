// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

#nullable disable

using osu.Framework.Bindables;
using osu.Game.Rulesets.Karaoke.Configuration;
using osu.Game.Rulesets.Karaoke.Edit.Generator.Lyrics.TimeTags.Ja;

namespace osu.Game.Rulesets.Karaoke.Screens.Edit.Beatmaps.Configs.Generator.Lyrics.TimeTags.Ja
{
    public partial class JaTimeTagGeneratorConfigPopover : LegacyGeneratorConfigPopover<JaTimeTagGeneratorConfig>
    {
        protected override KaraokeRulesetEditGeneratorSetting Config => KaraokeRulesetEditGeneratorSetting.JaTimeTagGeneratorConfig;

        protected override GeneratorConfigSection[] CreateConfigSection(Bindable<JaTimeTagGeneratorConfig> current)
        {
            return new GeneratorConfigSection[]
            {
                new CheckCharacterSection(current),
                new CheckLineEndSection(current),
                new CheckWhiteSpaceSection(current)
            };
        }
    }
}
