// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using osu.Game.Rulesets.Karaoke.Configuration;
using osu.Game.Rulesets.Karaoke.Edit.Generator;

namespace osu.Game.Rulesets.Karaoke.Tests.Editor.Generator;

public abstract class BaseGeneratorSelectorTest<TGenerator, TItem, TProperty>
    : BasePropertyGeneratorTest<TGenerator, TItem, TProperty>
    where TGenerator : PropertyGenerator<TItem, TProperty>
{
    protected TGenerator CreateSelector()
    {
        var configManager = new KaraokeRulesetEditGeneratorConfigManager();
        if (Activator.CreateInstance(typeof(TGenerator), configManager) is not TGenerator selector)
            throw new InvalidCastException();

        return selector;
    }
}
