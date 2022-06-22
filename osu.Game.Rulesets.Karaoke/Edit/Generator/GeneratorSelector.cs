// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

#nullable disable

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using osu.Framework.Localisation;
using osu.Game.Rulesets.Karaoke.Configuration;
using osu.Game.Rulesets.Karaoke.Edit.Generator.Types;
using osu.Game.Rulesets.Karaoke.Objects;

namespace osu.Game.Rulesets.Karaoke.Edit.Generator
{
    public abstract class GeneratorSelector<TProperty, TBaseConfig> : ILyricPropertyGenerator<TProperty>
    {
        protected Dictionary<CultureInfo, Lazy<ILyricPropertyGenerator<TProperty>>> Generator { get; } = new();

        private readonly KaraokeRulesetEditGeneratorConfigManager generatorConfigManager;

        protected GeneratorSelector(KaraokeRulesetEditGeneratorConfigManager generatorConfigManager)
        {
            this.generatorConfigManager = generatorConfigManager;
        }

        protected void RegisterGenerator<TGenerator, TConfig>(CultureInfo info) where TGenerator : ILyricPropertyGenerator<TProperty> where TConfig : TBaseConfig, new()
        {
            Generator.Add(info, new Lazy<ILyricPropertyGenerator<TProperty>>(() =>
            {
                var generatorSetting = GetGeneratorConfigSetting(info);
                var config = generatorConfigManager.Get<TConfig>(generatorSetting);
                var generator = Activator.CreateInstance(typeof(TGenerator), config) as ILyricPropertyGenerator<TProperty>;
                return generator;
            }));
        }

        protected abstract KaraokeRulesetEditGeneratorSetting GetGeneratorConfigSetting(CultureInfo info);

        public LocalisableString? GetInvalidMessage(Lyric lyric)
        {
            if (lyric.Language == null)
                return "Oops, language is missing.";

            var generator = Generator.FirstOrDefault(g => EqualityComparer<CultureInfo>.Default.Equals(g.Key, lyric.Language));
            if (generator.Key == null)
                return "Sorry, the language of lyric is not supported yet.";

            return generator.Value.Value.GetInvalidMessage(lyric);
        }

        public abstract TProperty Generate(Lyric lyric);
    }
}
