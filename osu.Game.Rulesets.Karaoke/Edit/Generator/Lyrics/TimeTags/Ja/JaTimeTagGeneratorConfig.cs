// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Bindables;
using osu.Game.Rulesets.Karaoke.Objects;

namespace osu.Game.Rulesets.Karaoke.Edit.Generator.Lyrics.TimeTags.Ja
{
    public class JaTimeTagGeneratorConfig : TimeTagGeneratorConfig
    {
        /// <summary>
        /// Add the <see cref="TimeTag"/> if character is "ん"
        /// </summary>
        [ConfigCategory(CATEGORY_CHECK_CHARACTER)]
        [ConfigSource("Check ん", "Check ん or not.")]
        public Bindable<bool> Checkん { get; } = new BindableBool(true);

        /// <summary>
        /// Add the <see cref="TimeTag"/> if character is "っ"
        /// </summary>
        [ConfigCategory(CATEGORY_CHECK_CHARACTER)]
        [ConfigSource("Check っ", "Check っ or not.")]
        public Bindable<bool> Checkっ { get; } = new BindableBool();

        /// <summary>
        /// Add the <see cref="TimeTag"/> if spacing is next of the alphabet.
        /// This feature will work only if enable the <see cref="TimeTagGeneratorConfig.CheckWhiteSpace"/>
        /// </summary>
        [ConfigCategory(CATEGORY_CHECK_WHITE_SPACE)]
        [ConfigSource("Check white space alphabet", "Check white space alphabet.")]
        public Bindable<bool> CheckWhiteSpaceAlphabet { get; } = new BindableBool();

        /// <summary>
        /// Add the <see cref="TimeTag"/> if spacing is next of the digit.
        /// This feature will work only if enable the <see cref="TimeTagGeneratorConfig.CheckWhiteSpace"/>
        /// </summary>
        [ConfigCategory(CATEGORY_CHECK_WHITE_SPACE)]
        [ConfigSource("Check white space digit", "Check white space digit.")]
        public Bindable<bool> CheckWhiteSpaceDigit { get; } = new BindableBool();

        /// <summary>
        /// Add the <see cref="TimeTag"/> if spacing is next of the symbol.
        /// This feature will work only if enable the <see cref="TimeTagGeneratorConfig.CheckWhiteSpace"/>
        /// </summary>
        [ConfigCategory(CATEGORY_CHECK_WHITE_SPACE)]
        [ConfigSource("Check white space ascii symbol", "Check white space ascii symbol.")]
        public Bindable<bool> CheckWhiteSpaceAsciiSymbol { get; } = new BindableBool();

        public JaTimeTagGeneratorConfig()
        {
            CheckLineEndKeyUp.Default = true;
            CheckLineEndKeyUp.SetDefault();

            CheckWhiteSpace.Default = true;
            CheckWhiteSpace.SetDefault();

            CheckWhiteSpaceKeyUp.Default = true;
            CheckWhiteSpaceKeyUp.SetDefault();
        }
    }
}
