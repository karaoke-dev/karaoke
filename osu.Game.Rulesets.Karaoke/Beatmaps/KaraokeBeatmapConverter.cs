// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
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

        protected override Beatmap<KaraokeHitObject> ConvertBeatmap(IBeatmap original, CancellationToken cancellationToken)
        {
            var beatmap = base.ConvertBeatmap(original, cancellationToken);

            // Apply property created from legacy decoder
            var propertyDictionary = beatmap.HitObjects.OfType<LegacyProperties>().FirstOrDefault();

            if (propertyDictionary == null)
                return beatmap;

            if (beatmap is not KaraokeBeatmap karaokeBeatmap)
                throw new InvalidCastException(nameof(beatmap));

            karaokeBeatmap.AvailableTranslates = propertyDictionary.AvailableTranslates;
            beatmap.HitObjects.Remove(propertyDictionary);

            return beatmap;
        }

        protected override IEnumerable<KaraokeHitObject> ConvertHitObject(HitObject original, IBeatmap beatmap, CancellationToken cancellationToken)
            => throw new NotImplementedException();

        protected override Beatmap<KaraokeHitObject> CreateBeatmap() => new KaraokeBeatmap();
    }
}
