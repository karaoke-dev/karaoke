// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using NUnit.Framework;
using osu.Game.Rulesets.Karaoke.Mods;
using osu.Game.Rulesets.Karaoke.Tests.Beatmaps;
using osu.Game.Tests.Visual;
using System.Linq;

namespace osu.Game.Rulesets.Karaoke.Tests.Mods
{
    public class TestSceneKaraokeModPractice : ModTestScene
    {
        public TestSceneKaraokeModPractice()
            : base(new KaraokeRuleset())
        {
        }

        [Test]
        public void TestAllPanelExist() => CreateModTest(new ModTestData
        {
            Mod = new KaraokeModPractice(),
            Autoplay = false,
            Beatmap = new TestKaraokeBeatmap(null),
            PassCondition = () =>
            {
                var hudOverlay = Player.HUDOverlay;
                var actionContainer = hudOverlay.OfType<KaraokeModPractice.KaraokeActionContainer>().FirstOrDefault();
                var practiceContainer = actionContainer?.Child as KaraokeModPractice.KaraokePracticeContainer;

                // todo : test overlays is exist.
                return practiceContainer != null;
            }
        });
    }
}
