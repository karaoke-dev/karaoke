// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Globalization;
using osu.Game.Rulesets.Karaoke.Configuration;
using osu.Game.Rulesets.Karaoke.Edit.Generator.Lyrics.RomajiTags.Ja;
using osu.Game.Rulesets.Karaoke.Objects;

namespace osu.Game.Rulesets.Karaoke.Edit.Generator.Lyrics.RomajiTags;

public class RomajiTagGeneratorSelector : LyricGeneratorSelector<RomajiTag[], RomajiTagGeneratorConfig>
{
    public RomajiTagGeneratorSelector(KaraokeRulesetEditGeneratorConfigManager generatorConfigManager)
        : base(generatorConfigManager)
    {
        RegisterGenerator<JaRomajiTagGenerator, JaRomajiTagGeneratorConfig>(new CultureInfo(17));
        RegisterGenerator<JaRomajiTagGenerator, JaRomajiTagGeneratorConfig>(new CultureInfo(1041));
    }
}
