// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using NUnit.Framework;
using osu.Game.Rulesets.Karaoke.Mods;
using osu.Game.Tests.Visual;

namespace osu.Game.Rulesets.Karaoke.Tests.Mods
{
    [TestFixture]
    public class TestSceneAutoplayMod : ModTestScene
    {
        public TestSceneAutoplayMod()
            : base(new KaraokeRuleset())
        {
        }

        [Test]
        public void TestMod() => CreateModTest(new ModTestData
        {
            Mod = new KaraokeModAutoplay(),
            Autoplay = true,
        });
    }
}
