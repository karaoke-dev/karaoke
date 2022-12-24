// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using osu.Framework.Localisation;
using osu.Game.Rulesets.Karaoke.Configuration;
using osu.Game.Rulesets.Karaoke.Objects;

namespace osu.Game.Rulesets.Karaoke.Edit.Generator.Lyrics
{
    public abstract class LyricGeneratorSelector<TProperty, TBaseConfig> : PropertyGenerator<Lyric, TProperty>
    {
        protected Dictionary<CultureInfo, Lazy<PropertyGenerator<Lyric, TProperty>>> Generator { get; } = new();

        private readonly KaraokeRulesetEditGeneratorConfigManager generatorConfigManager;

        protected LyricGeneratorSelector(KaraokeRulesetEditGeneratorConfigManager generatorConfigManager)
        {
            this.generatorConfigManager = generatorConfigManager;
        }

        protected void RegisterGenerator<TGenerator, TConfig>(CultureInfo info)
            where TGenerator : LyricPropertyGenerator<TProperty, TConfig>
            where TConfig : TBaseConfig, IHasConfig<TConfig>, new()
        {
            Generator.Add(info, new Lazy<PropertyGenerator<Lyric, TProperty>>(() =>
            {
                var generatorSetting = GetGeneratorConfigSetting(info);
                var config = generatorConfigManager.Get<TConfig>(generatorSetting);
                if (Activator.CreateInstance(typeof(TGenerator), config) is not PropertyGenerator<Lyric, TProperty> generator)
                    throw new InvalidCastException();

                return generator;
            }));
        }

        protected abstract KaraokeRulesetEditGeneratorSetting GetGeneratorConfigSetting(CultureInfo info);

        protected override LocalisableString? GetInvalidMessageFromItem(Lyric item)
        {
            if (item.Language == null)
                return "Oops, language is missing.";

            var generator = Generator.FirstOrDefault(g => EqualityComparer<CultureInfo>.Default.Equals(g.Key, item.Language));
            if (generator.Key == null)
                return "Sorry, the language of lyric is not supported yet.";

            return generator.Value.Value.GetInvalidMessage(item);
        }
    }
}
