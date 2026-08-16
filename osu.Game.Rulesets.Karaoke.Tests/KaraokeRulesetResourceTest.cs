// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Linq;
using NUnit.Framework;

namespace osu.Game.Rulesets.Karaoke.Tests;

[TestFixture]
public class KaraokeRulesetResourceTest
{
    [Test]
    public void TestResourcesAreEmbeddedInRulesetAssembly()
    {
        var assembly = typeof(KaraokeRuleset).Assembly;
        var resources = new KaraokeRuleset().CreateResourceStore();

        Assert.Multiple(() =>
        {
            Assert.That(assembly.GetReferencedAssemblies().Select(x => x.Name), Does.Not.Contain("osu.Game.Rulesets.Karaoke.Resources"));
            Assert.That(assembly.GetManifestResourceNames(), Does.Contain("osu.Game.Rulesets.Karaoke.Resources.Localisation.Common.resources"));
            Assert.That(assembly.GetManifestResourceStream("osu.Game.Rulesets.Karaoke.Resources.Textures.logo.png"), Is.Not.Null);
            Assert.That(resources.Get("Textures/logo.png"), Is.Not.Null);
            Assert.That(resources.Get("Mod/Snow/Snow.png"), Is.Not.Null);
        });
    }
}
