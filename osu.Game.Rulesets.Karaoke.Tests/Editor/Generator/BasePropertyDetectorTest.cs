// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using NUnit.Framework;
using osu.Game.Rulesets.Karaoke.Edit.Generator;
using osu.Game.Rulesets.Karaoke.Utils;

namespace osu.Game.Rulesets.Karaoke.Tests.Editor.Generator;

public abstract class BasePropertyDetectorTest<TDetector, TItem, TProperty, TConfig>
    : BasePropertyDetectorTest<TDetector, TItem, TProperty>
    where TDetector : PropertyDetector<TItem, TProperty>
    where TConfig : GeneratorConfig, new()
{
    protected static TConfig GeneratorEmptyConfig(Action<TConfig>? action = null)
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

    protected static void CheckCanDetect(TItem item, bool canDetect, TConfig config)
    {
        var detector = ActivatorUtils.CreateInstance<TDetector>(config);

        CheckCanDetect(item, canDetect, detector);
    }

    protected void CheckDetectResult(TItem item, TProperty expected, TConfig config)
    {
        var detector = ActivatorUtils.CreateInstance<TDetector>(config);

        CheckDetectResult(item, expected, detector);
    }
}

public abstract class BasePropertyDetectorTest<TDetector, TItem, TProperty>
    where TDetector : PropertyDetector<TItem, TProperty>
{
    protected static void CheckCanDetect(TItem item, bool canDetect, TDetector detector)
    {
        bool actual = detector.CanDetect(item);
        Assert.That(actual, Is.EqualTo(canDetect));
    }

    protected void CheckDetectResult(TItem item, TProperty expected, TDetector detector)
    {
        // create time tag and actually time tag.
        var actual = detector.Detect(item);
        AssertEqual(expected, actual);
    }

    protected abstract void AssertEqual(TProperty expected, TProperty actual);
}
