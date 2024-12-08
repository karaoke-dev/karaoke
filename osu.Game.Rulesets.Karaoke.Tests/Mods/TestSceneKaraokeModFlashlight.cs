// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Linq;
using NUnit.Framework;
using osu.Framework.Testing;
using osu.Game.Rulesets.Karaoke.Mods;
using osu.Game.Rulesets.Karaoke.Tests.Beatmaps;
using osu.Game.Rulesets.Karaoke.Tests.Extensions;

namespace osu.Game.Rulesets.Karaoke.Tests.Mods;

public partial class TestSceneKaraokeModFlashlight : KaraokeModTestScene
{
    [Test]
    public void TestFlashlightExist() => CreateModTest(new ModTestData
    {
        Mod = new KaraokeModFlashlight(),
        Autoplay = true,
        CreateBeatmap = () => new TestKaraokeBeatmap(Ruleset.Value),
        PassCondition = () =>
        {
            var drawableRuleset = Player.GetDrawableRuleset();
            if (drawableRuleset == null)
                return false;

            // Should has at least one flashlight
            return drawableRuleset.KeyBindingInputManager.ChildrenOfType<KaraokeModFlashlight.KaraokeFlashlight>().Any();
        },
    });
}
