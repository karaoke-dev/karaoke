// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

#nullable disable

using osu.Framework.Bindables;
using osu.Framework.Graphics;
using osu.Game.Graphics.UserInterfaceV2;
using osu.Game.Rulesets.Karaoke.Edit.Generator.Lyrics.TimeTags.Ja;

namespace osu.Game.Rulesets.Karaoke.Screens.Edit.Beatmaps.Configs.Generator.Lyrics.TimeTags.Ja
{
    internal partial class CheckCharacterSection : GeneratorConfigSection<JaTimeTagGeneratorConfig>
    {
        private readonly LabelledSwitchButton checkんSwitchButton;
        private readonly LabelledSwitchButton checkっSwitchButton;
        private readonly LabelledSwitchButton checkBlankLineSwitchButton;

        protected override string Title => "Character checking";

        public CheckCharacterSection(Bindable<JaTimeTagGeneratorConfig> config)
            : base(config)
        {
            Children = new Drawable[]
            {
                checkんSwitchButton = new LabelledSwitchButton
                {
                    Label = "Check ん",
                    Description = "Check ん or not.",
                },
                checkっSwitchButton = new LabelledSwitchButton
                {
                    Label = "Check っ",
                    Description = "Check っ or not.",
                },
                checkBlankLineSwitchButton = new LabelledSwitchButton
                {
                    Label = "Check blank line",
                    Description = "Check blank line or not.",
                },
            };

            RegisterConfig(checkんSwitchButton.Current, nameof(JaTimeTagGeneratorConfig.Checkん));
            RegisterConfig(checkっSwitchButton.Current, nameof(JaTimeTagGeneratorConfig.Checkっ));
            RegisterConfig(checkBlankLineSwitchButton.Current, nameof(JaTimeTagGeneratorConfig.CheckBlankLine));
        }
    }
}
