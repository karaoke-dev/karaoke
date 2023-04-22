// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using osu.Game.Rulesets.Karaoke.Configuration;

namespace osu.Game.Rulesets.Karaoke.Edit.Generator;

/// <summary>
/// Able to select generator by property.
/// </summary>
/// <typeparam name="TItem">The item that want to generate the property.</typeparam>
/// <typeparam name="TProperty">The property in the item that will be generated.</typeparam>
/// <typeparam name="TBaseConfig">The config.</typeparam>
public abstract class GeneratorSelector<TItem, TProperty, TBaseConfig> : PropertyGenerator<TItem, TProperty>
    where TBaseConfig : GeneratorConfig
{
    private Dictionary<Func<TItem, bool>, Lazy<PropertyGenerator<TItem, TProperty>>> generators { get; } = new();

    private readonly KaraokeRulesetEditGeneratorConfigManager generatorConfigManager;

    protected GeneratorSelector(KaraokeRulesetEditGeneratorConfigManager generatorConfigManager)
    {
        this.generatorConfigManager = generatorConfigManager;
    }

    protected void RegisterGenerator<TGenerator, TConfig>(Func<TItem, bool> selector)
        where TGenerator : PropertyGenerator<TItem, TProperty>
        where TConfig : TBaseConfig, new()
    {
        generators.Add(selector, new Lazy<PropertyGenerator<TItem, TProperty>>(() =>
        {
            var config = generatorConfigManager.Get<TConfig>();
            if (Activator.CreateInstance(typeof(TGenerator), config) is not PropertyGenerator<TItem, TProperty> propertyGenerator)
                throw new InvalidCastException();

            return propertyGenerator;
        }));
    }

    protected bool TryGetGenerator(TItem item, [MaybeNullWhen(false)] out PropertyGenerator<TItem, TProperty> generator)
    {
        foreach (var (func, propertyGenerator) in generators)
        {
            if (!func(item))
                continue;

            generator = propertyGenerator.Value;
            return true;
        }

        generator = null;
        return false;
    }
}
