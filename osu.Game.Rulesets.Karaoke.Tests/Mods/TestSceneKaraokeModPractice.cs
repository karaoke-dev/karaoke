// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Linq;
using NUnit.Framework;
using osu.Game.Rulesets.Karaoke.Mods;
using osu.Game.Rulesets.Karaoke.Tests.Beatmaps;
using osu.Game.Rulesets.Karaoke.UI.Overlays;
using static osu.Game.Rulesets.Karaoke.UI.Overlays.SettingHUDOverlay;

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
                var settingHUDOverlay = overlays.OfType<SettingHUDOverlay>().FirstOrDefault();
                var actionContainer = settingHUDOverlay.OfType<KaraokeActionContainer>().FirstOrDefault();

                // todo : test overlays if exist.
                return actionContainer?.Child is ControlLayer;
            }
        });
    }
}
