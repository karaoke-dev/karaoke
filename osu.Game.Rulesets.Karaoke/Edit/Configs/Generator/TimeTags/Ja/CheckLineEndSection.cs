// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Bindables;
using osu.Framework.Graphics;
using osu.Game.Graphics.UserInterfaceV2;
using osu.Game.Rulesets.Karaoke.Edit.Generator.TimeTags.Ja;

namespace osu.Game.Rulesets.Karaoke.Edit.Configs.Generator.TimeTags.Ja
{
    internal class CheckLineEndSection : GeneratorConfigSection<JaTimeTagGeneratorConfig>
    {
        private readonly LabelledSwitchButton checkLineEndKeyUpSwitchButton;

        protected override string Title => "Line end checking";

        public CheckLineEndSection(Bindable<JaTimeTagGeneratorConfig> config)
            : base(config)
        {
            Children = new Drawable[]
            {
                checkLineEndKeyUpSwitchButton = new LabelledSwitchButton
                {
                    Label = "Use key-up time tag in line end",
                    Description = "Use key-up time tag in line end",
                },
            };

            RegisterConfig(checkLineEndKeyUpSwitchButton.Current, nameof(JaTimeTagGeneratorConfig.CheckLineEndKeyUp));
        }
    }
}
