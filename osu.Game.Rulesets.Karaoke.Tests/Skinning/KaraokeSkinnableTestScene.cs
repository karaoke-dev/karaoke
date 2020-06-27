// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using System.Collections.Generic;
using osu.Game.Beatmaps;
using osu.Game.Rulesets.Karaoke.Beatmaps;
using osu.Game.Rulesets.Karaoke.Skinning;
using osu.Game.Tests.Visual;

namespace osu.Game.Rulesets.Karaoke.Tests.Skinning
{
    public abstract class KaraokeSkinnableTestScene : SkinnableTestScene
    {
        public override IReadOnlyList<Type> RequiredTypes => new[]
        {
            typeof(KaraokeRuleset),
            typeof(KaraokeLegacySkinTransformer),
        };

        protected override Ruleset CreateRulesetForSkinProvider() => new KaraokeRuleset();

        protected override IBeatmap CreateBeatmapForSkinProvider() => new KaraokeBeatmap();
    }
}
