// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Globalization;
using osu.Game.Rulesets.Karaoke.Edit.Generator.TimeTags.Ja;
using osu.Game.Rulesets.Karaoke.Edit.Generator.TimeTags.Zh;
using osu.Game.Rulesets.Karaoke.Objects;

namespace osu.Game.Rulesets.Karaoke.Edit.Generator.TimeTags
{
    public class TimeTagGeneratorSelector : GeneratorSelector<TimeTagGenerator, TimeTagGeneratorConfig>
    {
        public TimeTagGeneratorSelector()
        {
            RegistGenerator<JaTimeTagGenerator, JaTimeTagGeneratorConfig>(new CultureInfo(17));
            RegistGenerator<JaTimeTagGenerator, JaTimeTagGeneratorConfig>(new CultureInfo(1041));
            RegistGenerator<ZhTimeTagGenerator, ZhTimeTagGeneratorConfig>(new CultureInfo(1028));
        }

        public TimeTag[] GenerateTimeTags(Lyric lyric)
        {
            if (!Generator.TryGetValue(lyric.Language, out var generator))
                return null;

            return generator.Value.CreateTimeTags(lyric);
        }
    }
}
