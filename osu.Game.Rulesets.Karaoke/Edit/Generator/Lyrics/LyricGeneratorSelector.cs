// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using System.Collections.Generic;
using System.Globalization;
using osu.Framework.Localisation;
using osu.Game.Rulesets.Karaoke.Configuration;
using osu.Game.Rulesets.Karaoke.Objects;

namespace osu.Game.Rulesets.Karaoke.Edit.Generator.Lyrics
{
    public abstract class LyricGeneratorSelector<TProperty, TBaseConfig> : PropertyGenerator<Lyric, TProperty>
        where TBaseConfig : GeneratorConfig
    {
        private Dictionary<CultureInfo, Lazy<PropertyGenerator<Lyric, TProperty>>> generator { get; } = new();

        private readonly KaraokeRulesetEditGeneratorConfigManager generatorConfigManager;

        protected LyricGeneratorSelector(KaraokeRulesetEditGeneratorConfigManager generatorConfigManager)
        {
            this.generatorConfigManager = generatorConfigManager;
        }

        protected void RegisterGenerator<TGenerator, TConfig>(CultureInfo info)
            where TGenerator : LyricPropertyGenerator<TProperty, TConfig>
            where TConfig : TBaseConfig, new()
        {
            generator.Add(info, new Lazy<PropertyGenerator<Lyric, TProperty>>(() =>
            {
                var config = generatorConfigManager.Get<TConfig>();
                if (Activator.CreateInstance(typeof(TGenerator), config) is not PropertyGenerator<Lyric, TProperty> propertyGenerator)
                    throw new InvalidCastException();

                return propertyGenerator;
            }));
        }

        protected override TProperty GenerateFromItem(Lyric item)
        {
            if (item.Language == null)
                throw new GeneratorNotSupportedException();

            if (!generator.TryGetValue(item.Language, out var propertyGenerator))
                throw new GeneratorNotSupportedException();

            return propertyGenerator.Value.Generate(item);
        }

        protected override LocalisableString? GetInvalidMessageFromItem(Lyric item)
        {
            if (item.Language == null)
                return "Oops, language is missing.";

            if (string.IsNullOrWhiteSpace(item.Text))
                return "Should have the text in the lyric";

            if (!generator.TryGetValue(item.Language, out var propertyGenerator))
                return "Sorry, the language of lyric is not supported yet.";

            return propertyGenerator.Value.GetInvalidMessage(item);
        }
    }
}
