// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

#nullable disable

using osu.Framework.Bindables;
using osu.Framework.Graphics;
using osu.Game.Graphics.UserInterfaceV2;
using osu.Game.Rulesets.Karaoke.Edit.Generator.RubyTags.Ja;

namespace osu.Game.Rulesets.Karaoke.Edit.Configs.Generator.RubyTags.Ja
{
    public class GenericSection : GeneratorConfigSection<JaRubyTagGeneratorConfig>
    {
        private readonly LabelledSwitchButton rubyAsKatakanaSwitchButton;
        private readonly LabelledSwitchButton enableDuplicatedRubySwitchButton;

        protected override string Title => "Generic";

        public GenericSection(Bindable<JaRubyTagGeneratorConfig> config)
            : base(config)
        {
            Children = new Drawable[]
            {
                rubyAsKatakanaSwitchButton = new LabelledSwitchButton
                {
                    Label = "Ruby as Katakana",
                    Description = "Ruby as Katakana.",
                },
                enableDuplicatedRubySwitchButton = new LabelledSwitchButton
                {
                    Label = "Enable duplicated ruby.",
                    Description = "Enable output duplicated ruby even it's match with lyric.",
                },
            };

            RegisterConfig(rubyAsKatakanaSwitchButton.Current, nameof(JaRubyTagGeneratorConfig.RubyAsKatakana));
            RegisterConfig(enableDuplicatedRubySwitchButton.Current, nameof(JaRubyTagGeneratorConfig.EnableDuplicatedRuby));
        }
    }
}
