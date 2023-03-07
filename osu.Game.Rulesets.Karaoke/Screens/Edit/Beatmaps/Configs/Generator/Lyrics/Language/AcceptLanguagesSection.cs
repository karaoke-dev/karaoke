// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Bindables;
using osu.Game.Rulesets.Karaoke.Edit.Generator.Lyrics.Language;
using osu.Game.Rulesets.Karaoke.Screens.Edit.Beatmaps.Configs.Generator.Components;

namespace osu.Game.Rulesets.Karaoke.Screens.Edit.Beatmaps.Configs.Generator.Lyrics.Language
{
    public partial class AcceptLanguagesSection : GeneratorConfigSection<LanguageDetectorConfig>
    {
        protected override string Title => "Accept languages";

        public AcceptLanguagesSection(Bindable<LanguageDetectorConfig> current)
            : base(current)
        {
            var languagesSelector = new LanguagesSelector();

            Add(languagesSelector);
            RegisterConfig(languagesSelector.BindableCultureInfo, nameof(LanguageDetectorConfig.AcceptLanguages));
        }
    }
}
