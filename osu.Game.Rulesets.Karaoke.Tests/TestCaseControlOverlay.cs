// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using NUnit.Framework;
using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Game.Rulesets.Karaoke.UI;
using osu.Game.Tests.Visual;

namespace osu.Game.Rulesets.Karaoke.Tests
{
    [TestFixture]
    public class TestCaseControlOverlay : OsuTestScene
    {
        public ControlOverlay ControlOverlay { get; set; }

        [BackgroundDependencyLoader]
        private void load(RulesetStore rulesets)
        {
        }

        protected override void LoadComplete()
        {
            base.LoadComplete();

            // Cannot work now because it need extra BDL in child
            /*
            Add(new Container
            {
                RelativeSizeAxes = Axes.Both,
                Child = ControlOverlay = new ControlOverlay()
                {
                    RelativeSizeAxes = Axes.Both,
                }
            });

            AddStep("Toggle setting", ControlOverlay.ToggleGameplaySettingsOverlay);
            */
        }
    }
}
