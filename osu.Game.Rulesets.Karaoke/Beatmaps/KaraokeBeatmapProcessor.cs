// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using System.Linq;
using osu.Game.Beatmaps;
using osu.Game.Rulesets.Karaoke.Beatmaps.Patterns;
using osu.Game.Rulesets.Karaoke.Objects;
using osu.Game.Rulesets.Karaoke.Objects.Workings;

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

            applyInvalidProperty(Beatmap);
        }

        private void applyInvalidProperty(IBeatmap beatmap)
        {
            foreach (var hitObject in beatmap.HitObjects.OfType<KaraokeHitObject>())
            {
                switch (hitObject)
                {
                    case Lyric lyric:
                        foreach (var flag in lyric.WorkingPropertyValidator.GetAllInvalidFlags())
                        {
                            applyInvalidProperty(lyric, flag);
                            lyric.WorkingPropertyValidator.Validate(flag);
                        }

                        break;

                    case Note note:
                        foreach (var flag in note.WorkingPropertyValidator.GetAllInvalidFlags())
                        {
                            applyInvalidProperty(note, flag);
                            note.WorkingPropertyValidator.Validate(flag);
                        }

                        break;
                }
            }
        }

        private void applyInvalidProperty(Lyric lyric, LyricWorkingProperty flag)
        {
            switch (flag)
            {
                case LyricWorkingProperty.Page:
                    var pageInfo = Beatmap.PageInfo;
                    lyric.PageIndex = pageInfo.GetPageIndexAt(lyric.LyricStartTime);
                    break;

                case LyricWorkingProperty.ReferenceLyric:
                    if (lyric.ReferenceLyric != null || lyric.ReferenceLyricId == null)
                        return;

                    lyric.ReferenceLyric = findLyricById(lyric.ReferenceLyricId.Value);
                    break;

                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private void applyInvalidProperty(Note note, NoteWorkingProperty flag)
        {
            switch (flag)
            {
                case NoteWorkingProperty.Page:
                    var pageInfo = Beatmap.PageInfo;
                    note.PageIndex = pageInfo.GetPageIndexAt(note.StartTime);
                    break;

                case NoteWorkingProperty.ReferenceLyric:
                    if (note.ReferenceLyric != null || note.ReferenceLyricId == null)
                        return;

                    note.ReferenceLyric = findLyricById(note.ReferenceLyricId.Value);
                    break;

                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private Lyric findLyricById(int id) =>
            Beatmap.HitObjects.OfType<Lyric>().Single(x => x.ID == id);

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
