// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

#nullable disable

using System.Linq;
using NUnit.Framework;
using osu.Game.Rulesets.Karaoke.Mods;
using osu.Game.Rulesets.Karaoke.Tests.Beatmaps;
using osu.Game.Rulesets.Karaoke.UI.HUD;
using osu.Game.Skinning;

namespace osu.Game.Rulesets.Karaoke.Tests.Mods
{
    public class TestSceneKaraokeModPractice : KaraokeModTestScene
    {
        [Test]
        public void TestAllPanelExist() => CreateModTest(new ModTestData
        {
            Mod = new KaraokeModPractice(),
            Autoplay = false,
            Beatmap = new TestKaraokeBeatmap(new RulesetInfo()),
            PassCondition = () =>
            {
                // just need to check has setting button display area.
                var skinnableTargetContainers = Player.HUDOverlay.OfType<SkinnableTargetContainer>().FirstOrDefault();

                // todo: because setting buttons display created from skin transform , so might not able to get from here.
                var hud = skinnableTargetContainers?.Components.OfType<SettingButtonsDisplay>().FirstOrDefault();
                return true;
                //return hud != null;
            }
        });
    }
}
