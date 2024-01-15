// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Collections.Generic;
using System.Globalization;
using osu.Game.Rulesets.Karaoke.Configuration;
using osu.Game.Rulesets.Karaoke.Edit.Generator.Lyrics.Romanization.Ja;
using osu.Game.Rulesets.Karaoke.Objects;

namespace osu.Game.Rulesets.Karaoke.Edit.Generator.Lyrics.Romanization;

public class RomanizationGeneratorSelector : LyricGeneratorSelector<IReadOnlyDictionary<TimeTag, RomanizationGenerateResult>, RomanizationGeneratorConfig>
{
    public RomanizationGeneratorSelector(KaraokeRulesetEditGeneratorConfigManager generatorConfigManager)
        : base(generatorConfigManager)
    {
        RegisterGenerator<JaRomanizationGenerator, JaRomanizationGeneratorConfig>(new CultureInfo(17));
        RegisterGenerator<JaRomanizationGenerator, JaRomanizationGeneratorConfig>(new CultureInfo(1041));
    }
}
