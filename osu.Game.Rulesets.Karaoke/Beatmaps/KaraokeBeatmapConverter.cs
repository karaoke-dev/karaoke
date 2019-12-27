// Copyright (c) ppy Pty Ltd <contact@ppy.sh>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using System.Collections.Generic;
using System.Linq;
using osu.Game.Beatmaps;
using osu.Game.Rulesets.Karaoke.Objects;
using osu.Game.Rulesets.Objects;

namespace osu.Game.Rulesets.Karaoke.Beatmaps
{
    public class KaraokeBeatmapConverter : BeatmapConverter<KaraokeHitObject>
    {
        public KaraokeBeatmapConverter(IBeatmap beatmap, Ruleset ruleset)
            : base(beatmap, ruleset)
        {
        }

        public override bool CanConvert() => Beatmap.HitObjects.All(h => h is KaraokeHitObject);

        protected override IEnumerable<KaraokeHitObject> ConvertHitObject(HitObject original, IBeatmap beatmap)
        {
            // Because karaoke does not support any ruleset, so should not goes to here
            yield return new LyricLine();
        }

        protected override Beatmap<KaraokeHitObject> CreateBeatmap()
        {
            return new KaraokeBeatmap();
        }
    }
}
