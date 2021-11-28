// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using osu.Game.Rulesets.Karaoke.Objects;

namespace osu.Game.Rulesets.Karaoke.Edit.Generator
{
    public abstract class GeneratorSelector<TBaseGenerator, TBaseConfig> where TBaseGenerator : class
    {
        protected Dictionary<CultureInfo, Lazy<TBaseGenerator>> Generator { get; } = new();

        protected void RegisterGenerator<TGenerator, TConfig>(CultureInfo info) where TGenerator : TBaseGenerator where TConfig : TBaseConfig, new()
        {
            Generator.Add(info, new Lazy<TBaseGenerator>(() =>
            {
                // todo : get config from setting.
                var config = new TConfig();
                var generator = Activator.CreateInstance(typeof(TGenerator), config) as TBaseGenerator;
                return generator;
            }));
        }

        public bool CanGenerate(Lyric lyric)
            => Generator.Keys.Any(k => EqualityComparer<CultureInfo>.Default.Equals(k, lyric.Language));
    }
}
