// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Globalization;
using osu.Game.Rulesets.Karaoke.Edit.Generator.RubyTags.Ja;
using osu.Game.Rulesets.Karaoke.Objects;

namespace osu.Game.Rulesets.Karaoke.Edit.Generator.RubyTags
{
    public class RubyTagGeneratorSelector : GeneratorSelector<RubyTagGenerator, RubyTagGeneratorConfig>
    {
        public RubyTagGeneratorSelector()
        {
            RegistGenerator<JaRubyTagGenerator, JaRubyTagGeneratorConfig>(new CultureInfo(17));
            RegistGenerator<JaRubyTagGenerator, JaRubyTagGeneratorConfig>(new CultureInfo(1041));
        }

        public RubyTag[] GenerateRubyTags(Lyric lyric)
        {
            if (!Generator.TryGetValue(lyric.Language, out var generator))
                return null;

            return generator.Value.CreateRubyTags(lyric);
        }
    }
}
