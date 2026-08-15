// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using NUnit.Framework;

namespace osu.Game.Rulesets.Karaoke.Tests;

[TestFixture]
public class KaraokeRulesetTest
{
    [TestCase(0, "Vocal")]
    [TestCase(1, "Gameplay")]
    [TestCase(2, "Composer")]
    [TestCase(-1, "")]
    public void TestVariantName(int variant, string expected)
    {
        Assert.That(new KaraokeRuleset().GetVariantName(variant).ToString(), Is.EqualTo(expected));
    }
}
