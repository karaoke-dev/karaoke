// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using NUnit.Framework;
using osu.Game.Rulesets.Karaoke.Mods;
using osu.Game.Rulesets.Karaoke.Tests.Beatmaps;
using osu.Game.Tests.Visual;

namespace osu.Game.Rulesets.Karaoke.Tests.Mods
{
    public class TestSceneKaraokeModPractice : ModTestScene
    {
        public TestSceneKaraokeModPractice()
            : base(new KaraokeRuleset())
        {
        }

        [Test]
        public void TestMod() => CreateModTest(new ModTestData
        {
            Mod = new KaraokeModPractice(),
            Autoplay = false,
            Beatmap = new TestKaraokeBeatmap(null),
            PassCondition = () => true
        });
    }
}
