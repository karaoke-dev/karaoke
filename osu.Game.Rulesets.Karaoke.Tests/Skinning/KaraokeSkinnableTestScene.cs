// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Game.Rulesets.Karaoke.Beatmaps.Formats;
using osu.Game.Rulesets.Karaoke.Skinning;
using osu.Game.Tests.Visual;
using System;
using System.Collections.Generic;

namespace osu.Game.Rulesets.Karaoke.Tests.Skinning
{
    public abstract class KaraokeSkinnableTestScene : SkinnableTestScene
    {
        protected KaraokeSkinnableTestScene()
        {
            // It's a tricky to let osu! to read karaoke testing beatmap
            KaraokeLegacyBeatmapDecoder.Register();
        }

        public override IReadOnlyList<Type> RequiredTypes => new[]
        {
            typeof(KaraokeRuleset),
            typeof(KaraokeLegacySkinTransformer),
        };

        protected override Ruleset CreateRulesetForSkinProvider() => new KaraokeRuleset();
    }
}
