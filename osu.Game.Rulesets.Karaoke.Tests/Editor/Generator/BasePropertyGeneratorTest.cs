// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using NUnit.Framework;
using osu.Game.Rulesets.Karaoke.Edit.Generator;

namespace osu.Game.Rulesets.Karaoke.Tests.Editor.Generator;

public abstract class BasePropertyGeneratorTest<TGenerator, TItem, TProperty, TConfig>
    : BasePropertyGeneratorTest<TGenerator, TItem, TProperty>
    where TGenerator : PropertyGenerator<TItem, TProperty>
    where TConfig : GeneratorConfig, new()
{
    protected static TConfig GeneratorConfig(Action<TConfig>? action = null)
    {
        var config = new TConfig();
        GeneratorConfigHelper.ClearValue(config);

        action?.Invoke(config);
        return config;
    }

    protected static TConfig GeneratorDefaultConfig(Action<TConfig>? action = null)
    {
        var config = new TConfig();

        action?.Invoke(config);
        return config;
    }

    protected static TGenerator GenerateGenerator(TConfig config)
    {
        if (Activator.CreateInstance(typeof(TGenerator), config) is not TGenerator generator)
            throw new ArgumentNullException(nameof(generator));

        return generator;
    }

    protected static void CheckCanGenerate(TItem item, bool canGenerate, TConfig config)
    {
        var generator = GenerateGenerator(config);

        CheckCanGenerate(item, canGenerate, generator);
    }

    protected void CheckGenerateResult(TItem item, TProperty expected, TConfig config)
    {
        var generator = GenerateGenerator(config);

        CheckGenerateResult(item, expected, generator);
    }
}

public abstract class BasePropertyGeneratorTest<TGenerator, TItem, TProperty>
    where TGenerator : PropertyGenerator<TItem, TProperty>
{
    protected static void CheckCanGenerate(TItem item, bool canGenerate, TGenerator generator)
    {
        bool actual = generator.CanGenerate(item);
        Assert.AreEqual(canGenerate, actual);
    }

    protected void CheckGenerateResult(TItem item, TProperty expected, TGenerator generator)
    {
        // create time tag and actually time tag.
        var actual = generator.Generate(item);
        AssertEqual(expected, actual);
    }

    protected abstract void AssertEqual(TProperty expected, TProperty actual);
}
