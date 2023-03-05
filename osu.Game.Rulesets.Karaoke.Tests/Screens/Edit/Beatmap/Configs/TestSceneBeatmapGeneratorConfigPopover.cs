// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using NUnit.Framework;
using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Game.Rulesets.Karaoke.Configuration;
using osu.Game.Rulesets.Karaoke.Screens.Edit.Beatmaps.Configs.Generator.Beatmaps.Pages;
using osu.Game.Tests.Visual;

namespace osu.Game.Rulesets.Karaoke.Tests.Screens.Edit.Beatmap.Configs;

[TestFixture]
public partial class TestSceneBeatmapGeneratorConfigPopover : OsuTestScene
{
    [BackgroundDependencyLoader]
    private void load()
    {
        var config = new KaraokeRulesetEditGeneratorConfigManager();
        Dependencies.Cache(config);
    }

    [TestCase(typeof(PageGeneratorConfigPopover), TestName = nameof(PageGeneratorConfigPopover))]
    public void TestGenerate(Type configType)
    {
        AddStep("Show dialog", () =>
        {
            Schedule(() =>
            {
                Child = Activator.CreateInstance(configType) as Drawable;
                Child?.Show();
            });
        });
    }
}
