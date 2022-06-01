// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using osu.Game.Rulesets.Karaoke.Configuration;
using osu.Game.Rulesets.Karaoke.Edit.Generator.Types;
using osu.Game.Rulesets.Karaoke.Objects;

namespace osu.Game.Rulesets.Karaoke.Edit.Generator
{
    public abstract class GeneratorSelector<TProperty, TBaseConfig>
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

        public bool CanGenerate(Lyric lyric)
            => Generator.Keys.Any(k => EqualityComparer<CultureInfo>.Default.Equals(k, lyric.Language));
    }
}
