// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using NUnit.Framework;
using osu.Framework.Graphics;
using osu.Game.Rulesets.Karaoke.Screens.SaitenAdjustment;
using osu.Game.Screens;
using osu.Game.Tests.Visual;

namespace osu.Game.Rulesets.Karaoke.Tests
{
    public class TestSceneSaitenAdjustmentScreen : OsuTestScene
    {
        private OsuScreenStack stack;
        private SaitenAdjustmentScreen saitenAdjustmentScreen;

        [SetUp]
        public virtual void SetUp() => Schedule(() =>
        {
            var stack = new OsuScreenStack { RelativeSizeAxes = Axes.Both };
            Child = stack;

            stack.Push(saitenAdjustmentScreen = new SaitenAdjustmentScreen());
        });
    }
}
