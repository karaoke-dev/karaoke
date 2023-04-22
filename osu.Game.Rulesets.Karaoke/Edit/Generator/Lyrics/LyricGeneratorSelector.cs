// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Globalization;
using osu.Framework.Localisation;
using osu.Game.Rulesets.Karaoke.Configuration;
using osu.Game.Rulesets.Karaoke.Objects;

namespace osu.Game.Rulesets.Karaoke.Edit.Generator.Lyrics
{
    public abstract class LyricGeneratorSelector<TProperty, TBaseConfig> : GeneratorSelector<Lyric, TProperty, CultureInfo, TBaseConfig>
        where TBaseConfig : GeneratorConfig
    {
        protected LyricGeneratorSelector(KaraokeRulesetEditGeneratorConfigManager generatorConfigManager)
            : base(generatorConfigManager)
        {
        }

        protected override LocalisableString? GetInvalidMessageFromItem(Lyric item)
        {
            if (item.Language == null)
                return "Oops, language is missing.";

            if (string.IsNullOrWhiteSpace(item.Text))
                return "Should have the text in the lyric";

            if (!TryGetGenerator(item.Language, out var generator))
                return "Sorry, the language of lyric is not supported yet.";

            return generator.GetInvalidMessage(item);
        }

        protected override TProperty GenerateFromItem(Lyric item)
        {
            if (item.Language == null)
                throw new GeneratorNotSupportedException();

            if (!TryGetGenerator(item.Language, out var generator))
                throw new GeneratorNotSupportedException();

            return generator.Generate(item);
        }
    }
}
