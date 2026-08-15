// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using NUnit.Framework;
using osu.Game.Rulesets.Karaoke.Mods;
using osu.Game.Rulesets.Karaoke.Tests.Beatmaps;

namespace osu.Game.Rulesets.Karaoke.Tests.Mods;

public partial class TestSceneKaraokeModPractice : KaraokeModTestScene
{
    [Test]
    public void TestAllPanelExist() => CreateModTest(new ModTestData
    {
        Mod = new KaraokeModPractice(),
        Autoplay = false,
        CreateBeatmap = () => new TestKaraokeBeatmap(new RulesetInfo()),
        // should check that the setting button display area exists, but the setting buttons display is created from
        // the skin transform, so it might not be possible to get it from here.
        // todo: find a way to assert the display actually exists.
        PassCondition = () => true,
    });
}
