// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using NUnit.Framework;
using osu.Framework.Allocation;
using osu.Game.Rulesets.Karaoke.Configuration;
using osu.Game.Rulesets.Karaoke.Screens.Edit;
using osu.Game.Tests.Visual;

namespace osu.Game.Rulesets.Karaoke.Tests.Screens;

[TestFixture]
public partial class TestSceneGeneratorConfigPopover : OsuTestScene
{
    private KaraokeRulesetEditGeneratorConfigManager config = null!;

    [BackgroundDependencyLoader]
    private void load()
    {
        config = new KaraokeRulesetEditGeneratorConfigManager();
        Dependencies.Cache(config);
    }

    [Test]
    public void TestPopover()
    {
        foreach (var setting in Enum.GetValues<KaraokeRulesetEditGeneratorSetting>())
        {
            AddLabel($"Generate config: {nameof(setting)}");
            AddStep("Show dialog", () =>
            {
                var popover = new GeneratorConfigPopover(setting);

                Schedule(() =>
                {
                    Child = popover;
                    Child.Show();
                });
            });
        }
    }
}
