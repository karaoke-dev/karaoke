// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Bindables;
using osu.Framework.Graphics;
using osu.Game.Graphics.UserInterfaceV2;
using osu.Game.Rulesets.Karaoke.Edit.Generator.Lyrics.ReferenceLyric;

namespace osu.Game.Rulesets.Karaoke.Screens.Edit.Beatmaps.Configs.Generator.Lyrics.ReferenceLyric
{
    public partial class GenericSection : GeneratorConfigSection<ReferenceLyricDetectorConfig>
    {
        private readonly LabelledSwitchButton ignorePrefixAndPostfixSymbol;

        protected override string Title => "Generic";

        public GenericSection(Bindable<ReferenceLyricDetectorConfig> current)
            : base(current)
        {
            Children = new Drawable[]
            {
                ignorePrefixAndPostfixSymbol = new LabelledSwitchButton
                {
                    Label = "Ruby as Katakana",
                    Description = "Ruby as Katakana.",
                },
            };

            RegisterConfig(ignorePrefixAndPostfixSymbol.Current, nameof(ReferenceLyricDetectorConfig.IgnorePrefixAndPostfixSymbol));
        }
    }
}
