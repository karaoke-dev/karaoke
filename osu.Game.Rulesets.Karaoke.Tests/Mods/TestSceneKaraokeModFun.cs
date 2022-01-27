// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using NUnit.Framework;
using osu.Game.Rulesets.Karaoke.Mods;
using osu.Game.Rulesets.Karaoke.Tests.Beatmaps;

namespace osu.Game.Rulesets.Karaoke.Tests.Mods
{
    public class TestSceneKaraokeModFun : KaraokeModTestScene
    {
        [Test]
        public void TestSnowMod() => CreateModTest(new ModTestData
        {
            Mod = new KaraokeModSnow(),
            Autoplay = false,
            Beatmap = new TestKaraokeBeatmap(Ruleset.Value),
            PassCondition = () => true
        });

        [Test]
        public void TestWindowsUpdateMod() => CreateModTest(new ModTestData
        {
            Mod = new KaraokeModWindowsUpdate(),
            Autoplay = false,
            Beatmap = new TestKaraokeBeatmap(Ruleset.Value),
            PassCondition = () => true
        });
    }
}
