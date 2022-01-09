// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using osu.Game.Rulesets.Karaoke.Configuration;

namespace osu.Game.Rulesets.Karaoke.Tests.Editor.Generator
{
    public abstract class BaseGeneratorSelectorTest<TSelector> where TSelector : class
    {
        protected TSelector CreateSelector()
        {
            var configManager = new KaraokeRulesetEditGeneratorConfigManager();
            return Activator.CreateInstance(typeof(TSelector), configManager) as TSelector;
        }
    }
}
