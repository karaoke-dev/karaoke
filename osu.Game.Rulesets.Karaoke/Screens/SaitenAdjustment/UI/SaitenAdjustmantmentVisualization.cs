// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Game.Beatmaps;
using osu.Game.Rulesets.Karaoke.Objects;
using osu.Game.Rulesets.Karaoke.Objects.Drawables;
using osu.Game.Rulesets.Karaoke.UI;
using osu.Game.Rulesets.Mods;
using osu.Game.Rulesets.Objects.Drawables;
using System.Collections.Generic;
using System.Linq;
using osu.Framework.Allocation;
using osu.Framework.Graphics.Containers;
using osu.Game.Configuration;
using osu.Game.Rulesets.Karaoke.Resources;
using osu.Game.Rulesets.Karaoke.Screens.SaitenAdjustment.Beatmaps;
using osu.Game.Rulesets.Karaoke.Configuration;
using osu.Game.Rulesets.UI;

namespace osu.Game.Rulesets.Karaoke.Screens.SaitenAdjustment.UI
{
    public class SaitenAdjustmantmentVisualization : Container
    {
        private DependencyContainer dependencies;

        protected override IReadOnlyDependencyContainer CreateChildDependencies(IReadOnlyDependencyContainer parent)
        {
            dependencies = new DependencyContainer(base.CreateChildDependencies(parent));
            //var config = dependencies.Get<RulesetConfigCache>().GetConfigFor(new KaraokeRuleset());
            //dependencies.Cache(config);
            return dependencies;
        }

        [BackgroundDependencyLoader]
        private void load(RulesetStore rulesets, RulesetConfigCache configCache)
        {
            var ruleset = rulesets.AvailableRulesets.FirstOrDefault(x => x.Name.ToLower().Contains("karaoke")).CreateInstance();
            var config = dependencies.Get<RulesetConfigCache>().GetConfigFor(ruleset);
            dependencies.Cache(config);

            var beatmap = KaraokeResources.OpenBeatmap("saiten-result");
            var workingBeatmap = new SaitenAdjustmentWorkingBeatmap(beatmap);
            var convertedBeatmap = workingBeatmap.GetPlayableBeatmap(ruleset.RulesetInfo);

            var drawableRuleset = new DrawableSaitenAdjustmentRuleset(ruleset, convertedBeatmap, null);

            Children = new[]
            {
                drawableRuleset
            };
        }

        public class DrawableSaitenAdjustmentRuleset : DrawableKaraokeRuleset
        {
            public new NotePlayfield Playfield => (NotePlayfield)base.Playfield;

            public DrawableSaitenAdjustmentRuleset(Ruleset ruleset, IBeatmap beatmap, IReadOnlyList<Mod> mods)
                : base(ruleset, beatmap, mods)
            {
            }

            protected override Playfield CreatePlayfield() => new NotePlayfield(9);

            public override DrawableHitObject<KaraokeHitObject> CreateDrawableRepresentation(KaraokeHitObject h)
            {
                // Only get drawable note here
                var drawableHitObject = base.CreateDrawableRepresentation(h);
                if (drawableHitObject is DrawableNote)
                    return drawableHitObject;

                return null;
            }
        }
    }
}
