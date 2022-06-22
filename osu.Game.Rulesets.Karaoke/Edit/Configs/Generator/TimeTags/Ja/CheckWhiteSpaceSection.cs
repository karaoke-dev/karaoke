// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

#nullable disable

using osu.Framework.Bindables;
using osu.Framework.Graphics;
using osu.Game.Graphics.UserInterfaceV2;
using osu.Game.Rulesets.Karaoke.Edit.Generator.TimeTags.Ja;

namespace osu.Game.Rulesets.Karaoke.Edit.Configs.Generator.TimeTags.Ja
{
    internal class CheckWhiteSpaceSection : GeneratorConfigSection<JaTimeTagGeneratorConfig>
    {
        private readonly LabelledSwitchButton checkWhiteSpaceSwitchButton;
        private readonly LabelledSwitchButton checkWhiteSpaceKeyUpSwitchButton;
        private readonly LabelledSwitchButton checkWhiteSpaceAlphabetSwitchButton;
        private readonly LabelledSwitchButton checkWhiteSpaceDigitSwitchButton;
        private readonly LabelledSwitchButton checkWhiteSpaceAsciiSymbolSwitchButton;

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
                    Label = "Check white space ascii symbol",
                    Description = "Check white space ascii symbol.",
                },
            };

            RegisterConfig(checkWhiteSpaceSwitchButton.Current, nameof(JaTimeTagGeneratorConfig.CheckWhiteSpace));
            RegisterConfig(checkWhiteSpaceKeyUpSwitchButton.Current, nameof(JaTimeTagGeneratorConfig.CheckWhiteSpaceKeyUp));
            RegisterConfig(checkWhiteSpaceAlphabetSwitchButton.Current, nameof(JaTimeTagGeneratorConfig.CheckWhiteSpaceAlphabet));
            RegisterConfig(checkWhiteSpaceDigitSwitchButton.Current, nameof(JaTimeTagGeneratorConfig.CheckWhiteSpaceDigit));
            RegisterConfig(checkWhiteSpaceAsciiSymbolSwitchButton.Current, nameof(JaTimeTagGeneratorConfig.CheckWhiteSpaceAsciiSymbol));

            RegisterDisableTrigger(checkWhiteSpaceSwitchButton.Current, new Drawable[]
            {
                checkWhiteSpaceKeyUpSwitchButton,
                checkWhiteSpaceAlphabetSwitchButton,
                checkWhiteSpaceDigitSwitchButton,
                checkWhiteSpaceAsciiSymbolSwitchButton
            });
        }
    }
}
