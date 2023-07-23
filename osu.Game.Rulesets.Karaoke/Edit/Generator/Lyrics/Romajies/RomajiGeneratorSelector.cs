// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Globalization;
using osu.Game.Rulesets.Karaoke.Configuration;
using osu.Game.Rulesets.Karaoke.Edit.Generator.Lyrics.Romajies.Ja;

namespace osu.Game.Rulesets.Karaoke.Edit.Generator.Lyrics.Romajies;

public class RomajiGeneratorSelector : LyricGeneratorSelector<RomajiGenerateResult[], RomajiGeneratorConfig>
{
    public RomajiGeneratorSelector(KaraokeRulesetEditGeneratorConfigManager generatorConfigManager)
        : base(generatorConfigManager)
    {
        RegisterGenerator<JaRomajiGenerator, JaRomajiGeneratorConfig>(new CultureInfo(17));
        RegisterGenerator<JaRomajiGenerator, JaRomajiGeneratorConfig>(new CultureInfo(1041));
    }
}
