// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Bindables;
using osu.Framework.Graphics;
using osu.Game.Graphics.UserInterfaceV2;
using osu.Game.Rulesets.Karaoke.Edit.Generator.TimeTags.Ja;

namespace osu.Game.Rulesets.Karaoke.Edit.Configs.Generator.TimeTags.Ja
{
    internal class CheckWhiteSpaceSection : GeneratorConfigSection<JaTimeTagGeneratorConfig>
    {
        private LabelledSwitchButton checkWhiteSpaceSwitchButton;
        private LabelledSwitchButton checkWhiteSpaceKeyUpSwitchButton;
        private LabelledSwitchButton checkWhiteSpaceAlphabetSwitchButton;
        private LabelledSwitchButton checkWhiteSpaceDigitSwitchButton;
        private LabelledSwitchButton checkWhiteSpaceAsciiSymbolSwitchButton;

        protected override string Title => "White space checking";

        public CheckWhiteSpaceSection(Bindable<JaTimeTagGeneratorConfig> config)
            : base(config)
        {
            Children = new Drawable[]
            {
                checkWhiteSpaceSwitchButton = new LabelledSwitchButton
                {
                    Label = "Check white space",
                    Description = "Check white space",
                },
                checkWhiteSpaceKeyUpSwitchButton = new LabelledSwitchButton
                {
                    Label = "Use key-up",
                    Description = "Use key-up",
                },
                checkWhiteSpaceAlphabetSwitchButton = new LabelledSwitchButton
                {
                    Label = "Check white space alphabet",
                    Description = "Check white space alphabet.",
                },
                checkWhiteSpaceDigitSwitchButton = new LabelledSwitchButton
                {
                    Label = "Check white space digit",
                    Description = "Check white space digit.",
                },
                checkWhiteSpaceAsciiSymbolSwitchButton = new LabelledSwitchButton
                {
                    Label = "Check white space ascii symble",
                    Description = "Check white space ascii symble.",
                },
            };

            RegistConfig(checkWhiteSpaceSwitchButton.Current, nameof(JaTimeTagGeneratorConfig.CheckWhiteSpace));
            RegistConfig(checkWhiteSpaceKeyUpSwitchButton.Current, nameof(JaTimeTagGeneratorConfig.CheckWhiteSpaceKeyUp));
            RegistConfig(checkWhiteSpaceAlphabetSwitchButton.Current, nameof(JaTimeTagGeneratorConfig.CheckWhiteSpaceAlphabet));
            RegistConfig(checkWhiteSpaceDigitSwitchButton.Current, nameof(JaTimeTagGeneratorConfig.CheckWhiteSpaceDigit));
            RegistConfig(checkWhiteSpaceAsciiSymbolSwitchButton.Current, nameof(JaTimeTagGeneratorConfig.CheckWhiteSpaceAsciiSymbol));
        }
    }
}
