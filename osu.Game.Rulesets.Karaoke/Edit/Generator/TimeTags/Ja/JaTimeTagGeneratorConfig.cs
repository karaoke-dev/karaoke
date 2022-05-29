// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Game.Rulesets.Karaoke.Edit.Generator.Types;

namespace osu.Game.Rulesets.Karaoke.Edit.Generator.TimeTags.Ja
{
    public class JaTimeTagGeneratorConfig : TimeTagGeneratorConfig, IHasConfig<JaTimeTagGeneratorConfig>
    {
        public bool Checkん { get; set; }

        public bool Checkっ { get; set; }

        public JaTimeTagGeneratorConfig CreateDefaultConfig() =>
            new()
            {
                Checkん = true,
                CheckLineEndKeyUp = true,
                CheckWhiteSpace = true,
                CheckWhiteSpaceKeyUp = true,
            };
    }
}
