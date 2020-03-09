// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Game.Rulesets.Karaoke.Configuration;
using osu.Game.Rulesets.Karaoke.Resources;
using osu.Game.Rulesets.Karaoke.Screens.SaitenAdjustment.Beatmaps;
using osu.Game.Screens;

namespace osu.Game.Rulesets.Karaoke.Screens.SaitenAdjustment
{
    public class SaitenAdjustmentScreen : OsuScreen
    {
        private DependencyContainer dependencies;

        protected override IReadOnlyDependencyContainer CreateChildDependencies(IReadOnlyDependencyContainer parent)
            => dependencies = new DependencyContainer(base.CreateChildDependencies(parent));

        [BackgroundDependencyLoader]
        private void load(RulesetConfigCache configCache)
        {
            var config = (KaraokeRulesetConfigManager)configCache.GetConfigFor(Ruleset.Value.CreateInstance());
            dependencies.Cache(new KaraokeSessionStatics(config, null));

            var beatmap = KaraokeResources.OpenBeatmap("saiten-result");
            var workingBeatmap = new SaitenAdjustmentWorkingBeatmap(beatmap);

            var ruleset = new KaraokeRuleset();
            var drawableRuleset = ruleset.CreateDrawableRulesetWith(workingBeatmap.GetPlayableBeatmap(ruleset.RulesetInfo));

            AddInternal(new Container
            {
                RelativeSizeAxes = Axes.Both,
                Children = new[]
                {
                    drawableRuleset
                }
            });
        }
    }
}
