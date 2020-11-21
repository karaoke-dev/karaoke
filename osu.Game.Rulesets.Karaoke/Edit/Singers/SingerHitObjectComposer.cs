// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using System.Collections.Generic;
using osu.Game.Rulesets.Edit.Tools;
using osu.Game.Rulesets.Objects.Drawables;

namespace osu.Game.Rulesets.Karaoke.Edit.Singers
{
    public class SingerHitObjectComposer : KaraokeHitObjectComposer
    {
        public SingerHitObjectComposer(Ruleset ruleset)
            : base(ruleset)
        {
        }

        public override IEnumerable<DrawableHitObject> HitObjects => new List<DrawableHitObject>();

        protected override IReadOnlyList<HitObjectCompositionTool> CompositionTools => Array.Empty<HitObjectCompositionTool>();
    }
}
