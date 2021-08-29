// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using osu.Game.Rulesets.Karaoke.Objects;

namespace osu.Game.Rulesets.Karaoke.Edit.Generator
{
    public abstract class GeneratorSelector<TGenerator, TConfig> where TGenerator : class
    {
        protected Dictionary<CultureInfo, Lazy<TGenerator>> Generator { get; } = new();

        protected void RegisterGenerator<T, Tc>(CultureInfo info) where T : TGenerator where Tc : TConfig, new()
        {
            Generator.Add(info, new Lazy<TGenerator>(() =>
            {
                // todo : get config from setting.
                var config = new Tc();
                var generator = Activator.CreateInstance(typeof(T), config) as TGenerator;
                return generator;
            }));
        }

        public bool Generatable(Lyric lyric)
            => Generator.Keys.Any(k => EqualityComparer<CultureInfo>.Default.Equals(k, lyric.Language));
    }
}
