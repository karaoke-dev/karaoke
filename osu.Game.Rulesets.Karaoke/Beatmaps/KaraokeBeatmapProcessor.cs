// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using System.Linq;
using osu.Game.Beatmaps;
using osu.Game.Rulesets.Karaoke.Beatmaps.Metadatas;
using osu.Game.Rulesets.Karaoke.Beatmaps.Patterns;
using osu.Game.Rulesets.Karaoke.Beatmaps.Stages;
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
                        foreach (var flag in lyric.GetAllInvalidWorkingProperties())
                        {
                            applyInvalidProperty(lyric, flag);
                        }

                        break;

                    case Note note:
                        foreach (var flag in note.GetAllInvalidWorkingProperties())
                        {
                            applyInvalidProperty(note, flag);
                        }

                        break;
                }
            }
        }

        private void applyInvalidProperty(Lyric lyric, LyricWorkingProperty flag)
        {
            switch (flag)
            {
                case LyricWorkingProperty.StartTime:
                    lyric.StartTime = getStartTime();
                    break;

                case LyricWorkingProperty.Duration:
                    lyric.Duration = getDuration();
                    break;

                case LyricWorkingProperty.Timing:
                    // start time and duration should be set by other condition.
                    break;

                case LyricWorkingProperty.Singers:
                    lyric.Singers = getSingerInfo().GetSingerByIds(lyric.SingerIds.ToArray());
                    break;

                case LyricWorkingProperty.Page:
                    var pageInfo = Beatmap.PageInfo;
                    lyric.PageIndex = pageInfo.GetPageIndexAt(lyric.LyricStartTime);
                    break;

                case LyricWorkingProperty.ReferenceLyric:
                    lyric.ReferenceLyric = findLyricById(lyric.ReferenceLyricId);
                    break;

                default:
                    throw new ArgumentOutOfRangeException();
            }

            double getStartTime()
            {
                (double? startTime, double? _) = getWorkingStage()?.GetStartAndEndTime(lyric) ?? new Tuple<double?, double?>(null, null);
                return startTime ?? 0;
            }

            double getDuration()
            {
                (double? startTime, double? endTime) = getWorkingStage()?.GetStartAndEndTime(lyric) ?? new Tuple<double?, double?>(null, null);
                return endTime - startTime ?? 0;
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
                    note.ReferenceLyric = findLyricById(note.ReferenceLyricId);
                    break;

                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private SingerInfo getSingerInfo()
            => Beatmap.SingerInfo;

        // todo: should use better way to get the correct stage.
        private StageInfo? getWorkingStage()
            => Beatmap.StageInfos.FirstOrDefault();

        private Lyric? findLyricById(int? id) =>
            id == null ? null : Beatmap.HitObjects.OfType<Lyric>().Single(x => x.ID == id);

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
