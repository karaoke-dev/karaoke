// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using NUnit.Framework;
using osu.Framework.Allocation;
using osu.Game.Rulesets.Karaoke.Configuration;
using osu.Game.Tests.Visual;

namespace osu.Game.Rulesets.Karaoke.Tests.Edit
{
    [TestFixture]
    public class TestSceneGeneratorConfigDialog : OsuTestScene
    {
        [BackgroundDependencyLoader]
        private void load()
        {
            var config = new KaraokeRulesetEditGeneratorConfigManager();
            Dependencies.Cache(config);
        }
    }
}
