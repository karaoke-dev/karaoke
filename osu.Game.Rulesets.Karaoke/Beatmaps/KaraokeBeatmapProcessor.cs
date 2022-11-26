// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

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

        public override void PreProcess()
        {
            base.PreProcess();

            applyReferenceObject(Beatmap);
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

        private void applyReferenceObject(IBeatmap beatmap)
        {
            foreach (var obj in beatmap.HitObjects.OfType<KaraokeHitObject>())
            {
                switch (obj)
                {
                    case Lyric lyric:
                        if (lyric.ReferenceLyric != null || lyric.ReferenceLyricId == null)
                            return;

                        lyric.ReferenceLyric = findLyricById(lyric.ReferenceLyricId.Value);
                        break;

                    case Note note:
                        if (note.ReferenceLyric != null || note.ReferenceLyricId == null)
                            return;

                        note.ReferenceLyric = findLyricById(note.ReferenceLyricId.Value);
                        break;
                }
            }

            Lyric findLyricById(int id) =>
                beatmap.HitObjects.OfType<Lyric>().Single(x => x.ID == id);
        }
    }
}
