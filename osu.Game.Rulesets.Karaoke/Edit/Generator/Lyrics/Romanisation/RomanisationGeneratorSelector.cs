// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Collections.Generic;
using System.Globalization;
using osu.Game.Rulesets.Karaoke.Configuration;
using osu.Game.Rulesets.Karaoke.Edit.Generator.Lyrics.Romanisation.Ja;
using osu.Game.Rulesets.Karaoke.Objects;

namespace osu.Game.Rulesets.Karaoke.Edit.Generator.Lyrics.Romanisation;

public class RomanisationGeneratorSelector : LyricGeneratorSelector<IReadOnlyDictionary<TimeTag, RomanisationGenerateResult>, RomanisationGeneratorConfig>
{
    public RomanisationGeneratorSelector(KaraokeRulesetEditGeneratorConfigManager generatorConfigManager)
        : base(generatorConfigManager)
    {
        RegisterGenerator<JaRomanisationGenerator, JaRomanisationGeneratorConfig>(new CultureInfo(17));
        RegisterGenerator<JaRomanisationGenerator, JaRomanisationGeneratorConfig>(new CultureInfo(1041));
    }
}
