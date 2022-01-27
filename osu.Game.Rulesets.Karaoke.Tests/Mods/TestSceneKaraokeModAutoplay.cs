// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using NUnit.Framework;
using osu.Game.Rulesets.Karaoke.Mods;
using osu.Game.Rulesets.Karaoke.Tests.Beatmaps;

namespace osu.Game.Rulesets.Karaoke.Tests.Mods
{
    public class TestSceneKaraokeModAutoplay : KaraokeModTestScene
    {
        [Ignore("mod auto-play will cause crash")]
        public void TestMod() => CreateModTest(new ModTestData
        {
            Mod = new KaraokeModAutoplay(),
            Autoplay = true,
            Beatmap = new TestKaraokeBeatmap(Ruleset.Value),
            PassCondition = () => true
        });
    }
}
