// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Linq;
using NUnit.Framework;
using osu.Game.Rulesets.Karaoke.Mods;
using osu.Game.Rulesets.Karaoke.Tests.Beatmaps;
using osu.Game.Rulesets.Karaoke.UI.HUD;

namespace osu.Game.Rulesets.Karaoke.Tests.Mods
{
    public class TestSceneKaraokeModPractice : KaraokeModTestScene
    {
        [Test]
        public void TestAllPanelExist() => CreateModTest(new ModTestData
        {
            Mod = new KaraokeModPractice(),
            Autoplay = false,
            Beatmap = new TestKaraokeBeatmap(null),
            PassCondition = () =>
            {
                var overlays = Player.DrawableRuleset.Overlays;
                var controlLayer = overlays.OfType<SettingOverlayContainer>().FirstOrDefault();

                // todo : test overlays is exist.
                return controlLayer != null;
            }
        });
    }
}
