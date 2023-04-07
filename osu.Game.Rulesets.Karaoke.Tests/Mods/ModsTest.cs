// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using NUnit.Framework;
using osu.Game.Rulesets.Mods;

namespace osu.Game.Rulesets.Karaoke.Tests.Mods;

public class ModsTest
{
    [Test]
    public void TestCheckDuplicatedProperty()
    {
        var mods = getAllModsFromTheRuleset().ToArray();

        // Name should not duplicated.
        bool hasDuplicatedName = mods.GroupBy(x => x.Name).Any(g => g.Count() > 1);
        Assert.IsFalse(hasDuplicatedName);

        // Acronym should not duplicated.
        bool hasDuplicatedAcronym = mods.GroupBy(x => x.Acronym).Any(g => g.Count() > 1);
        Assert.IsFalse(hasDuplicatedAcronym);
    }

    /// <summary>
    /// get the mods inherit the <see cref="Mod"/> class in the karaoke ruleset by reflection.
    /// </summary>
    /// <returns></returns>
    private IEnumerable<Mod> getAllModsFromTheRuleset() =>
        Assembly.GetAssembly(typeof(KaraokeRuleset))!
                .GetTypes()
                .Where(t => t.IsSubclassOf(typeof(Mod)) && !t.IsAbstract)
                .Select(t => (Mod)Activator.CreateInstance(t)!);
}
