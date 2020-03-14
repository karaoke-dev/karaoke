// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
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
using osu.Game.Rulesets.Karaoke.Resources;
using osu.Game.Rulesets.Karaoke.Screens.SaitenAdjustment.Beatmaps;
using osu.Framework.Graphics;
using osu.Game.Rulesets.Karaoke.Configuration;
using osu.Game.Skinning;
using osu.Game.Rulesets.Karaoke.Skinning;

namespace osu.Game.Rulesets.Karaoke.Screens.SaitenAdjustment.UI
{
    public class SaitenAdjustmentVisualization : Container
    {
        private readonly string beatmapName;
        private DrawableSaitenAdjustmentRuleset drawableRuleset;

        public KaraokeSessionStatics Session => drawableRuleset.Session;

        public SaitenAdjustmentVisualization(string resourcesBeatmapName)
        {
            beatmapName = resourcesBeatmapName;
        }

        private DependencyContainer dependencies;

        protected override IReadOnlyDependencyContainer CreateChildDependencies(IReadOnlyDependencyContainer parent)
        {
            dependencies = new DependencyContainer(base.CreateChildDependencies(parent));
            return dependencies;
        }

        [BackgroundDependencyLoader]
        private void load(RulesetStore rulesets, RulesetConfigCache configCache)
        {
            // Get karaoke ruleset
            var ruleset = rulesets.AvailableRulesets?.FirstOrDefault(x => x.Name.ToLower().Contains("karaoke"))?.CreateInstance();
            if (ruleset == null)
                throw new ArgumentNullException($"{nameof(ruleset)} cannot be null.");

            // Cache
            var config = dependencies.Get<RulesetConfigCache>().GetConfigFor(ruleset);
            dependencies.Cache(config);

            // Create beatmap
            var beatmap = KaraokeResources.OpenBeatmap(beatmapName);
            var workingBeatmap = new SaitenAdjustmentWorkingBeatmap(beatmap);
            var convertedBeatmap = workingBeatmap.GetPlayableBeatmap(ruleset.RulesetInfo);

            // Create skin
            var skin = new KaraokeLegacySkinTransformer(null);

            Children = new[]
            {
                new SkinProvidingContainer(skin)
                {
                    Child = drawableRuleset = new DrawableSaitenAdjustmentRuleset(ruleset, convertedBeatmap, null)
                    {
                        RelativeSizeAxes = Axes.Both
                    }
                }
            };
        }

        private class DrawableSaitenAdjustmentRuleset : DrawableKaraokeRuleset
        {
            public DrawableSaitenAdjustmentRuleset(Ruleset ruleset, IBeatmap beatmap, IReadOnlyList<Mod> mods)
                : base(ruleset, beatmap, mods)
            {
                // Hide lyric playfield
                Playfield.LyricPlayfield.Hide();
            }

            public override DrawableHitObject<KaraokeHitObject> CreateDrawableRepresentation(KaraokeHitObject h)
            {
                // Only get drawable note here
                var drawableHitObject = base.CreateDrawableRepresentation(h);
                return drawableHitObject is DrawableNote ? drawableHitObject : null;
            }
        }
    }
}
