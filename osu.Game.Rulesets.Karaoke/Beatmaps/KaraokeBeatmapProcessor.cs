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
        public new KaraokeBeatmap Beatmap => (KaraokeBeatmap)base.Beatmap;

        public KaraokeBeatmapProcessor(IBeatmap beatmap)
            : base(beatmap)
        {
        }

        public override void PreProcess()
        {
            base.PreProcess();

            applyReferenceObject(Beatmap);
            applyPage(Beatmap);
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
            foreach (var hitObject in beatmap.HitObjects.OfType<KaraokeHitObject>())
            {
                switch (hitObject)
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

        private void applyPage(KaraokeBeatmap beatmap)
        {
            var pageIndo = beatmap.PageInfo;

            foreach (var hitObject in beatmap.HitObjects.OfType<KaraokeHitObject>())
            {
                switch (hitObject)
                {
                    case Lyric lyric:
                        lyric.PageIndex = pageIndo.GetPageIndexAt(lyric.LyricStartTime);
                        break;

                    case Note note:
                        note.PageIndex = pageIndo.GetPageIndexAt(note.StartTime);
                        break;
                }
            }
        }
    }
}
