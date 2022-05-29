// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Game.Rulesets.Karaoke.Edit.Generator.Types;
using osu.Game.Rulesets.Karaoke.Objects;

namespace osu.Game.Rulesets.Karaoke.Edit.Generator.TimeTags.Ja
{
    public class JaTimeTagGeneratorConfig : TimeTagGeneratorConfig, IHasConfig<JaTimeTagGeneratorConfig>
    {
        /// <summary>
        /// Add the <see cref="TimeTag"/> if spacing is next of the alphabet.
        /// This feature will work only if enable the <see cref="TimeTagGeneratorConfig.CheckWhiteSpace"/>
        /// </summary>
        public bool CheckWhiteSpaceAlphabet { get; set; }

        /// <summary>
        /// Add the <see cref="TimeTag"/> if spacing is next of the digit.
        /// This feature will work only if enable the <see cref="TimeTagGeneratorConfig.CheckWhiteSpace"/>
        /// </summary>
        public bool CheckWhiteSpaceDigit { get; set; }

        /// <summary>
        /// Add the <see cref="TimeTag"/> if spacing is next of the symbol.
        /// This feature will work only if enable the <see cref="TimeTagGeneratorConfig.CheckWhiteSpace"/>
        /// </summary>
        public bool CheckWhiteSpaceAsciiSymbol { get; set; }

        /// <summary>
        /// Add the <see cref="TimeTag"/> if character is "ん"
        /// </summary>
        public bool Checkん { get; set; }

        /// <summary>
        /// Add the <see cref="TimeTag"/> if character is "っ"
        /// </summary>
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
