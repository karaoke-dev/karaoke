// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

#nullable disable

using System.Linq;
using osu.Game.Beatmaps;
using osu.Game.Rulesets.Karaoke.Beatmaps.Patterns;
using osu.Game.Rulesets.Karaoke.Objects;

namespace osu.Game.Rulesets.Karaoke.Beatmaps
{
    public class KaraokeBeatmapProcessor : BeatmapProcessor
    {
        public KaraokeBeatmapProcessor(IBeatmap beatmap)
            : base(beatmap)
        {
        }

        public override void PostProcess()
        {
            base.PostProcess();

            var lyrics = Beatmap.HitObjects.OfType<Lyric>().ToList();

            if (!lyrics.Any())
                return;

            var pattern = new LegacyLyricTimeGenerator();
            pattern.Generate(lyrics);
        }
    }
}
